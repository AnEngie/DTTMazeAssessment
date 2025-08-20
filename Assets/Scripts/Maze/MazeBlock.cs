using System;
using System.Collections;
using System.Collections.Generic;
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

    public void RemoveUpperWall()
    {
        UpperWall.DisabledByPath = true;
        UpperWall.DisableWall();
        LowerWall.DisableWall();
        LeftWall.DisableWall();
        RightWall.DisableWall();
    }

    public void RemoveLowerWall()
    {
        LowerWall.DisabledByPath = true;
        UpperWall.DisableWall();
        LowerWall.DisableWall();
        LeftWall.DisableWall();
        RightWall.DisableWall();
    }

    public void RemoveLeftWall()
    {
        LeftWall.DisabledByPath = true;
        UpperWall.DisableWall();
        LowerWall.DisableWall();
        LeftWall.DisableWall();
        RightWall.DisableWall();
    }

    public void RemoveRightWall()
    {
        RightWall.DisabledByPath = true;
        UpperWall.DisableWall();
        LowerWall.DisableWall();
        LeftWall.DisableWall();
        RightWall.DisableWall();
    }
}