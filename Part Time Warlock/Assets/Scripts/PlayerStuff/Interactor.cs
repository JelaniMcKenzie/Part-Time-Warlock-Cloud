using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform interactionPoint;
    public LayerMask interactionLayer;
    public float interactionPointRadius = 1f;
    public bool isInteracting { get; private set; }

    private void Update()
    {
        /* As the player walks around, if it overlaps with anything within the given radius, that's on the interactionLayer,
         * and on the defined posiition, its stored within an array of Colliders
         */

        
        var colliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionPointRadius, interactionLayer);

        if (Keyboard.current.fKey.wasPressedThisFrame) // F is interact rn. Subject to change
        {
            /**
             * The below code could be repurposed for multiple things. Ex: if the colliders is null, then set isInteracting to false.
             * could use that same logic if the player is a certain distance away from an object, then its not interacting
             * 
             * IDEA: use the var interactable code to determine if a player is being chased by slimes
             */
            for (int i = 0; i < colliders.Length; i++)
            {
                //check every collider its found to see if it has the interactable interface on it
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null)
                {
                    StartInteraction(interactable);
                }
            }
        }
        
        
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        isInteracting = true; //can do multiple things here. Ex: write a script that if the player is interacting with something, they can't move
    }

    void EndInteraction(IInteractable interactable)
    {
        isInteracting = false;
    }

}
