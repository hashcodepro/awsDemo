using System;
using System.Collections.Generic;
using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace awsDemos.Tables
{
    [DynamoDBTable("DemoTable")]
    public class Demo
    {
        [DynamoDBHashKey]
        public string PublicKeySessionId { get; set; }

        [DynamoDBProperty(typeof(PublicKeyConvertor))]
        public PublicKey PublicKey { get; set; }

        [DynamoDBProperty]
        public string Created { get; set; }

        [DynamoDBProperty]
        public List<string> SetExample { get; set; }
    }

    public class PublicKey
    {
        public string keyId { get; set; }
        public DER der { get; set; }
    }
    public class DER
    {
        public string algorithm { get; set; }
        public string format { get; set; }
        public string publicKey { get; set; }
    }

    class PublicKeyConvertor : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            PublicKey publicKey = value as PublicKey;

            if (publicKey == null)
                throw new ArgumentOutOfRangeException();

            var derDocument = new Document();
            derDocument["algorithm"] = publicKey.der.algorithm;
            derDocument["format"] = publicKey.der.format;
            derDocument["publicKey"] = publicKey.der.publicKey;

            var publicKeyDocument = new Document();
            publicKeyDocument["keyId"] = publicKey.keyId;
            publicKeyDocument["der"] = derDocument;

            DynamoDBEntry entry = publicKeyDocument;

            return entry;
        }
        public object FromEntry(DynamoDBEntry entry)
        {
            Document publicKeyDocument = entry as Document;

            if (publicKeyDocument == null)
                throw new ArgumentOutOfRangeException();

            Document derDocument = publicKeyDocument["der"] as Document;

            DER der = new DER
            {
                algorithm = derDocument["algorithm"],
                format = derDocument["format"],
                publicKey = derDocument["publicKey"]
            };

            PublicKey publicKey = new PublicKey
            {
                keyId = publicKeyDocument["keyId"],
                der = der
            };

            return publicKey;

        }
    }
}