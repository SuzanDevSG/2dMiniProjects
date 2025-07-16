using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    [SerializeField] private GameObject cell;
    [SerializeField] private RectTransform gridParent;

    [SerializeField] private int gridSize = 3;
    [SerializeField] private CellController[,] cellController;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gridSize = SettingsManager.Instance.GridSize;
        GenerateGrid(gridSize);
        TurnManager.Instance.SetGridSize(gridSize);
        WinChecker.Instance.SetGridCells(cellController, gridSize);
    }
    private void GenerateGrid(int size)
    {
        // check if any object in grid parent is available
        foreach (Transform child in gridParent)
        {
            Destroy(child);
        }

        //attributes for the grid
        GridLayoutGroup layout = gridParent.GetComponent<GridLayoutGroup>();
        float totalGridSize = gridParent.rect.width;
        float cellSize = totalGridSize / size;

        // Format the grid for the cells
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = size;
        layout.cellSize = new Vector2(cellSize, cellSize);

        // Initialize the array first
        cellController = new CellController[size, size];

        // spawn cells in the grid .
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                GameObject newCell = Instantiate(cell, gridParent);
                CellController newCellController = newCell.GetComponent<CellController>();
                newCellController.InitializeRowCol(row, col); // send row col data of the current cell
                cellController[row, col] = newCellController;
            }
        }
    }
    public List<CellController> GetAllUnMarkedCells()
    {
        List<CellController> unmarkedCells = new List<CellController>();
        foreach (CellController cells in cellController)
        {
            if (!cells.IsMarked)
            {
                unmarkedCells.Add(cells);
            }
        }
        return unmarkedCells;
    }
}
