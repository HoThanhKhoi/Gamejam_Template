using UnityEngine;

public class ShrinkOverTime : MonoBehaviour
{
	[Header("Shrink Settings")]
	[Tooltip("How fast the object shrinks (scale per second).")]
	public float shrinkSpeed = 1f;

	[Tooltip("Minimum scale before the object is destroyed.")]
	public float minScale = 0.1f;

	void Update()
	{
		transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

		if (transform.localScale.x <= minScale || transform.localScale.y <= minScale || transform.localScale.z <= minScale)
		{
			Destroy(gameObject);
		}
	}
}
