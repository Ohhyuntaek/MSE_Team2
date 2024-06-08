using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid.GameResolvers;
using TbsFramework.Grid.GridStates;
using TbsFramework.Grid.TurnResolvers;
using TbsFramework.Grid.UnitGenerators;
using TbsFramework.Network;
using TbsFramework.Players;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;

namespace TbsFramework.Grid
{
    /// <summary>
    /// CellGrid class keeps track of the game, stores cells, units and players objects. It starts the game and makes turn transitions. 
    /// It reacts to user interacting with units or cells, and raises events related to game progress. 
    /// </summary>
    public partial class CellGrid : MonoBehaviour
    {
        /// <summary>
        /// LevelLoading event is invoked before Initialize method is run.
        /// 레벨 로딩 이벤트 : Initialize 메서드가 실행되기 전에 호출
        /// </summary>
        public event EventHandler LevelLoading;
        /// <summary>
        /// LevelLoadingDone event is invoked after Initialize method has finished running.
        /// 레벨 로딩 완료 이벤트 : Initialize 메서드가 완료된 후에 호출
        /// </summary>
        public event EventHandler LevelLoadingDone;
        /// <summary>
        /// GameStarted event is invoked at the beggining of StartGame method.
        /// 게임 시작 이벤트 : StartGame 메서드 시작 시에 호출
        /// </summary>
        public event EventHandler GameStarted;
        /// <summary>
        /// GameEnded event is invoked when there is a single player left in the game.
        /// 게임 종료 이벤트  : 게임에서 단 한명의 플레이어만 남았을 때 호출
        /// </summary>
        public event EventHandler<GameEndedArgs> GameEnded;
        /// <summary>
        /// Turn ended event is invoked at the end of each turn.
        /// 턴 종료 이벤트 : 각 턴의 끝에 호출
        /// </summary>
        public event EventHandler<bool> TurnEnded;

        /// <summary>
        /// UnitAdded event is invoked each time AddUnit method is called.
        /// AddUnit 메서드가 호출될 때마다 발생
        /// </summary>
        public event EventHandler<UnitCreatedEventArgs> UnitAdded;

        private CellGridState _cellGridState; // 셀 그리드의 현재 상태 객체
        public CellGridState cellGridState
        {
            get
            {
                return _cellGridState;
            }
            set
            {
                CellGridState nextState;
                if (_cellGridState != null)
                {
                    _cellGridState.OnStateExit();       // 현재 상태의 종료 로직실행
                    nextState = _cellGridState.MakeTransition(value); // 상태 전환
                }
                else
                {
                    nextState = value;
                }
                   
                _cellGridState = nextState;         // 새 상태 설정
                _cellGridState.OnStateEnter();      // 새 상태의 시작 로직 실행
            }
        }

        public int NumberOfPlayers { get { return Players.Count; } }    // 플레이어 수 반환

