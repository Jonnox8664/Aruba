// SP Controller

using Microsoft.AspNetCore.Mvc;

namespace SP.Controller
{
    [ApiController]
    [Route("sp")]
    public class SPController : ControllerBase
    {
        // API 1
        [HttpPost, Route("api1")]
        public IActionResult CreatePractice([FromBody] string raw)
        {
            try
            {
                var bl = new BusinessLogic.BusinessLogic();
                var result = bl.API1PracticeCreation(raw);
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
        public async Task<IActionResult> CreateFileAsync(long practiceId, IFormFile file)
        {
            try
            {
                if (practiceId > 0 && file.Length > 0)
                {
                    var bl = new BusinessLogic.BusinessLogic();
                    string? filePath = bl.API1UploadPracticeAttachment(practiceId, file.FileName);

                    if (!string.IsNullOrEmpty(filePath))
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
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
        public IActionResult API2PracticeUpdate(long practiceId, [FromBody] string raw)
        {
            try
            {
                // Update the case data
                if (practiceId <= 0)
                    return StatusCode(400);

                var bl = new BusinessLogic.BusinessLogic();
                var result = bl.API2PracticeUpdate(practiceId, raw);
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

    }
}