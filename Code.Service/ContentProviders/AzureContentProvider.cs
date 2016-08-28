using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Trainer.Domain;

namespace Code.Service.ContentProviders
{
    public class AzureContentProvider : CloudProvider, IContentProvider<Presentation>
    {
        private readonly DocumentClient _client;
        private const string _databaseId = "Trainer";
        private const string _collectionId = "Presentations";

        public AzureContentProvider()
        {
            try
            {
                _client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["azureServiceEndpoint"]), ConfigurationManager.AppSettings["azureAuthKey"]);
            } catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        public async Task<IEnumerable<Presentation>> GetAllContent(string path, string pattern)
        {
            var list = new List<Presentation>();
            if (null != _client) return list;

            var db = _client.CreateDatabaseQuery()
                .Where(x => x.Id.Equals(_databaseId))
                .AsEnumerable()
                .FirstOrDefault();
            if(null == db)
            {
                //db does not exist, create it
                db = await _client.CreateDatabaseAsync(new Database { Id = _databaseId });
            }

            var collection = _client.CreateDocumentCollectionQuery(db.SelfLink)
                .Where(x => x.Id.Equals(_collectionId))
                .AsEnumerable()
                .FirstOrDefault();
            if(null == collection)
            {
                //docs collection does not exist, create it
                collection = await _client.CreateDocumentCollectionAsync(db.SelfLink, new DocumentCollection { Id = _collectionId });
            }

            var query = from p in _client.CreateDocumentQuery<Presentation>(collection.SelfLink)
                        select p;
            if(!query.AsEnumerable().Any())
            {
                var contents = GetAllLocalContent(ConfigurationManager.AppSettings["path"], ConfigurationManager.AppSettings["pattern"]);
                foreach (var presentation in contents)
                {
                    await _client.CreateDocumentAsync(collection.SelfLink, presentation);
                }

                query = contents.AsQueryable();
            }

            return query.AsEnumerable();
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
