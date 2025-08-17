using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public MazeBlock mazeBlock;

    [SerializeField]
    private int Columns = 2, Rows = 2;

    private MazeBlock[,] MazeGrid;

    void Start()
    {
        MazeGrid = new MazeBlock[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                MazeGrid[i, j] = Instantiate(mazeBlock, new Vector3(i, 0, j), Quaternion.identity);
            }
        }

        StartCoroutine(GenerateMaze(MazeGrid[0, 0]));
    }

    private IEnumerator GenerateMaze(MazeBlock currentBlock)
    {
        currentBlock.MakePath();

        var nextBlock = GetNextMazeBlock(currentBlock);

        if (nextBlock != null)
        {
            yield return GenerateMaze(nextBlock);
        }
    }

    private MazeBlock GetNextMazeBlock(MazeBlock currentBlock)
    {
        List<MazeBlock> unvisitedNeighbours = GetUnvisitedNeighbours(currentBlock);

        var nextBlock = Random.Range(0, unvisitedNeighbours.Count);

        return unvisitedNeighbours[nextBlock];
    }

    private List<MazeBlock> GetUnvisitedNeighbours(MazeBlock currentBlock)
    {
        int x = (int)currentBlock.transform.position.x;
        int z = (int)currentBlock.transform.position.z;
        List<MazeBlock> unvisitedNeighbours = new();

        if (x + 1 < Columns)
        {
            var blockRight = MazeGrid[x + 1, z];

            if (blockRight.IsPath == false)
            {
                unvisitedNeighbours.Add(blockRight);
            }
        }

        if (x - 1 >= 0)
        {
            var blockLeft = MazeGrid[x - 1, z];

            if (blockLeft.IsPath == false)
            {
                unvisitedNeighbours.Add(blockLeft);
            }
        }

        if (z + 1 < Rows)
        {
            var blockUp = MazeGrid[x, z + 1];

            if (blockUp.IsPath == false)
            {
                unvisitedNeighbours.Add(blockUp);
            }
        }

        if (z - 1 >= 0)
        {
            var blockDown = MazeGrid[x, z - 1];

            if (blockDown.IsPath == false)
            {
                unvisitedNeighbours.Add(blockDown);
            }
        }

        return unvisitedNeighbours;
    }
}
