using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeUiEvents : MonoBehaviour
{
    private UIDocument document;

    private Button GenerateGridButton;
    private Button GenerateMazeButton;
    private Button DestroyGridButton;

    private IntegerField GridColumnsField;
    private IntegerField GridRowField;


    [SerializeField]
    private MazeSpawner mazeSpawner;

    void Awake()
    {
        document = GetComponent<UIDocument>();

        GenerateGridButton = document.rootVisualElement.Q("GenerateGrid") as Button;
        GenerateGridButton.RegisterCallback<ClickEvent>(OnGenerateGridClick);

        GenerateMazeButton = document.rootVisualElement.Q("GenerateMaze") as Button;
        GenerateMazeButton.RegisterCallback<ClickEvent>(OnGenerateMazeClick);

        DestroyGridButton = document.rootVisualElement.Q("DestroyGrid") as Button;
        DestroyGridButton.RegisterCallback<ClickEvent>(OnDestroyGridClick);

        GridColumnsField = document.rootVisualElement.Q("GridColumns") as IntegerField;
        GridRowField = document.rootVisualElement.Q("GridRows") as IntegerField;
    }

    void OnDisable()
    {
        GenerateGridButton.UnregisterCallback<ClickEvent>(OnGenerateGridClick);

        GenerateMazeButton.UnregisterCallback<ClickEvent>(OnGenerateMazeClick);

        DestroyGridButton.UnregisterCallback<ClickEvent>(OnDestroyGridClick);
    }

    private void OnGenerateGridClick(ClickEvent evt)
    {
        mazeSpawner.Columns = GridColumnsField.value;
        mazeSpawner.Rows = GridRowField.value;
        mazeSpawner.GenerateGrid();
    }

    private void OnGenerateMazeClick(ClickEvent evt)
    {
        mazeSpawner.Columns = GridColumnsField.value;
        mazeSpawner.Rows = GridRowField.value;
        mazeSpawner.StartMazeGeneration();
    }
    
    private void OnDestroyGridClick(ClickEvent evt)
    {
        mazeSpawner.RemoveGrid();
    }
}
