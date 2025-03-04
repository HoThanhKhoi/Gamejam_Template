using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f; // Initial movement speed
    [SerializeField] private float minSpeed = 1f; // Minimum speed
    [SerializeField] private float decelerationRate = 7f; // Rate at which speed decreases
    [SerializeField] private float lifespan = 5f; // Time before the bubble disappears
    [SerializeField] private float floatIntensity = 0.5f; // Intensity of floating effect
    [SerializeField] private float floatFrequency = 2f; // Frequency of floating effect

    private Vector2 moveDirection; // Direction of movement in 2D space (x, y)
    private float currentSpeed; // Current speed of the bubble
    private float spawnTime;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized; // Ensure the direction is normalized
        currentSpeed = initialSpeed; // Set the initial speed
        spawnTime = Time.time;
    }

    private void Update()
    {
        // Gradually decrease speed but keep it above the minimum threshold
        currentSpeed = Mathf.Max(minSpeed, currentSpeed - decelerationRate * Time.deltaTime);

        // Move the bubble in the given direction
        Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0) * currentSpeed * Time.deltaTime;
        transform.position += movement;

        // Apply floating effect along the Y-axis
        ApplyFloatingEffect();

        // Deactivate after lifespan
        if (Time.time - spawnTime >= lifespan)
        {
            gameObject.SetActive(false);
        }
    }

    private void ApplyFloatingEffect()
    {
        // Floating effect using sine wave
        float floatOffset = Mathf.Sin((Time.time - spawnTime) * floatFrequency) * floatIntensity;
        transform.position += new Vector3(0, floatOffset * Time.deltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision logic (e.g., deal damage, pop the bubble, etc.)
        Debug.Log($"Bubble hit {collision.gameObject.name}");
        gameObject.SetActive(false); // Return to the object pool

        if (collision.gameObject.tag == "Boss")
        {
            ObjectPoolingManager.Instance.SpawnFromPool("NonDamageImpact", transform.position, Quaternion.identity);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision logic (e.g., deal damage, pop the bubble, etc.)
        Debug.Log($"Bubble hit {collision.gameObject.name}");
        gameObject.SetActive(false); // Return to the object pool

        if(collision.gameObject.tag == "Boss")
        {
            ObjectPoolingManager.Instance.SpawnFromPool("NonDamageImpact", transform.position, Quaternion.identity);
        }
    }
}
