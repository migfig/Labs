using System.Linq;
using System.Threading.Tasks;
using Interviewer.Common;
using System.Data.SqlClient;
using Dapper;
using System.Xml.Serialization;
using System.IO;
using System;

namespace Interviewer.Data.Repositories
{
    public class InterviewerRepository : IRepository, IDbConnection
    {
        internal enum QueryType
        {
            Add,
            Update,
            Delete
        }

        private string _connectionSTring;
        public string ConnectionString
        {
            get { return _connectionSTring; }
            private set { _connectionSTring = value; }
        }

        public InterviewerRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private configuration _configuration;
        public async Task<configuration> GetConfiguration()
        {
            if(null == _configuration)
            {
                _configuration = await GetConfiguration(null, null, null, null, null);
            }

            return _configuration;
        }

        public async Task<configuration> GetConfiguration(string platform, string knowledgeArea, string area, string question, string profile)
        {
            using (var connection = new SqlConnection(_connectionSTring))
            {
                await connection.OpenAsync();
                var cfg = await connection.ExecuteScalarAsync("[dbo].[usp_GetInterviewerData]", new
                {
                    Platform = platform,
                    KnowledgeArea = knowledgeArea,
                    Area = area,
                    Question = question,
                    Profile = profile
                }, commandType: System.Data.CommandType.StoredProcedure);

                var ser = new XmlSerializer(typeof(configuration));
                _configuration = (configuration)ser.Deserialize(new StringReader((string)cfg));
            }

            return _configuration;
        }

        public async Task<int> AddItem<T>(T item)
        {
            var objName = item.GetType().FullName.Split('.').Last();
            using (var connection = new SqlConnection(_connectionSTring))
            {
                await connection.OpenAsync();
                var result = connection.ExecuteScalar("[dbo].[usp_Add" + objName + "]"
                    ,GetParameters(item, QueryType.Add)
                    ,commandType: System.Data.CommandType.StoredProcedure);
                return (int)result;
            }
        }

        public async Task<int> UpdateItem<T>(T item)
        {
            var objName = item.GetType().FullName.Split('.').Last();
            using (var connection = new SqlConnection(_connectionSTring))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteScalarAsync("[dbo].[usp_Update" + objName + "]"
                    ,GetParameters(item, QueryType.Update)
                    ,commandType: System.Data.CommandType.StoredProcedure);
                return (int)result;
            }
        }

        public async Task<int> DeleteItem<T>(T item)
        {
            var objName = item.GetType().FullName.Split('.').Last();
            using (var connection = new SqlConnection(_connectionSTring))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteScalarAsync("[dbo].[usp_Delete" + objName + "]"
                    ,GetParameters(item, QueryType.Delete)
                    ,commandType: System.Data.CommandType.StoredProcedure);
                return (int)result;
            }
        }

        private SqlParameter[] GetParameters(object item, QueryType queryType = QueryType.Update)
        {
            if(item is Platform)
            {
                var objItem = item as Platform;
                switch(queryType)
                {
                    case QueryType.Add:
                        return new SqlParameter[] {
                            new SqlParameter("@Name", objItem.Name)
                        };
                    case QueryType.Update:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id),
                            new SqlParameter("@Name", objItem.Name)
                        };
                    case QueryType.Delete:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id)
                        };
                }
            }
            else if (item is KnowledgeArea)
            {
                var objItem = item as KnowledgeArea;
                switch (queryType)
                {
                    case QueryType.Add:
                        return new SqlParameter[] {
                            new SqlParameter("@PlatformId", objItem.PlatformId),
                            new SqlParameter("@Name", objItem.Name)
                        };                      
                    case QueryType.Update:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id),
                            new SqlParameter("@Name", objItem.Name)
                        };
                    case QueryType.Delete:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id)
                        };
                }
            }
            if (item is Area)
            {
                var objItem = item as Area;
                switch (queryType)
                {
                    case QueryType.Add:
                        return new SqlParameter[] {
                            new SqlParameter("@KnowledgeAreaId", objItem.KnowledgeAreaId),
                            new SqlParameter("@Name", objItem.Name)
                        };                        
                    case QueryType.Update:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id),
                            new SqlParameter("@Name", objItem.Name)
                        };
                    case QueryType.Delete:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id)
                        };
                }
            }
            if (item is Question)
            {
                var objItem = item as Question;
                switch (queryType)
                {
                    case QueryType.Add:
                        return new SqlParameter[] {
                            new SqlParameter("@AreaId", objItem.AreaId),
                            new SqlParameter("@Value", objItem.Value),
                            new SqlParameter("@Weight", objItem.Weight),
                            new SqlParameter("@Level", objItem.Level)
                        };                        
                    case QueryType.Update:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id),
                            new SqlParameter("@Value", objItem.Value),
                            new SqlParameter("@Weight", objItem.Weight),
                            new SqlParameter("@Level", objItem.Level)
                        };                        
                    case QueryType.Delete:
                        return new SqlParameter[] {
                            new SqlParameter("@Id", objItem.Id)
                        };
                }
            }

            return null;
        }
    }
}
