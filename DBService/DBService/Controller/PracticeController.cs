// Practice CRUD controller

using DBAccess.Manager;
using DBAccess.Mapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DBService.Controller
{
    [ApiController]
    [Route("practice")]
    public class PracticeController : ControllerBase
    {
        // Receives data for practice insertion
        [HttpPost]
        public IActionResult Create([FromBody] string raw)
        {
            try
            {
                var um = new PracticeManager();
                var res = um.Create(raw);

                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Recovers data of all practices
        [HttpGet]
        public IActionResult RetrieveAll()
        {
            try
            {
                var um = new PracticeManager();
                var res = um.RetrieveAll();

                return StatusCode(200, res.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Recovers data of a practice using the received id
        [HttpGet, Route("{id}")]
        public IActionResult Retrieve(long id)
        {
            try
            {
                if (id < 0)
                    return StatusCode(400);

                var um = new PracticeManager();
                var res = um.Retrieve(id);

                return StatusCode(200, res.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Recovers full data of a practice using the received practiceId
        [HttpGet, Route("fulldata/{practiceId}")]
        public IActionResult RetrieveFullData(long practiceId)
        {
            try
            {
                if (practiceId < 0)
                    return StatusCode(400);

                var um = new PracticeManager();
                var res = um.RetrieveFullData(practiceId);

                return StatusCode(200, res.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Receives data for practice update and updates using the received id
        [HttpPut, Route("{id}")]
        public IActionResult Update(long id, [FromBody] string raw)
        {
            try
            {
                if (id < 0)
                    return StatusCode(400);

                var um = new PracticeManager();
                var res = um.Update(id, raw);

                if (res < 0)
                    return StatusCode(400);

                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("{id}/{state}")]
        public IActionResult UpdateState(long id, int state)
        {

            try
            {
                if (id < 0 || state < 0 || state > 4)
                    return StatusCode(400);

                var um = new PracticeMapper();
                var res = um.UpdateState(id, state);

                if (res < 0)
                    return StatusCode(400);

                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Delete a practice using the received id
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                if (id < 0)
                    return StatusCode(400);

                var um = new PracticeMapper();
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