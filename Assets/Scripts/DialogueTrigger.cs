using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;

    GlobalVariables globalVariables;

    bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
    }

    void Update()
    {
        Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !globalVariables.isInteracting)
        {
            globalVariables.changeText("Press[E] to Interact");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            globalVariables.changeText("");
            canInteract = false;
            globalVariables.isInteracting = false;
        }
    }


    public void Interact()
    {
        if (canInteract && !globalVariables.isInteracting)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                globalVariables.isInteracting = true;
                TriggerDialogue();
            }
        }
    }

    public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        globalVariables.changeText("");
    }

}
