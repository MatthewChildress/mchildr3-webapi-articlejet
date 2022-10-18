using mchildr3_webapi_articlejet.Model;
using MySqlConnector;
using System.Data;

namespace mchildr3_webapi_articlejet.Data
{
    internal class DataLayer
    {

        #region "Properties"

        /// <summary>
        /// This variable holds the connection details
        /// such as name of database server, database name, username, and password.
        /// The ? makes the property nullable
        /// </summary>
        private string? connectionString = null;

        #endregion

        #region "Constructors"

        /// <summary>
        /// This is the default constructor and has the default 
        /// connection string specified 
        /// </summary>
        public DataLayer()
        {
            //preprocessor directives can help by using a debug build database environment for testing
            // while using a production database environment for production build.
#if (DEBUG)
            connectionString = @"server=localhost;uid=citc1317;pwd=Password1;database=neArticleJet";
#else
            connectionString = @"server=192.168.79.131;uid=citc1317;pwd=Password1;database=neArticleJet";
#endif
        }

        /// <summary>
        /// Parameterized Constructor passing in a connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public DataLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region "User Database Operations"

        /// <summary>
        /// Get a user by using the user GUID (key)
        /// returns a single User object or a null User
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<User>? GetAUserByGUIDAsync(string? GUID)
        {

            User? user = null;

            try
            {

                //test for guid to be null and throw and exception back to the caller
                if (GUID == null)
                {
                    throw new ArgumentNullException("GUID can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAUserByGUID", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("GUID", GUID));

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // if the reader contains a data set, load a local user object
                if (rdr.Read())
                {
                    user = new User();
                    user.Guid = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);
                    user.FirstName = (string?)rdr.GetValue(2);
                    user.LastName = (string?)rdr.GetValue(3);
                    UInt64 test = (UInt64)rdr.GetValue(4);
                    if (test == 0)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                    user.LevelID = (int)rdr.GetValue(5);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return user;

        } // end GetAUserByGUID

        /// <summary>
        /// Get a user by Username and Password (key)
        /// returns a single User object or a null User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<User>? GetAUserByUserPassAsync(string? username, string? password)
        {

            User? user = null; // new syntax can replace User? user = new User()

            try
            {
                if (username == null || password == null)
                {
                    throw new ArgumentNullException("Username or Password can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAUserByUserAndPass", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("userEmail", username));
                cmd.Parameters.Add(new MySqlParameter("password", password));

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // if the reader contains a data set, load a local user object
                if (rdr.Read())
                {
                    user = new User();
                    user.Guid = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);

                    user.FirstName = (string?)rdr.GetValue(2);
                    user.LastName = (string?)rdr.GetValue(3);
                    UInt64 test = (UInt64)rdr.GetValue(4);
                    if (test == 1)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                    user.LevelID = (int)rdr.GetValue(5);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return user;

        } // end GetAUserByUserPassAsync

        /// <summary>
        /// Get all the active users in the database and return as a List of Users.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>>? GetAllUsersAsync()
        {

            // instantiate a new empty List of Users
            List<User> users = new List<User>();

            try
            {
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllUsers", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results adding a new user to a generic List of users
                while (rdr.Read())
                {
                    User user = new User();
                    user.Guid = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);
                    user.Password = (string?)rdr.GetValue(2);
                    user.FirstName = (string?)rdr.GetValue(3);
                    user.LastName = (string?)rdr.GetValue(4);
                    UInt64 test = (UInt64)rdr.GetValue(5);
                    if (test == 1)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                    user.LevelID = (int)rdr.GetValue(6);

                    users.Add(user);
                }
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return users;

        } // end GetAllUsersAsync

        /// <summary>
        /// Update a User by their GUID key to an active state of 0 or 1.
        /// Return 0 if no row in the database is modified.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> PutAUsersStateAsync(string? guid, bool? state)
        {
            // integer value that shows if a row is updated in the database
            int results = 0;

            try
            {
                // check for null parameters being passed in
                if (guid == null || state == null)
                {
                    throw new ArgumentNullException("GUID and State can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPutUserActiveState", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("GUID", guid));
                cmd.Parameters.Add(new MySqlParameter("state", state));

                // execute the none query command that returns an integer for number of rows changed
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PutAUsersStateAsync

        /// <summary>
        /// Insert a new User into the database.
        /// Return 0 if no row is modified.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PostANewUserAsync(User? user)
        {
            // local variable to return the row count altered
            int results = 0;

            try
            {
                // check for User to be null
                if (user == null)
                {
                    throw new ArgumentNullException("New user can not be null.");
                }

                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPostNewJetUser", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("GUID", user.Guid));
                cmd.Parameters.Add(new MySqlParameter("mail", user.Email));
                cmd.Parameters.Add(new MySqlParameter("pass", user.Password));
                cmd.Parameters.Add(new MySqlParameter("fname", user.FirstName));
                cmd.Parameters.Add(new MySqlParameter("lname", user.LastName));
                cmd.Parameters.Add(new MySqlParameter("isActive", user.IsActive));
                cmd.Parameters.Add(new MySqlParameter("userLevel", user.LevelID));

                // execute the command
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PostANewUserAsync

        #endregion
    }
}
