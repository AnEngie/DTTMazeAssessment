using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MazeUiEvents : MonoBehaviour
{
    private UIDocument document;

    private Button GenerateGridButton;
    private Button GenerateMazeButton;
    private Button DestroyGridButton;

    private IntegerField GridColumnsField;
    private IntegerField GridRowField;

    private ProgressBar progressBar;

    [SerializeField]
    private MazeSpawner mazeSpawner;

    void Awake()
    {
        document = GetComponent<UIDocument>();

        AssignVisualElements();

        GenerateMazeButton.SetEnabled(false);
        DestroyGridButton.SetEnabled(false);
        progressBar.visible = false;
    }

    private void AssignVisualElements()
    {
        GenerateGridButton = document.rootVisualElement.Q("GenerateGrid") as Button;
        GenerateGridButton.RegisterCallback<ClickEvent>(OnGenerateGridClick);

        GenerateMazeButton = document.rootVisualElement.Q("GenerateMaze") as Button;
        GenerateMazeButton.RegisterCallback<ClickEvent>(OnGenerateMazeClick);

        DestroyGridButton = document.rootVisualElement.Q("DestroyGrid") as Button;
        DestroyGridButton.RegisterCallback<ClickEvent>(OnDestroyGridClick);

        GridColumnsField = document.rootVisualElement.Q("GridColumns") as IntegerField;
        GridRowField = document.rootVisualElement.Q("GridRows") as IntegerField;

        progressBar = document.rootVisualElement.Q("MazeProgressBar") as ProgressBar;
    }

    private void OnGenerateGridClick(ClickEvent evt)
    {
        GenerateMazeButton.SetEnabled(true);
        DestroyGridButton.SetEnabled(true);

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
        GenerateMazeButton.SetEnabled(false);
        DestroyGridButton.SetEnabled(false);

        mazeSpawner.RemoveGrid();
    }

    public void ActiveProgressBar(int maxValue)
    {
        progressBar.highValue = maxValue;
        progressBar.visible = true;
    }
    public void UpdateProgressBar(int progress)
    {
        progressBar.value = progress;

        if (progressBar.value == progressBar.highValue)
        {
            progressBar.visible = false;
        }
    }
}
