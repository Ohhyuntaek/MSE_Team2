using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using TbsFramework.Grid;
using UnityEngine;
using System.Collections;
using System.Linq;
using TbsFramework.Units;

namespace TbsFramework.Network
{
    /// <summary>
    /// An abstract class for managing network connections in a multiplayer game.
    /// Implementations of this class can provide different online backends such as Nakama, Photon, etc.
    /// 멀티플레이어 게임에서 네트워크 연결을 관리하는 추상 클래스.
    /// 서로 다른 온라인 백엔드를 위한 구현체 제공.
    /// </summary>
    public abstract class NetworkConnection : MonoBehaviour
    {
        [SerializeField] private CellGrid _cellGrid;  // 게임의 셀 그리드 참조


        /// <summary>
        /// Event triggered when the server connection is successfully established.
        /// </summary>
        public event EventHandler ServerConnected; // 서버 연결 성공 시 발생 이벤트

        /// <summary>
        /// Event triggered when a room is successfully joined. Carries room data as event arguments.
        /// </summary>
        public event EventHandler<RoomData> RoomJoined; // 방 입장 성공시 발생

        /// <summary>
        /// Event triggered when the local player exits a room.
        /// </summary>
        public event EventHandler RoomExited; // 방 퇴장시 발생

        /// <summary>
        /// Event triggered when a new player enters the room. Carries the player's network user data.
        /// </summary>
        public event EventHandler<NetworkUser> PlayerEnteredRoom; // 새 플레이어 방 입장시 발생

        /// <summary>
        /// Event triggered when a player leaves the room. Carries the player's network user data.
        /// </summary>
        public event EventHandler<NetworkUser> PlayerLeftRoom;//  플레이어 방 퇴장시 발생

        /// <summary>
        /// Event triggered if joining a room fails. Carries a message detailing the failure.
        /// </summary>
        public event EventHandler<string> JoinRoomFailed; // 방 입장 실패시 발생

        /// <summary>
        /// Event triggered if creating a room fails. Carries a message detailing the failure.
        /// </summary>
        public event EventHandler<string> CreateRoomFailed; // 방 생성 실패시 발생

        /// <summary>
        /// Property indicating if the local player is the host of the current room.
        /// </summary>
        public virtual bool IsHost { get; protected set; } // 현재 로컬 플레이어가 호스트인지 여부

        // 핸들러와 이벤트 큐
        protected Dictionary<long, Action<Dictionary<string, string>>> Handlers = new Dictionary<long, Action<Dictionary<string, string>>>();
        public Queue<(Action preAction, Func<IEnumerator> routine)> EventQueue = new Queue<(Action preAction, Func<IEnumerator> routine)>();
        protected bool processingEvents;

        /// <summary>
        /// Connect to the multiplayer game server.
        /// </summary>
        /// <param name="userName">The name of the user connecting to the server.</param>
        /// <param name="customParams">Additional custom parameters for the connection.</param>
        // 네트워크 서버에 연결하는 메소드
        public abstract void ConnectToServer(string userName, Dictionary<string, string> customParams);

        /// <summary>
        /// Join a quick match with the specified maximum number of players.
        /// </summary>
        /// <param name="maxPlayers">Maximum number of players in the match.</param>
        /// <param name="customParams">Additional custom parameters for the match.</param>
        // 빠른 매치 입장 메서드
        public abstract void JoinQuickMatch(int maxPlayers, Dictionary<string, string> customParams);

        /// <summary>
        /// Create a new room with the specified parameters.
        /// </summary>
        /// <param name="roomName">Name of the room to create.</param>
        /// <param name="maxPlayers">Maximum number of players in the room.</param>
        /// <param name="isPrivate">Whether the room is private. A private room will not be listed by the <see cref="GetRoomList"/> method</param>
        /// <param name="customParams">Additional custom parameters for the room.</param>
        // 새로운 방 생성 메서드
        public abstract void CreateRoom(string roomName, int maxPlayers, bool isPrivate, Dictionary<string, string> customParams);

        /// <summary>
        /// Join an existing room by its name.
        /// </summary>
        /// <param name="roomName">The name of the room to join.</param>
        /// 방 이름으로 방에 입장하는 메서드
        public abstract void JoinRoomByName(string roomName);

