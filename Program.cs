using System;
using System.Collections.Generic;
using System.Text.Json;
using Amazon.SQS.Model;
using awsDemos.Queues;
using awsDemos.Tables;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace awsDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            // Queue Operations
            // executeQueue();

            // DynamoDB Operations
            // insertIntoTable();
            // scanTable();
            // retrieveSpecificRow();
            // updateRow();
            // deleteRow();
            // batchInsert();
            // batchGet();
            // getRecordBasedOnCondition();
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

        static void scanTable()
        {
            try
            {
                Console.WriteLine("Executing Scan Operation on DemoTable");

                List<Demo> response = new DemoTable().getAllRows().GetAwaiter().GetResult();

                Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void insertIntoTable()
        {
            try
            {
                Console.WriteLine("Executing Insert Operation on DemoTable");

                Demo obj = new Demo
                {
                    PublicKeySessionId = "uiuieuireu",
                    PublicKey = new PublicKey
                    {
                        keyId = "8989898",
                        der = new DER
                        {
                            algorithm = "SHA256",
                            format = "x.201",
                            publicKey = "io12io12io12"
                        }
                    },
                    Created = "30-06-2021",
                    SetExample = new List<string>() { "element1", "element2", "element3", "element4" }
                };

                new DemoTable().addRow(obj).GetAwaiter();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void retrieveSpecificRow()
        {
            try
            {
                Console.WriteLine("Execution Load Operation On DemoTable");

                Demo obj = new DemoTable().getRow("asuirje").GetAwaiter().GetResult();

                Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void updateRow()
        {
            try
            {
                Console.WriteLine("Executing Update Operation On DemoTable");
                DemoTable table = new DemoTable();
                Demo obj = table.getRow("asuirje").GetAwaiter().GetResult();
                obj.SetExample.Add("updateExample");
                table.updateRow(obj).GetAwaiter().GetResult();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void deleteRow()
        {
            try
            {
                Console.WriteLine("Executing Delete Operation On Demo Table");
                DemoTable table = new DemoTable();
                Demo obj = table.getRow("asdf1234").GetAwaiter().GetResult();

                table.DeleteRow(obj).GetAwaiter().GetResult();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void batchInsert()
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

                new DemoTable().BatchInsert(items).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void batchGet()
        {
            try
            {
                Console.WriteLine("Executing Batch Get Operation On DemoTable");
                List<string> items = new List<string>() { "batchinsert002", "asuirje" };
                List<Demo> demoItems = new DemoTable().BatchRetrieve(items).GetAwaiter().GetResult();

                Console.WriteLine(JsonSerializer.Serialize(demoItems, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void getRecordBasedOnCondition()
        {
            List<ScanCondition> conditions = new List<ScanCondition>
            {
                new ScanCondition("Created",ScanOperator.Equal,"30-06-2021")
            };
            List<Demo> items = new DemoTable()._dynamoDBService.GetRecords<Demo>(conditions).GetAwaiter().GetResult();

            Console.WriteLine(JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
