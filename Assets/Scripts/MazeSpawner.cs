using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeSpawner : MonoBehaviour
{
    public int[,] Maze;

    public GameObject MazeBlock;

    public GameObject MazeParent;

    [SerializeField]
    private int Columns = 2;

    [SerializeField]
    private int Rows = 2;

    // Start is called before the first frame update
    void Start()
    {
        Maze = new int[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                Instantiate(MazeBlock, new Vector3(MazeParent.transform.position.x + i, transform.position.y, MazeParent.transform.position.z + j), Quaternion.identity, MazeParent.transform);
            }
        }
    }
}
