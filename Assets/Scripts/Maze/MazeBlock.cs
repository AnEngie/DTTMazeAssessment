using System.Collections.Generic;
using UnityEngine;

public class MazeBlock : MonoBehaviour
{
    public bool IsPath { get; private set; }

    [SerializeField]
    private List<MazeWall> mazeWalls;

    public void MakePath()
    {
        IsPath = true; // Stop algorithm from revisiting
    }

    public void ResetBlock()
    {
        IsPath = false;

        foreach (var wallType in mazeWalls)
        {
            wallType.ResetWall();
        }
    }

    public void RemoveWall(int wall)
    {
        // Disable chosen wall and enable all others
        mazeWalls[wall].DisabledByPath = true;

        foreach (var wallType in mazeWalls)
        {
            wallType.DisableWall();
        }
    }
}