using UnityEngine;
using Utils;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/Player_Input_Moving", fileName = "Player_Input_Moving")]
    public class Player_Input_Moving : ScriptableCondition
    {
        public override bool Verify(StateComponent statesComponent)
        {
            return statesComponent.GetCachedComponent<PlayerController>().InputReader.IsMoving;
        }
    }
}
