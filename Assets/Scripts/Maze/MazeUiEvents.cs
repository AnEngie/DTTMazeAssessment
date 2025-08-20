using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class MazeUIEvents : MonoBehaviour
{
    [SerializeField]
    private MazeSpawner mazeSpawner;
    
    
    private UIDocument document;

    // Buttons
    private Button generateMazeButton;

    // InputFields
    private IntegerField columnsField;
    private IntegerField rowField;

    // Progressbar
    private ProgressBar progressBar;

    void Awake()
    {
        document = GetComponent<UIDocument>();

        AssignVisualElements();

        progressBar.visible = false;
    }

    private void AssignVisualElements()
    {
        generateMazeButton = document.rootVisualElement.Q("GenerateMazeBTTN") as Button;
        generateMazeButton.RegisterCallback<ClickEvent>(OnGenerateMazeClick);

        columnsField = document.rootVisualElement.Q("ColumnsField") as IntegerField;
        rowField = document.rootVisualElement.Q("RowsField") as IntegerField;

        progressBar = document.rootVisualElement.Q("MazeProgressBar") as ProgressBar;
    }

    private void OnGenerateMazeClick(ClickEvent evt)
    {
        if (columnsField.value > mazeSpawner.maxColumns || rowField.value > mazeSpawner.maxRows)
        {
            columnsField.value = mazeSpawner.maxColumns;
            rowField.value = mazeSpawner.maxColumns;
            return;
        }

        if (columnsField.value < 10 || rowField.value < 10)
        {
            columnsField.value = 10;
            rowField.value = 10;
            return;
        }
        
        mazeSpawner.columns = columnsField.value;
        mazeSpawner.rows = rowField.value;

        Debug.Log("Start Maze Generation");
        mazeSpawner.StartMazeGeneration();
    }

    public void ActiveProgressBar(int maxValue)
    {
        progressBar.highValue = maxValue - 1;
        progressBar.visible = true;
    }

    public void UpdateProgressBar(int progress)
    {
        progressBar.value = progress;

        if (progressBar.value >= progressBar.highValue)
        {
            progressBar.visible = false;
        }
    }
}
