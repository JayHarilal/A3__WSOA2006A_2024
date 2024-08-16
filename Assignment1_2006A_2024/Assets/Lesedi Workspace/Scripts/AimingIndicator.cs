using UnityEngine;

public class AimingIndicator : MonoBehaviour
{
    public Transform playerCamera; 
    public Transform shootpoint;   
    public float distanceFromShootpoint = 10f;

    private void Update()
    {
        if (playerCamera == null || shootpoint == null)
        {
            Debug.LogWarning("Player Camera or Shootpoint not assigned!");
            return;
        }

        // Calculate the position in front of the shootpoint in the direction of the camera's forward vector
        Vector3 aimingDirection = playerCamera.forward;
        transform.position = shootpoint.position + aimingDirection * distanceFromShootpoint;

        // Align the reticle to face the direction the player is aiming
        transform.rotation = Quaternion.LookRotation(aimingDirection);
    }
}
