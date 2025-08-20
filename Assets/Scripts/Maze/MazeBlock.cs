using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class MazeBlock : MonoBehaviour
{
    public bool IsPath { get; private set; }

    [SerializeField]
    private MazeWall UpperWall, LowerWall, LeftWall, RightWall;

    public void MakePath()
    {
        IsPath = true;
    }

    public void ResetBlock()
    {
        IsPath = false;

        UpperWall.ResetWall();
        LowerWall.ResetWall();
        LeftWall.ResetWall();
        RightWall.ResetWall();
    }

    public void RemoveWall(int WallNumber)
    {
        switch (WallNumber)
        {
            case 1:
                UpperWall.DisabledByPath = true;
                break;
            case 2:
                LowerWall.DisabledByPath = true;
                break;
            case 3:
                LeftWall.DisabledByPath = true;
                break;
            case 4:
                RightWall.DisabledByPath = true;
                break;
        }

        UpperWall.DisableWall();
        LowerWall.DisableWall();
        LeftWall.DisableWall();
        RightWall.DisableWall();
    }
}