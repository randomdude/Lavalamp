namespace lavalampData
{
    using System.Configuration;
    using System.Data;
    using System.Data.SQLite;

    public class userManagement : IUserManagement
    {

        public IUser getUser()
        {
            using (
                var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["PRIMARY_DB"].ConnectionString))
            {
                conn.Open();

            }
            return null;
        }

        public bool createOrUpdateUser(IUser user)
        {
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["PRIMARY_DB"].ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    if (user.Id.HasValue)
                    {
                        cmd.CommandText = "UPDATE users SET name=:name, password=:password,facialdescription:facialdescription WHERE id=:userId LIMIT 1";
                        var userId = cmd.CreateParameter();
                        userId.DbType = DbType.Int64;
                        userId.Direction = ParameterDirection.Input;
                        userId.IsNullable = false;
                        userId.ParameterName = "userId";
                        userId.Value = user.Id.Value;
                        cmd.Parameters.Add(userId);

                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO users(name, password) VALUES(:name, :password, :facialdescription)";
                    }
                    var name = cmd.CreateParameter();
                    name.DbType = DbType.String;
                    name.Direction = ParameterDirection.Input;
                    name.IsNullable = false;
                    name.ParameterName = "name";
                    name.Value = user.Name;
                    cmd.Parameters.Add(name);

                    var password = cmd.CreateParameter();
                    password.DbType = DbType.String;
                    password.Direction = ParameterDirection.Input;
                    password.IsNullable = false;
                    password.ParameterName = "password";
                    password.Value = user.hashedPassword();
                    cmd.Parameters.Add(password);
                    var facial = cmd.CreateParameter();
                    facial.DbType = DbType.Xml;
                    facial.Direction = ParameterDirection.Input;
                    facial.ParameterName = "facialdescription";
                    facial.Value = user.FacialDescription;
                    cmd.Parameters.Add(facial);
                    if (user.Id.HasValue)
                        cmd.ExecuteNonQuery();
                    else
                        user.Id =(long)cmd.ExecuteScalar();


                }
                conn.Close();
            }
            return true;
        }

        public IUser isUser(IUser user)
        {

            using (
                var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["PRIMARY_DB"].ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM users WHERE id=:userID AND password=:password LIMIT 1";
                    var userId = cmd.CreateParameter();
                    userId.DbType = DbType.Int64;
                    userId.Direction = ParameterDirection.Input;
                    userId.IsNullable = false;
                    userId.ParameterName = "userId";
                    userId.Value = user.Id.Value;
                    userId.DbType = DbType.String;
                    userId.Direction = ParameterDirection.Input;
                    userId.IsNullable = false;
                    userId.ParameterName = "password";
                    userId.Value = user.hashedPassword();
                    cmd.Parameters.Add(userId);
                    using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                    //    reader["facialdescription"];
                    }

                }
            }
            return null;
        }

        public void deleteUser(IUser user)
        {
            if (!user.Id.HasValue)
                return;
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["PRIMARY_DB"].ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM users WHERE id=:userId LIMIT 1";
                    var userId = cmd.CreateParameter();
                    userId.DbType = DbType.Int64;
                    userId.Direction = ParameterDirection.Input;
                    userId.IsNullable = false;
                    userId.ParameterName = "userId";
                    userId.Value = user.Id.Value;
                    cmd.Parameters.Add(userId);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

    }
}
