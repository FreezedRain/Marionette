using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : PuzzleElement
{

    private Vector3 boatDirection = Vector3.forward;
    private float speed = 0;

    private float maxSpeed = 5;

    private bool docked = false;
    private Dock dock = null;

    private bool inDockingAnimation = false;

    public Socket socket;

    private void Start()
    {
        boatDirection = transform.forward;
    }

    public override void Engage()
    {
        Undock();
        speed = 0;
    }

    public override void Input(Vector3 input, Vector3 dir)
    {
        if (docked) return;

        print("Boat input: " + input);

        float forwardForce = Vector3.Dot(transform.forward, input);

        print("FORWARD: " + forwardForce);

        speed += forwardForce * Time.deltaTime * 1;

        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        print("SPEED: " + speed);

        transform.position += boatDirection.normalized * speed * Time.deltaTime;

        boatDirection = Vector3.Lerp(boatDirection.normalized, input.normalized, 2 * Time.deltaTime).normalized;

        boatDirection = boatDirection.normalized;

        transform.rotation = Quaternion.LookRotation(boatDirection, transform.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Enter");
        
        Dock newdock = other.GetComponent<Dock>();

        docked = true;

        if (newdock != null)
        {

            speed = 0;

            inDockingAnimation = true;
            dock = newdock;

            Transform bestBoatPoint = dock.GetBestBoatPoint(boatDirection);

            LeanTween.move(gameObject, bestBoatPoint.position, 0.5f);
            LeanTween.rotate(gameObject, bestBoatPoint.eulerAngles, 0.5f).setOnUpdate((float value)=>
            {
                boatDirection = transform.forward;
            });
            LeanTween.delayedCall(0.75f, () =>
            {

                socket.Eject();
                dock.Open();
                inDockingAnimation = false;
                
            });
        }
    }

    private void Undock()
    {
        dock.Close();
        docked = false;
    }
}
