using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactables : MonoBehaviour
{
    public bool useEvents;
    [SerializeField]
    public string messagePrompt;

    public void objectInteract()
    {
        if (useEvents)
            GetComponent<interactionEvent>().onInteract.Invoke();
        Interact();
    }

    protected virtual void Interact()
    {

    }
}
