using UnityEngine;

public class BubbleSpawner2D : MonoBehaviour
{
	[Header("Bubble Prefab & Count")]
	public GameObject bubblePrefab;
	public int numberOfBubbles = 10;

	[Header("Random Scale")]
	public float minScale = 0.5f;
	public float maxScale = 2f;

	[Header("Annulus Spawn Settings")]
	[Tooltip("Bubbles will NOT spawn within this radius from the center (0,0).")]
	public float minSpawnRadius = 5f;
	[Tooltip("Bubbles will NOT spawn beyond this radius from the center (0,0).")]
	public float maxSpawnRadius = 10f;

	[Header("Overlap Check (2D)")]
	public LayerMask bubbleLayer2D;   // Layer for your 2D bubble colliders
	public int maxSpawnAttempts = 50;

	void Start()
	{
		for (int i = 0; i < numberOfBubbles; i++)
		{
			bool foundPosition = false;

			for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
			{
				// 1. Pick a random distance in [minSpawnRadius, maxSpawnRadius].
				float distance = Random.Range(minSpawnRadius, maxSpawnRadius);

				// 2. Pick a random direction in a unit circle, normalize it.
				Vector2 randomDir = Random.insideUnitCircle.normalized;

				// 3. Get random position = direction × distance
				Vector2 randomPos2D = randomDir * distance;
				Vector3 spawnPos = new Vector3(randomPos2D.x, randomPos2D.y, 0f);

				// 4. Decide a random scale
				float randomScale = Random.Range(minScale, maxScale);

				// 5. If using a CircleCollider2D with default radius ~0.5 at scale=1,
				// the bubble’s world radius is 0.5 × randomScale.
				float bubbleRadius = 0.5f * randomScale;

				// 6. Overlap check in 2D
				Collider2D hit = Physics2D.OverlapCircle(spawnPos, bubbleRadius, bubbleLayer2D);
				if (hit == null)
				{
					// No overlap → safe to spawn
					GameObject newBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
					newBubble.transform.localScale = Vector3.one * randomScale;

					// (Optional) Keep Z = 0 if you're mixing 2D in a 3D scene
					newBubble.AddComponent<KeepZAtZero>();

					foundPosition = true;
					break; // stop trying once we succeed
				}
			}

			if (!foundPosition)
			{
				Debug.LogWarning(
				  $"Could not find valid position for bubble #{i} after {maxSpawnAttempts} attempts.");
			}
		}
	}
}