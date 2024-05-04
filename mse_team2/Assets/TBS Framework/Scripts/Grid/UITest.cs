using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units;
using TbsFramework.Cells;

public class UITest : MonoBehaviour
{
    [SerializeField]
    CellGrid cellGrid;


    [SerializeField]
    Cell targetCell;

    public int index = 0;
    public bool isAbleSpawn = false;

    [SerializeField]
    Player[] players;

    PrefabManager prefabManager
    {
        get { return FindObjectOfType<PrefabManager>(); }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            cellGrid.AddUnit(unit.transform, cell, players[cellGrid.CurrentPlayerNumber]);
            isAbleSpawn = false;

            cellGrid.PlayableUnits().Add(unit);
        }

    }
}
