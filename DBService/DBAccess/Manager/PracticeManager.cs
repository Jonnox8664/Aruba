// Practice manager

using DBAccess.DataModel;
using DBAccess.Mapper;
using Newtonsoft.Json.Linq;

namespace DBAccess.Manager
{
    public class PracticeManager
    {
        // Check practice data consistency
        private bool _mandatoryDataMissing(Practice practice)
        {
            try
            {
                if (practice.Id < 0)
                    return true;
                if (practice.UserId < 0)
                    return true;
                if (practice.State < 0)
                    return true;
                if (string.IsNullOrEmpty(practice.Attachment))
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Convet raw json to JObject json
        private Practice? _practiceObjConverter(string raw)
        {
            try
            {
                JObject jPractice = JObject.Parse(raw);

                return jPractice.ToObject<Practice>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for practice creation
        public long Create(string raw)
        {
            try
            {
                var tmpPractice = _practiceObjConverter(raw);
                if (tmpPractice == null)
                    return -1;

                var practice = (Practice) tmpPractice;
                if (_mandatoryDataMissing(practice))
                    return -1;

                var um = new PracticeMapper();
                return um.Create(practice);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for practices retrieve
        public JArray RetrieveAll()
        {
            try
            {
                var um = new PracticeMapper();
                return JArray.FromObject(um.RetrieveAll());
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for practice retrieve
        public JObject Retrieve(long id)
        {
            try
            {
                var um = new PracticeMapper();
                var practice = um.Retrieve(id);
                return (JObject)JToken.FromObject(practice);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Check and parse data for practice update
        public long Update(long id, string raw)
        {
            try
            {
                var tmpPractice = _practiceObjConverter(raw);
                if (tmpPractice == null)
                    return -1;

                var practice = (Practice)tmpPractice;
                if (_mandatoryDataMissing(practice))
                    return -1;

                var um = new PracticeMapper();
                return um.Update(id, practice);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}