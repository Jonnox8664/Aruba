// History CRUD controller

using DBAccess.Manager;
using Microsoft.AspNetCore.Mvc;

namespace DBService.Controller
{
    // Recovers history of a specific practice using the received practiceId
    [ApiController]
    [Route("history")]
    public class HistoryController : ControllerBase
    {
        [HttpGet, Route("{practiceId}")]
        public IActionResult Retrieve(long practiceId)
        {
            try
            {
                if (practiceId < 0)
                    return StatusCode(400);

                var um = new HistoryManager();
                var res = um.Retrieve(practiceId);

                return StatusCode(200, res.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}