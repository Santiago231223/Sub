using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{

    public GameObject interactable;
    bool open=false;
    
    public string getDescription()
    {
        return "Abrir puerta";
    }
    public void Interact()
    {
        if (open) {
            interactable.GetComponent<Animator>().Play("DoorClose");
                        open = false;
                };
        if (!open) { interactable.GetComponent<Animator>().Play("DoorOpen");
            open = true;
        }
    }
}
