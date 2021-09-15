using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json;
using SharpGLTF.Collections;

namespace SharpGLTF.Schema2
{
    public partial class FeatureMetadata : ExtraProperties
    {
        private string? _schemastring;
        private Dictionary<String, Dictionary<String, Int32>> _featuretables;

        protected override void SerializeProperties(Utf8JsonWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            base.SerializeProperties(writer);

            if (_schemastring == null)
                return;

            writer.WritePropertyName("schema");
            writer.WriteRawValue(_schemastring);

            writer.WritePropertyName("featuretables");
            writer.WriteStartObject();

            foreach (var table in _featuretables)
            {
                writer.WritePropertyName(table.Key);

                writer.WriteStartObject();

                writer.WritePropertyName("class");
                writer.WriteStringValue(table.Key);
                writer.WritePropertyName("count");
                writer.WriteNumberValue(table.Value.Count);

                writer.WritePropertyName("properties");
                writer.WriteStartObject();

                foreach (var pop in table.Value)
                {
                    writer.WritePropertyName(pop.Key);
                    writer.WriteStartObject();
                    writer.WritePropertyName("bufferView");
                    writer.WriteNumberValue(pop.Value);
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        protected override void DeserializeProperty(string jsonPropertyName, ref Utf8JsonReader reader)
        {
            if (jsonPropertyName == "schema")
            {
                int level = int.MaxValue;
                bool isp = false;
                StringBuilder str = new StringBuilder();
                while (reader.Read())
                {
                    if (level == int.MaxValue)
                        level = 0;
                    else if (level == 0)
                        break;

                    switch (reader.TokenType)
                    {
                        case JsonTokenType.Null: break;
                        case JsonTokenType.String: str.Append($"\"{reader.GetString()}\""); isp = true; break;
                        case JsonTokenType.PropertyName: if (isp) str.AppendLine(","); isp = false; str.Append($"\"{reader.GetString()}\"" + ":"); break;
                        case JsonTokenType.True: str.Append("true"); isp = true; break;
                        case JsonTokenType.False: str.Append("false"); isp = true; break;
                        case JsonTokenType.Number: str.Append(reader.GetDecimal().ToString(System.Globalization.CultureInfo.InvariantCulture)); isp = true; break;
                        case JsonTokenType.StartObject:
                            str.AppendLine("{");
                            level++;
                            continue;
                        case JsonTokenType.EndObject:
                            str.AppendLine("\r\n}");
                            level--;
                            continue;
                        default:
                            break;
                    }
                }
                _schemastring = str.ToString();
            }

            if (reader.GetString() == "featuretables")
            {
                int level = 1;
                string tablename = "";
                int count = 0;

                while (level > 0 && reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.PropertyName:

                            if (reader.GetString() == "class")
                            {
                                reader.Read();
                                tablename = reader.GetString();
                                _featuretables.Add(tablename, new Dictionary<string, int>());
                                break;
                            }

                            if (reader.GetString() == "count")
                            {
                                reader.Read();
                                count = reader.GetInt32();
                                break;
                            }

                            if (reader.GetString() == "properties")
                            {
                                reader.Read();
                                for (int i = 0; i < count; i++)
                                {
                                    reader.Read();
                                    var pname = reader.GetString();
                                    reader.Read(); reader.Read(); reader.Read();
                                    var index = reader.GetInt32();
                                    _featuretables[tablename].Add(pname, index);
                                    reader.Read();
                                }

                                reader.Read();
                                break;
                            }

                            break;
                        case JsonTokenType.StartObject:
                            level++;
                            break;
                        case JsonTokenType.EndObject:
                            level--;
                            if (level == 1)
                                return;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public partial class FeatureMetadata
    {
        private readonly ModelRoot _Owner;

        public ModelRoot LogicalParent => _Owner;

        internal FeatureMetadata(ModelRoot root)
        {
            _Owner = root;
            _featuretables = new Dictionary<String, Dictionary<String, Int32>>();
        }

        public int Count => _GetCount();
        private int _GetCount()
        {
            return _featuretables.Values.Sum(x => x.Count) == 0
                ? 0
                : _featuretables.SelectMany(x => x.Value.Values)
                .Select(item => _Owner.LogicalAccessors[item].Count)
                .Min();
        }

        //TODO：读取未写
        //private IReadOnlyDictionary<string, Accessor> _GetAccessors()
        //{
        //    return new ReadOnlyLinqDictionary<String, int, Accessor>(_properties, alidx => this._Owner.LogicalAccessors[alidx]);
        //}

        public void ClearAccessors()
        {
            _featuretables.Clear();
        }

        public Accessor GetAccessor(string tablekey, string attributeKey)
        {
            Guard.NotNullOrEmpty(attributeKey, nameof(attributeKey));
            if (!_featuretables.TryGetValue(tablekey, out var _properties))
            {
                return null;
            }

            if (!_properties.TryGetValue(attributeKey, out int idx)) return null;

            return _Owner.LogicalAccessors[idx];
        }

        public void SetAccessor(string tablekey, string attributeKey, Accessor accessor)
        {
            Guard.NotNullOrEmpty(attributeKey, nameof(attributeKey));

            if (!_featuretables.TryGetValue(tablekey, out var _properties))
            {
                _properties = new Dictionary<string, int>();
                _featuretables.Add(tablekey, _properties);
            }

            if (accessor != null)
            {
                Guard.MustShareLogicalParent(_Owner, nameof(_Owner), accessor, nameof(accessor));
                if (_properties.Count > 0) Guard.MustBeEqualTo(Count, accessor.Count, nameof(accessor));
                _properties[attributeKey] = accessor.LogicalIndex;
            }
            else
            {
                _properties.Remove(attributeKey);
            }
        }

        public void SetShcema(string schemastring)
        {
            Guard.NotNullOrEmpty(schemastring, nameof(schemastring));
            _schemastring = schemastring;
        }
    }

    public sealed partial class ModelRoot
    {
        public FeatureMetadata GetFeatureMetadata()
        {
            return this.GetExtension<FeatureMetadata>();
        }

        public FeatureMetadata UseFeatureMetadata()
        {
            var ext = GetFeatureMetadata();
            if (ext == null)
            {
                ext = new FeatureMetadata(this);
                this.SetExtension(ext);
            }

            return ext;
        }

        public void RemoveFeatureMetadata()
        {
            this.RemoveExtensions<FeatureMetadata>();
        }
    }
}
