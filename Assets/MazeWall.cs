using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class MazeWall : MonoBehaviour
{
    public bool DisabledByPath = false;

    public BoxCollider boxCollider;
    public MeshRenderer meshRenderer;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();

        boxCollider.enabled = false;
        meshRenderer.enabled = false;
    }

    public void DisableWall()
    {
        if (!DisabledByPath)
        {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;
        }
        else if (DisabledByPath)
        {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    public void ResetWall()
    {
        boxCollider.enabled = false;
        meshRenderer.enabled = false;

        DisabledByPath = false;
    }
}
