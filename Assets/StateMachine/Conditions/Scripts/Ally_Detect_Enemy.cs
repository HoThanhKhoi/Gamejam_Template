using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/Ally_Detect_Enemy", fileName = "Ally_Detect_Enemy")]
    public class Ally_Detect_Enemy : ScriptableCondition
    {
        public override bool Verify(StateComponent statesComponent)
        {
            return statesComponent.GetComponent<IRadiusDetect>().IsDetected();
        }
    }
}
