using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;
    // Start is called before the first frame update
    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mousey = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation = mousey;

        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        yRotation = mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 90f);
    }
}