using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteract : MonoBehaviour, IInteractable
{
    public string getDescription()
    {
        return "Interact with Cube";
    }

    public void Interact()
    {
        Debug.Log("Sí");
    }

   
}
