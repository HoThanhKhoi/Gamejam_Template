using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Scriptable State Machine/Actions/Stop_Moving_Action", fileName = "Stop_Moving_Action")]
    public class Stop_Moving_Action : ScriptableAction
    {
        public override void Act(StateComponent statesComponent)
        {
            Rigidbody2DComponent rbComponent = statesComponent.GetCachedComponent<Rigidbody2DComponent>();
            rbComponent.SetVelocity(0, rbComponent.GetVelocity().y);
        }
    }
}
