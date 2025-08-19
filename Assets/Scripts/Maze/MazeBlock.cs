using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBlock : MonoBehaviour
{
    public bool IsPath { get; private set; }

    [SerializeField]
    private GameObject UpperWall, LowerWall, LeftWall, RightWall, Inside;

    public void MakePath()
    {
        IsPath = true;
        Inside.SetActive(false);
    }

    public void RemoveUpperWall()
    {
        UpperWall.SetActive(false);
    }

    public void RemoveLowerWall()
    {
        LowerWall.SetActive(false);
    }

    public void RemoveLeftWall()
    {
        LeftWall.SetActive(false);
    }
    
    public void RemoveRightWall()
    {
        RightWall.SetActive(false);
    }
}