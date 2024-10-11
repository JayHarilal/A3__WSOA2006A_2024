using UnityEditor;

[CustomEditor(typeof(Interactables), true)]
public class interactableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactables interactables = (Interactables)target;
        if (target.GetType() == typeof(eventOnlyInteractable))
        {
            interactables.messagePrompt = EditorGUILayout.TextField("Promp Message", interactables.messagePrompt);
            if (interactables.GetComponent<interactionEvent>() == null)
            {
                interactables.useEvents = true;
                interactables.gameObject.AddComponent<interactionEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (interactables.useEvents)
            {
                if (interactables.GetComponent<interactionEvent>() == null)
                    interactables.gameObject.AddComponent<interactionEvent>();
            }
            else
            {
                if (interactables.GetComponent<interactionEvent>() == null)
                    DestroyImmediate(interactables.GetComponent<interactionEvent>());
            }
        }
    }
}
