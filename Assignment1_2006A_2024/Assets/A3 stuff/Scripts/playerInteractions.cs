using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteractions : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<playerLook>().cam;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitCheck;
        if (Physics.Raycast(ray, out hitCheck, distance, mask))
        {
            if(hitCheck.collider.GetComponent<Interactables>()!= null)
            {
                Debug.Log(hitCheck.collider.GetComponent<Interactables>().messagePrompt);
            }
        }
    }
}
