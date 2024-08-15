using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform shootingPoint; // Reference to the point where projectiles will be shot from
    public float projectileSpeed = 20f; // Speed at which the projectile will travel

    void Update()
    {
        // Handle shooting
        if (Input.GetButtonDown("Fire1")) // Left mouse button by default
        {
            Shoot();
        }
    }

    // Method to shoot the projectile
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = shootingPoint.forward * projectileSpeed;
    }
}
