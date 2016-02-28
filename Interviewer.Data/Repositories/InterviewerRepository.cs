using System.Linq;
using System.Threading.Tasks;
using Interviewer.Common;
using System.Data.SqlClient;
using Dapper;
using System.Xml.Serialization;
using System.IO;

namespace Interviewer.Data.Repositories
{
    public class InterviewerRepository : IRepository, IDbConnection
    {
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

                //var config = await connection.QueryAsync<configuration>(
                //    "[dbo].[usp_GetInterviewerData]", new
                //    {
                //        Platform = platform,
                //        KnowledgeArea = knowledgeArea,
                //        Area = area,
                //        Question = question,
                //        Profile = profile
                //    }, commandType: System.Data.CommandType.StoredProcedure);
                //_configuration = config.SingleOrDefault();
            }

            return _configuration;
        }
    }
}
