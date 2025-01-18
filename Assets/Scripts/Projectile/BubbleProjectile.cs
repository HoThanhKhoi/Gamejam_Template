using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Movement speed
    [SerializeField] private float lifespan = 5f; // Time before the bubble disappears
    [SerializeField] private float floatIntensity = 0.5f; // Intensity of floating effect
    [SerializeField] private float floatFrequency = 2f; // Frequency of floating effect

    private Vector2 moveDirection; // Direction of movement in 2D space (x, y)
    private float spawnTime;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized; // Ensure the direction is normalized
        spawnTime = Time.time;
    }

    private void Update()
    {
        // Move the bubble in the given direction
        Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0) * speed * Time.deltaTime;
        transform.position += movement;

        // Apply floating effect along the Y-axis
        float floatOffset = Mathf.Sin((Time.time - spawnTime) * floatFrequency) * floatIntensity;
        transform.position += new Vector3(0, floatOffset * Time.deltaTime, 0);

        // Deactivate after lifespan
        if (Time.time - spawnTime >= lifespan)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle collision logic (e.g., deal damage, pop the bubble, etc.)
        Debug.Log($"Bubble hit {other.gameObject.name}");
        gameObject.SetActive(false); // Return to the object pool
    }
}
