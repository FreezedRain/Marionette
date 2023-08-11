using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElement : MonoBehaviour
{

    protected Puzzle puzzle;

    public virtual void Input(Vector3 input, Vector3 dir)
    {
        print("MEC");
    }

    public virtual void SetPuzzle(Puzzle puzz)
    {
        this.puzzle = puzz;
    }

}
