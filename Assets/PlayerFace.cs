using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    public List<GameObject> faces = new List<GameObject>();

    public void SetFace(int index)
    {
        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].SetActive(i == index);
        }
    }
}
