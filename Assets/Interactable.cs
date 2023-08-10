using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual bool Interact()
    {
        return false;
    }

    public virtual Vector3 GetInterestPosition()
    {
        return transform.position;
    }

    public virtual void OnHover()
    {
        return;
    }

    public virtual void OnUnhover()
    {
        return;
    }
}
