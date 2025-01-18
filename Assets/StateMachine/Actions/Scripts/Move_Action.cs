using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Actions/Platformer_Move_Horizontal", fileName = "Platformer_Move_Horizontal")]
    public class Move_Action : ScriptableAction
    {
        private IMoveable moveable;
        private IFacing horizontalFacing;
        public override void Act(StateComponent statesComponent)
        {
            if(moveable == null)
            {
                moveable = statesComponent.GetComponent<IMoveable>();
            }
            
            if(horizontalFacing == null)
            {
                horizontalFacing = statesComponent.GetComponent<IFacing>();
            } 
            
            moveable.Move();
            horizontalFacing.HandleFacing();
        }
    }
}
