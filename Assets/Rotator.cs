using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationSpeed = Vector3.zero;

    private void Update()
    {
        transform.localEulerAngles += rotationSpeed * Time.deltaTime;
    }
}
