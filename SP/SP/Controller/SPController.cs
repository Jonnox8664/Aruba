// SP Controller

using Microsoft.AspNetCore.Mvc;

namespace SP.Controller
{
    [ApiController]
    [Route("sp")]
    public class SPController : ControllerBase
    {
        [HttpPost("api1")]
        public async Task<IActionResult> PracticeCreation([FromForm] FileRequestLayout layout)
        {
            try
            {
                if(layout.raw == null || layout.file == null)
                    return StatusCode(400, "Ivalid data");

                if(_isNotPDFExtension(layout.file.FileName))
                    return StatusCode(400, "Ivalid file, Oly PDF are allowed");

                var bl = new BusinessLogic.BusinessLogic();
                long practiceId = bl.API1PracticeCreation(layout.raw, layout.file.FileName).Result;

                if (practiceId > 0 && layout.file.Length > 0)
                {
                    string? filePath = bl.API1UploadPracticeAttachment(practiceId, layout.file.FileName);

                    if (!string.IsNullOrEmpty(filePath))
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await layout.file.CopyToAsync(fileStream);
                        return StatusCode(200, practiceId);
                    }
                }

                return StatusCode(400);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // API 1
        [HttpPost, Route("api1/data")]
        public IActionResult PracticeCreationData([FromBody] string raw)
        {
            try
            {
                var bl = new BusinessLogic.BusinessLogic();
                var result = bl.API1PracticeCreation(raw, "");
                if (result.Result > 0)
                    return StatusCode(200, result.Result);  
                else
                    return StatusCode(400, result.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // API 1
        [HttpPost]
        [Route("ap1/file/{practiceId}")]
        public async Task<IActionResult> PracticeCreationFile(long practiceId, IFormFile file)
        {
            try
            {
                if (practiceId > 0 && file.Length > 0)
                {
                    if (_isNotPDFExtension(file.FileName))
                        return StatusCode(400, "Ivalid file, Oly PDF are allowed");

                    var bl = new BusinessLogic.BusinessLogic();
                    if(bl.API1UpdatePracticeAttachmentName1(practiceId).Result <= 0)
                        return StatusCode(400);

                    string? filePath = bl.API1UploadPracticeAttachment(practiceId, file.FileName);

                    if (!string.IsNullOrEmpty(filePath))
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);

                            if (bl.API1UpdatePracticeAttachmentName2(practiceId, file.FileName).Result > 0)
                                return StatusCode(200, "File uploaded");
                        }
                }
                return StatusCode(400);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // API 2
        [HttpPut, Route("api2/{practiceId}")]
        public async Task<IActionResult> API2PracticeUpdate(long practiceId, [FromForm] FileRequestLayout layout)
        {
            try
            {
                // Update the case data

                if (layout.raw == null || layout.file == null)
                    return StatusCode(400, "Ivalid data");

                if (_isNotPDFExtension(layout.file.FileName))
                    return StatusCode(400, "Ivalid file, Oly PDF are allowed");

                var bl = new BusinessLogic.BusinessLogic();
                var userId = bl.API2PracticeUpdate1(practiceId).Result;
                if (userId <= 0)
                    return StatusCode(400);
                var result = bl.API2PracticeUpdate2(userId, layout.raw);
                if (result.Result > 0 && layout.file.Length > 0)
                {
                    string? filePath = bl.API1UploadPracticeAttachment(practiceId, layout.file.FileName);

                    if (!string.IsNullOrEmpty(filePath))
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await layout.file.CopyToAsync(fileStream);
                            if (bl.API1UpdatePracticeAttachmentName2(practiceId, layout.file.FileName).Result > 0)
                                return StatusCode(200, practiceId);
                        }
                    return StatusCode(200, result.Result);
                }
                else
                    return StatusCode(400, result.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // API 2
        [HttpPut, Route("api2/data/{userId}")]
        public IActionResult API2PracticeDataUpdate(long userId, [FromBody] string raw)
        {
            try
            {
                // Update the case data
                if (userId <= 0)
                    return StatusCode(400);

                var bl = new BusinessLogic.BusinessLogic();
                var result = bl.API2PracticeUpdate2(userId, raw);
                if (result.Result > 0)
                    return StatusCode(200, result.Result);
                else
                    return StatusCode(400, result.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // API 3
        [HttpGet, Route("api3/{practiceId}")]
        public IActionResult API3RetrieveFullPracticeData(long practiceId)
        {
            try
            {
                // Retrieve practice data, its user and history
                if(practiceId <= 0)
                    return StatusCode(400);

                var bl = new BusinessLogic.BusinessLogic();
                var result = bl.API3RetrieveFullPracticeData(practiceId);
                if (result != null)
                    return StatusCode(200, result.Result.ToString());
                else
                    return StatusCode(400);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // API 4
        [HttpGet]
        [Route("ap4/{practiceId}")]
        public IActionResult API4GetPracticeAttachment(long practiceId)
        {
            try
            {
                // Retrieve the practice attachment from the filesystem
                if (practiceId > 0)
                {
                    var bl = new BusinessLogic.BusinessLogic();
                    var res = bl.API4GetPracticeAttachment(practiceId);
                    if (res.fileBytes != null && res.fileBytes != new byte[] { } && !string.IsNullOrEmpty(res.fileName))
                    {
                        FileContentResult file = File(res.fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, res.fileName);
                        return StatusCode(200, file);
                    }
                }
                return StatusCode(400);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // Support API 1
        [HttpPut]
        [Route("support1/{practiceId}/{act}")]
        public IActionResult Support1UpdatePracticeState(long practiceId, int act)
        {
            try
            {
                if (practiceId > 0 && act >= 0 && act < 5)
                {
                    var bl = new BusinessLogic.BusinessLogic();
                    long newState = bl.Support1UpdatePracticeState(practiceId, act).Result;
                    return StatusCode(200, newState);
                }
                return StatusCode(400);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private bool _isNotPDFExtension(string FileName)
        {
            var extension = System.IO.Path.GetExtension(FileName);
            return string.IsNullOrEmpty(extension) || extension.ToLower().CompareTo(".pdf") != 0;
        }
    }
}