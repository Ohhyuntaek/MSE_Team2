using System;
using System.Collections;
using System.Collections.Generic;
using TbsFramework.Grid;
using TbsFramework.Players;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TbsFramework.Network
{
    public class NetworkGUI : MonoBehaviour
    {
        /// <summary>
        /// The NetworkConnection object assosiated with this scene
        /// 이 씬에 연결된 NetworkConnection 객체
        /// </summary>
        [SerializeField] private NetworkConnection _networkConnection;
        [SerializeField] private CellGrid _cellGrid;                // 셀 그리드의 참조

        [SerializeField] private Text _statusText;                  // 상태 메세지를 표시할 텍스트 UI 컴포넌트
        [SerializeField] private Text _roomNameText;                // 방 이름을 표시할 텍스트 UI 컴포넌트

        [SerializeField] private GameObject _lobbyPanel;            // 로비 패널 UI 객체
        [SerializeField] private GameObject _roomPanel;             // 방 패널 UI 객체

        [SerializeField] private GameObject _playerEntryPrefab;     // 플레이어 항목을 위한 프리팹
        [SerializeField] private GameObject _roomEntryPrefab;       // 방 항목을 위한 프리펩

        [SerializeField] private InputField _usernameInput;         // 사용자 이름을 입력받는 인풋 필드
        [SerializeField] private InputField _createRoomNameInput;   // 방 생성 시 방 이름을 입력받는 인풋 필드
        [SerializeField] private InputField _joinRoomNameInput;     // 방 입장 시 방 이름을 입력받는 인풋 필드

        [SerializeField] private Button _connectButton;             // 서버에 연결하는 버튼
        [SerializeField] private Button _quickMatchButton;          // 빠른 매치에 참여하는 버튼
        [SerializeField] private Button _createRoomButton;          // 방을 생성하는 버튼
        [SerializeField] private Button _joinRoomButton;            // 방에 입장하는 버튼
        [SerializeField] private Button _leaveRoomButton;           // 방을 나가는 버튼
           
        [SerializeField] private Toggle _isPrivateToggle;           // 방이 비공개인지 여부를 설정하는 토글

        [SerializeField] private Transform _playersParent;          // 플레이어 UI 항목들을 담을 부모 트랜스폼

        private NetworkUser _localUser;         // 로컬 사용자 객체
        private int _localPlayerNumber;         // 로컬 플레이어의 번호
        private Dictionary<string, GameObject> _playerPanels = new Dictionary<string, GameObject>();    // 플레이어 패널을 관리하기 위한 딕셔너리
        private List<GameObject> _roomPanels = new List<GameObject>();  // 방 패널을 관리하기 위한 리스트
        private int _readyCount = 0;            // 준비된 플레이어의 수

        /// <summary>
        /// The maximum number of human players that can join and participate in a multiplayer game session.
        /// 멀티플레이어 게임 세션에 참가할 수 있는 최대 플레이어 수
        /// </summary>
        [SerializeField] private int _maxPlayers = 2;

        private void Start()
        {
            // 네트워크 연결 관련 이벤트 핸들러 등록
            _networkConnection.ServerConnected += OnServerConnected;
            _networkConnection.RoomJoined += OnRoomJoined;
            _networkConnection.RoomExited += OnRoomExited;
            _networkConnection.PlayerEnteredRoom += OnPlayerEnteredRoom;
            _networkConnection.PlayerLeftRoom += OnPlayerLeftRoom;
            _networkConnection.CreateRoomFailed += OnFailed;
            _networkConnection.JoinRoomFailed += OnFailed;

            // 특정 OpCode에 대한 네트워킁 이벤트 핸들러 추가
            _networkConnection.AddHandler((actionParams) => OnPlayerNumberChanged(actionParams), (long)OpCode.PlayerNumberChanged);
            _networkConnection.AddHandler((actionParams) => OnPlayerReadyChanged(actionParams), (long)OpCode.IsReadyChanged);

            // 버튼 이벤트 리스너 등록
            _createRoomButton.onClick.AddListener(() => _networkConnection.CreateRoom(_createRoomNameInput.text, _maxPlayers, _isPrivateToggle.isOn, new Dictionary<string, string>()));
            _joinRoomButton.onClick.AddListener(() => _networkConnection.JoinRoomByName(_joinRoomNameInput.text));
            _leaveRoomButton.onClick.AddListener(() => { _networkConnection.LeaveRoom(); });
        }

        /// <summary>
        /// 연걸, 방 입장 실패 등의 오류 시 상태 메세지를 업데이트하는 메서드
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체</param>
        /// <param name="message">표시할 실패 메세지</param>
        private void OnFailed(object sender, string message)
        {
            SetStatus(message); // 실패 메세지 설정
        }

        /// <summary>
        /// 플레이어가 방을 떠났을 때 실행되는 메서드
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체</param>
        /// <param name="networkUser">방을 떠난 네트워크 사용자 정보</param>
        private void OnPlayerLeftRoom(object sender, NetworkUser networkUser)
        {
            Destroy(_playerPanels[networkUser.UserID]); // 플레이어 패널 제거
            _playerPanels.Remove(networkUser.UserID);   // 플레이어 패널 리스트에서 제거
            _readyCount -= networkUser.CustomProperties.ContainsKey("isReady") ? bool.Parse(networkUser.CustomProperties["isReady"]) ? 1 : 0 : 0;

            _cellGrid.CheckGameFinished();  // 게임 종료 확인
        }

        /// <summary>
        /// 새로운 플레이어가 방에 입장했을 때 실행되는 메서드
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체</param>
        /// <param name="networkUser">방에 입장한 네트워크 사용자 정보</param>
        private void OnPlayerEnteredRoom(object sender, NetworkUser networkUser)
        {
            var playerSelectionPanelInstance = CreatePlayerPanel(networkUser, _playerPanels.Count + 1, _maxPlayers, string.Empty, false);
            playerSelectionPanelInstance.SetActive(true);

            _playerPanels.Add(networkUser.UserID, playerSelectionPanelInstance);
        }

        /// <summary>
        /// 방에 성공적으로 입장했을 때 실행되는 메서드
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체</param>
        /// <param name="roomData">입장한 방의 데이터</param>
        private void OnRoomJoined(object sender, RoomData roomData)
        {
            _createRoomNameInput.text = string.Empty;               // 방 생성 필드 초기화
            _quickMatchButton.interactable = false;                 // 빠른 매치 버튼 비활성화

            _playerPanels = new Dictionary<string, GameObject>();   // 플레이어 패널 초기화
            _localUser = roomData.LocalUser;                        // 로컬 사용자 설정

            _lobbyPanel.SetActive(false);               // 로비 패널 비활성화
            _roomPanel.SetActive(true);                 // 방 패널 활성화
            _roomNameText.text = roomData.RoomName;     // 방 이름 설정
            int userIndex = 1;
            foreach (var networkUser in roomData.Users)
            {
                var playerNumber = networkUser.CustomProperties.ContainsKey("playerNumber") ? networkUser.CustomProperties["playerNumber"] : string.Empty;
                var isReady = networkUser.CustomProperties.ContainsKey("isReady") && bool.Parse(networkUser.CustomProperties["isReady"]);
                var playerSelectionPanelInstance = CreatePlayerPanel(networkUser, userIndex, roomData.MaxUsers, playerNumber, isReady);
                playerSelectionPanelInstance.SetActive(true);

                _playerPanels.Add(networkUser.UserID, playerSelectionPanelInstance);
                userIndex += 1;
            }
            SetStatus("Room joined");           // 상태 메세지 업데이트
        }

        /// <summary>
        /// 방에서 나갔을 때 실행되는 메서드
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체</param>
        /// <param name="e">이벤트 데이터</param>
        private void OnRoomExited(object sender, EventArgs e)
        {
            _lobbyPanel.SetActive(true);                // 로비 패널 활성화
            _roomPanel.SetActive(false);                // 방 패널 비활성화
            _quickMatchButton.interactable = true;      // 빠른 매치 버튼 활성화
            _readyCount = 0;

            foreach (var key in _playerPanels.Keys)
            {
                Destroy(_playerPanels[key]);
            }
            _playerPanels = new Dictionary<string, GameObject>();

            SetStatus("Room exited");           // 상태 메세지 업데이트
        }

        /// <summary>
        /// 플레이어 패널을 생성하고 반환하는 메서드
        /// </summary>
        /// <param name="user">플레이어에 대한 네트워크 사용자 정보</param>
        /// <param name="userIndex">플레이어의 인덱스</param>
        /// <param name="maxUserCount">최대 플레이어 수</param>
        /// <param name="playerNumber">플레이어의 번호</param>
        /// <param name="isReady">플레이어의 준비 상태</param>
        /// <returns>생성된 플레이어 패널</returns>
        private GameObject CreatePlayerPanel(NetworkUser user, int userIndex, int maxUserCount, string playerNumber, bool isReady)
        {
            // localUser가 설정되지 않았을 경우 에러를 발생시키는 Assert문
            Assert.IsNotNull(_localUser, $"{nameof(_localUser)} field is not set up");

            // 플레이어 패널 프리팹을 복제하여 인스턴스 생성
            var playerSelectionPanelInstance = Instantiate(_playerEntryPrefab, _playerEntryPrefab.transform.parent);
            // 플레이어 번호를 표시하는 UI 업데이트
            playerSelectionPanelInstance.transform.Find("Player#").GetComponent<Text>().text = string.Format("#{0}", userIndex.ToString());
            // 플레이어 이름을 표시하는 UI 업데이트
            playerSelectionPanelInstance.transform.Find("PlayerName").GetComponent<Text>().text = user.UserName;

            // 플레이어 번호 입력 필드의 텍스트 설정
            playerSelectionPanelInstance.transform.Find("PlayerNumber").GetComponentInChildren<InputField>().text = playerNumber;
            // 현재 사용자의 ID와 일치하면 플레이어 번호 입력 필드 활성화
            playerSelectionPanelInstance.transform.Find("PlayerNumber").GetComponent<InputField>().interactable = user.UserID.Equals(_localUser.UserID);
            // 현재 사용자의 ID와 일치하면 플레이어 번호 변경 이벤트 처리
            if (user.UserID.Equals(_localUser.UserID))
            {
                playerSelectionPanelInstance.transform.Find("PlayerNumber").GetComponent<InputField>().onValueChanged.AddListener((value) =>
                {
                    // 준비 토글이 비어있지 않고 현재 사용자의 ID와 일치하면 토글 활성화
                    playerSelectionPanelInstance.transform.Find("IsReady").GetComponent<Toggle>().interactable = value != string.Empty && user.UserID.Equals(_localUser.UserID);
                    // 로컬 플레이어 번호 업데이트
                    _localPlayerNumber = int.Parse(value);
                    // 플레이어 번호 ㅅ변경 메세지 작성 및 전송
                    var actionParams = new Dictionary<string, string>
                    {
                        { "user_id", _localUser.UserID },
                        { "player_number", value.ToString() }
                    };
                    _networkConnection.SendMatchState((long)OpCode.PlayerNumberChanged, actionParams);
                });
            }

            // 준비 토글의 상태 설정
            playerSelectionPanelInstance.transform.Find("IsReady").GetComponent<Toggle>().isOn = isReady;
            // 플레이어 번호가 비어 있지 않고 현재 사용자의 ID와 일치하면 준비 토글 활성화
            playerSelectionPanelInstance.transform.Find("IsReady").GetComponent<Toggle>().interactable = playerNumber != string.Empty && user.UserID.Equals(_localUser.UserID);
            // 현재 사용자의 ID와 일치하면 준비 토글 이벤트 처리
            if (user.UserID.Equals(_localUser.UserID))
            {
                playerSelectionPanelInstance.transform.Find("IsReady").GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                {
                    // 플레이어 번호 입력 필드 비활성화
                    playerSelectionPanelInstance.transform.Find("PlayerNumber").GetComponent<InputField>().interactable = !value;

                    // 준비 상태 변경 메세지 작성 및 전송
                    var actionParams = new Dictionary<string, string>
                    {
                    { "user_id", _localUser.UserID },
                    { "is_ready", value.ToString() }
                    };
                    _networkConnection.SendMatchState((long)OpCode.IsReadyChanged, actionParams);

                    // 준비된 플레이어 수 업데이트 및 모든 플레이어가 준비되었는지 확인
                    _readyCount += value ? 1 : -1;
                    if (_readyCount == maxUserCount)
                    {
                        // 매치 설정 시작
                        StartCoroutine(SetupMatch());
                    }
                });
            }

            // 생성된 플레이어 패널 반환
            return playerSelectionPanelInstance;
        }

        /// <summary>
        /// 플레이어의 준비 상태가 변경되었을 때 호출되는 메서드
        /// </summary>
        /// <param name="actionParams"></param>
        private void OnPlayerReadyChanged(Dictionary<string, string> actionParams)
        {
            // 메세지에서 유저 아이디 가져오기
            var userID = actionParams["user_id"];
            // 로컬 사용자와 메세지에서 가져온 아이디가 같으면 함수 종료
            if (userID.Equals(_localUser.UserID))
            {
                return;
            }

            // 준비 상태 가져오기
            var isReady = bool.Parse(actionParams["is_ready"]);
            // 준비 상태에 따라 _readyCount 업데이트
            _readyCount += isReady ? 1 : -1;

            // 모든 플레이어가 준비되었는지 확인
            if (_readyCount == _maxPlayers)
            {
                // 매치 설정 시작 코루틴 호출
                StartCoroutine(SetupMatch());
            }

            // 해당 유저의 준비 상태를 UI에 반영
            _playerPanels[userID].transform.Find("IsReady").GetComponent<Toggle>().isOn = bool.Parse(actionParams["is_ready"]);
        }

        /// <summary>
        /// 플레이어 번호가 변경되었을 때 호출되는 메서드
        /// </summary>
        /// <param name="actionParams"></param>
        private void OnPlayerNumberChanged(Dictionary<string, string> actionParams)
        {
            // 메세지에서 유저 아이디 가져오기
            var userID = actionParams["user_id"];
            // 로컬 사용자와 메세지에서 가져온 아이디가 같으면 함수 종료
            if (userID.Equals(_localUser.UserID))
            {
                return;
            }

            // 해당 유저의 플레이어 번호 입력 필드를 활성화
            _playerPanels[userID].transform.Find("PlayerNumber").GetComponent<InputField>().interactable = true;
            // 해당 유저의 플레이어 번호를 메세지에서 가져온 값으로 설정
            _playerPanels[userID].transform.Find("PlayerNumber").GetComponent<InputField>().text = actionParams["player_number"];
            // 해당 유저의 플레이어 번호 입력 필드를 비활성화
            _playerPanels[userID].transform.Find("PlayerNumber").GetComponent<InputField>().interactable = false;
        }

        /// <summary>
        /// 서버에 연결되었을 때 호출되는 메서드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnServerConnected(object sender, EventArgs e)
        {
            // 상태를 "Connected"로 설정
            SetStatus("Connected");
            // 퀵 매치 버튼 활성화
            _quickMatchButton.interactable = true;
            // 연결 버튼 비활성화
            _connectButton.interactable = false;
            // 로비 패널 활성화
            _lobbyPanel.SetActive(true);
        }

        /// <summary>
        /// 매치 설정을 위한 코루틴
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetupMatch()
        {
            // 모든 플레이어에 대해 반복
            for (int i = 0; i < _playersParent.childCount; i++)
            {
                var playerGO = _playersParent.GetChild(i).gameObject;
                // 플레이어가 활성화되어 있지 않으면 다음으로 넘어감
                if (!playerGO.activeInHierarchy)
                {
                    continue;
                }

                // 플레이어 컴포넌트 가져오기
                var player = playerGO.GetComponent<Player>();
                // 플레이어 번호 가져오기
                var playerNumber = player.PlayerNumber;

                // 로컬 플레이어가 아니고, 플레이어가 HumanPlayer이거나 AIPlayer이고 호스트가 아닌 경우
                if (!playerNumber.Equals(_localPlayerNumber) && player is HumanPlayer || player is AIPlayer && !_networkConnection.IsHost)
                {
                    // 플레이어 오브젝트 제거
                    Destroy(player);
                    // RemotePlayer 컴포넌트 추가
                    var remotePlayer = playerGO.AddComponent<RemotePlayer>();
                    // 네트워크 연결 설정
                    remotePlayer.NetworkConnection = _networkConnection;
                    // 플레이어 번호 설정
                    remotePlayer.PlayerNumber = playerNumber;
                }
            }

            // 한 프레임을 기다린 후 게임 오브젝트 비활성화
            yield return new WaitForEndOfFrame();
            gameObject.SetActive(false);

            // 셀 그리도 초기화 및 시작
            _cellGrid.InitializeAndStart();
        }

        /// <summary>
        /// 서버에 연결하는 메서드
        /// </summary>
        public void ConnectToServer()
        {
            // 상태를 "Connecting..."으로 설정
            SetStatus("Connecting...");
            // 사용자 이름 가져오기
            var userName = _usernameInput.text;
            // 서버에 연결 요청
            _networkConnection.ConnectToServer(userName, new Dictionary<string, string>());
        }

        /// <summary>
        /// 퀵 매치에 참여하는 메서드
        /// </summary>
        public void JoinQuickMatch()
        {
            // 상태를 "Looking for match..."으로 설정
            SetStatus("Looking for match...");
            // 최대 플레이어 수를 지정하여 퀵 매치에 참여
            _networkConnection.JoinQuickMatch(_maxPlayers, new Dictionary<string, string>());
        }

        /// <summary>
        /// 로비를 새로고침하는 매서드
        /// </summary>
        public async void RefreshLobby()
        {
            // 기존의 모든 방 패널을 제거
            for (int i = 0; i < _roomPanels.Count; i++)
            {
                Destroy(_roomPanels[i]);
            }
            _roomPanels.Clear();

            // 방 인덱스 초기화
            var roomIndex = 1;
            // 서버로부터 방 목록을 가져와서 각각의 방 패널을 생성
            foreach (var room in await _networkConnection.GetRoomList())
            {
                var roomEntry = Instantiate(_roomEntryPrefab, _roomEntryPrefab.transform.parent);
                roomEntry.transform.Find("Room#").GetComponent<Text>().text = string.Format("#{0}", roomIndex.ToString());
                roomEntry.transform.Find("RoomNameText").GetComponent<Text>().text = room.RoomName;
                roomEntry.transform.Find("RoomCapacityText").GetComponent<Text>().text = string.Format("{0}/{1}", room.UserCount, room.MaxUsers);
                roomEntry.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => _networkConnection.JoinRoomByID(room.RoomID));
                roomEntry.SetActive(true);

                _roomPanels.Add(roomEntry);
            }
        }

        // 상태를 설정하는 메서드
        private void SetStatus(string status)
        {
            // 상태 텍스트 업데이트
            _statusText.text = status;
        }
    }
}