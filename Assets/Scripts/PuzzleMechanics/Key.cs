using UnityEngine;

public class Key : MonoBehaviour, IFollow
{
    public string keyColor;          // The color/type of the key
    private Transform target;        // The player to follow
    private bool isFollowing = false; // Whether the key is following the player
    private float followSpeed = 5f;   // Speed at which the key follows
    private float followDelay = 0.5f; // Distance to lag behind the player

    /// <summary>
    /// Makes the key follow the specified target (e.g., the player).
    /// </summary>
    public void Follow(Transform target)
    {
        this.target = target;
        isFollowing = true;
    }

    private void Update()
    {
        if (isFollowing && target != null)
        {
            // Smoothly move toward the player with a delay
            Vector2 targetPosition = target.position;  // Get the player's position as a Vector2
            Vector2 direction = targetPosition - (Vector2)transform.position;
            float distance = direction.magnitude;

            if (distance > followDelay)
            {
                Vector2 newPosition = Vector2.Lerp(
                    transform.position,
                    targetPosition - direction.normalized * followDelay,
                    followSpeed * Time.deltaTime
                );

                transform.position = newPosition;
            }
        }
    }

    public void UseKey()
    {
        // Destroy the key when it is successfully used
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Follow(collision.transform); // Start following the player
        }
    }
}
