using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : PuzzleElement
{

    private Vector3 boatDirection = Vector3.forward;
    private float speed = 0;

    private float maxSpeed = 5;

    [SerializeField]
    private Transform conePivotRight;

    private void Start()
    {
        boatDirection = transform.forward;
    }

    public override void Input(Vector3 input, Vector3 dir)
    {
        print("Boat input: " + input);

        float forwardForce = Vector3.Dot(transform.forward, input);
        float steer = Vector3.Dot(transform.right, input);

        print("FORWARD: " + forwardForce);

        speed += forwardForce * Time.deltaTime * 1;

        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        print("SPEED: " + speed);

        transform.position += boatDirection.normalized * speed * Time.deltaTime;

        if (Mathf.Abs(steer) > 0)
        {
            boatDirection = Quaternion.Euler(0, steer * Time.deltaTime * 60, 0) * boatDirection;
        }

        boatDirection = boatDirection.normalized;

        transform.rotation = Quaternion.LookRotation(boatDirection, transform.up);

        conePivotRight.localScale = Vector3.one * speed/2.5f;
    }
}
