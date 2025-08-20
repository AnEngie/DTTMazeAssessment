using UnityEngine;
using UnityEngine.UIElements;

public class MazeUIEvents : MonoBehaviour
{
    [SerializeField]
    private MazeSpawner mazeSpawner;


    [Header("Events")]
    public GameEvent onSwitchFlight;

    
    private UIDocument document;

    // Buttons
    private Button GenerateMazeButton;

    // InputFields
    private IntegerField ColumnsField;
    private IntegerField RowField;

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
        GenerateMazeButton = document.rootVisualElement.Q("GenerateMazeBTTN") as Button;
        GenerateMazeButton.RegisterCallback<ClickEvent>(OnGenerateMazeClick);

        ColumnsField = document.rootVisualElement.Q("ColumnsField") as IntegerField;
        RowField = document.rootVisualElement.Q("RowsField") as IntegerField;

        progressBar = document.rootVisualElement.Q("MazeProgressBar") as ProgressBar;
    }

    private void OnGenerateMazeClick(ClickEvent evt)
    {        
        mazeSpawner.columns = ColumnsField.value;
        mazeSpawner.rows = RowField.value;

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
