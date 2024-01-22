// State machine engine
/*
 * Permitted actions
 * 
 *  Start       ->      Create      ->      Created      (Started)
 *  Created     ->      Update      ->      Created
 *  Created     ->      Approve     ->      Approved     (Closed)
 *  Created     ->      Refuse      ->      Refused      (Closed)
 *  
*/

namespace StateMachineWorkFlow
{
    public class StateMachine
    {
        // Work flow states
        public enum State
        {
            Start = 0,
            Created = 1,
            Approved = 2,
            Refused = 3,
            Error = -1
        }

        // Work flow actions
        public enum Action
        {
            Create = 0,
            Update = 1,
            Approve = 2,
            Refuse = 3,
        }

        // Current state
        private State _state = State.Start;

        public State CurrentState
        {
            set { _state = value; }
            get { return _state; }
        }

        // Work flow engine
        public void TakeAction(Action action)
        {
            try
            {
                _state = (_state, action) switch
                {
                    (State.Start, Action.Create) => State.Created,
                    (State.Created, Action.Update) => State.Created,
                    (State.Created, Action.Approve) => State.Approved,
                    (State.Created, Action.Refuse) => State.Refused,
                    _ => State.Error
                };

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}