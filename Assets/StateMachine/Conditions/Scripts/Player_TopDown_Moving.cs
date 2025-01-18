using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/Player_TopDown_Moving", fileName = "Player_TopDown_Moving")]
    public class Player_TopDown_Moving : ScriptableCondition
    {
        public override bool Verify(StateComponent statesComponent)
        {
            return statesComponent.GetCachedComponent<InputReaderComponent>().InputReader.IsTopDownMoving;
        }
    }
}
