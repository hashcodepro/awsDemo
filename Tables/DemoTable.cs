using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace awsDemos.Tables
{
    public class DemoTable
    {
        public DynamoDBService _dynamoDBService;
        public DemoTable()
        {
            _dynamoDBService = new DynamoDBService();
        }

        public async Task addRow(Demo obj) => await _dynamoDBService.Save<Demo>(obj);

        public async Task<List<Demo>> getAllRows() => await _dynamoDBService.GetRecords<Demo>(new List<ScanCondition>());

        public async Task<Demo> getRow(string key) => await _dynamoDBService.GetRecord<Demo>(key);

        public async Task updateRow(Demo obj) => await _dynamoDBService.UpdateRecord<Demo>(obj);

        public async Task DeleteRow(Demo obj) => await _dynamoDBService.DeleteRecord<Demo>(obj);

        public async Task BatchInsert(List<Demo> items) => await _dynamoDBService.BatchWrite<Demo>(items);

        public async Task<List<Demo>> BatchRetrieve(List<string> items) => await _dynamoDBService.BatchGet<Demo>(items);

    }
}