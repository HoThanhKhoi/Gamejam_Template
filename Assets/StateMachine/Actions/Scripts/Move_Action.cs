using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Actions/Platformer_Move_Horizontal", fileName = "Platformer_Move_Horizontal")]
    public class Move_Action : ScriptableAction
    {
        public override void Act(StateComponent statesComponent)
        {
            statesComponent.GetCachedInterface<IMoveable>().Move();
            statesComponent.GetCachedInterface<IFacing>().HandleFacing();
            AudioManager.Instance.PlayMovementSound();
        }
    }
}
