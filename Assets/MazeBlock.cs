using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBlock : MonoBehaviour
{
    public bool IsPath { get; private set; }

    public void MakePath()
    {
        IsPath = true;
        gameObject.SetActive(false);

        // var visible = gameObject.GetComponent<MeshRenderer>();
        // visible.enabled = false;
    }
}