using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public List<PuzzleElement> elements = new List<PuzzleElement>();

    public Transform topLeftLimit;
    public Transform bottomRightLimit;

    private void Start()
    {
        foreach (PuzzleElement el in elements)
        {
            el.SetPuzzle(this);
        }
    }

}
