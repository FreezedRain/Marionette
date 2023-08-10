using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Interactable hoverInteractable;
    private Socket stuckSocket;

    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private PlayerAnimations animations;

    public void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            if (hoverInteractable != null)
            {              
                if (stuckSocket != null)
                {
                    hoverInteractable = stuckSocket;
                    stuckSocket = null;

                    animations.Unsocket();

                    movement.SetFree();
                }
                else
                {
                    if (hoverInteractable is Socket)
                    {

                        stuckSocket = (Socket)hoverInteractable;
                        movement.SetSocket(stuckSocket.transform);

                        animations.Socket();
                    }
                    if (hoverInteractable is DialogTrigger)
                    {

                        if (hoverInteractable.Interact())
                        {
                            movement.SetInteracting(hoverInteractable);
                        } else
                        {
                            movement.SetFree();
                        }
                    }
                }

            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        hoverInteractable = other.gameObject.GetComponent<Interactable>();
        if (hoverInteractable != null) hoverInteractable.OnHover();

        print("Enter " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable i = other.gameObject.GetComponent<Interactable>();
        if (i != null && i == hoverInteractable)
        {
            hoverInteractable.OnUnhover();
            hoverInteractable = null;
        }

        print("Exit " + other.gameObject.name);
    }

}
