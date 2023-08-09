using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    [SerializeField]
    private Camera currentCamera;

    private Vector3 prevPos;
    private Quaternion prevRot;

    private float transit = 1;

    public void SetCamera(Camera c)
    {
        currentCamera = c;
        transit = 0;

        prevPos = transform.position;
        prevRot = transform.rotation;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(prevPos, currentCamera.transform.position, transit);
        transform.rotation = Quaternion.Lerp(prevRot, currentCamera.transform.rotation, transit);

        transit = Mathf.Lerp(transit, 1, 0.2f);
    }
}
