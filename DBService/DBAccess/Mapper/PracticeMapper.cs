// DB CRUD for Practice

using DBAccess.DataModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DBAccess.Mapper
{
    public class PracticeMapper : BaseMapper
    {
        // Create the practice in the DB
        public long Create(Practice practice)
        {
            try
            {
                long resultId = -1;
                using (connection)
                {
                    var query = @"
                                INSERT INTO [dbo].[Practice]
                                (
                                    UserId,
                                    State,
                                    Attachment
                                )
                                OUTPUT INSERTED.Id
                                VALUES
                                (
                                    @UserId,
                                    @State,
                                    @Attachment
                                );
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.BigInt).Value = practice.UserId;
                    cmd.Parameters.AddWithValue("@State", SqlDbType.Int).Value = practice.State;
                    cmd.Parameters.AddWithValue("@Attachment", SqlDbType.VarChar).Value = practice.Attachment;
                    var res = cmd.ExecuteScalar();
                    if (res != null)
                        resultId = (long)res;

                    connection.Close();

                    practice.Id = resultId;
                    var hm = new HistoryMapper();
                    hm.Create(practice);
                }

                return (long)resultId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Retrieve all practices data from DB
        public List<Practice> RetrieveAll()
        {
            try
            {
                var PracticeList = new List<Practice>();

                using (connection)
                {
                    var query = @"
                                SELECT
                                    Id,
                                    UserId,
                                    State,
                                    Attachment
                                FROM [dbo].[Practice]
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var practice = new Practice()
                        {
                            Id = (long) rdr["Id"],
                            UserId = (long) rdr["UserId"],
                            State = (int) rdr["State"],
                            Attachment = (string) rdr["Attachment"]
                        };
                        PracticeList.Add(practice);
                    }

                    connection.Close();
                }

                return PracticeList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Retrieve practice data from DB by id
        public Practice Retrieve(long id)
        {
            try
            {
                Practice practice = new Practice();

                using (connection)
                {
                    var query = @"
                                SELECT
                                    Id,
                                    UserId,
                                    State,
                                    Attachment
                                FROM [dbo].[Practice]
                                WHERE Id = @Id
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = id;
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        practice = new Practice()
                        {
                            Id = (long)rdr["Id"],
                            UserId = (long)rdr["UserId"],
                            State = (int)rdr["State"],
                            Attachment = (string)rdr["Attachment"]
                        };
                    }
                    else
                        practice = new Practice() { Id = -1 };

                    connection.Close();
                }
                return practice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update practice data from DB by id
        public long Update(long id, Practice practice)
        {
            try
            {
                var affectedRows = 0;
                using (connection)
                {
                    var query = @"
                                UPDATE [dbo].[Practice]
                                SET
                                    State = @State,
                                    Attachment = @Attachment
                                WHERE Id = @Id
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.AddWithValue("@State", SqlDbType.Int).Value = practice.State;
                    cmd.Parameters.AddWithValue("@Attachment", SqlDbType.VarChar).Value = practice.Attachment;
                    affectedRows = cmd.ExecuteNonQuery();

                    connection.Close();

                    practice.Id = id;
                    var hm = new HistoryMapper();
                    hm.Create(practice);
                }
                return (affectedRows <= 0) ? 0 : affectedRows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Delete practice data from DB by id
        public int Delete(long id)
        {
            try
            {
                var affectedRows = 0;
                using (connection)
                {
                    var query = @"
                                DELETE
                                FROM [dbo].[Practice]
                                WHERE Id = @Id
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = id;
                    affectedRows = cmd.ExecuteNonQuery();

                    connection.Close();
                }

                return affectedRows;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