        public Player CurrentPlayer
        {
            // 현재 플레이어 반환
            get { return Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)); }
        }
        public int CurrentPlayerNumber { get; private set; }    // 현재 플레이어 번호

        [HideInInspector]
        public bool Is2D;   // 2D 게임인지 여부

        /// <summary>
        /// GameObject that holds player objects.
        /// 플레이어 객체를 보관하는 GameObject
        /// </summary>
        public Transform PlayersParent;
        public bool ShouldStartGameImmediately = true;          // 게임을 즉시 시작할지 여부
        public GameState currentState = GameState.SelectCard;

        public enum GameState
        {
            SelectCard,
            Spawn,
            Play,
            End
        }
        CardManager cardManager;

        private int UnitId = 0;     // 유닛 ID 카운터

        public bool GameFinished { get; private set; }      // 게임이 끝났는지 여부
        public List<Player> Players { get; private set; }   // 플레이어 리스트
        public List<Cell> Cells { get; private set; }       // 셀 리스트
        public List<Unit> Units { get; private set; }       // 유닛 리스트
        public Func<List<Unit>> PlayableUnits = () => new List<Unit>(); // 플레이어블 유닛 반환 함수

        //[SerializeField]
        //public GameObject inGameMusicObject;

        private void Start()
        {
            cardManager = FindObjectOfType<CardManager>();
            if (ShouldStartGameImmediately)
            {
                // 초기화 및 게임 시작
                InitializeAndStart();
            }
        }


        public void InitializeAndStart()
        {
            //inGameMusicObject.GetComponent<AudioSource>().Play();
            AudioManager.Instance.PlayBackgroundMusicByName("inGameBgm");
            cardManager.SendNickname();
            Initialize();   // 초기화
            StartGame();    // 게임 시작
        }

        public void Initialize()
        {
            //OnLevelLoading() 이벤트 실행
            if (LevelLoading != null)
                LevelLoading.Invoke(this, EventArgs.Empty);     // 레벨 로딩 이벤트 호출

            //변수 초기화
            GameFinished = false;           // 게임 종료 상태 초기화
            Players = new List<Player>();   // 플레이어 리스트 초기화
            for (int i = 0; i < PlayersParent.childCount; i++)
            {
                var player = PlayersParent.GetChild(i).GetComponent<Player>();
                if (player != null && player.gameObject.activeInHierarchy)
                {
                    player.Initialize(this);
                    Players.Add(player);    // 플레이어 추가
                }
            }

            //Cells 초기화
            Cells = new List<Cell>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
                if (cell != null)
                {
                    if (cell.gameObject.activeInHierarchy)
                    {
                        Cells.Add(cell);    // 셀 추가
                    }

                    // JY add
                    // for cell property setting (special buff effect to the characters)
                    cell.SetCellProperty(Players[0].mapIndex);
                }
                else
                {
                    // 오류 로깅
                    Debug.LogError("Invalid object in cells parent game object");
                }
            }
            
            //각 Cell에 이벤트 등록
            foreach (var cell in Cells)
            {
                cell.CellClicked += OnCellClicked;
                cell.CellHighlighted += OnCellHighlighted;
                cell.CellDehighlighted += OnCellDehighlighted;
                cell.GetComponent<Cell>().GetNeighbours(Cells); // 이웃 셀 설정
            }

            //UnitGenerator 초기화
            Units = new List<Unit>();   // 유닛 리스트 초기화
            var unitGenerator = GetComponent<IUnitGenerator>();
            if (unitGenerator != null)
            {
                //var units = unitGenerator.SpawnUnits(Cells);
                var units = FindObjectsOfType<Unit>();
                foreach (var unit in units)
                {
                    // 유닛 추가
                    AddUnit(unit.GetComponent<Transform>());
                }
            }
            else
            {
                // 유닛 생성기 오류 로깅
                Debug.LogError("No IUnitGenerator script attached to cell grid");
            }

            if (LevelLoadingDone != null)
                LevelLoadingDone.Invoke(this, EventArgs.Empty); // 레벨 로딩 완료 이벤트 호출
        }

        private void OnCellDehighlighted(object sender, EventArgs e)
        {
            // 셀 선택 해제 처리
            cellGridState.OnCellDeselected(sender as Cell);
        }
        private void OnCellHighlighted(object sender, EventArgs e)
        {
            // 셀 선택 처리
            cellGridState.OnCellSelected(sender as Cell);
        }
        private void OnCellClicked(object sender, EventArgs e)
        {
            // 셀 클릭했을 때 실행되는 이벤트
            cellGridState.OnCellClicked(sender as Cell);
        }

        private void OnUnitClicked(object sender, EventArgs e)
        {
            // 유닛 클릭 처리
            cellGridState.OnUnitClicked(sender as Unit);
        }
        private void OnUnitHighlighted(object sender, EventArgs e)
        {
            if (FindObjectOfType<CellGrid>().currentState != GameState.Play) return;
            // 유닛 하이라이트 처리
            cellGridState.OnUnitHighlighted(sender as Unit);
        }
        private void OnUnitDehighlighted(object sender, EventArgs e)
        {
            if (FindObjectOfType<CellGrid>().currentState != GameState.Play) return;
            // 유닛 하이라이트 해제 처리
            cellGridState.OnUnitDehighlighted(sender as Unit);
        }

        private void OnUnitDestroyed(object sender, AttackEventArgs e)
        {
            // HT When animal was destroyed
            //GameObject.Find("DestroySound").GetComponent<AudioSource>().Play();
            AudioManager.Instance.PlaySFX("Destroy");
            Units.Remove(e.Defender);   // 유닛 제거
            e.Defender.GetComponents<Ability>().ToList().ForEach(a => a.OnUnitDestroyed(this)); // 유닛 파괴 이벤트 처리
            e.Defender.UnitClicked -= OnUnitClicked;
            e.Defender.UnitHighlighted -= OnUnitHighlighted;
            e.Defender.UnitDehighlighted -= OnUnitDehighlighted;
            e.Defender.UnitDestroyed -= OnUnitDestroyed;
            e.Defender.UnitMoved -= OnUnitMoved;
            CheckGameFinished();        // 게임 종료 확인
        }

        /// <summary>
        /// Adds unit to the game
        /// 유닛 생성
        /// </summary>
        /// <param name="unit">Unit to add</param>
        /// 
        // 유닛 생성
        public void AddUnit(Transform unit, Cell targetCell = null, Player ownerPlayer = null)
        {
            unit.GetComponent<Unit>().UnitID = UnitId++;    // 유닛 ID 할당
            if(Units != null)
                Units.Add(unit.GetComponent<Unit>());           // 유닛 리스트에 추가

            if (targetCell != null)
            {
                targetCell.IsTaken = unit.GetComponent<Unit>().Obstructable;    // 타겟 셀의 점유 상태 업데이트

                unit.GetComponent<Unit>().Cell = targetCell;    // 유닛의 셀 설정
                unit.GetComponent<Unit>().transform.position = targetCell.transform.position; // 유닛의 위치 설정 // JY modified : localpos to worldpos
            }

            if (ownerPlayer != null)
            {
                // 유닛의 플레이어 번호 설정
                unit.GetComponent<Unit>().PlayerNumber = ownerPlayer.PlayerNumber;
            }

            if(unit.GetComponent<Unit>().Cell != null)
            {
                // 셀의 현재 유닛 리스트에 추가
                unit.GetComponent<Unit>().Cell.CurrentUnits.Add(unit.GetComponent<Unit>());
            }

            unit.GetComponent<Unit>().transform.localRotation = Quaternion.Euler(0, 0, 0);  // 유닛의 회전 초기화
            unit.GetComponent<Unit>().Initialize(); // 유닛 초기화
            
            // 이벤트 연결
            unit.GetComponent<Unit>().UnitClicked += OnUnitClicked;
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitDestroyed += OnUnitDestroyed;
            unit.GetComponent<Unit>().UnitMoved += OnUnitMoved;

            if (UnitAdded != null)
            {
                // 유닛 추가 이벤트 호출
                UnitAdded.Invoke(this, new UnitCreatedEventArgs(unit));
            }
        }

        private void OnUnitMoved(object sender, MovementEventArgs e)
        {
            // 게임 종료 확인
            CheckGameFinished();
        }

        /// <summary>
        /// Method is called once, at the beggining of the game.
        /// </summary>
        public void StartGame()
        {
            //턴 실행, 현재 턴 정보 등록
            TransitionResult transitionResult = GetComponent<TurnResolver>().ResolveStart(this);    // 턴 해석기를 통해 게임 시작 처리
            PlayableUnits = transitionResult.PlayableUnits;     // 플레이어블 유닛 설정
            CurrentPlayerNumber = transitionResult.NextPlayer.PlayerNumber; // 다음 플레이어 번호 설정

            //OnGameStarted() 이벤트 실행
            // 게임 시작 이벤트 호출
            GameStarted?.Invoke(this, EventArgs.Empty);

            //모든 어빌리티에 현재 턴 실행
            // 모든 유닛에 대해 턴 시작 처리
            PlayableUnits().ForEach(u => { u.GetComponents<Ability>().ToList().ForEach(a => a.OnTurnStart(this)); u.OnTurnStart(); });
            CurrentPlayer.Play(this);   // 현재 플레이어의 턴 실행
            Debug.Log("Game started");  // 게임 시작 로그 출력
        }

        public void EndTurn(bool isNetworkInvoked=false)
        {
            switch (currentState)
            {
                case GameState.SelectCard:
                    cardManager.EndTurn();

                    break;

                case GameState.Spawn:

                    // 턴 종료
                    _cellGridState.EndTurn(isNetworkInvoked);
                    break;
                case GameState.Play:
                    _cellGridState.EndTurn(isNetworkInvoked);
                    break;
            }
        }

        /// <summary>
        /// Method makes the actual turn transitions.
        /// </summary>
        private void EndTurnExecute(bool isNetworkInvoked=false)
        {
            cellGridState = new CellGridStateBlockInput(this);  // 입력 차단 상태로 전환
            bool isGameFinished = CheckGameFinished();          // 게임 종료 상태 확인
            if (isGameFinished)
            {
                return;
            }

            var playableUnits = PlayableUnits();            // 플레이어블 유닛 리스트 가져오기
            for (int i = 0; i < playableUnits.Count; i++)
            {
                var unit = playableUnits[i];
                if (unit == null)
                {
                    continue;
                }
                    
                unit.OnTurnEnd();       // 유닛의 턴 종료 처리
                var abilities = unit.GetComponents<Ability>();  // 유닛의 능력 리스트 가져오기
                for (int j = 0; j < abilities.Length; j++)
                {
                    var ability = abilities[j];
                    ability.OnTurnEnd(this);    // 능력의 턴 종료 처리
                }
            }
            // 턴 해석기를 통해 턴 전환 처리
            TransitionResult transitionResult = GetComponent<TurnResolver>().ResolveTurn(this);

            PlayableUnits = transitionResult.PlayableUnits;     // 플레이어블 유닛 업데이트
            CurrentPlayerNumber = transitionResult.NextPlayer.PlayerNumber; // 다음 플레이어 번호 업데이트

            if (TurnEnded != null)
                TurnEnded.Invoke(this, isNetworkInvoked);   // 턴 종료 이벤트 호출

            Debug.Log($"{cardManager.nicknames[CurrentPlayerNumber]} turn");   // 현재 플레이어의 턴 로그 출력

            playableUnits = PlayableUnits();    // 업데이트된 플레이어블 유닛 리스트 가져오기
            for (int i = 0; i < playableUnits.Count; i++)
            {
                var unit = playableUnits[i];
                if (unit == null)
                {
                    continue;
                }

                // 유닛의 늉력 리스트 가져오기
                var abilities = unit.GetComponents<Ability>();
                for (int j = 0; j < abilities.Length; j++)
                {
                    var ability = abilities[j];
                    ability.OnTurnStart(this);  // 능력의 턴 시작처리
                }
                unit.OnTurnStart();
            }
            CurrentPlayer.Play(this);
        }

        // 현재 플레이어의 유닛 목록을 반환하는 메서드
        public List<Unit> GetCurrentPlayerUnits()
        {
            return PlayableUnits();     // 플레이 가능한 유닛 목록을 반환
        }
        // 특정 플레이어의 적 유닛 목록을 반환하는 메서드
        public List<Unit> GetEnemyUnits(Player player)
        {
            return Units.FindAll(u => u.PlayerNumber != player.PlayerNumber);
        }
        // 특정 플레이어의 유닛 목록을 반환하는 메서드
        public List<Unit> GetPlayerUnits(Player player)
        {
            return Units.FindAll(u => u.PlayerNumber == player.PlayerNumber);
        }

        // 게임이 끝났는지 확인하는 메서드
        public bool CheckGameFinished()
        {
            // HT Pause the InGame music
            //GameObject.Find("InGameMusicObject").GetComponent<AudioSource>().Pause();
            List<GameResult> gameResults =
                GetComponents<GameEndCondition>()       // 게임 종료 조건 컴포넌트를 모두 가져옴
                .Select(c => c.CheckCondition(this))    // 각 조건에 대해 현재 게임 상태를 검사
                .ToList();

            foreach (var gameResult in gameResults)
            {
                // 게임이 끝난 조건이 하나라도 있을 경우
                if (gameResult.IsFinished)
                {
                    cellGridState = new CellGridStateGameOver(this);    // 게임 오버 상태로 전환
                    GameFinished = true;        // 게임 종료 플래그를 참으로 설정
                    if (GameEnded != null)
                    {
                        // 게임 종료 이벤트 발생
                        GameEnded.Invoke(this, new GameEndedArgs(gameResult));
                    }

                    break;
                }
            }
            // 게임 종료 여부 반환
            return GameFinished;
        }
    }
}

