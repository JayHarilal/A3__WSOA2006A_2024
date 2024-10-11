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
    private playerUI playerUI;
    private inputManager inputManager;
    void Start()
    {
        cam = GetComponent<playerLook>().cam;
        playerUI = GetComponent<playerUI>();
        inputManager = GetComponent<inputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.updateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitCheck;
        if (Physics.Raycast(ray, out hitCheck, distance, mask))
        {
            if(hitCheck.collider.GetComponent<Interactables>()!= null)
            {
                //Debug.Log(hitCheck.collider.GetComponent<Interactables>().messagePrompt);
                Interactables interactables = (hitCheck.collider.GetComponent<Interactables>());
                playerUI.updateText(interactables.messagePrompt);
                if (inputManager.walking.Interact.triggered)
                {
                    interactables.objectInteract();
                }
            }
        }
    }
}
