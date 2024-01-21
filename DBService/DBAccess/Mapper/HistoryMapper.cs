// DB CRUD for History

using DBAccess.DataModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DBAccess.Mapper
{
    public class HistoryMapper : BaseMapper
    {
        // Create the history row in the DB
        public long Create(Practice practice)
        {
            try
            {
                long resultId = -1;
                using (connection)
                {
                    var query = @"
                                INSERT INTO [dbo].[History]
                                (
                                    PracticeId,
                                    State,
                                    Timestamp
                                )
                                OUTPUT INSERTED.Id
                                VALUES
                                (
                                    @PracticeId,
                                    @State,
                                    GETDATE()
                                );
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@PracticeId", SqlDbType.BigInt).Value = practice.Id;
                    cmd.Parameters.AddWithValue("@State", SqlDbType.Int).Value = practice.State;
                    var res = cmd.ExecuteScalar();
                    if (res != null)
                        resultId = (long)res;

                    connection.Close();
                }

                return (long)resultId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Retrieves practice history data from DB by practiceId
        public List<History> Retrieve(long practiceId)
        {
            try
            {
                var HistoryList = new List<History>();

                using (connection)
                {
                    var query = @"
                                SELECT
                                    Id,
                                    PracticeId,
                                    State,
                                    Timestamp
                                FROM [dbo].[History]
                                Where PracticeId = @PracticeId
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@PracticeId", SqlDbType.BigInt).Value = practiceId;
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var history = new History()
                        {
                            Id = (long)rdr["Id"],
                            PracticeId = (long)rdr["PracticeId"],
                            State = (int) rdr["State"],
                            Timestamp = (DateTime) rdr["Timestamp"]
                        };
                        HistoryList.Add(history);
                    }

                    connection.Close();
                }

                return HistoryList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}