using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public List<GameObject> environmentalEffects; // Prefabs of effects

	void Start()
	{
		ActivateRandomEffects();
	}

	void ActivateRandomEffects()
	{
		// Ensure we have at least one environmental effect in the list
		if (environmentalEffects == null || environmentalEffects.Count == 0)
		{
			Debug.LogWarning("No environmental effects available!");
			return;
		}

		// Randomize the number of effects between 1 and the total number of effects
		int effectCount = Random.Range(1, environmentalEffects.Count + 1);
		List<GameObject> chosenEffects = new List<GameObject>();

		while (chosenEffects.Count < effectCount)
		{
			GameObject effect = environmentalEffects[Random.Range(0, environmentalEffects.Count)];
			if (!chosenEffects.Contains(effect))
				chosenEffects.Add(effect);
		}

		foreach (var effect in chosenEffects)
		{
			Instantiate(effect, transform.position, Quaternion.identity).GetComponent<IEnvironmentalEffect>().ActivateEffect();
		}
	}
}
