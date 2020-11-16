using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    [StructLayout(LayoutKind.Explicit)]
    public struct NullAttributeValue : IAttributeValue
    {
        private static readonly object NullValue = new object();
        
        [FieldOffset(0)]
        private readonly object? _value;

        public bool IsNull => _value == NullValue;

        public NullAttributeValue(bool _)
        {
            _value = NullValue;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WriteBoolean("NULL", true);
            
            writer.WriteEndObject();
        }
    }
}