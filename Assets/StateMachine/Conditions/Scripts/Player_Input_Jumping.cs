using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/Player_Input_Jumping", fileName = "Player_Input_Jumping")]
    public class Player_Input_Jumping : ScriptableCondition
    {
        public override bool Verify(StateComponent statesComponent)
        {
            return statesComponent.GetCachedComponent<PlayerController>().InputReader.IsJumping;
        }
    }
}
