using UnityEngine;

public class WeaponFollowMouseComponent : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distanceFromPlayer = 1.0f; // Distance of the weapon from the player

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (player == null || mainCamera == null)
        {
            Debug.LogError("Ensure both player and main camera are properly assigned!");
        }
    }

    private void Update()
    {
        UpdateWeaponPositionAndRotation();
    }

    private void UpdateWeaponPositionAndRotation()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(mainCamera.transform.position.z - player.position.z)));

        // Calculate direction from player to mouse
        Vector3 direction = (mouseWorldPosition - player.position).normalized;

        // Position the weapon at a fixed distance from the player
        transform.position = player.position + direction * distanceFromPlayer;

        // Calculate rotation angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust rotation based on player's facing direction
        if (player.localScale.x < 0) // Player is flipped (facing left)
        {
            transform.rotation = Quaternion.Euler(0, 180, angle); // Flip weapon on Y-axis
        }
        else // Player is facing right
        {
            transform.rotation = Quaternion.Euler(0, 180, -angle);
        }
    }
}