        /// <summary>
        /// Join an existing room by its unique ID.
        /// </summary>
        /// <param name="roomID">The unique identifier of the room to join.</param>
        /// 방 ID로 방에 입장하는 메서드
        public abstract void JoinRoomByID(string roomID);

        /// <summary>
        /// Leave the current room.
        /// </summary>
        /// 현재 방을 나가는 메서드
        public abstract void LeaveRoom();

        /// <summary>
        /// Get a list of available public rooms.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of RoomData.</returns>
        /// 공개 방 목록을 가져오는 메서드
        public abstract Task<IEnumerable<RoomData>> GetRoomList();

        /// <summary>
        /// Send the current match state to other players in the room.
        /// </summary>
        /// <param name="opCode">Operation code indicating the type of the match state.</param>
        /// <param name="actionParams">Parameters representing the match state.</param>
        /// 매치 상태를 다른 플레이어에게 전송하는 메서드
        public abstract void SendMatchState(long opCode, IDictionary<string, string> actionParams);

        /// <summary>
        /// Adds a handler for processing specific network operations identified by an operation code.
        /// </summary>
        /// <param name="handler">The action to perform when the specified OpCode is received. The action takes a dictionary of string key-value pairs representing the parameters of the network operation.</param>
        /// <param name="OpCode">The operation code that identifies the type of network operation.</param>
        /// 특정 OPCODE에 대한 핸들러를 추가하는 메서드
        public virtual void AddHandler(Action<Dictionary<string, string>> handler, long OpCode)
        {
            Handlers.Add(OpCode, handler);
        }

        /// <summary>
        /// Initializes the random number generator with a specific seed. This is useful for ensuring that random number generation is consistent and replicable across different instances of the game, which is important in multiplayer environments.
        /// </summary>
        /// <param name="seed">The seed value to initialize the random number generator. Typically, this seed should be synchronized across all clients in a multiplayer game to ensure consistent random number generation.</param>
        /// 랜덤 넘버 제너레이터 초기화 메서드
        public virtual void InitializeRng(int seed)
        {
            UnityEngine.Random.InitState(seed);
        }

        // Unity 생명주기의 Awake 단계에서 기본 핸들러 추가
        protected virtual void Awake()
        {
            Handlers.Add((long)OpCode.TurnEnded, HandleRemoteTurnEnding);
            Handlers.Add((long)OpCode.AbilityUsed, HandleRemoteAbilityUsed);
        }

        // Unity 생명주기의 Start단계에서 게임의 셀 그리드에 이벤트 리스너 추가
        protected virtual void Start()
        {
            _cellGrid.UnitAdded += OnUnitAdded;
            _cellGrid.TurnEnded += OnTurnEndedLocal;
        }

        // 서버 연결 성공 이벤트를 호출하는 메서드
        protected void InvokeServerConnected()
        {
            Debug.Log("Server connected");
            ServerConnected?.Invoke(this, EventArgs.Empty);
        }

        // 방 입장 성공 이벤트를 호출하는 메서드
        protected void InvokeRoomJoined(RoomData roomData)
        {
            var players = roomData.Users.ToList();
            Debug.Log($"Joined room: {roomData.RoomID}; players inside: {players.Count}");
            RoomJoined?.Invoke(this, roomData);
        }

        // 방 퇴장 이벤트를 호출하는 메서드
        protected void InvokeRoomExited()
        {
            Debug.Log("Exited room");
            RoomExited?.Invoke(this, EventArgs.Empty);
        }

        // 플레이어 방 입장 이벤트를 호출하는 메서드
        protected void InvokePlayerEnteredRoom(NetworkUser networkUser)
        {
            Debug.Log($"Player {networkUser.UserID} entered room");
            PlayerEnteredRoom?.Invoke(this, networkUser);
        }

        // 플레이어 방 퇴장 이벤트를 호출하는 메서드
        protected void InvokePlayerLeftRoom(NetworkUser networkUser)
        {
            Debug.Log($"Player {networkUser.UserID} left room");
            PlayerLeftRoom?.Invoke(this, networkUser);
        }

        // 방 생성 실패 이벤트를 호출하는 메서드
        protected void InvokeCreateRoomFailed(string message)
        {
            CreateRoomFailed?.Invoke(this, message);
        }

