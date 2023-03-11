using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCam;
    public float interactionDistance = 2f;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    private void Update()
    {
        InteractionRay();
    }

    public void InteractionRay()
    {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one / 2f);
        RaycastHit hit;

        bool hitSomething = false;

        if(Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
          


            if (interactable != null)
            {
                hitSomething = true;
                interactionText.text = interactable.getDescription();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();

                }

            }
            
        }
        interactionUI.SetActive(hitSomething);
    }
}

