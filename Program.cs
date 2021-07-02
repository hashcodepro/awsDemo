using System;
using System.Collections.Generic;
using System.Text.Json;
using Amazon.SQS.Model;
using awsDemos.Queues;
using awsDemos.Tables;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading.Tasks;

namespace awsDemos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Queue Operations
            // executeQueue();

            // DynamoDB Operations
            await insertIntoTable();
            // await scanTable();
            // await retrieveSpecificRow();
            // await updateRow();
            // await deleteRow();
            // await batchInsert();
            // await batchGet();
            // await getRecordBasedOnCondition();
        }

        static void executeQueue()
        {
            try
            {
                Console.WriteLine("Publishing Message To Queue");

                DemoQueue queue = new DemoQueue();
                SendMessageResponse response = queue.publish("hello").GetAwaiter().GetResult();

                Console.WriteLine($"Http Status Code : {response.HttpStatusCode}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task scanTable()
        {
            try
            {
                Console.WriteLine("Executing Scan Operation on DemoTable");

                List<Demo> response = await new DemoTable().getAllRows();

                Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task insertIntoTable()
        {
            try
            {
                Console.WriteLine("Executing Insert Operation on DemoTable");

                Demo obj = new Demo
                {
                    PublicKeySessionId = "newDemo001",
                    PublicKey = new PublicKey
                    {
                        keyId = "43ljk4jl3",
                        der = new DER
                        {
                            algorithm = "SHA5128",
                            format = "x.901",
                            publicKey = "asdasd"
                        }
                    },
                    Created = "02-07-2021",
                    SetExample = new List<string>() { "demo1", "demo2", "demo3" }
                };

                await new DemoTable().addRow(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task retrieveSpecificRow()
        {
            try
            {
                Console.WriteLine("Execution Load Operation On DemoTable");

                Demo obj = await new DemoTable().getRow("newSessionId001");

                Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task updateRow()
        {
            try
            {
                Console.WriteLine("Executing Update Operation On DemoTable");
                DemoTable table = new DemoTable();
                Demo obj = await table.getRow("newSessionId001");
                obj.SetExample.Add("updateExample");
                table.updateRow(obj).GetAwaiter().GetResult();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task deleteRow()
        {
            try
            {
                Console.WriteLine("Executing Delete Operation On Demo Table");
                DemoTable table = new DemoTable();
                Demo obj = await table.getRow("newSessionId002");

                await table.DeleteRow(obj);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task batchInsert()
        {
            try
            {
                Console.WriteLine("Executing Batch Insert Operation on DemoTable");
                List<Demo> items = new List<Demo>
                {
                    new Demo
                    {
                        PublicKeySessionId = "batchinsert001",
                        PublicKey = new PublicKey
                        {
                            keyId = "e2e2e2",
                            der = new DER
                            {
                                algorithm = "Crypto",
                                format = "x.901",
                                publicKey = "rtyu567"
                            }
                        },
                        Created = "01-07-2021",
                        SetExample = new List<string>() { "batchInsert1","batchInsert2" }
                    },
                    new Demo
                    {
                        PublicKeySessionId = "batchinsert002",
                        PublicKey = new PublicKey
                        {
                            keyId = "y7y7y7",
                            der = new DER
                            {
                                algorithm = "Secret",
                                format = "x.101",
                                publicKey = "u2u2u2"
                            }
                        },
                        Created = "01-07-2021",
                        SetExample = new List<string>() { "batchInsert101","batchInsert202" }
                    }
                };

                await new DemoTable().BatchInsert(items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task batchGet()
        {
            try
            {
                Console.WriteLine("Executing Batch Get Operation On DemoTable");
                List<string> items = new List<string>() { "batchinsert002", "asuirje" };
                List<Demo> demoItems = await new DemoTable().BatchRetrieve(items);

                Console.WriteLine(JsonSerializer.Serialize(demoItems, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task getRecordBasedOnCondition()
        {
            List<ScanCondition> conditions = new List<ScanCondition>
            {
                new ScanCondition("Created",ScanOperator.Equal,"01-07-2021")
            };
            List<Demo> items = await new DemoTable()._dynamoDBService.GetRecords<Demo>(conditions);

            Console.WriteLine(JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
