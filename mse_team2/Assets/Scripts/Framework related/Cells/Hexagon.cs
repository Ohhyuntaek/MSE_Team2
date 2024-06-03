using System.Collections.Generic;
using UnityEngine;

namespace TbsFramework.Cells
{
    /// <summary>
    /// Implementation of hexagonal cell.
    /// </summary>
    public abstract class Hexagon : Cell
    {
        List<Cell> neighbours = null; // 인접셀들을 저장하는 리스트
        /// <summary>
        /// HexGrids comes in four types regarding the layout. 
        /// This distinction is necessary to convert cube coordinates to offset and vice versa.
        /// HexGrids는 레이아웃에 따라 네 가지 유형이 있습니다.
        /// 이 구분은 큐브 좌표와 오프셋 좌표 간의 변환에 필요합니다.
        /// </summary>
        [HideInInspector]
        public HexGridType HexGridType; // 헥사그리드 유형

        protected Vector3 CubeCoord
        {
            get
            {
                return OffsetToCubeCoords(OffsetCoord, HexGridType);
            }
        }

        /// <summary>
        /// Converts offset coordinates into cube coordinates.
        /// Cube coordinates is another system of coordinates that makes calculation on hex grids easier.
        /// 오프셋 좌표를 큐브 좌표로 변환합니다.
        /// 큐브 좌표는 헥사 그리드 계산을 용이하게 하는 좌표 체계입니다.
        /// </summary>
        protected Vector3 OffsetToCubeCoords(Vector2 offsetCoords, HexGridType gridType)
        {
            Vector3 cubeCoords = new Vector3(); // 큐브 좌표를 저장할 변수
            switch (gridType)
            {
                case HexGridType.odd_q:
                    {
                        cubeCoords.x = offsetCoords.x;
                        cubeCoords.z = offsetCoords.y - (offsetCoords.x + (Mathf.Abs(offsetCoords.x) % 2)) / 2;
                        cubeCoords.y = -cubeCoords.x - cubeCoords.z;
                        break;
                    }
                case HexGridType.even_q:
                    {
                        cubeCoords.x = offsetCoords.x;
                        cubeCoords.z = offsetCoords.y - (offsetCoords.x - (Mathf.Abs(offsetCoords.x) % 2)) / 2;
                        cubeCoords.y = -cubeCoords.x - cubeCoords.z;
                        break;
                    }
                case HexGridType.odd_r:
                    {
                        cubeCoords.x = OffsetCoord.x - (OffsetCoord.y - (Mathf.Abs(OffsetCoord.y) % 2)) / 2;
                        cubeCoords.z = OffsetCoord.y;
                        cubeCoords.y = -cubeCoords.x - cubeCoords.z;
                        break;
                    }
                case HexGridType.even_r:
                    {
                        cubeCoords.x = OffsetCoord.x - (OffsetCoord.y + (Mathf.Abs(OffsetCoord.y) % 2)) / 2;
                        cubeCoords.z = OffsetCoord.y;
                        cubeCoords.y = -cubeCoords.x - cubeCoords.z;
                        break;
                    }
            }
            return cubeCoords; // 계산된 큐브 좌표 반환
        }
        /// <summary>
        /// Converts cube coordinates back to offset coordinates.
        /// 큐브 좌표를 오프셋 좌표로 다시 변환
        /// </summary>
        /// <param name="cubeCoords">Cube coordinates to convert. 변환할 큐브 좌표</param>
        /// <returns>Offset coordinates corresponding to given cube coordinates. 해당 큐브 좌표에 대응하는 오프셋 좌표</returns>
        protected Vector2 CubeToOffsetCoords(Vector3 cubeCoords)
        {
            Vector2 offsetCoords = new Vector2(); // 오프셋 좌표를 저장할 변수

            switch (HexGridType)
            {
                case HexGridType.odd_q:
                    {
                        offsetCoords.x = cubeCoords.x;
                        offsetCoords.y = cubeCoords.z + (cubeCoords.x + (Mathf.Abs(cubeCoords.x) % 2)) / 2;
                        break;
                    }
                case HexGridType.even_q:
                    {
                        offsetCoords.x = cubeCoords.x;
                        offsetCoords.y = cubeCoords.z + (cubeCoords.x - (Mathf.Abs(cubeCoords.x) % 2)) / 2;
                        break;
                    }
                case HexGridType.odd_r:
                    {
                        offsetCoords.x = cubeCoords.x + (cubeCoords.z - (Mathf.Abs(cubeCoords.z) % 2)) / 2;
                        offsetCoords.y = cubeCoords.z;
                        break;
                    }
                case HexGridType.even_r:
                    {
                        offsetCoords.x = cubeCoords.x + (cubeCoords.z + (Mathf.Abs(cubeCoords.z) % 2)) / 2;
                        offsetCoords.y = cubeCoords.z;
                        break;
                    }
            }
            return offsetCoords; // 계산된 오프셋 좌표 변환
        }

        protected static readonly Vector3[] _directions =  {
        new Vector3(+1, -1, 0), new Vector3(+1, 0, -1), new Vector3(0, +1, -1),
        new Vector3(-1, +1, 0), new Vector3(-1, 0, +1), new Vector3(0, -1, +1)};

        public override int GetDistance(Cell other)
        {
            var _other = other as Hexagon; // 다른 셀을 Hexagon으로 변환

            var cubeCoords = CubeCoord; // 현재 셀의 큐브 좌표
            var otherCubeCoords = _other.CubeCoord; // 다른 셀의 큐브 좌표

            // 두 셀 간의 거리르 맨해튼 노름으로 게산
            int distance = (int)(Mathf.Abs(cubeCoords.x - otherCubeCoords.x) + Mathf.Abs(cubeCoords.y - otherCubeCoords.y) + Mathf.Abs(cubeCoords.z - otherCubeCoords.z)) / 2;
            return distance;
        }//Distance is given using Manhattan Norm. 거리는 맨해튼 노름을 사용하여 게산
        public override List<Cell> GetNeighbours(List<Cell> cells)
        {
            if (neighbours == null) // 인접 셀이 초기화되지 않았으면
            {
                neighbours = new List<Cell>(6); // 인접 셀 리스트 초기화
                foreach (var direction in _directions) // 모든 방향에 대해 
                {
                    // 해당 방향의 인접 셀 찾기
                    var neighbour = cells.Find(c => c.OffsetCoord == CubeToOffsetCoords(CubeCoord + direction));
                    if (neighbour == null) continue; // 인접 셀이 없으면 다음 반복
                    neighbours.Add(neighbour); // 인접 셀 추가
                }
            }
            return neighbours;

        }//Each hex cell has six neighbors, which positions on grid relative to the cell are stored in _directions constant.
        // 각 헥사 셀은 여섯 개의 이웃을 가지며, 그 위치는 _directions상수에 저장

        public override void CopyFields(Cell newCell)
        {
            newCell.OffsetCoord = OffsetCoord;                  // 오프셋 좌표 복사
            (newCell as Hexagon).HexGridType = HexGridType;     // 헥스그리드 타입 복사
        }
    }

    public enum HexGridType
    {
        even_q,
        odd_q,
        even_r,
        odd_r
    }; // 헥사그리드 유형 열거
}