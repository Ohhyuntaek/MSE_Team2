using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Cells;

public class UITest : MonoBehaviour
{
    [SerializeField]
    CellGrid cellGrid;

    [SerializeField]
    Unit unitPrefab;

    [SerializeField]
    Cell targetCell;
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
        Unit unit = Instantiate(unitPrefab, targetCell.transform.position, Quaternion.identity);
        cellGrid.AddUnit(unit.transform, targetCell);
    }
}
