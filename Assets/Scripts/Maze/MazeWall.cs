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

        boxCollider.isTrigger = true;
        meshRenderer.enabled = false;
    }

    public void DisableWall()
    {
        if (!DisabledByPath) // enable wall
        {
            boxCollider.isTrigger = false;
            meshRenderer.enabled = true;
        }
        else if (DisabledByPath) // disable wall
        {
            boxCollider.isTrigger = true;
            meshRenderer.enabled = false;
        }
    }

    public void ResetWall()
    {
        boxCollider.isTrigger = true;
        meshRenderer.enabled = false;

        DisabledByPath = false;
    }
}
