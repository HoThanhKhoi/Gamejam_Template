using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/Player_Falling", fileName = "Player_Falling")]
    public class Player_Falling : ScriptableCondition
    {
        public override bool Verify(StateComponent statesComponent)
        {
            //return statesComponent.GetCachedComponent<PlayerPlatformerController>().JumpComponent;
            return false;
        }
    }
}
