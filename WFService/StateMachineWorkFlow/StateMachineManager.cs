// State machine manager

using Newtonsoft.Json.Linq;
using static StateMachineWorkFlow.StateMachine;

namespace StateMachineWorkFlow
{
    public interface IStateMachineManager<StateMachineManager>
    {
        public JObject StateMachineHandler(int currentState, int action);
    }

    public class StateMachineManager
    {
        // Check and return data for next state machine allowed state
        public JObject StateMachineHandler(int currentState, int action)
        {
            try
            {
                var receivedState = (State) currentState;
                var receivedAction = (StateMachine.Action) action;

                StateMachine sm = new StateMachine();
                sm.CurrentState = receivedState;

                System.Diagnostics.Debug.WriteLine($"State: {sm.CurrentState}");
                System.Diagnostics.Debug.WriteLine($"Action: {receivedAction}");
                sm.TakeAction(receivedAction);
                System.Diagnostics.Debug.WriteLine($"State: {sm.CurrentState}");

                var result = new JObject()
                {
                    { "ReceivedState", receivedState.ToString() },
                    { "ReceivedStateCode", (int) receivedState },
                    { "ReceivedAction", receivedAction.ToString() },
                    { "ReceivedActionCode", (int) receivedAction },
                    { "CurrentState", sm.CurrentState.ToString() },
                    { "CurrentStateCode", (int) sm.CurrentState }
                };

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}