        // 방 입장 실패 이벤트를 호출하는 메서드
        protected void InvokeJoinRoomFailed(string message)
        {
            JoinRoomFailed?.Invoke(this, message);
        }

        // 유닛 추가시 능력 이벤트에 대한 리스너 추가
        private void OnUnitAdded(object sender, UnitCreatedEventArgs e)
        {
            // Get Abilities of newly added unit and subscribe to their AbilityUsed event
            // 새로 추가된 유닛의 능력을 가져오고 그들의 AbilityUsed 이벤트에 저장
            foreach (var ability in e.unit.GetComponent<Unit>().Abilities)
            {
                ability.AbilityUsed += OnAbilityUsedLocal;
            }
            e.unit.GetComponent<Unit>().AbilityAddded += OnAbilityAdded;
        }

        // 유닛 추가시 능력 이벤트에 대한 리스너 추가
        private void OnAbilityAdded(object sender, AbilityAddedEventArgs e)
        {
            e.ability.AbilityUsed += OnAbilityUsedLocal;
        }

        // 로컬에서 능력 사용 이벤트 처리
        private void OnAbilityUsedLocal(object sender, (bool isNetworkInvoked, IDictionary<string, string> actionParams) e)
        {
            // If Ability was triggered by a remote instance, do nothing
            if (e.isNetworkInvoked)
            {
                return; // 네트워크에서 호출된 경우 아무 작업도 수행하지 않음
            }

            // If ability was triggered by the local instance, forward it to other players
            SendMatchState((int)OpCode.AbilityUsed, e.actionParams); // 로컬에서 호출된 경우 다른 플레이어에게 상태 전송
        }

        // 로컬에서 턴 종료 이벤트 처리
        private void OnTurnEndedLocal(object sender, bool isNetworkInvoked)
        {
            // If turn ending was triggered by a remote instance, do nothing
            if (isNetworkInvoked)
            {
                return; // 네크워크에서 호출된 경우 아무 작업도 수행하지 않음
            }

            // If turn ending was triggered by the local instance, forward it to other players
            SendMatchState((int)OpCode.TurnEnded, new Dictionary<string, string>()); // 로컬에서 호출된 경우 다른 플레이어에게 상태 전송
        }

        // 네트워크에서 능력 사용 이벤트 처리
        private void HandleRemoteAbilityUsed(Dictionary<string, string> actionParams)
        {
            var unit = _cellGrid.Units.Find(u => u.UnitID == int.Parse(actionParams["unit_id"]));
            var ability = unit.Abilities.Find(a => a.AbilityID == int.Parse(actionParams["ability_id"]));

            EventQueue.Enqueue((() => ability.OnAbilitySelected(_cellGrid), () => ability.Apply(_cellGrid, actionParams)));
            if (!processingEvents)
            {
                StartCoroutine(ProcessEvents());
            }
        }

        // 네트워크에서 턴 종료 이벤트 처리
        private void HandleRemoteTurnEnding(Dictionary<string, string> actionParams)
        {
            EventQueue.Enqueue((new Action(() => { }), () => EndTurn(true)));
            if (!processingEvents)
            {
                StartCoroutine(ProcessEvents());
            }
        }

        public void NetworkTest(IEnumerator coroutine)
        {
            EventQueue.Enqueue((new Action(() => { }), () => coroutine));
            if (!processingEvents)
            {
                StartCoroutine(ProcessEvents());
            }
        }

        // 턴 종료 처리
        protected IEnumerator EndTurn(bool isNetworkInvoked)
        {
            _cellGrid.EndTurn(isNetworkInvoked);
            yield return null;
        }

        // 이벤트 큐 처리 코루틴
        protected virtual IEnumerator ProcessEvents()
        {
            processingEvents = true;
            while (EventQueue.Count > 0)
            {
                var (preAction, routine) = EventQueue.Dequeue();

                preAction.Invoke();
                yield return StartCoroutine(routine.Invoke());
            }
            processingEvents = false;
        }
    }

    /// <summary>
    /// Represents the data for a room in a multiplayer game.
    /// Contains information about the room, such as the users in the room, room name, and ID.
    /// 멀티플레이어 게임에서 방의 데이터를 나타내는 클래스
    /// 방에 있는 사용자 정보, 방 이름, 방 ID등을 포함
    /// </summary>
    public class RoomData
    {
        /// <summary>
        /// The local user's network user data.
        /// 로컬 사용자의 네트워크 사용자 데이터
        /// </summary>
        public NetworkUser LocalUser { get; private set; }

