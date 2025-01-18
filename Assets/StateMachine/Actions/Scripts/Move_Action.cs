using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Actions/Platformer_Move_Horizontal", fileName = "Platformer_Move_Horizontal")]
    public class Move_Action : ScriptableAction
    {
        private IMoveable moveable;
        private IHorizontalFacing horizontalFacing;
        public override void Act(StateComponent statesComponent)
        {
            Vector2 moveDirection = statesComponent.GetCachedComponent<PlayerController>().InputReader.MoveDirection;

            if(moveable == null)
            {
                moveable = statesComponent.GetComponent<IMoveable>();
            }
            
            if(horizontalFacing == null)
            {
                horizontalFacing = statesComponent.GetComponent<IHorizontalFacing>();
            } 
            
            moveable.Move(moveDirection);
            horizontalFacing.FaceTo(-moveDirection.x);
        }
    }
}
