using UnityEngine;

public class Keyhole : MonoBehaviour
{
	public string keyColor; // Assign "Red", "Blue", etc. in Inspector
	public GameObject[] colorObjects; // Assign blocks, flags, etc. of the same color

	public void Unlock()
	{
		// Destroy all objects of the same color
		foreach (GameObject obj in colorObjects)
		{
			Destroy(obj);
		}

		// Damage the boss
		//FindFirstObjectByType<Boss>().TakeDamage(1);

		// Destroy the keyhole
		Destroy(gameObject);
	}
}
