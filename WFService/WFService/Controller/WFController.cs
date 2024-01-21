// State machine CRUD controller

using Microsoft.AspNetCore.Mvc;
using StateMachineWorkFlow;
using static StateMachineWorkFlow.StateMachine;

namespace WFService.Controller
{
    [Serializable]
    [ApiController]
    public class WFController : ControllerBase
    {
        // Recovers data of next allowed state using the received currentState and action
        [HttpGet, Route("state/{currentState}/action/{act}")]
        public IActionResult Action(int currentState, int act)
        {
            try
            {
                var stateCount = Enum.GetNames(typeof(State)).Length;
                var actionCount = Enum.GetNames(typeof(StateMachine.Action)).Length;

                if (
                        currentState < 0 || currentState > stateCount ||
                        act < 0 || act > actionCount
                    )
                    return StatusCode(400);

                var smm = new StateMachineManager();
                var result = smm.StateMachineHandler(currentState, act);

                return StatusCode(200, result.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}