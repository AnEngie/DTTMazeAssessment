using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public int columns = 10, rows = 10;

    public int maxColumns = 250, maxRows = 250;

    [SerializeField]
    private MazeSpawner mazeParent; // Needed to assign individual blocks under Maze parent class

    [SerializeField]
    private MazeBlock mazeBlock;

    private enum WallTypes // The different types of wall that can be disabled
    {
        UpperWall,
        LowerWall,
        LeftWall,
        RightWall
    };

    [SerializeField]
    private MazeUIEvents mazeUIEvents;

    [Header("Events")]
    public GameEvent OnLoaded;


    private MazeBlock[,] MazeGrid;

    private bool MazeGenerated = false;

    private int progress = 0;

    void Start()
    {
        // Pre-generate Grid as to reduce lag when big mazes are requested

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

                StaticBatchingUtility.Combine(MazeGrid[i, j].gameObject); // Batch Game Objects with same materials together for better performance
            }
        }

        OnLoaded.TriggerEvent();
    }

    public void StartMazeGeneration()
    {
        mazeUIEvents.ActiveProgressBar(columns * rows);
        progress = 0;

        if (MazeGenerated) // Reset the maze back before generating again
        {
            ResetMaze();

            MazeGenerated = false;
        }

        StartCoroutine(GenerateMaze());
    }

    private IEnumerator GenerateMaze()
    {
        // When Algorithm is done generate entrance and exit

        yield return StartAlgorithm(null, MazeGrid[0, 0]);

        MazeGrid[0, 0].RemoveWall((int)WallTypes.LeftWall);
        MazeGrid[0, 0].RemoveWall((int)WallTypes.LowerWall);

        MazeGenerated = true;
    }

    private IEnumerator StartAlgorithm(MazeBlock previousBlock, MazeBlock currentBlock)
    {
        currentBlock.MakePath(); // Mark block as visited by algorithm

        RemoveWall(previousBlock, currentBlock); // Remove walls between the 2 blocks

        MazeBlock nextBlock;

        do
        {
            nextBlock = GetNextMazeBlock(currentBlock);

            if (nextBlock != null)
            {
                progress++;
                mazeUIEvents.UpdateProgressBar(progress);

                yield return StartAlgorithm(currentBlock, nextBlock);

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
            previousBlock.RemoveWall((int)WallTypes.RightWall);
            currentBlock.RemoveWall((int)WallTypes.LeftWall);
            return;
        }

        if (previousBlock.transform.position.x > currentBlock.transform.position.x)
        {
            previousBlock.RemoveWall((int)WallTypes.LeftWall);
            currentBlock.RemoveWall((int)WallTypes.RightWall);
            return;
        }

        if (previousBlock.transform.position.z < currentBlock.transform.position.z)
        {
            previousBlock.RemoveWall((int)WallTypes.UpperWall);
            currentBlock.RemoveWall((int)WallTypes.LowerWall);
            return;
        }


        if (previousBlock.transform.position.z > currentBlock.transform.position.z)
        {
            previousBlock.RemoveWall((int)WallTypes.LowerWall);
            currentBlock.RemoveWall((int)WallTypes.UpperWall);
        }
    }

    private MazeBlock GetNextMazeBlock(MazeBlock currentBlock)
    {
        List<MazeBlock> unvisitedNeighbours = GetUnvisitedNeighbours(currentBlock);
        MazeBlock nextBlock = null;

        if (unvisitedNeighbours.Count != 0) // Gets random neighbour from all available ones
        {
            int chosenBlock = Random.Range(0, unvisitedNeighbours.Count);
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

    private void ResetMaze()
    {
        StopAllCoroutines(); // Stop Maze Generation coroutine if it was active

        for (int i = 0; i < maxColumns; i++) // Generate grid size to its given size
        {
            for (int j = 0; j < maxRows; j++)
            {
                MazeGrid[i, j].ResetBlock(); // Set Mesh renderer and box collider to unactive
            }
        }
    }
}