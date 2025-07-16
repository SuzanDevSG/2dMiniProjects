using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<Button> Cells;
    private readonly List<Vector3Int> winPatterns = new()
    {
        new Vector3Int(1,2,3),
        new Vector3Int(4,5,6),
        new Vector3Int(7,8,9),
        new Vector3Int(1,4,7),
        new Vector3Int(2,5,8),
        new Vector3Int(3,6,9),
        new Vector3Int(1,5,9),
        new Vector3Int(3,5,7),
    };

    private List<int> PlayerXCells = new();
    private List<int> PlayerOCells = new();

    public Sprite PlayerX;
    public Sprite PlayerO;

    private bool IsPlayerXturn = true;

    public UnityAction SwitchPlayerTurn;

    void Start()
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            int index = i;
            Cells[i].onClick.AddListener(() => OnCellClicked(Cells[index],index+1));
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            int index = i;
            Cells[i].onClick.RemoveListener(() => OnCellClicked(Cells[index],index+1));
        }
    }
    private void OnCellClicked(Button btn,int index)
    {
        if (IsPlayerXturn)
        {
            PlayerXCells.Add(index);
            btn.GetComponent<Image>().sprite = PlayerX;

        }
        else
        {
            PlayerOCells.Add(index);
            btn.GetComponent<Image>().sprite = PlayerO;
        }

        SwitchPlayerTurn.Invoke();

        IsPlayerXturn = !IsPlayerXturn;
        btn.enabled = false;

        bool CheckPlayerX = CheckWinState(PlayerXCells);
        bool CheckPlayerO = CheckWinState(PlayerOCells);

        if (CheckPlayerX)
        {
            Debug.Log($"PlayerX wins ");
        }
        if (CheckPlayerO)
        {
            Debug.Log($"PlayerO wins ");
        }

    }
    private bool CheckWinState(List<int> OccupiedCells)
    {
        return winPatterns.Any(p =>
        OccupiedCells.Contains(p.x) &&
        OccupiedCells.Contains(p.y) &&
        OccupiedCells.Contains(p.z));
    }
}
