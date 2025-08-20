using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public int columns = 2, rows = 2;

    [SerializeField]
    private int maxColumns = 250, maxRows = 250;

    [SerializeField]
    private MazeSpawner mazeParent; // Needed to assign individual blocks under Maze parent class

    [SerializeField]
    private MazeBlock mazeBlock;

    [SerializeField]
    private MazeUIEvents mazeUIEvents;


    private MazeBlock[,] MazeGrid;

    private bool MazeGenerated = false;

    private int progress = 0;

    void Start()
    {
        MazeGrid = new MazeBlock[maxColumns, maxRows];

        GenerateGrid(maxColumns, maxRows);
    }

    private void GenerateGrid(int columns, int rows)
    {
        for (int i = 0; i < columns; i++) // Generate grid size to its given size
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 blockPos = new(i, 0, j);
                MazeGrid[i, j] = Instantiate(mazeBlock, blockPos, Quaternion.identity, mazeParent.transform);

                StaticBatchingUtility.Combine(MazeGrid[i, j].gameObject);
            }
        }
    }

    public void StartMazeGeneration()
    {
        mazeUIEvents.ActiveProgressBar(MazeGrid.Length - 1);
        progress = 0;

        if (MazeGenerated)
        {
            Debug.Log("Stop Maze Generation");
            ResetMaze();
            
            MazeGenerated = false;
        }

        Debug.Log("Maze Generation Started");
        MazeGenerated = true;
        StartCoroutine(GenerateMaze(null, MazeGrid[0, 0]));  // Start generating paths in grid
    }

    private void ResetMaze()
    {
        StopAllCoroutines();

        for (int i = 0; i < maxColumns; i++) // Generate grid size to its given size
        {
            for (int j = 0; j < maxRows; j++)
            {
                MazeGrid[i, j].ResetBlock();
            }
        }
    }

    private IEnumerator GenerateMaze(MazeBlock previousBlock, MazeBlock currentBlock)
    {
        currentBlock.MakePath(); // Mark block as visited by algorithm

        RemoveWall(previousBlock, currentBlock);

        MazeBlock nextBlock;

        do
        {
            nextBlock = GetNextMazeBlock(currentBlock);

            if (nextBlock != null)
            {
                progress++;
                mazeUIEvents.UpdateProgressBar(progress);

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

        if (x + 1 < columns) // Check if coords are inside grid
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

        if (z + 1 < rows) // Check if coords are inside grid
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