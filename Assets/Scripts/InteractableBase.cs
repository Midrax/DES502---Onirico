using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    Collider collider;
    GlobalVariables globalVariables;

    bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        globalVariables = GameObject.FindObjectOfType<GlobalVariables>();
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            globalVariables.interactionText.text = "Press[E] to Interact";
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            globalVariables.interactionText.text = "";
            canInteract = false;
        }
    }


    public void Interact() 
    { 
        if (canInteract) 
        {
            if (Input.GetKey(KeyCode.E)) 
            {
                DoInteraction();
            }
        }
    }

    public void DoInteraction()
    {

    }

}
