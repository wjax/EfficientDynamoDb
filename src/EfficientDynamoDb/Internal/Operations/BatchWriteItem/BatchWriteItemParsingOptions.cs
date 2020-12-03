using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    public class BatchWriteItemParsingOptions : IParsingOptions
    { 
        public static readonly BatchWriteItemParsingOptions Instance = new BatchWriteItemParsingOptions();

        public JsonObjectMetadata? Metadata { get; } = new JsonObjectMetadata(new DictionaryFieldsMetadata
        {
            {"UnprocessedItems", new JsonObjectMetadata(new AnyFieldsMetadata(new JsonObjectMetadata(true, false)))}
        });

        public bool HasNumberCallback => false;
        
        public void StartParsing(ref DdbReadStack state)
        {
            
        }
        
        public void OnNumber(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
           
        }
    }
}