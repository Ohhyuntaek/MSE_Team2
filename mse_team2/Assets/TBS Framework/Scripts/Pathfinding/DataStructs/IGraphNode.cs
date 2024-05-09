namespace TbsFramework.Pathfinding.DataStructs
{
    public interface IGraphNode
    {
        /// <summary>
        /// Method returns distance to a IGraphNode that is given as parameter. 
        /// 메서드는 매개변수로 주어진 IGraphNode까지의 거리를 반환합니다.
        /// </summary>
        int GetDistance(IGraphNode other);
    }
}

