using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TbsFramework.Grid.GameResolvers
{
    public class DominationConditionUI : GameEndCondition
    {
        public int myPlayerNumber = 0;
        public GameObject networkGUI;
        public GameObject endingUI;

        public int Result { get; set; }
        public Text resultText_all;
        public Text resultText_self;

        private AnimalsUIAnimaitionManager animalsUIAnimaitionManager;

        public override GameResult CheckCondition(CellGrid cellGrid)
        {
            var playersAlive = cellGrid.Units.Select(u => u.PlayerNumber).Distinct().ToList();
            if (playersAlive.Count == 1)
            {
                var playersDead = cellGrid.Players.Where(p => p.PlayerNumber != playersAlive[0])
                                                  .Select(p => p.PlayerNumber)
                                                  .ToList();

                networkGUI.SetActive(true);
                endingUI.SetActive(true);

                resultText_all.text = "Winner: Player " + Result;

                animalsUIAnimaitionManager = FindObjectOfType<AnimalsUIAnimaitionManager>();

                if (cellGrid != null)  // Get the player's own id and compare it to the winning player's id
                {
                    if (myPlayerNumber + 1 == Result)
                    {
                        resultText_self.text = "You Win!";
                        animalsUIAnimaitionManager.isWin = true;
                    }
                    else
                    {
                        resultText_self.text = "You Lose!";
                        animalsUIAnimaitionManager.isWin = false;
                    }
                }

                return new GameResult(true, playersAlive, playersDead);
            }
            return new GameResult(false, null, null);
        }
    }
}

