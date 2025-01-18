using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Actions/Player_Platformer_Jump_Action", fileName = "Player_Platformer_Jump_Action")]
    public class Player_Platformer_Jump_Action : ScriptableAction
    {
        public override void Act(StateComponent statesComponent)
        {
            statesComponent.GetCachedInterface<IJumpable>().Jump();
        }
    }
}
