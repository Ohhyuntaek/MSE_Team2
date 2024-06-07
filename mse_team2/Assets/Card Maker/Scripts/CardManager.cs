using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Network;
using UnityEngine;
using UnityEngine.EventSystems;
using TbsFramework.Cells;
using TbsFramework.Units;
using TbsFramework.Players;
using UnityEngine.UI;
using TbsFramework.Example1;
using TMPro;

/// <summary>
/// HT : How to select 3 animal cards, How to spawn animals, How to start the game with Synchronization
/// </summary>
public class CardManager : MonoBehaviour
{
    [SerializeField]
    List<int> cardArray = new List<int>();

    [SerializeField]
    Transform parent;

    [SerializeField]
    Transform initPos;

    [SerializeField]
    float yInterval;
    public bool isAbleSpawn = false;
    public int spawnNumber = 0;

    [SerializeField]
    List<GameObject> spawnedCards = new List<GameObject>();

    [SerializeField]
    LayerMask cellLayer;

    [SerializeField]
    Player[] players;
    public int index = 0;

    [SerializeField]
    public Camera cardUICamera;

    [SerializeField]
    public GameObject scrollView;
    [SerializeField]
    public GameObject showButton;
    [SerializeField]
    public GameObject hideButton;

    [SerializeField]
    public TMP_Text nicknameUIText;

    public int localPlayerNum = -1;

    public int turnNumber = 0;

    bool isLocalSelectEnd = false;
    bool isRemoteSelectEnd = false;

    bool isLocalSpawnEnd = false;
    bool isRemoteSpawnEnd = false;

    CellGrid cellGrid
    {
        get { return FindObjectOfType<CellGrid>(); }
    }

    PrefabManager prefabManager
    {
        get { return FindObjectOfType<PrefabManager>(); }
    }
    GuiController guiController
    {
        get { return FindObjectOfType<GuiController>(); }
    }

    public Button nextTurnButton;

    public string currentNickname = "";
    public string remoteNickname = "";

    public string[] nicknames;

    // Start is called before the first frame update
    private void Start()
    {
        // The function to synchronize 
        FindObjectOfType<NetworkConnection>().AddHandler(OnCardSelectEnded, (long)TbsFramework.Network.OpCode.SelectCard);
        FindObjectOfType<NetworkConnection>().AddHandler(SendNickname, (long)TbsFramework.Network.OpCode.SendNickname);
        FindObjectOfType<NetworkConnection>().AddHandler(SpawnUnit, (long)TbsFramework.Network.OpCode.SpawnUnit);
        guiController.InfoText.text = "Select 3 Cards";

        // The function to synchronize each players' nickname
        currentNickname = PlayerServer.Player.nickname;
        nicknameUIText.text = currentNickname;
    }

    public void SendNickname()
    {
        // Send my nickname to the other computer

        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("nickname", currentNickname);

        FindObjectOfType<NetworkConnection>().SendMatchState((long)TbsFramework.Network.OpCode.SendNickname, dict);
    }

