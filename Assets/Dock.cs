using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour
{
    public Transform boatPoint;
    public Transform boatPoint2;

    public GameObject dockExtension;

    public void Open()
    {
        LeanTween.moveLocalX(dockExtension, 0.5f, 0.25f).setEaseOutCubic();
        print("DOCK OPEN");
    }

    public void Close()
    {
        LeanTween.moveLocalX(dockExtension, -0.5f, 0.25f).setEaseOutCubic();
        print("DOCK CLOSE");
    }

    public Transform GetBestBoatPoint(Vector3 forward)
    {
        if (Vector3.Dot(boatPoint.forward, forward) > 0)
        {
            return boatPoint;
        } else
        {
            return boatPoint2;
        }
    }
}
