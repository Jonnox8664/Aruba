// History manager

using DBAccess.Mapper;
using Newtonsoft.Json.Linq;

namespace DBAccess.Manager
{
    public class HistoryManager
    {
        // Check and parse data for history retrieve
        public JArray Retrieve(long practiceId)
        {
            try
            {
                var hm = new HistoryMapper();
                return JArray.FromObject(hm.Retrieve(practiceId));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}