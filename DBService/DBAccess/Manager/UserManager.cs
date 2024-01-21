// User manager

using DBAccess.DataModel;
using Newtonsoft.Json.Linq;
using DBAccess.Mapper;

namespace DBAccess.Manager
{
    public class UserManager
    {
        // Check user data consistency
        private bool _mandatoryDataMissing(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Surname))
                    return true;
                if (string.IsNullOrEmpty(user.Name))
                    return true;
                if (string.IsNullOrEmpty(user.FiscalCode))
                    return true;
                if (user.Birthday == null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Convet raw json to JObject json
        private User? _userObjConverter(string raw)
        {
            try
            {
                JObject jUser = JObject.Parse(raw);

                return jUser.ToObject<User>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for user creation
        public long Create(string  raw)
        {
            try
            {
                var tmpUser = _userObjConverter(raw);
                if (tmpUser == null)
                    return -1;

                var user = (User) tmpUser;
                if(_mandatoryDataMissing(user))
                    return -1;

                var um = new UserMapper();
                return um.Create(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for users retrieve
        public JArray RetrieveAll()
        {
            try
            {
                var um = new UserMapper();
                return JArray.FromObject(um.RetrieveAll());
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for user retrieve
        public JObject Retrieve(long id)
        {
            try
            {
                var um = new UserMapper();
                var user = um.Retrieve(id);
                return (JObject)JToken.FromObject(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for user update
        public long Update(long id, string raw)
        {
            try
            {
                var tmpUser = _userObjConverter(raw);
                if (tmpUser == null)
                    return -1;

                var user = (User)tmpUser;
                if (_mandatoryDataMissing(user))
                    return -1;

                var um = new UserMapper();
                return um.Update(id, user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}