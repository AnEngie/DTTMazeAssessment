using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public MazeBlock mazeBlock;

    [SerializeField]
    private MazeSpawner mazeParent; // Needed to assign individual blocks under Maze parent class

    public int Columns = 2, Rows = 2;
    private int OldColumns, OldRows;

    [SerializeField]
    private float mazeGenerationDelay = 0.05f;

    private MazeBlock[,] MazeGrid;

    private bool MazeGenerated = false;
    private bool GridGenerated = false;

    public void GenerateGrid()
    {
        Debug.Log("Start Grid Generation");

        if (GridGenerated)
        {
            RemoveGrid();
        }

        MazeGrid ??= new MazeBlock[Columns, Rows];

        for (int i = 0; i < Columns; i++) // Generate grid size to its given size
        {
            for (int j = 0; j < Rows; j++)
            {
                MazeGrid[i, j] = Instantiate(mazeBlock, new Vector3(i, 0, j), Quaternion.identity, mazeParent.transform);
            }
        }

        OldColumns = Columns;
        OldRows = Rows;
        GridGenerated = true;
    }

    public void RemoveGrid()
    {
        Debug.Log("Remove Grid");

        for (int i = 0; i < OldColumns; i++) // Generate grid size to its given size
        {
            for (int j = 0; j < OldRows; j++)
            {
                Destroy(MazeGrid[i, j].gameObject);
            }
        }

        MazeGrid = null;
        MazeGenerated = false;
        GridGenerated = false;
    }

    public void StartMazeGeneration()
    {
        if (MazeGenerated)
        {
            RemoveGrid();
            GenerateGrid();
        }

        if (!GridGenerated)
        {
            GenerateGrid();
        }

        Debug.Log("Start Maze Generation");
        StartCoroutine(GenerateMaze(null, MazeGrid[0, 0])); // Start generating paths in grid
        MazeGenerated = true;
    }

    private IEnumerator GenerateMaze(MazeBlock previousBlock, MazeBlock currentBlock)
    {
        currentBlock.MakePath(); // Mark block as visited by algorithm

        RemoveWall(previousBlock, currentBlock);

        yield return new WaitForSeconds(mazeGenerationDelay);

        MazeBlock nextBlock;

        do
        {
            nextBlock = GetNextMazeBlock(currentBlock);

            if (nextBlock != null)
            {
                yield return GenerateMaze(currentBlock, nextBlock);
            }
        } while (nextBlock != null); // Makes algorithm visit older blocks when path hits dead end
    }

    private void RemoveWall(MazeBlock previousBlock, MazeBlock currentBlock)
    {
        if (previousBlock == null)
        {
            return;
        }

        if (previousBlock.transform.position.x < currentBlock.transform.position.x)
        {
            previousBlock.RemoveRightWall();
            currentBlock.RemoveLeftWall();
            return;
        }

        if (previousBlock.transform.position.x > currentBlock.transform.position.x)
        {
            previousBlock.RemoveLeftWall();
            currentBlock.RemoveRightWall();
            return;
        }


        if (previousBlock.transform.position.z < currentBlock.transform.position.z)
        {
            previousBlock.RemoveUpperWall();
            currentBlock.RemoveLowerWall();
            return;
        }


        if (previousBlock.transform.position.z > currentBlock.transform.position.z)
        {
            previousBlock.RemoveLowerWall();
            currentBlock.RemoveUpperWall();
            return;
        }
    }

    private MazeBlock GetNextMazeBlock(MazeBlock currentBlock)
    {
        List<MazeBlock> unvisitedNeighbours = GetUnvisitedNeighbours(currentBlock);
        MazeBlock nextBlock = null;

        if (unvisitedNeighbours.Count != 0) // Gets random neighbour from all available ones
        {
            var chosenBlock = Random.Range(0, unvisitedNeighbours.Count);
            nextBlock = unvisitedNeighbours[chosenBlock];
        }

        return nextBlock; // Returns null if block has no neighbours, otherwise returns the chosen block
    }

    private List<MazeBlock> GetUnvisitedNeighbours(MazeBlock currentBlock)
    {
        int x = (int)currentBlock.transform.position.x;
        int z = (int)currentBlock.transform.position.z;
        List<MazeBlock> unvisitedNeighbours = new();

        if (x + 1 < Columns) // Check if coords are inside grid
        {
            var blockRight = MazeGrid[x + 1, z];

            if (blockRight.IsPath == false)
            {
                unvisitedNeighbours.Add(blockRight);
            }
        }

        if (x - 1 >= 0) // Check if coords are inside grid
        {
            var blockLeft = MazeGrid[x - 1, z];

            if (blockLeft.IsPath == false)
            {
                unvisitedNeighbours.Add(blockLeft);
            }
        }

        if (z + 1 < Rows) // Check if coords are inside grid
        {
            var blockUp = MazeGrid[x, z + 1];

            if (blockUp.IsPath == false)
            {
                unvisitedNeighbours.Add(blockUp);
            }
        }

        if (z - 1 >= 0) // Check if coords are inside grid
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