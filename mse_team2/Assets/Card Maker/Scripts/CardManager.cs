using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using TbsFramework.Grid;
using TbsFramework.Network;
using UnityEngine;
using UnityEngine.EventSystems;
using TbsFramework.Cells;
using TbsFramework.Units;
using TbsFramework.Players;
using UnityEngine.UI;
using TbsFramework.Example1;
using TbsFramework.Gui;


public class CardManager : MonoBehaviour
{
    [SerializeField]
    List<int> cardArray = new List<int> ();

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

    [SerializeField]
    Text consoleText;

    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<NetworkConnection>().AddHandler(OnCardSelectEnded, (long)TbsFramework.Network.OpCode.SelectCard);
        FindObjectOfType<NetworkConnection>().AddHandler(SpawnUnit, (long)TbsFramework.Network.OpCode.SpawnUnit);
        guiController.InfoText.text = "Select 3 Cards";
    }

    void Update()
    {
        GetInput();

        if (isRemoteSelectEnd == true && isLocalSelectEnd == true && cellGrid.currentState == CellGrid.GameState.SelectCard)
        {
            cellGrid.currentState = CellGrid.GameState.Spawn;

            guiController.InfoText.text = "Player 1";
        }
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //스폰 로직
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
        if (cellGrid.currentState != CellGrid.GameState.SelectCard) return;
        if (isLocalSelectEnd) return;


        CardObject current = EventSystem.current.currentSelectedGameObject.GetComponent<CardObject>();
        int selectedIndex = current.selectedPrefabIndex;

        //배열에 현재 인덱스가 있으면
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
        if (cardArray.Count == 3)
        {
            isLocalSelectEnd = true;
            FindObjectOfType<NetworkConnection>().SendMatchState((long)TbsFramework.Network.OpCode.SelectCard, new Dictionary<string, string>());
        }
        else
        {
            Debug.Log("카드가 부족합니다.");
        }
    }

    private void OnCardSelectEnded(Dictionary<string, string> dict)
    {
        isRemoteSelectEnd = true;
    }

    public void SpawnUnit(Dictionary<string, string> dict)
    {
        int player = int.Parse(dict["player"]);
        int prefabNum = int.Parse(dict["prefabNum"]);

        string coordStr = dict["Cell"];
        string[] s = coordStr.Split(',');
        Vector2 coord = new Vector2(float.Parse(s[0]), float.Parse(s[1]));
        Cell cell = FindObjectsOfType<Cell>().ToList().Find(a => a.OffsetCoord == coord);


        Debug.Log($"{player} {prefabNum} {coordStr} {isAbleSpawn}");
        consoleText.text = $"{player} {prefabNum} {coordStr} {isAbleSpawn}";

        Unit unit = Instantiate(prefabManager.unitPrefabs[prefabNum], cell.transform.position, Quaternion.identity);
        cellGrid.AddUnit(unit.transform, cell,  players[player]);
        spawnNumber++;

        nextTurnButton.interactable = true;

        cellGrid.PlayableUnits().Add(unit);

        isAbleSpawn = false;

        if (spawnNumber >= 3)
        {
            switch (turnNumber)
            {
                case 0:
                    turnNumber = 1;
                    guiController.InfoText.text = "Player 2";
                    spawnNumber = 0;
                    break;
                case 1:
                    // ##################### 게임 실행시점 #####################
                    cellGrid.currentState = CellGrid.GameState.Play;
                    break;
            }
        }
    }

    public void OnCellClicked(Cell cell)
    {
        // 셀 클릭 후 소환하고자 하는 플레이어, 소환할 몹, 셀 위치 저장

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
        consoleText.text = index.ToString();
        isAbleSpawn = true;
    }
}
