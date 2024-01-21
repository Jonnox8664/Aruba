// Implementation of SP service logic

using HTTPClient;
using Newtonsoft.Json.Linq;

namespace BusinessLogic
{
    public class BusinessLogic
    {
        public const string basePracticeDirectory = @".\PracticeAttachments";
        private const string _wfsBaseURI = @"http://localhost:5075/";
        private const string _dbsBaseURI = @"http://localhost:5074/";

        // API1
        public async Task<long> API1PracticeCreation(string rawData)
        {
            try
            {
                // It checks whether the action is valid for the current state
                var url1 = @$"{_wfsBaseURI}state/0/action/0";
                var stateCheckResult = await BaseClient.JObjectHTTPGet(url1);
                var state = RequestResponce.ResponceCheck(stateCheckResult, "CurrentStateCode");
                if (state <= 0)
                    return state;

                // It stores user data in the DB
                var url2 = @$"{_dbsBaseURI}user";
                var userCreationResult = await BaseClient.StringHTTPPost(url2, rawData);
                var userId = RequestResponce.ResponceCheck(userCreationResult);
                if (userId <= 0)
                    return userId;

                // It stores practice data in the DB
                var url3 = @$"{_dbsBaseURI}practice";
                var rawPracticeData = $"{{UserId: {userId}, Attachment: ''}}";
                var practiceCreationResult = await BaseClient.StringHTTPPost(url3, rawPracticeData);
                var practiceId = RequestResponce.ResponceCheck(practiceCreationResult);
                if (practiceId <= 0)
                {
                    // In case of failure, it deletes the data of the inserted user from the DB
                    await BaseClient.StringHTTDelete(@$"{_dbsBaseURI}user/{userId}");
                    return -5;
                }

                return practiceId;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // API1
        public string? API1UploadPracticeAttachment(long practiceId, string filename)
        {
            try
            {
                // Store the practice attachment on the filesystem
                if (string.IsNullOrEmpty(filename))
                {
                    string practiceDirectory = @$"{basePracticeDirectory}\{practiceId}";
                    bool exists = System.IO.Directory.Exists(practiceDirectory);
                    if (!exists)
                        System.IO.Directory.CreateDirectory(practiceDirectory);
                    else
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo(practiceDirectory);
                        foreach (FileInfo targetFile in di.GetFiles())
                            targetFile.Delete();
                    }

                    return Path.Combine(practiceDirectory, filename);
                }
                return null;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // API2
        public async Task<long> API2PracticeUpdate(long practiceId, string rawData)
        {
            try
            {
                // Check if current practice stae is "New"
                var url1 = @$"{_dbsBaseURI}practice/{practiceId}";
                var practiceData = await BaseClient.JObjectHTTPGet(url1);
                var state = RequestResponce.ResponceCheck(practiceData, "State");
                if (state > 1)
                    return -7;

                // Updates the practice data
                var url2 = @$"{_dbsBaseURI}user/{practiceId}";
                var userCreationResult = await BaseClient.StringHTTPPut(url2, rawData);
                var affectedRows = RequestResponce.ResponceCheck(userCreationResult);
                if (affectedRows <= 0)
                    return affectedRows;

                return 1;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // API3
        public async Task<JObject> API3RetrieveFullPracticeData(long practiceId)
        {
            try
            {
                // Retrieve practice data, its user and history
                var url1 = @$"{_dbsBaseURI}practice/fulldata/{practiceId}";
                var practiceData = await BaseClient.JObjectHTTPGet(url1);

                if(practiceData.JObjectBody != null)
                    return practiceData.JObjectBody;
                else
                    return new JObject();
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // API4
        public (byte[] fileBytes, string fileName) API4GetPracticeAttachment(long practiceId)
        {
            try
            {
                // Retrieve the practice attachment data
                string practiceDirectory = @$"{basePracticeDirectory}\{practiceId}";
                bool exists = System.IO.Directory.Exists(practiceDirectory);
                if (!exists)
                    return (new byte[] { }, "");
                else
                {
                    var files = Directory.GetFiles(practiceDirectory);
                    string fileName = new FileInfo(files[0]).Name;
                    string fileExtension = new FileInfo(files[0]).Extension;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(@$"{practiceDirectory}\{fileName}");

                    return (fileBytes, fileName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Support 1
        public async Task<long> Support1UpdatePracticeState(long practiceId, int action)
        {

            // Check if current practice stae is "New"
            var url1 = @$"{_dbsBaseURI}practice/{practiceId}";
            var practiceData = await BaseClient.JObjectHTTPGet(url1);
            var currentState = RequestResponce.ResponceCheck(practiceData, "State");
            if (currentState < 0 || currentState > 4)
                return -8;

            // It checks whether the action is valid for the current state
            var url2 = @$"{_wfsBaseURI}state/{currentState}/action/{action}";
            var stateCheckResult = await BaseClient.JObjectHTTPGet(url2);
            var newState = RequestResponce.ResponceCheck(stateCheckResult, "CurrentStateCode");
            if (newState < 0)
                return -9;

            // Updates the practice data
            var url3 = @$"{_dbsBaseURI}practice/{practiceId}/{newState}";
            var userCreationResult = await BaseClient.StringHTTPPut(url3, "");
            var affectedRows = RequestResponce.ResponceCheck(userCreationResult);
            if (affectedRows <= 0)
                return affectedRows;

            return newState;
        }
    }
}