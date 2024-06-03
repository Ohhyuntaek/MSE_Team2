using TbsFramework.Grid;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace TbsFramework.Players
{
    /// <summary>
    /// Class represents a game participant.
    /// </summary>
    public abstract class Player : MonoBehaviour
    {
        public int PlayerNumber;

        // JY add for map random selection
        private int _mapIndex;
        public int mapIndex {
            get => _mapIndex;
            set => _mapIndex = value;
        }
        public void setCameraPos(Vector3 pos){
            Camera.main.transform.position = pos;
            print("Camera position: "+Camera.main.transform.position);
        }




        public virtual void Initialize(CellGrid cellGrid) { }
        /// <summary>
        /// Method is called every turn. Allows player to interact with his units.
        /// </summary>         
        public abstract void Play(CellGrid cellGrid);
    }
}