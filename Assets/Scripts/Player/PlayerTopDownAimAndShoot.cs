using UnityEngine;

public class PlayerAimAndShoot : MonoBehaviour
{
    [SerializeField] private string poolName;
    [SerializeField] private Transform firePoint ;
    [SerializeField] private float shootCooldown = 0.5f;
    private Camera mainCamera;
    private float shootTimer;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        AimAtMouse();

        if (Input.GetMouseButtonDown(0) && Time.time >= shootTimer + shootCooldown)
        {
            Shoot();
            shootTimer = Time.time;
        }
    }

    private void AimAtMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        Vector3 aimDirection = (mouseWorldPosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned!");
            return;
        }

        AudioManager.Instance.PlayShootSound();

        // Calculate the direction the gun is pointing
        Vector2 shootDirection = new Vector2(firePoint.right.x, firePoint.right.y).normalized;

        // Spawn the projectile from the pool
        GameObject bubble = ObjectPoolingManager.Instance.SpawnFromPool("Bubble", firePoint.position, firePoint.rotation);

        if (bubble != null)
        {
            // Initialize the bubble with the calculated direction
            bubble.GetComponent<BubbleProjectile>().Initialize(shootDirection);
        }
    }

}
