// State machine engine
/*
 * Permitted actions
 * 
 *  Start       -> Create   -> New          (Started)
 *  New         -> Update   -> InProgress
 *  InProgress  -> Update   -> InProgress
 *  InProgress  -> Approve   -> Approved    (Closed)
 *  InProgress  -> Refuse   -> Refused      (Closed)
*/

namespace StateMachineWorkFlow
{
    public class StateMachine
    {
        // Work flow states
        public enum State
        {
            Start = 0,
            New = 1,
            InProgress = 2,
            Approved = 3,
            Refused = 4,
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
                    (State.Start, Action.Create) => State.New,
                    (State.New, Action.Update) => State.InProgress,
                    (State.InProgress, Action.Update) => State.InProgress,
                    (State.InProgress, Action.Approve) => State.Approved,
                    (State.InProgress, Action.Refuse) => State.Refused,
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