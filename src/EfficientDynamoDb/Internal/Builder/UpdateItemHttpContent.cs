using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Builder
{
    public class UpdateItemHttpContent : DynamoDbHttpContent
    {
        private readonly string _tableName;
        private readonly string _pkName;
        private readonly string _skName;
        private readonly UpdateItemRequest _request;

        public UpdateItemHttpContent(UpdateItemRequest request, string tableName, string pkName, string skName) : base("DynamoDB_20120810.UpdateItem")
        {
            _request = request;
            _tableName = tableName;
            _pkName = pkName;
            _skName = skName;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();

            WritePrimaryKey(writer);
            
            writer.WriteString("TableName", _tableName);
            
            if (_request.ConditionExpression != null)
                writer.WriteString("ConditionExpression", _request.ConditionExpression);
            
            if (_request.ExpressionAttributeNames?.Count > 0)
                WriteExpressionAttributeNames(writer);
            
            if (_request.ExpressionAttributeValues?.Count > 0)
                WriteExpressionAttributeValues(writer);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                WriteReturnConsumedCapacity(writer);

            if (_request.ReturnItemCollectionMetrics != ReturnItemCollectionMetrics.None)
                WriteReturnItemCollectionMetrics(writer);
            
            if (_request.ReturnValues != ReturnValues.None)
                WriteReturnValues(writer);

            if (_request.UpdateExpression != null)
                writer.WriteString("UpdateExpression", _request.UpdateExpression);

            writer.WriteEndObject();

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WritePrimaryKey(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _request.Key!.PartitionKeyValue.Write(writer);

            if (_request.Key.SortKeyValue != null)
            {
                writer.WritePropertyName(_skName);
                _request.Key.SortKeyValue.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReturnValues(Utf8JsonWriter writer)
        {
            writer.WriteString("ReturnValues", _request.ReturnValues switch
            {
                ReturnValues.None => "NONE",
                ReturnValues.AllOld => "ALL_OLD",
                ReturnValues.UpdatedOld => "UPDATED_OLD",
                ReturnValues.AllNew => "ALL_NEW",
                ReturnValues.UpdatedNew => "UPDATED_NEW",
                _ => "NONE"
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReturnConsumedCapacity(Utf8JsonWriter writer)
        {
            writer.WriteString("ReturnConsumedCapacity", _request.ReturnConsumedCapacity switch
            {
                ReturnConsumedCapacity.Indexes => "INDEXES",
                ReturnConsumedCapacity.Total => "TOTAL",
                ReturnConsumedCapacity.None => "NONE",
                _ => "NONE"
                
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReturnItemCollectionMetrics(Utf8JsonWriter writer)
        {
            writer.WriteString("ReturnItemCollectionMetrics", _request.ReturnItemCollectionMetrics switch
            {
                ReturnItemCollectionMetrics.None => "NONE",
                ReturnItemCollectionMetrics.Size => "SIZE",
                _ => "NONE"
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteExpressionAttributeNames(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("ExpressionAttributeNames");
                
            writer.WriteStartObject();

            foreach (var pair in _request.ExpressionAttributeNames!)
                writer.WriteString(pair.Key, pair.Value);
                
            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteExpressionAttributeValues(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("ExpressionAttributeValues");

            WriteAttributesDictionary(writer, _request.ExpressionAttributeValues!);
        }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteAttributesDictionary(Utf8JsonWriter writer, IReadOnlyDictionary<string, AttributeValue> attributesDictionary)
        {
            writer.WriteStartObject();

            foreach (var pair in attributesDictionary)
            {
                writer.WritePropertyName(pair.Key);

                pair.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }
    }
}