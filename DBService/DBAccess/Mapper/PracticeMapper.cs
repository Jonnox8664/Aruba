﻿// DB CRUD for Practice

using DBAccess.DataModel;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
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
                            Id = (long)rdr["Id"],
                            UserId = (long)rdr["UserId"],
                            State = (int)rdr["State"],
                            Attachment = (string)rdr["Attachment"]
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

        // Retrieve userId from DB by practiceId
        public long RetrieveUserId(long practiceId)
        {
            try
            {
                long userId = -1;

                using (connection)
                {
                    var query = @"
                                SELECT u.Id AS PracticeId 
                                FROM [dbo].[User] u
                                INNER JOIN [dbo].[Practice] p
	                                ON p.UserId=u.Id
                                WHERE p.Id = @PracticeId
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@PracticeId", SqlDbType.VarChar).Value = practiceId;
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        userId = (long)rdr["PracticeId"];
                    }

                    connection.Close();
                }
                return userId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Retrieve practice, user and history data from DB by practiceId
        public JObject RetrieveFullData(long practiceId)
        {
            try
            {
                var fullData = new JObject();
                PracticeFullData practice = new PracticeFullData();

                using (connection)
                {
                    var query = @"
                                    SELECT 
	                                    P.Id AS PracticeId,
	                                    P.State AS State,
	                                    P.Attachment,
	                                    U.Id AS UserId,
	                                    U.Surname,
	                                    U.Name,
	                                    U.FiscalCode,
	                                    U.Birthday
                                    FROM [dbo].[Practice] AS P
                                    INNER JOIN [dbo].[user] as U
	                                    ON U.Id = P.UserId
                                    WHERE P.Id = @Id
                        ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = practiceId;
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        practice = new PracticeFullData()
                        {
                            PracticeId = (long)rdr["PracticeId"],
                            State = (int)rdr["State"],
                            Attachment = (string)rdr["Attachment"],
                            UserId = (long)rdr["UserId"],
                            Surname = (string)rdr["Surname"],
                            Name = (string)rdr["Name"],
                            FiscalCode = (string)rdr["FiscalCode"],
                            Birthday = (DateTime)rdr["Birthday"]
                        };
                    }
                    else
                        practice = new PracticeFullData() { PracticeId = -1 };

                    var practiceJObject = (JObject)JToken.FromObject(practice);

                    var hm = new HistoryMapper();
                    var historyJObject = JArray.FromObject(hm.Retrieve(practiceId));

                    fullData = new JObject()
                    {
                        new JProperty("PracticeData", practiceJObject),
                        new JProperty("History", historyJObject)
                    };

                    connection.Close();
                }
                return fullData;
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

        // Update practice attachment from DB by id
        public long UpdateAttachment(long id, string attachment)
        {
            try
            {
                var affectedRows = 0;
                using (connection)
                {
                    var query = @"
                                UPDATE [dbo].[Practice]
                                SET
                                    Attachment = @Attachment
                                WHERE Id = @Id
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.AddWithValue("@Attachment", SqlDbType.VarChar).Value = attachment;
                    affectedRows = cmd.ExecuteNonQuery();

                    connection.Close();
                }
                return (affectedRows <= 0) ? 0 : affectedRows;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Update practice state from DB by id
        public long UpdateState(long id, int state)
        {
            try
            {
                var affectedRows = 0;
                using (connection)
                {
                    var query = @"
                                UPDATE [dbo].[Practice]
                                SET
                                    State = @State
                                WHERE Id = @Id
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.AddWithValue("@State", SqlDbType.Int).Value = state;
                    affectedRows = cmd.ExecuteNonQuery();

                    connection.Close();

                    if (affectedRows > 0)
                    {
                        Practice practice = new Practice()
                        {
                            Id = id,
                            State = state
                        };

                        var hm = new HistoryMapper();
                        hm.Create(practice);
                    }
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
