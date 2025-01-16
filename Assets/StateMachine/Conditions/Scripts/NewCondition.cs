using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/NewCondition", fileName = "NewCondition")]
    public class NewCondition : ScriptableCondition
    {
        public override bool Verify(StateComponent statesComponent)
        {
            // TODO: Implement your condition logic here
            return false;
        }
    }
}
