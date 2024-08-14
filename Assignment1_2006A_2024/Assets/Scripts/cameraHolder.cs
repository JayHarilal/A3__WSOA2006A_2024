using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHolder : MonoBehaviour
{
    public Transform playerCamera;

    private void Update()
    {
        transform.position = playerCamera.position;
    }

}