    void Update()
    {
        GetInput();

        if (isRemoteSelectEnd == true && isLocalSelectEnd == true && cellGrid.currentState == CellGrid.GameState.SelectCard)
        {
            // Disable Full card deck
            scrollView.SetActive(false);
            showButton.SetActive(false);
            hideButton.SetActive(false);

            // Go to spawn level
            cellGrid.currentState = CellGrid.GameState.Spawn;
            nextTurnButton.interactable = false;
            guiController.InfoText.text = $"{(nicknames == null ? "" : nicknames[0])} is Spawning";
        }
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Spawn animals when click the cell
            if(cellGrid.currentState == CellGrid.GameState.Spawn)
            {
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100, cellLayer);
                
                if (hits.Length > 0)
                {
                    OnCellClicked(hits[0].transform.GetComponent<Cell>());
                }
            }
        }
    }

    public void CardObjectBtn()
    {
        // Select 3 animal cards
        
        if (cellGrid.currentState != CellGrid.GameState.SelectCard) return;
        if (isLocalSelectEnd) return;

        CardObject current = EventSystem.current.currentSelectedGameObject.GetComponent<CardObject>();
        int selectedIndex = current.selectedPrefabIndex;

        if(cardArray.Contains(selectedIndex))
        {
            int index = cardArray.IndexOf(selectedIndex);
            Destroy(spawnedCards[index].gameObject);
            spawnedCards.RemoveAt(index);
            InitCardsPositions();
            cardArray.Remove(selectedIndex);
            return;
        }
        else
        {
            if(cardArray.Count >= 3)
            {
                int index = 0;
                Destroy(spawnedCards[index].gameObject);
                spawnedCards.RemoveAt(index);
                cardArray.Add(selectedIndex);
                cardArray.RemoveAt(0);
            }
            else
            {
                cardArray.Add(selectedIndex);
            }
        }

        GameObject spawned = Instantiate(current.gameObject);
        spawned.transform.parent = parent;
        spawned.name = spawned.GetComponent<CardObject>().selectedPrefabIndex.ToString();
        Destroy(spawned.GetComponent<CardObject>());
        Destroy(spawned.GetComponent<Button>());

        StartCoroutine(AddBtn(spawned));

        spawnedCards.Add(spawned);

        spawned.transform.localScale = current.transform.localScale;
        InitCardsPositions();
    }

    IEnumerator AddBtn(GameObject go)
    {
        yield return new WaitForEndOfFrame();

        Button spawnedBtn = go.AddComponent<Button>();
        spawnedBtn.onClick.AddListener(NumBtn);
    }

    void InitCardsPositions()
    {
        int i = 0;
        foreach(GameObject card in spawnedCards)
        {
            card.transform.localPosition = initPos.localPosition + (Vector3.down * i * yInterval);
            i++;
        }
    }

    public void EndTurn()
    {
        // When the player finish selecting 3 animal cards

        if (cardArray.Count == 3)
        {
            isLocalSelectEnd = true;
            guiController.NextTurnButton.interactable = false;
            FindObjectOfType<NetworkConnection>().SendMatchState((long)TbsFramework.Network.OpCode.SelectCard, new Dictionary<string, string>());
        }
        else
        {
            guiController.InfoText.text = "Lack of cards";
        }
    }

    private void OnCardSelectEnded(Dictionary<string, string> dict)
    {
        isRemoteSelectEnd = true;
    }

    private void SendNickname(Dictionary<string, string> dict)
    {
        remoteNickname = dict["nickname"];

        nicknames = new string[2];

        if (localPlayerNum == 0)
        {
            nicknames[0] = currentNickname;
            nicknames[1] = remoteNickname;
        }
        else
        {
            nicknames[0] = remoteNickname;
            nicknames[1] = currentNickname;
        }
    }

    public void SpawnUnit(Dictionary<string, string> dict)
    {
        int player = int.Parse(dict["player"]);
        int prefabNum = int.Parse(dict["prefabNum"]);

        string coordStr = dict["Cell"];
        string[] s = coordStr.Split(',');
        Vector2 coord = new Vector2(float.Parse(s[0]), float.Parse(s[1]));
        Cell cell = FindObjectsOfType<Cell>().ToList().Find(a => a.OffsetCoord == coord);

        // Debug.Log($"{player} {prefabNum} {coordStr} {isAbleSpawn}");
        // consoleText.text = $"{player} {prefabNum} {coordStr} {isAbleSpawn}";

        Unit unit = Instantiate(prefabManager.unitPrefabs[prefabNum], cell.transform.position, Quaternion.identity);
        
        cellGrid.AddUnit(unit.transform, cell,  players[player]);
        unit.PlayerNumber = player;

        MeshRenderer mr = unit.transform.GetComponentsInChildren<MeshRenderer>().ToList().Find(a => a.gameObject.name == "Highlighter");
        Color color = (unit.PlayerNumber == 1) ? Color.red : Color.blue;
        color.a = 0.6f;
        mr.material.color = color;

        spawnNumber++;

        cellGrid.PlayableUnits().Add(unit);

        isAbleSpawn = false;

        cell.IsTaken = true;

        if (spawnNumber >= 3)
        {
            switch (turnNumber)
            {
                case 0:
                    turnNumber = 1;
                    guiController.InfoText.text = $"{nicknames[1]} is Spawning";
                    spawnNumber = 0;
                    break;
                case 1:
                    // ##################### Game Start #####################
                    cellGrid.currentState = CellGrid.GameState.Play;
                    cellGrid.InitializeAndStart();


                    scrollView.SetActive(true);
                    showButton.SetActive(true);
                    hideButton.SetActive(true);
                    cardUICamera.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void OnCellClicked(Cell cell)
    {
        // Store player information, animal information, cell information, and send it to your opponent's computer

        if (turnNumber != localPlayerNum) return;

        if (isAbleSpawn == true)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "player", $"{localPlayerNum}" },
                { "prefabNum", $"{index}" },
                { "Cell", $"{cell.OffsetCoord.x},{cell.OffsetCoord.y}" }
            };

            SpawnUnit(dict);
            FindObjectOfType<NetworkConnection>().SendMatchState((long)TbsFramework.Network.OpCode.SpawnUnit, dict);
        }
    }

    public void NumBtn()
    {
        index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log(index);
        isAbleSpawn = true;
    }
}
