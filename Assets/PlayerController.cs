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

    private Transform ogPartner;

    private void Start()
    {
        ogPartner = transform.parent;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            if (hoverInteractable != null)
            {              
                if (stuckSocket != null)
                {
                    stuckSocket.Eject();
                }
                else
                {
                    if (hoverInteractable is Socket)
                    {

                        stuckSocket = (Socket)hoverInteractable;
                        movement.SetSocket(stuckSocket);

                        stuckSocket.ActivateSocket(this);

                        transform.SetParent(stuckSocket.transform);

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

    public void Unsocket()
    {
        hoverInteractable = stuckSocket;
        stuckSocket.DeactivateSocket();
        stuckSocket = null;

        transform.SetParent(ogPartner);

        animations.Unsocket();

        movement.SetFree();
    }

}
