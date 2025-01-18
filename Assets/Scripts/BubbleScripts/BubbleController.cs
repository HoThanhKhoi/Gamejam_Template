using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleController : MonoBehaviour
{
	[Header("Center Pull (Optional)")]
	[Tooltip("If > 0, applies a gentle force pulling the bubble toward (0,0).")]
	public float centerPullStrength = 0.2f;

	[Header("Random Push")]
	[Tooltip("Min time (seconds) between random pushes.")]
	public float minPushInterval = 1f;
	[Tooltip("Max time (seconds) between random pushes.")]
	public float maxPushInterval = 3f;

	[Tooltip("Min force of each random push (impulse).")]
	public float minPushForce = 0.1f;
	[Tooltip("Max force of each random push (impulse).")]
	public float maxPushForce = 0.5f;

	private Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void OnEnable()
	{
		// Start the coroutine that applies random pushes
		StartCoroutine(RandomPushRoutine());
	}

	void FixedUpdate()
	{
		// (Optional) Gently pull bubble toward the center
		if (centerPullStrength > 0f)
		{
			Vector2 toCenter = -rb.position.normalized;
			// You could also do (Vector2.zero - rb.position).normalized;
			rb.AddForce(toCenter * centerPullStrength, ForceMode2D.Force);
		}
	}

	private IEnumerator RandomPushRoutine()
	{
		while (true)
		{
			// Wait a random interval before applying the next push
			float waitTime = Random.Range(minPushInterval, maxPushInterval);
			yield return new WaitForSeconds(waitTime);

			// Apply a small random push in a random direction
			Vector2 randomDir = Random.insideUnitCircle.normalized;
			float randomForce = Random.Range(minPushForce, maxPushForce);

			rb.AddForce(randomDir * randomForce, ForceMode2D.Impulse);
		}
	}
}
