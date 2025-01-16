using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public abstract class ScriptableAction : ScriptableObject
    {
        public abstract void Act(StateComponent statesComponent);
    }
}