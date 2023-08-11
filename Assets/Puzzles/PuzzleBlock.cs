using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : PuzzleElement
{

    private Vector3 tpos;

    private void Start()
    {
        tpos = transform.position;
    }

    public override void Input(Vector3 input, Vector3 dir)
    {

        Vector3 newPos = transform.position + dir;

        if (IsInsideSquare(newPos) && IsFree(newPos))
        {
            tpos = new Vector3Int(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z));
        }
    }

    private void Update()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, tpos, Time.deltaTime * 10);

        transform.position = newPos;
    }

    public bool IsInsideSquare(Vector3 point)
    {
        Vector3 corner1 = puzzle.topLeftLimit.position;
        Vector3 corner2 = puzzle.bottomRightLimit.position;
        
        // Ignore the y component of the positions
        corner1.y = corner2.y = point.y = 0;

        // Calculate the minimum and maximum x and z values of the square
        float minX = Mathf.Min(corner1.x, corner2.x);
        float maxX = Mathf.Max(corner1.x, corner2.x);
        float minZ = Mathf.Min(corner1.z, corner2.z);
        float maxZ = Mathf.Max(corner1.z, corner2.z);

        // Check if the point's x and z values are within the square's bounds
        bool insideX = point.x >= minX && point.x <= maxX;
        bool insideZ = point.z >= minZ && point.z <= maxZ;

        return insideX && insideZ;
    }

    private bool IsFree(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, 0.1f, LayerMask.GetMask("Ground"));

        return colliders.Length == 0;
    }
}
