using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;

    public float xSensitivity = 20f;
    public float ySensitivity = 20f;

    public void lookAround(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

        yRotation -= (mouseY * Time.deltaTime) * xSensitivity;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * ySensitivity);
    }
}
