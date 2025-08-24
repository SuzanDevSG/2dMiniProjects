using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public static AiController Instance { get; private set; }
    private Coroutine AiPlay;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AiTurn()
    {
        AiPlay = StartCoroutine(Play());
    }
    public void StopAiCorutine()
    {
        if (AiPlay != null)
        {
            StopCoroutine(AiPlay);
            AiPlay = null;
        }
    }

    private IEnumerator Play()
    {
        List<CellController> unmarkedCells = GridManager.Instance.GetAllUnMarkedCells();
        if (unmarkedCells.Count == 0) yield break;
        Debug.Log("Getting Best Move");

        yield return new WaitForSeconds(0.5f);

        CellController chosenCell = MiniMaxAI.Instance.GetBestMove(unmarkedCells);


        chosenCell.CellClicked();
    }
}
