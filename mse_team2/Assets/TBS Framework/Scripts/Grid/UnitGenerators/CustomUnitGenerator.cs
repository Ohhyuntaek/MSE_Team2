using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Grid.UnitGenerators
{
    public class CustomUnitGenerator : MonoBehaviour, IUnitGenerator
    {
        public Transform UnitsParent;
        public Transform CellsParent;

        [SerializeField]
        Unit unitPrefab;

        /// <summary>
        /// Returns units that are children of UnitsParent object.
        /// </summary>
        public List<Unit> SpawnUnits(List<Cell> cells)
        {

            //UnitsParent 오브젝트 하위에 있는 유닛 클래스 상속받는 모든 오브젝트 리스트에 대입
            List<Unit> ret = new List<Unit>();
            for (int i = 0; i < UnitsParent.childCount; i++)
            {
                var unit = UnitsParent.GetChild(i).GetComponent<Unit>();
                // var unit = Instantiate(unitPrefab);
                if (unit != null)
                {
                    ret.Add(unit);
                }
                else
                {
                    Debug.LogError("Invalid object in Units Parent game object");
                }
            }
            return ret;
        }

        public void SnapToGrid()
        {

        }
    }
}
