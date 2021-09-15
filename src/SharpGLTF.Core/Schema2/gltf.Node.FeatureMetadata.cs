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
    public partial class Node_FeatureMetadata : ExtraProperties
    {
        private Dictionary<string, string> _featuretables;

        protected override void SerializeProperties(Utf8JsonWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            base.SerializeProperties(writer);

            writer.WritePropertyName("featureIdAttributes");
            writer.WriteStartArray();
            foreach (var table in _featuretables)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("featureTable");
                writer.WriteStringValue(table.Key);
                writer.WritePropertyName("featureIds");
                writer.WriteStartObject();
                writer.WritePropertyName("attribute");
                writer.WriteStringValue(table.Value);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        protected override void DeserializeProperty(string jsonPropertyName, ref Utf8JsonReader reader)
        {
            if (jsonPropertyName == "featureIdAttributes")
            {
                int level = 1;
                string tablename = "";
                int count = 0;

                while (level > 0 && reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.PropertyName:

                            if (reader.GetString() == "featureTable")
                            {
                                reader.Read();
                                tablename = reader.GetString();
                                _featuretables.Add(tablename, "");
                                break;
                            }

                            if (reader.GetString() == "attribute")
                            {
                                reader.Read();
                                _featuretables[_featuretables.Last().Key] = reader.GetString();
                                break;
                            }
                            break;
                        case JsonTokenType.StartArray:
                            level++;
                            break;
                        case JsonTokenType.EndArray:
                            level--;
                            if (level == 1)
                                return;
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

    public partial class Node_FeatureMetadata
    {
        private readonly Node _Owner;

        public Node LogicalParent => _Owner;
        public Node VisualParent => _Owner;

        internal Node_FeatureMetadata(Node root)
        {
            _Owner = root;
            _featuretables = new Dictionary<String, String>();
        }

        public void ClearAccessors()
        {
            _featuretables.Clear();
        }

        public void SetFeatureData(string table, string feature)
        {
            if (!_featuretables.ContainsKey(table))
            {
                _featuretables.Add(table, feature);
            }
            _featuretables[table] = feature;
        }
    }

    public sealed partial class Node
    {
        public Node_FeatureMetadata GetFeatureMetadata()
        {
            return this.GetExtension<Node_FeatureMetadata>();
        }

        public Node_FeatureMetadata UseFeatureMetadata()
        {
            var ext = GetFeatureMetadata();
            if (ext == null)
            {
                ext = new Node_FeatureMetadata(this);
                this.SetExtension(ext);
            }

            return ext;
        }

        public void RemoveFeatureMetadata()
        {
            this.RemoveExtensions<Node_FeatureMetadata>();
        }
    }
}
