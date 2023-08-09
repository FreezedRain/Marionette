using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Socket hoverSocket;
    private Socket stuckSocket;

    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private PlayerAnimations animations;

    public void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            print("Test");
            print("hoverSocket: " + hoverSocket);
            print("stuckSocket: " + stuckSocket);
            if (hoverSocket != null && stuckSocket == null)
            {
                stuckSocket = hoverSocket;
                movement.SetSocket(stuckSocket.transform);
                movement.SetActive(false);

                animations.Socket();

            } else if (stuckSocket != null)
            {
                hoverSocket = stuckSocket;
                stuckSocket = null;
                movement.SetActive(true);

                animations.Unsocket();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        hoverSocket = other.gameObject.GetComponent<Socket>();
        print("Enter");
    }

    private void OnTriggerExit(Collider other)
    {
        Socket s = other.gameObject.GetComponent<Socket>();
        if (s != null && s == hoverSocket)
        {
            hoverSocket = null;
        }
        print("Leave");
    }

}
