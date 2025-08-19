using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (!UpperWall.IsDestroyed())
        {
            Destroy(UpperWall);
        }

        if (!LowerWall.IsDestroyed())
        {
            LowerWall.SetActive(true);
        }

        if (!LeftWall.IsDestroyed())
        {
            LeftWall.SetActive(true);
        }

        if (!RightWall.IsDestroyed())
        {
            RightWall.SetActive(true);
        }
    }

    public void RemoveLowerWall()
    {
        if (!LowerWall.IsDestroyed())
        {
            Destroy(LowerWall);
        }

        if (!UpperWall.IsDestroyed())
        {
            UpperWall.SetActive(true);
        }

        if (!LeftWall.IsDestroyed())
        {
            LeftWall.SetActive(true);
        }

        if (!RightWall.IsDestroyed())
        {
            RightWall.SetActive(true);
        }
    }

    public void RemoveLeftWall()
    {
        if (!LeftWall.IsDestroyed())
        {
            Destroy(LeftWall);
        }

        if (!LowerWall.IsDestroyed())
        {
            LowerWall.SetActive(true);
        }

        if (!UpperWall.IsDestroyed())
        {
            UpperWall.SetActive(true);
        }

        if (!RightWall.IsDestroyed())
        {
            RightWall.SetActive(true);
        }
    }

    public void RemoveRightWall()
    {
        if (!RightWall.IsDestroyed())
        {
            Destroy(RightWall);
        }

        if (!LowerWall.IsDestroyed())
        {
            LowerWall.SetActive(true);
        }

        if (!UpperWall.IsDestroyed())
        {
            UpperWall.SetActive(true);
        }

        if (!LeftWall.IsDestroyed())
        {
            LeftWall.SetActive(true);
        }
    }
}