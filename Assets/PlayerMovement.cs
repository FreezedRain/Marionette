using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    public float speed = 1;

    private float _gravity = -9.8f;

    private float vsp = 0;
    private Vector2 hsp = Vector2.zero;

    private Rigidbody rb;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private Transform cameraPivotWorld;
    [SerializeField]
    private Transform cameraPivotPlayerControl;

    [SerializeField]
    private Transform core;

    private Vector3 previousPos;

    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private SmartCamera smartCamera;

    private bool isGrounded = false;

    private Transform world;

    private Vector3 coreDefault;
    private float t = 0;

    private Vector3 tup = Vector3.up;

    private Vector3 up = Vector3.up;
    private Vector3 forward = Vector3.forward;
    private Vector3 side = Vector3.right;

    [SerializeField]
    private Transform colliderTransform;

    [SerializeField]
    private Transform groundPoint;
    [SerializeField]
    private Transform groundPointChar;

    private Vector3 lastGroundedPosition;

    private Vector3 groundNormal;

    private bool active = true;

    private Quaternion trot;
    private Vector3 tpos;

    [SerializeField]
    private Transform neck;

    private Socket socket;
    private Quaternion rotationRelativeToSocket;

    public enum MOVEMENT_STATE
    {
        FREE,
        SOCKETED,
        INTERACTING
    }

    private MOVEMENT_STATE movementState = MOVEMENT_STATE.FREE;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private Vector3 cameraForward = Vector3.forward;

    private Vector3 characterForward = Vector3.forward;


    // Update is called once per frame
    void Update()
    {

        Vector2 move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 move2 = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));

        cameraForward = Vector3.ProjectOnPlane(cameraPivotPlayerControl.forward, up).normalized;
        Vector3 cameraRight = (Quaternion.AngleAxis(90, up) * cameraForward).normalized;

        switch (movementState)
        {
            case MOVEMENT_STATE.INTERACTING:

                groundPointChar.rotation = Quaternion.Lerp(groundPointChar.rotation, trot, 10 * Time.deltaTime);
                break;
            case MOVEMENT_STATE.SOCKETED:
                cameraPivotPlayerControl.localEulerAngles += new Vector3(0, move2.x, 0) * Time.deltaTime * 60f;

                transform.position = Vector3.Lerp(transform.position, socket.transform.position, 10 * Time.deltaTime);

                Vector3 cameraMove = cameraForward * move.y + cameraRight * move.x;

                Vector3 joystickVector = Vector3.LerpUnclamped(up.normalized, cameraMove.normalized, 0.5f * move.magnitude).normalized;
                Debug.DrawLine(transform.position, transform.position + joystickVector * 10, Color.red);

                Vector3 cameraMoveRotated = Quaternion.AngleAxis(-groundPointChar.localEulerAngles.y, up) * cameraMove;


                Vector3 characterSide = -Vector3.Cross(up, characterForward);

                //neck.transform.rotation = Quaternion.LookRotation(Vector3.Cross(joystickVector, characterSide), joystickVector);

                socket.SetInput(cameraMove);

                Debug.DrawLine(transform.position, transform.position + cameraMove * 10, Color.green);

                //groundPointChar.rotation = Quaternion.LookRotation(socket.GetSittingDir(), socket.GetUpDir());*/

                break;
            case MOVEMENT_STATE.FREE:

                groundPointChar.rotation = Quaternion.Lerp(groundPointChar.rotation, trot, 10 * Time.deltaTime);
                cameraPivotPlayerControl.localEulerAngles += new Vector3(0, move2.x, 0) * Time.deltaTime * 60f;

                neck.transform.localEulerAngles = new Vector3(0, 0, 0);

                StateFree();
                break;
        }
        
    }

    private void Sit()
    {
        LeanTween.moveLocal(gameObject, Vector3.zero, 0.25f);
        LeanTween.rotateLocal(groundPointChar.gameObject, socket.GetRotationRelativeToSocket().eulerAngles, 0.25f);
    }

    private void StateFree()
    {
        Vector2 move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        cameraForward = Vector3.ProjectOnPlane(cameraPivotPlayerControl.forward, up).normalized;
        Vector3 cameraRight = (Quaternion.AngleAxis(90, up) * cameraForward).normalized;

        if (Physics.Raycast(new Ray(groundPoint.position + up * 0.2f, -up), out var hit, 0.3f, groundMask))
        {
            groundNormal = hit.normal;
            isGrounded = true;

            transform.position = hit.point;

            lastGroundedPosition = hit.point;



        }
        else
        {
            isGrounded = false;
        }

        //forward = cameraForward;
        //side = cameraRight;

        hsp = move.normalized * speed;

        if (isGrounded)
        {
            vsp = 0;
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.position, lastGroundedPosition, 10 * Time.deltaTime);
        }

        Vector3 flatVelocity = cameraForward * hsp.y + cameraRight * hsp.x;

        if (flatVelocity.magnitude > 0)
        {
            characterForward = flatVelocity;
        }

        if (isGrounded)
        {
            rb.velocity = flatVelocity;
        }
        else
        {
            transform.position = lastGroundedPosition;
        }

        //groundPoint.rotation = Quaternion.LookRotation(groundPoint.forward, up);
        trot = Quaternion.LookRotation(characterForward, up);


        //GravitySwap();



        DrawDebug();
    }

    private void GravitySwap()
    {

        if (!isGrounded) return;

        tup = groundNormal;

        up = Vector3.Slerp(up, tup, 5f * Time.deltaTime);

        forward = Vector3.ProjectOnPlane(forward, up).normalized;
        side = Vector3.ProjectOnPlane(side, up).normalized;

        //cameraPivotWorld.up = up;
        groundPoint.rotation = Quaternion.LookRotation(forward, up);


    }

    private void DrawDebug()
    {
        Debug.DrawRay(colliderTransform.position, up * 2, Color.blue);
        Debug.DrawRay(colliderTransform.position, forward * 2, Color.blue);
        Debug.DrawRay(colliderTransform.position, side * 2, Color.blue);


        Debug.DrawRay(colliderTransform.position, characterForward * 2, Color.red);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(lastGroundedPosition, 0.05f);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(groundPoint.position, 0.05f);
    }

    public void SetSocket(Socket socket)
    {
        this.socket = socket;
        
        tup = socket.transform.up;

        Vector3 sitForward = socket.GetClosestDirection(characterForward);

        characterForward = sitForward;

        trot = Quaternion.LookRotation(characterForward, up);
        tpos = socket.transform.position;

        hsp = Vector2.zero;
        rb.velocity = Vector3.zero;

        movementState = MOVEMENT_STATE.SOCKETED;

        Sit();
    }

    public void SetInteracting(Interactable interactable)
    {
        hsp = Vector2.zero;
        rb.velocity = Vector3.zero;

        trot = Quaternion.LookRotation(interactable.GetInterestPosition() - transform.position, up);

        movementState = MOVEMENT_STATE.INTERACTING;
    }

    public void SetFree()
    {
        movementState = MOVEMENT_STATE.FREE;
    }

    public float GetSpeed()
    {
        return hsp.magnitude;
    }

    public bool IsActive()
    {
        return active;
    }

    public MOVEMENT_STATE GetMovementState()
    {
        return movementState;
    }
}
