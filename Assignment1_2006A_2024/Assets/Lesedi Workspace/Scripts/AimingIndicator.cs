using UnityEngine;

public class AimingIndicator : MonoBehaviour
{
    public Transform playerCamera; // Reference to the player's camera
    public float distanceFromCamera = 10f; // Distance where the indicator will appear

    private void Update()
    {
        // Move the indicator to be in front of the camera
        Vector3 aimingDirection = playerCamera.forward;
        transform.position = playerCamera.position + aimingDirection * distanceFromCamera;
    }
}
