using System.Linq;
using TbsFramework.Players;
using UnityEngine;
using UnityEngine.UI;

namespace TbsFramework.Grid.GameResolvers
{
    public class DominationConditionUI : GameEndCondition
    {
        //public int myPlayerNumber = 0;
        public GameObject networkGUI;
        public GameObject endingUI;

        public CardManager cardManager;
        public CellGrid cellGrid;

        [SerializeField]
        public Button restartButton;

        public int Result { get; set; }
        public Text resultText_all;
        public Text resultText_self;

        private AnimalsUIAnimaitionManager animalsUIAnimaitionManager;

        private void Start()
        {
            cardManager = FindObjectOfType<CardManager>();
            cellGrid = FindObjectOfType<CellGrid>();
        }

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
                //resultText_all.text = "Winner: Player " + Result;
                resultText_all.text = $"Winner: {cardManager.nicknames[cellGrid.CurrentPlayerNumber]}";

                animalsUIAnimaitionManager = FindObjectOfType<AnimalsUIAnimaitionManager>();

                //if (cellGrid != null)  // Get the player's own id and compare it to the winning player's id
                //{
                //    if (myPlayerNumber + 1 == Result)
                //    {
                //        resultText_self.text = "You Win!";
                //        animalsUIAnimaitionManager.isWin = true;
                //    }
                //    else
                //    {
                //        resultText_self.text = "You Lose!";
                //        animalsUIAnimaitionManager.isWin = false;
                //    }
                //}

                if (cellGrid.CurrentPlayer is HumanPlayer)
                {
                    GameObject.Find("WinSound").GetComponent<AudioSource>().Play();

                    resultText_self.text = "You Win!";
                    animalsUIAnimaitionManager.isWin = true; 
                    
                    GamehistoryManager gamehistoryManager = FindObjectOfType<GamehistoryManager>();

                    GameResultInfo gri = new GameResultInfo(PlayerServer.Player.privateCode, "Easy", "Win");

                    gamehistoryManager.UpdatePlayerHistory(gri);
                }
                else
                {
                    GameObject.Find("LoseSound").GetComponent<AudioSource>().Play();

                    resultText_self.text = "You Lose!";
                    animalsUIAnimaitionManager.isWin = false;

                    GamehistoryManager gamehistoryManager = FindObjectOfType<GamehistoryManager>();

                    GameResultInfo gri = new GameResultInfo(PlayerServer.Player.privateCode, "Easy", "Lose");

                    gamehistoryManager.UpdatePlayerHistory(gri);
                }

                return new GameResult(true, playersAlive, playersDead);
            }
            return new GameResult(false, null, null);
        }

    }
}

