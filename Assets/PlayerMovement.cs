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

        cameraPivotPlayerControl.localEulerAngles += new Vector3(0, move2.x, 0) * Time.deltaTime * 60f;

        groundPointChar.rotation = Quaternion.Lerp(groundPointChar.rotation, trot, 10 * Time.deltaTime);

        if (!active)
        {
            transform.position = Vector3.Lerp(transform.position, tpos, 10 * Time.deltaTime);


            neck.transform.localEulerAngles = new Vector3(move.y * 30, 0, -move.x*30);
        } else
        {
            neck.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (!active) return;

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

        hsp = move * speed;

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


        GravitySwap();

        

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

    public void SetActive(bool active)
    {
        this.active = active;
        colliderTransform.GetComponent<Collider>().enabled = active;
        hsp = Vector2.zero;
        rb.velocity = Vector3.zero;
    }

    public void SetSocket(Transform socketTransform)
    {
        tup = socketTransform.up;

        characterForward = socketTransform.forward;

        trot = Quaternion.LookRotation(characterForward, up);
        tpos = socketTransform.position;
    }

    public float GetSpeed()
    {
        return hsp.magnitude;
    }

    public bool IsActive()
    {
        return active;
    }
}