        /// <summary>
        /// The collection of network users currently in the room.
        /// 방에 현재 있는 네트워크 사용자들의 모임
        /// </summary>
        public IEnumerable<NetworkUser> Users { get; private set; }

        /// <summary>
        /// The current number of users in the room.
        /// 방에 현재 있는 사용자의 수
        /// </summary>
        public int UserCount { get; private set; }

        /// <summary>
        /// The maximum number of users allowed in the room.
        /// 방에 허용된 최대 사용자 수
        /// </summary>
        public int MaxUsers { get; private set; }

        /// <summary>
        /// The name of the room.
        /// 방의 이름
        /// </summary>
        public string RoomName { get; private set; }

        /// <summary>
        /// The unique identifier for the room.
        /// 방의 고유 식별자
        /// </summary>
        public string RoomID { get; private set; }

        /// <summary>
        /// Constructor for creating a new RoomData instance.
        /// 새로운 RoomData 인스턴스를 생성
        /// </summary>
        /// <param name="localUser">Local user's network data.</param>
        /// <param name="users">List of users in the room.</param>
        /// <param name="userCount">Number of users currently in the room.</param>
        /// <param name="maxUsers">Maximum number of users allowed in the room.</param>
        /// <param name="roomName">Name of the room.</param>
        /// <param name="roomID">Unique identifier of the room.</param>
        public RoomData(NetworkUser localUser, IEnumerable<NetworkUser> users, int userCount, int maxUsers, string roomName, string roomID)
        {
            LocalUser = localUser;
            Users = users;
            UserCount = userCount;
            MaxUsers = maxUsers;
            RoomName = roomName;
            RoomID = roomID;
        }
    }

    /// <summary>
    /// Represents a user in a multiplayer network environment.
    /// Contains information such as the user's ID, name, and custom properties.
    /// 멀티플레이어 네트워크 환경에서 사용자를 나타냅니다.
    /// 사용자의 ID, 이름, 사용자 정의 속성 등의 정보를 포함
    /// </summary>
    public class NetworkUser
    {
        /// <summary>
        /// The unique identifier for the user.
        /// 사용자의 고유 식별자
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// The name of the user.
        /// 사용자의 이름
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Indicates whether the user is the host of the room.
        /// 사용자가 방의 호스트인지 여부
        /// </summary>
        public bool IsHost { get; private set; }

        /// <summary>
        /// Custom properties associated with the user, represented as key-value pairs.
        /// 사용자 정의 속성으로, 키-값으로 표현
        /// </summary>
        public Dictionary<string, string> CustomProperties { get; private set; }

        /// <summary>
        /// Constructor for creating a new NetworkUser instance.
        /// 새로운 NetworkUser 인스턴스 생성
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userID">Unique identifier of the user.</param>
        /// <param name="customProperties">Custom properties of the user.</param>
        /// <param name="isHost">Indicates whether the user is the host of the room.</param>
        public NetworkUser(string userName, string userID, Dictionary<string, string> customProperties, bool isHost = false)
        {
            UserName = userName;
            UserID = userID;
            CustomProperties = customProperties;
            IsHost = isHost;
        }

        /// <summary>
        /// 지정된 객체가 현재 객체와 같은지 결정
        /// </summary>
        /// <param name="obj">현재 객체와 비교할 객체</param>
        /// <returns>지정된 객체가 NetworkUser형식이고 UserID가 현재 사용자와 같으면 true, 그렇지 않으면 false</returns>
        public override bool Equals(object obj)
        {
            return obj is NetworkUser && (obj as NetworkUser).UserID.Equals(UserID);
        }

        /// <summary>
        /// 기본 해시 함수로 사용
        /// </summary>
        /// <returns>현재 사용자의 UserID에 대한 해시코드를 반환</returns>
        public override int GetHashCode()
        {
            return UserID.GetHashCode();
        }
    }


    public enum OpCode : long
    {
        TurnEnded,              // 턴이 종료되었음
        AbilityUsed,            // 능력이 사용되었음
        PlayerNumberChanged,    // 플레이어 수가 변경되었음
        IsReadyChanged,         // 준비 상태가 변경되었음
        Else,
    }
}