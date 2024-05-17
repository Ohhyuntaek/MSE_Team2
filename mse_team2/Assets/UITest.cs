using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units;
using TbsFramework.Cells;
using TbsFramework.Gui;


public class UITest : MonoBehaviour
{
    [SerializeField]
    CellGrid cellGrid;

    [SerializeField]
    Cell targetCell;

    int currentPlayer = 0;
    public int index = 0;
    public int spawnNumber = 0;
    public bool isAbleSpawn = false;

    [SerializeField]
    Player[] players;
    public Text infoText;
    public Button nextTurnButton;

    [SerializeField]
    LayerMask cellLayer;
    PrefabManager prefabManager
    {
        get { return FindObjectOfType<PrefabManager>(); }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetTurnText();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (cellGrid.currentState != CellGrid.GameState.Spawn) return;
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100, cellLayer);

            if(hits.Length > 0)
            {
                OnCellClicked(hits[0].transform.GetComponent<Cell>());
            }
        }
    }

    public void TestButton()
    {
        Unit unit = Instantiate(prefabManager.unitPrefabs[index], targetCell.transform.position, Quaternion.identity);
        cellGrid.AddUnit(unit.transform, targetCell);
    }

    public void NumBtn(int index)
    {
        this.index = index;
        isAbleSpawn = true;
    }

    public void OnCellClicked(Cell cell)
    {

        if (isAbleSpawn == true)
        {
            Unit unit = Instantiate(prefabManager.unitPrefabs[index], cell.transform.position, Quaternion.identity);
            cellGrid.AddUnit(unit.transform, cell, players[currentPlayer]);
            isAbleSpawn = false;
            spawnNumber++;

            nextTurnButton.interactable = true;

            cellGrid.PlayableUnits().Add(unit);
        }

    }

    public void EndTurn()
    {
        if (spawnNumber == 0) { return; }

        switch(currentPlayer)
        {
            case 0:
                currentPlayer = 1; 
                SetTurnText();
                break;
            case 1:
                cellGrid.InitializeAndStart();
                break;
            default:
                break;
        }
    }

    public void SetTurnText()
    {
        nextTurnButton.interactable = false;
        infoText.text = "Player " + (currentPlayer + 1);
    }
}
