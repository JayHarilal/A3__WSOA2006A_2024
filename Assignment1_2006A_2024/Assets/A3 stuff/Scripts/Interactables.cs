using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactables : MonoBehaviour
{
    public string messagePrompt;

    public void objectInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {

    }
}
