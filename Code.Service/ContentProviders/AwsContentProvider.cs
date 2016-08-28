using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Trainer.Domain;

namespace Code.Service.ContentProviders
{
    public class AwsContentProvider : CloudProvider, IContentProvider<Presentation>
    {
        private readonly AmazonDynamoDBClient _client;

        public AwsContentProvider()
        {
            try
            {
                _client = new AmazonDynamoDBClient();
            } catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        public async Task<IEnumerable<Presentation>> GetAllContent(string path, string pattern)
        {
            var list = new List<Presentation>();
            if (null == _client) return list;

            var context = new DynamoDBContext(_client);
            var tables = _client.ListTables().TableNames;
            if(!tables.Any())
            {
                //presentations table not found, create it
                await _client.CreateTableAsync(new CreateTableRequest
                {
                    TableName = "Presentations",
                    ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 3, WriteCapacityUnits = 1 },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Title",
                            KeyType = KeyType.HASH
                        }
                    },
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = "Title", AttributeType = ScalarAttributeType.S }
                    }
                });

                var contents = GetAllLocalContent(ConfigurationManager.AppSettings["path"], ConfigurationManager.AppSettings["pattern"]);
                foreach (var presentation in contents)
                {
                    await context.SaveAsync<Presentation>(presentation);
                }
            }

            var query = context.QueryAsync<Presentation>("Title",  QueryOperator.GreaterThan, new string[] { "" }.AsEnumerable());
            while (!query.IsDone)
            {
                list = await query.GetNextSetAsync();
            }

            return list;
        }

        public Task<Presentation> GetContentById(object id)
        {
            throw new NotImplementedException();
        }

        public void UpdateContent(Presentation item)
        {
            throw new NotImplementedException();
        }

        public void DeleteContent(object id)
        {
            throw new NotImplementedException();
        }
    }
}
