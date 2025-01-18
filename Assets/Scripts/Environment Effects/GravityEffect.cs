using UnityEngine;

public class GravityEffect : MonoBehaviour, IEnvironmentalEffect
{
	public void ActivateEffect()
	{
		Physics.gravity = new Vector3(0, -9.81f, 0);
	}

	public void DeactivateEffect()
	{
		Physics.gravity = Vector3.zero;
	}
}
