using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.GetItem;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal abstract class GetItemHttpContentBase<TRequest> : DynamoDbHttpContent where TRequest : GetItemRequestBase
    {
        private readonly string? _tablePrefix;
        
        protected TRequest Request { get; }

        public GetItemHttpContentBase(TRequest request, string? tablePrefix) : base("DynamoDB_20120810.GetItem")
        {
            Request = request;
            _tablePrefix = tablePrefix;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();
            
            WritePrimaryKey(in ddbWriter);

            writer.WriteTableName(_tablePrefix, Request.TableName);

            if (Request.ConsistentRead)
                writer.WriteBoolean("ConsistentRead", true);
            
            if (Request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(Request.ExpressionAttributeNames);
            
            if (Request.ProjectionExpression != null)
                writer.WriteString("ProjectionExpression", Request.ProjectionExpression);
            
            if (Request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(Request.ReturnConsumedCapacity);

            writer.WriteEndObject();

            return default;  
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract void WritePrimaryKey(in DdbWriter writer);
    }
}