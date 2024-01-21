// DB CRUD for User

using DBAccess.DataModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DBAccess.Mapper
{
    public class UserMapper : BaseMapper
    {
        // Create the user in the DB
        public long Create(User user)
        {
            try
            {
                long resultId = -1;
                using (connection)
                {
                    var query = @"
                                IF NOT EXISTS (SELECT Id FROM [dbo].[User] WHERE FiscalCode = @FiscalCode)
                                BEGIN
                                    INSERT INTO [dbo].[User]
                                    (
                                        Surname,
                                        Name,
                                        FiscalCode,
                                        Birthday
                                    )
                                    OUTPUT INSERTED.Id
                                    VALUES
                                    (
                                        @Surname,
                                        @Name,
                                        @FiscalCode,
                                        @Birthday
                                    );
                                END
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Surname", SqlDbType.VarChar).Value = user.Surname;
                    cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = user.Name;
                    cmd.Parameters.AddWithValue("@FiscalCode", SqlDbType.VarChar).Value = user.FiscalCode;
                    cmd.Parameters.AddWithValue("@Birthday", SqlDbType.DateTime).Value = user.Birthday;
                    var res = cmd.ExecuteScalar();
                    if (res != null)
                        resultId =(long) res;

                    connection.Close();
                }

                return (long) resultId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Retrieve all users data from DB
        public List<User> RetrieveAll()
        {
            try
            {
                var UserList = new List<User>();

                using (connection)
                {
                    var query = @"
                                SELECT
                                    Id,
                                    Surname,
                                    Name,
                                    FiscalCode,
                                    Birthday
                                FROM [dbo].[User]
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var user = new User()
                        {
                            Id = (long) rdr["Id"],
                            Surname = (string) rdr["Surname"],
                            Name = (string) rdr["Name"],
                            FiscalCode = (string) rdr["FiscalCode"],
                            Birthday = (DateTime) rdr["Birthday"]
                        };
                        UserList.Add(user);
                    }

                    connection.Close();
                }

                return UserList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Retrieve user data from DB by id
        public User Retrieve(long id)
        {
            try
            {
                User user = new User();

                using (connection)
                {
                    var query = @"
                                SELECT
                                    Id,
                                    Surname,
                                    Name,
                                    FiscalCode,
                                    Birthday
                                FROM [dbo].[User]
                                WHERE Id = @Id
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = id;
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        user = new User()
                        {
                            Id = (long)rdr["Id"],
                            Surname = (string)rdr["Surname"],
                            Name = (string)rdr["Name"],
                            FiscalCode = (string)rdr["FiscalCode"],
                            Birthday = (DateTime)rdr["Birthday"]
                        };
                    }
                    else
                        user = new User() { Id = -1 };

                    connection.Close();
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update user data from DB by id
        public long Update(long id,User user)
        {
            try
            {
                var affectedRows = 0;
                using (connection)
                {
                    var query = @"

                                IF NOT EXISTS (SELECT ID FROM [dbo].[User] WHERE FiscalCode = @FiscalCode AND ID <> @Id)
                                BEGIN
                                    UPDATE [dbo].[User]
                                    SET
                                        Surname = @Surname,
                                        Name = @Name,
                                        FiscalCode = @FiscalCode,
                                        Birthday = @Birthday
                                    WHERE Id = @Id
                                END
                    ";
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.AddWithValue("@Surname", SqlDbType.VarChar).Value = user.Surname;
                    cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = user.Name;
                    cmd.Parameters.AddWithValue("@FiscalCode", SqlDbType.VarChar).Value = user.FiscalCode;
                    cmd.Parameters.AddWithValue("@Birthday", SqlDbType.DateTime).Value = user.Birthday;
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

        // Delete user data from DB by id
        public int Delete(long id)
        {
            try
            {
                var affectedRows = 0;
                using (connection)
                {
                    var query = @"
                                DELETE
                                FROM [dbo].[User]
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