using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using SharpGLTF.Collections;
using SharpGLTF.IO;

namespace SharpGLTF.Schema2
{
    public class FeatureId : ExtraProperties
    {
        public int? attribute;
        public int? offset;
        public int? repeat;

        protected override void DeserializeProperty(string jsonPropertyName, ref Utf8JsonReader reader)
        {
            switch (jsonPropertyName)
            {
                case "attribute": attribute = DeserializePropertyValue<Int32>(ref reader); break;
                case "offset": offset = DeserializePropertyValue<Int32>(ref reader); break;
                case "repeat": repeat = DeserializePropertyValue<Int32>(ref reader); break;
                default: base.DeserializeProperty(jsonPropertyName, ref reader); break;
            }
        }

        protected override void SerializeProperties(Utf8JsonWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            base.SerializeProperties(writer);

            if (attribute.HasValue)
                SerializeProperty(writer, "attribute", attribute.Value);

            if (offset.HasValue)
                SerializeProperty(writer, "offset", offset.Value);

            if (repeat.HasValue)
                SerializeProperty(writer, "repeat", repeat.Value);
        }
    }
}
