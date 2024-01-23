// State machine engine
/*
 * Permitted actions
 * 
 *  Start           ->      Create      ->      Created      (Started)
 *  Created         ->      Update      ->      Created
 *  Created         ->      Handle      ->      InProgress
 *  InProgress      ->      Approve     ->      Approved     (Closed)
 *  InProgress      ->      Refuse      ->      Refused      (Closed)
 *  
*/

namespace StateMachineWorkFlow
{
    public interface IStateMachine
    {
    }
}