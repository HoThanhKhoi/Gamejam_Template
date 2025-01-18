using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/Player_IsGrounded", fileName = "Player_IsGrounded")]
    public class Player_IsGrounded : ScriptableCondition
    {
        public override bool Verify(StateComponent statesComponent)
        {
            return statesComponent.GetCachedInterface<ICheckGrounded>().IsGrounded();
        }
    }
}
