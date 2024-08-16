using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f; // Time in seconds before the bullet is destroyed automatically

    void Start()
    {
        // Destroy the bullet after 'lifeTime' seconds if it doesn't hit anything
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet on impact
        Destroy(gameObject);
    }

    // If using a trigger collider, use OnTriggerEnter instead
    void OnTriggerEnter(Collider other)
    {
        // Destroy the bullet on trigger enter
        Destroy(gameObject);
    }
}
