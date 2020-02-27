using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInteractable : MonoBehaviour
{
    AudioSource audio;
    Collider collider;
    public GlobalVariables globalVariables;

    bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        Interact();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
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

    void DoInteraction() 
    {
        if (!audio.isPlaying)
        {
            audio.Play(0);
        }
    }
}
