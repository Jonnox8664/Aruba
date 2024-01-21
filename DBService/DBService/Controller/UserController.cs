// User CRUD controller

using Microsoft.AspNetCore.Mvc;
using DBAccess.Manager;
using DBAccess.Mapper;

namespace DBService.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        // Receives data for user insertion
        [HttpPost]
        public IActionResult Create([FromBody] string raw)
        {
            try
            {
                var um = new UserManager();
                var res = um.Create(raw);

                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Recovers data of all users
        [HttpGet]
        public IActionResult RetrieveAll()
        {
            try
            {
                var um = new UserManager();
                var res = um.RetrieveAll();

                return StatusCode(200, res.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Recovers data of a user using the received id
        [HttpGet, Route("{id}")]
        public IActionResult Retrieve(long id)
        {
            try
            {
                if (id < 0)
                    return StatusCode(400);

                var um = new UserManager();
                var res = um.Retrieve(id);

                return StatusCode(200, res.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Receives data for user update and updates using the received id
        [HttpPut, Route("{id}")]
        public IActionResult Update(long id, [FromBody] string raw)
        {
            try
            {
                if (id < 0)
                    return StatusCode(400);

                var um = new UserManager();
                var res = um.Update(id, raw);

                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Delete a user using the received id
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                if (id < 0)
                    return StatusCode(400);

                var um = new UserMapper();
                var res = um.Delete(id);

                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}