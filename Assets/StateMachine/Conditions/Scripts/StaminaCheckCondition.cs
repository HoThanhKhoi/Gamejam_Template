using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(menuName = "Scriptable State Machine/Conditions/StaminaCheckCondition", fileName = "new StaminaCheckCondition")]
public class StaminaCheckCondition : ScriptableCondition
{
	public StaminaStatus checkStatus;

	public override bool Verify(StateComponent statesComponent)
	{
		return checkStatus == statesComponent.GetComponent<StaminaComponent>().GetStaminaStatus();
	}
}