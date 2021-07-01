using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace awsDemos.Tables
{
    public class DynamoDBService
    {
        AmazonDynamoDBClient client;
        DynamoDBContext dbContext;

        public DynamoDBService()
        {
            client = new AmazonDynamoDBClient();
            dbContext = new DynamoDBContext(client);
        }

        public async Task Save<T>(T item) where T : new() => await dbContext.SaveAsync<T>(item);

        public async Task<List<T>> GetRecords<T>(List<ScanCondition> conditions) where T : class
        {
            AsyncSearch<T> scanResult = dbContext.ScanAsync<T>(conditions);

            List<T> obj = new List<T>();

            foreach (T item in await scanResult.GetNextSetAsync())
                obj.Add(item);

            return obj;
        }

        public async Task<T> GetRecord<T>(string key) where T : class => await dbContext.LoadAsync<T>(key);

        public async Task UpdateRecord<T>(T item) where T : new()
        {
            T savedItem = await dbContext.LoadAsync<T>(item);

            if (savedItem == null)
                throw new AmazonDynamoDBException("The item does not exist in the Table");

            await Save<T>(item);
        }

        public async Task DeleteRecord<T>(T item) where T : new()
        {
            T savedItem = await dbContext.LoadAsync<T>(item);

            if (savedItem == null)
                throw new AmazonDynamoDBException("The item does not exist in the Table");

            await dbContext.DeleteAsync<T>(item);
        }

        public async Task BatchWrite<T>(List<T> items) where T : new()
        {
            BatchWrite<T> batch = dbContext.CreateBatchWrite<T>();

            batch.AddPutItems(items);

            await batch.ExecuteAsync();
        }

        public async Task<List<T>> BatchGet<T>(List<string> items) where T : class
        {
            BatchGet<T> batch = dbContext.CreateBatchGet<T>();

            foreach (string item in items)
                batch.AddKey(item);

            await batch.ExecuteAsync();

            return batch.Results as List<T>;
        }

        // Query operation requires both Primary and Sort Key
    }
}