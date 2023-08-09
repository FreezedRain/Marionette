using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour
{
    [SerializeField]
    private List<Transform> cubes = new List<Transform>();

    [SerializeField]
    private Transform center;

    [SerializeField]
    private Transform top;
    [SerializeField]
    private Transform bottom;
    [SerializeField]
    private Transform right;
    [SerializeField]
    private Transform left;
    [SerializeField]
    private Transform front;
    [SerializeField]
    private Transform back;

    private bool rotating = false;

    private void Start()
    {
        
    }

    public void TopRotate()
    {
        foreach(Transform cube in cubes)
        {
            if (cube.position.y > center.position.y)
            {
                cube.parent = top;
            }
            else
            {
                cube.parent = center;
            }
        }

        rotating = true;
        LeanTween.rotateAround(top.gameObject, top.position - center.position, 90, 0.5f).setOnComplete(() =>
        {
            rotating = false;
        });
    }

    public void BottomRotate()
    {
        foreach (Transform cube in cubes)
        {
            if (cube.position.y < center.position.y)
            {
                cube.parent = bottom;
            }
            else
            {
                cube.parent = center;
            }
        }

        rotating = true;
        LeanTween.rotateAround(bottom.gameObject, bottom.position - center.position, 90, 0.5f).setOnComplete(() =>
        {
            rotating = false;
        });
    }

    public void RightRotate()
    {
        foreach (Transform cube in cubes)
        {
            if (cube.position.z > center.position.z)
            {
                cube.parent = right;
            }
            else
            {
                cube.parent = center;
            }
        }

        rotating = true;
        LeanTween.rotateAround(right.gameObject, right.position - center.position, 90, 0.5f).setOnComplete(() =>
        {
            rotating = false;
        });
        print("right");
    }

    public void LeftRotate()
    {
        foreach (Transform cube in cubes)
        {
            if (cube.position.z < center.position.z)
            {
                cube.parent = left;
            } else
            {
                cube.parent = center;
            }
        }

        rotating = true;
        LeanTween.rotateAround(left.gameObject, left.position - center.position, 90, 0.5f).setOnComplete(() =>
        {
            rotating = false;
        });
        print("left");
    }

    public void FrontRotate()
    {
        foreach (Transform cube in cubes)
        {
            if (cube.position.x > center.position.x)
            {
                cube.parent = front;
            }
            else
            {
                cube.parent = center;
            }
        }

        rotating = true;
        LeanTween.rotateAround(front.gameObject, front.position - center.position, 90, 0.5f).setOnComplete(() =>
        {
            rotating = false;
        });
    }

    public void BackRotate()
    {
        foreach (Transform cube in cubes)
        {
            if (cube.position.x < center.position.x)
            {
                cube.parent = back;
            }
            else
            {
                cube.parent = center;
            }
        }

        rotating = true;
        LeanTween.rotateAround(back.gameObject, back.position - center.position, 90, 0.5f).setOnComplete(() =>
        {
            rotating = false;
        });
        print("left");
    }

    private void RandomRotate()
    {
        int choice = Random.Range(0, 5);

        if (choice == 0) TopRotate();
        if (choice == 1) BottomRotate();
        if (choice == 2) LeftRotate();
        if (choice == 3) RightRotate();
        if (choice == 4) BackRotate();
        if (choice == 5) FrontRotate();
    }

    private void Update()
    {
        if (rotating) return;

        RandomRotate();
    }
}
