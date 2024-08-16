using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public enum ShootingMode
    {
        Single,
        Spread
    }

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform shootingPoint; // Reference to the point where projectiles will be shot from
    public float projectileSpeed = 20f; // Speed at which the projectile will travel
    public int spreadShotCount = 5; // Number of projectiles in spreadshot
    public float spreadAngle = 30f; // Angle of spread for the spreadshot

    private ShootingMode currentMode = ShootingMode.Single;

    void Update()
    {
        // Handle shooting
        if (Input.GetButtonDown("Fire1")) // Left mouse button by default
        {
            Shoot();
        }

        // Switch shooting mode with mouse scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0f)
        {
            SwitchShootingMode(true);
        }
        else if (scrollInput < 0f)
        {
            SwitchShootingMode(false);
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || shootingPoint == null)
        {
            Debug.LogWarning("Projectile Prefab or Shooting Point is not assigned!");
            return;
        }

        if (currentMode == ShootingMode.Single)
        {
            // Single shot
            ShootProjectile(shootingPoint.position, shootingPoint.rotation);
        }
        else if (currentMode == ShootingMode.Spread)
        {
            // Spreadshot
            for (int i = 0; i < spreadShotCount; i++)
            {
                float angle = Random.Range(-spreadAngle, spreadAngle);
                Quaternion spreadRotation = Quaternion.Euler(0, angle, 0) * shootingPoint.rotation;
                ShootProjectile(shootingPoint.position, spreadRotation);
            }
        }
    }

    void ShootProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject projectile = Instantiate(projectilePrefab, position, rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.velocity = rotation * Vector3.forward * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("The projectile prefab does not have a Rigidbody component!");
        }
    }

    void SwitchShootingMode(bool nextMode)
    {
        if (nextMode)
        {
            currentMode = (ShootingMode)(((int)currentMode + 1) % System.Enum.GetValues(typeof(ShootingMode)).Length);
        }
        else
        {
            currentMode = (ShootingMode)(((int)currentMode - 1 + System.Enum.GetValues(typeof(ShootingMode)).Length) % System.Enum.GetValues(typeof(ShootingMode)).Length);
        }
        Debug.Log("Current Shooting Mode: " + currentMode);
    }
}
