using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using TbsFramework.Grid;
using TbsFramework.Network;
using UnityEngine;
using UnityEngine.EventSystems;


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

    [SerializeField]
    List<GameObject> spawnedCards = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<NetworkConnection>().AddHandler(OnCardSelectEnded, (long)TbsFramework.Network.OpCode.Else);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardObjectBtn()
    {
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
        Destroy(spawned.GetComponent<CardObject>());
        spawnedCards.Add(spawned);


        spawned.transform.localScale = current.transform.localScale;
        InitCardsPositions();
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
            FindObjectOfType<CellGrid>().currentState = CellGrid.GameState.Spawn;

            // FindObjectOfType<NetworkConnection>().EventQueue.Enqueue((new Action(() => { }), () => OnCardSelectEnded()));
        }
        else
        {
            Debug.Log("카드가 부족합니다.");
        }
    }

    public void NetworkTest()
    {
        IDictionary<string, string> a = new Dictionary<string, string>();
        a.Add("aaa", "bbb");
        FindObjectOfType<NetworkConnection>().SendMatchState((long)TbsFramework.Network.OpCode.Else,a);
        Debug.Log("현재 클라이언트");
    }
    private void OnCardSelectEnded(Dictionary<string, string> dict)
    {
        Debug.Log(dict["aaa"]);
    }
}
