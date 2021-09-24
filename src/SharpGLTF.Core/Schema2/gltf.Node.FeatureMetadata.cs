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
    public partial class FeatureMetadataInstancer<T> : ExtraProperties
    {
        private Dictionary<string, object> _featuretables;

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

                if (_Owner is Node)
                {
                    writer.WriteStartArray();
                    writer.WriteNumberValue(_featuretables.Keys.ToList().IndexOf(table.Key));
                    writer.WriteEndArray();
                }
                else
                {
                    writer.WriteStringValue(table.Key);
                }

                writer.WritePropertyName("featureIds");
                if (_Owner is Node)
                {
                    writer.WriteStartArray();
                    writer.WriteStartObject();
                    writer.WritePropertyName("attribute");
                    writer.WriteNumberValue((UInt32)table.Value);
                    writer.WriteEndObject();
                    writer.WriteEndArray();
                }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("attribute");
                    writer.WriteStringValue((string)table.Value);
                    writer.WriteEndObject();
                }
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
                                if (_Owner is Node) reader.Read();
                                if (reader.TokenType == JsonTokenType.Number)
                                {
                                    tablename = ((int)reader.GetUInt32()).ToString();
                                    _featuretables.Add(tablename, 0);
                                }
                                if (reader.TokenType == JsonTokenType.String)
                                {
                                    tablename = reader.GetString();
                                    _featuretables.Add(tablename, "");
                                }
                                if (_Owner is Node) reader.Read();
                                break;
                            }
                            if (reader.GetString() == "featureIds")
                            {
                                if (_Owner is Node) reader.Read();
                                reader.Read();
                                reader.Read();
                                if (reader.GetString() == "attribute")
                                {
                                    reader.Read();
                                    if (reader.TokenType == JsonTokenType.Number)
                                        _featuretables[_featuretables.Last().Key] = reader.GetUInt32();
                                    if (reader.TokenType == JsonTokenType.String)
                                        _featuretables[_featuretables.Last().Key] = reader.GetString();

                                }
                                reader.Read(); 
                                if (_Owner is Node) reader.Read();
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

    public partial class FeatureMetadataInstancer<T>
    {
        private readonly T _Owner;

        internal FeatureMetadataInstancer(T root)
        {
            _Owner = root;
            _featuretables = new Dictionary<String, object>();
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

        public void SetFeatureData(string table, UInt32 feature)
        {
            if (!_featuretables.ContainsKey(table))
            {
                _featuretables.Add(table, feature);
            }

            _featuretables[table] = feature;
        }


    }

    public static class FeatureMetadataInstancerExt
    {
        public static FeatureMetadataInstancer<MeshGpuInstancing> GetFeatureMetadata(this MeshGpuInstancing obj)
        {
            return obj.GetExtension<FeatureMetadataInstancer<MeshGpuInstancing>>();
        }

        public static FeatureMetadataInstancer<MeshPrimitive> GetFeatureMetadata(this MeshPrimitive obj)
        {
            return obj.GetExtension<FeatureMetadataInstancer<MeshPrimitive>>();
        }

        public static FeatureMetadataInstancer<Node> GetFeatureMetadata(this Node obj)
        {
            return obj.GetExtension<FeatureMetadataInstancer<Node>>();
        }

        public static FeatureMetadataInstancer<MeshGpuInstancing> UseFeatureMetadata(this MeshGpuInstancing obj)
        {
            var ext = obj.GetFeatureMetadata();
            if (ext == null)
            {
                ext = new FeatureMetadataInstancer<MeshGpuInstancing>(obj);
                obj.SetExtension(ext);
            }

            return ext;
        }

        public static FeatureMetadataInstancer<Node> UseFeatureMetadata(this Node obj)
        {
            var ext = obj.GetFeatureMetadata();
            if (ext == null)
            {
                ext = new FeatureMetadataInstancer<Node>(obj);
                obj.SetExtension(ext);
            }

            return ext;
        }

        public static FeatureMetadataInstancer<MeshPrimitive> UseFeatureMetadata(this MeshPrimitive obj)
        {
            var ext = obj.GetFeatureMetadata();
            if (ext == null)
            {
                ext = new FeatureMetadataInstancer<MeshPrimitive>(obj);
                obj.SetExtension(ext);
            }

            return ext;
        }

        public static void RemoveFeatureMetadata(this MeshGpuInstancing obj)
        {
            obj.RemoveExtensions<FeatureMetadataInstancer<MeshGpuInstancing>>();
        }

        public static void RemoveFeatureMetadata(this Node obj)
        {
            obj.RemoveExtensions<FeatureMetadataInstancer<Node>>();
        }

        public static void RemoveFeatureMetadata(this MeshPrimitive obj)
        {
            obj.RemoveExtensions<FeatureMetadataInstancer<MeshPrimitive>>();
        }
    }
}
