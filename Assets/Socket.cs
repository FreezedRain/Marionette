using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : Interactable
{
    public PuzzleElement puzzleElement;
    
    private float inputCD = 0.35f;

    private float t = 0;

    private Vector3 input;
    private Vector3 dir;

    private bool active = false;

    private PlayerController pc;

    private Quaternion relativeSittingDir = Quaternion.identity;

    [SerializeField]
    private bool discrete = true;
    
    public override bool Interact()
    {
        
        return false;
    }

    public void SetInput(Vector3 input)
    {
        this.input = input;
        
        Vector3 newdir = DirFromInput(input);

        dir = newdir;

    }

    private void Update()
    {
        if (!active) return;

        if (discrete)
        {
            if (t > 0)
            {
                t -= Time.deltaTime;
            }
            else
            {
                if (dir != Vector3.zero)
                {
                    puzzleElement?.Input(input, dir);
                    print("Echo: " + dir);
                    t = inputCD;
                }
            }
        } else
        {
            puzzleElement?.Input(input, dir);
        }

        print(relativeSittingDir);
        Debug.DrawLine(transform.position, transform.position + transform.forward * 20, Color.magenta);
        Debug.DrawLine(transform.position, transform.position + relativeSittingDir * transform.forward * 30, Color.yellow);

    }

    public void ActivateSocket(PlayerController pc)
    {
        t = 0.5f;
        dir = Vector3.zero;
        active = true;
        this.pc = pc;

        puzzleElement.Engage();
    }

    public void DeactivateSocket()
    {
        active = false;

    }

    private Vector3 DirFromInput(Vector3 input)
    {
        dir = Vector3.zero;

        if (input == Vector3.zero) return Vector3.zero;
        
        if (Mathf.Abs(input.x) >= Mathf.Abs(input.z))
        {
            if (input.x > 0)
            {
                dir = Vector3.right;
            }
            else
            {
                dir = Vector3.left;
            }
        }
        else
        {
            if (input.z > 0)
            {
                dir = Vector3.forward;
            }
            else
            {
                dir = Vector3.back;
            }
        }

        print(dir);
        Vector3 trueDir = GetSittingDir() * dir.z + Vector3.Cross(transform.up, GetSittingDir()) * dir.x;
        print(trueDir);

        return dir;
    }

    public Vector3 GetClosestDirection(Vector3 inputDirection)
    {
        Vector3 forward = transform.forward;
        Vector3 back = -forward;
        Vector3 right = transform.right;
        Vector3 left = -right;

        // Normalize the input and reference directions
        inputDirection.Normalize();
        forward.Normalize();
        right.Normalize();

        // Calculate the angles between the input direction and the reference directions
        float angleForward = Vector3.Angle(inputDirection, forward);
        float angleBack = Vector3.Angle(inputDirection, -forward);
        float angleRight = Vector3.Angle(inputDirection, right);
        float angleLeft = Vector3.Angle(inputDirection, -right);

        // Find the minimum angle
        float minAngle = Mathf.Min(angleForward, angleBack, angleRight, angleLeft);

        Vector3 sittingDir = forward.normalized;

        // Return the direction corresponding to the minimum angle
        if (minAngle == angleForward) {
            sittingDir = forward.normalized;
            relativeSittingDir = Quaternion.Euler(0, 0, 0);
        } else if (minAngle == angleBack) {
            sittingDir = back.normalized;
            relativeSittingDir = Quaternion.Euler(0, 180, 0);
        } else if (minAngle == angleRight) {
            sittingDir = right.normalized;
            relativeSittingDir = Quaternion.Euler(0, 90, 0);
        } else {
            sittingDir = left.normalized;
            relativeSittingDir = Quaternion.Euler(0, -90, 0);
        }


        return sittingDir;
    }

    public Vector3 GetRelativeSittingDir()
    {
        return GetSittingDir();
    }

    public Vector3 GetSittingDir()
    {
        return relativeSittingDir * transform.forward;
    }

    public Quaternion GetRotationRelativeToSocket()
    {
        return relativeSittingDir;
    }

    public Vector3 GetUpDir()
    {
        return transform.up;
    }

    public void Eject()
    {
        pc?.Unsocket();
        pc = null;
    }


}
