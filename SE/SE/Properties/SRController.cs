// SE Controller

using Microsoft.AspNetCore.Mvc;

namespace SE.Properties
{
    [ApiController]
    [Route("se")]
    public class SRController : ControllerBase
    {
        // Support API 2
        [HttpPost, Route("support2")]
        public IActionResult CallbackReceiver([FromBody] string raw)
        {
            try
            {
                // Display callback content in console 
                System.Diagnostics.Debug.WriteLine(raw);
                return StatusCode(200, "Callback received");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}