using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace TbsFramework.Network
{
    public class PhotonConnection : NetworkConnection, IConnectionCallbacks, IMatchmakingCallbacks, IOnEventCallback, IInRoomCallbacks, ILobbyCallbacks
    {
        [SerializeField] private string _appID;

        private LoadBalancingClient _client = new LoadBalancingClient();
        private List<string> _roomList = new List<string>();
        private Dictionary<string, RoomData> _roomInfo = new Dictionary<string, RoomData>();

        public override bool IsHost { get => _client.LocalPlayer.IsMasterClient; protected set => base.IsHost = value; }

        public override void ConnectToServer(string nickName, Dictionary<string, string> customParams)
        {
            if (_appID.Equals(string.Empty))
            {
                throw new System.ArgumentNullException(_appID, "Set up AppID parameter before connection to Photon server\nFor more information, visit https://doc.photonengine.com/en-us/realtime/current/getting-started/obtain-your-app-id/ \n");
            }

            _client.NickName = nickName;
            _client.AddCallbackTarget(this);
            _client.ConnectUsingSettings(new AppSettings() { AppIdRealtime = _appID, FixedRegion = "eu" });
        }
        public override void JoinQuickMatch(int maxPlayers, Dictionary<string, string> customParams)
        {
            ClearLocalPlayerCustomProps();

            var localSeedCandidate = Random.Range(0, int.MaxValue);
            Hashtable customProperties = new Hashtable() { { "rng_seed", localSeedCandidate } };
            _client.LocalPlayer.SetCustomProperties(customProperties);

            _client.OpJoinRandomOrCreateRoom(null, new EnterRoomParams() { RoomOptions = new RoomOptions() { BroadcastPropsChangeToAll = true, MaxPlayers = (byte)maxPlayers, PublishUserId = true } });
        }
        public override void CreateRoom(string roomName, int maxPlayers, bool isPrivate, Dictionary<string, string> customParams)
        {
            ClearLocalPlayerCustomProps();

            var localSeedCandidate = Random.Range(0, int.MaxValue);
            Hashtable customProperties = new Hashtable() { { "rng_seed", localSeedCandidate } };
            _client.LocalPlayer.SetCustomProperties(customProperties);

            _client.OpCreateRoom(new EnterRoomParams() { RoomName = roomName, RoomOptions = new RoomOptions() { BroadcastPropsChangeToAll = true, IsVisible = !isPrivate, PublishUserId = true, MaxPlayers = (byte)maxPlayers } });
        }
        public override void JoinRoomByName(string roomName)
        {
            ClearLocalPlayerCustomProps();

            var localSeedCandidate = Random.Range(0, int.MaxValue);
            Hashtable customProperties = new Hashtable() { { "rng_seed", localSeedCandidate } };
            _client.LocalPlayer.SetCustomProperties(customProperties);

            _client.OpJoinRoom(new EnterRoomParams() { RoomName = roomName, RoomOptions = new RoomOptions() { BroadcastPropsChangeToAll = true, PublishUserId = true } });
        }

        public override void JoinRoomByID(string roomID)
        {
            _client.OpJoinRoom(new EnterRoomParams() { RoomName = roomID , RoomOptions = new RoomOptions() { BroadcastPropsChangeToAll = true, PublishUserId = true } });
        }
        public override void LeaveRoom()
        {
            _client.OpLeaveRoom(false);
        }
        //public override void UpdateLocalPlayerProperties(string key, string value)
        //{
        //    var properties = client.LocalPlayer.CustomProperties;
        //    if(properties.ContainsKey(key))
        //    {
        //        properties[key] = value;
        //    }
        //    else
        //    {
        //        properties.Add(key, value);
        //    }
        //    client.LocalPlayer.SetCustomProperties(properties);
        //}

        public override Task<IEnumerable<RoomData>> GetRoomList()
        {
            return Task.FromResult(_roomList.Where(r => _roomInfo[r].UserCount > 0).Select(r => _roomInfo[r]));
        }
        public override void SendMatchState(long opCode, IDictionary<string, string> actionParams)
        {
            _client.OpRaiseEvent((byte)opCode, actionParams, null, SendOptions.SendReliable);
        }

        public void OnConnectedToMaster()
        {
            _client.OpJoinLobby(TypedLobby.Default);
            InvokeServerConnected();
        }

        public void OnEvent(EventData photonEvent)
        {
            if (!Handlers.ContainsKey(photonEvent.Code))
            {
                return;
            }

            var actionParams = photonEvent.CustomData as Dictionary<string, string>;
            var actionHandler = Handlers[photonEvent.Code];
            actionHandler(actionParams);
        }

        public void OnJoinedRoom()
        {
            var masterClient = _client.CurrentRoom.GetPlayer(_client.CurrentRoom.MasterClientId);
            var rngSeed = (int)masterClient.CustomProperties["rng_seed"];
            InitializeRng(rngSeed);
            NetworkUser localUser = new NetworkUser(_client.NickName, _client.UserId, HashtableToDict(_client.LocalPlayer.CustomProperties));
            IEnumerable<NetworkUser> users = _client.CurrentRoom.Players.Keys.OrderBy(i => i).Select(i => _client.CurrentRoom.Players[i]).Select(p => new NetworkUser(p.NickName, p.UserId, HashtableToDict(p.CustomProperties)));
            InvokeRoomJoined(new RoomData(localUser, users, _client.CurrentRoom.PlayerCount, _client.CurrentRoom.MaxPlayers, _client.CurrentRoom.Name, _client.CurrentRoom.Name));
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            InvokePlayerEnteredRoom(new NetworkUser(newPlayer.NickName, newPlayer.UserId, HashtableToDict(newPlayer.CustomProperties)));
        }

        public void OnConnected()
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            InvokeCreateRoomFailed(message);
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnDisconnected(DisconnectCause cause)
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            InvokeJoinRoomFailed(message);
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            InvokeJoinRoomFailed(message);
        }

        public void OnLeftRoom()
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            InvokePlayerLeftRoom(new NetworkUser(otherPlayer.NickName, otherPlayer.UserId, HashtableToDict(otherPlayer.CustomProperties)));
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            var rngSeed = (int)newMasterClient.CustomProperties["rng_seed"];
            InitializeRng(rngSeed);
        }

        private void Update()
        {
            _client.Service();
        }

        public void OnJoinedLobby()
        {
        }

        public void OnLeftLobby()
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (var room in roomList)
            {
                if (!_roomInfo.ContainsKey(room.Name))
                {
                    _roomInfo.Add(room.Name, new RoomData(new NetworkUser(_client.LocalPlayer.NickName, _client.LocalPlayer.UserId, HashtableToDict(_client.LocalPlayer.CustomProperties)), Enumerable.Empty<NetworkUser>(), room.PlayerCount, room.MaxPlayers, room.Name, room.Name));
                    this._roomList.Add(room.Name);
                }
                _roomInfo[room.Name] = new RoomData(new NetworkUser(_client.LocalPlayer.NickName, _client.LocalPlayer.UserId, HashtableToDict(_client.LocalPlayer.CustomProperties)), Enumerable.Empty<NetworkUser>(), room.PlayerCount, room.MaxPlayers, room.Name, room.Name);
            }
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        private Dictionary<string, string> HashtableToDict(Hashtable hashtable)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var key in hashtable.Keys)
            {
                dict.Add(key.ToString(), hashtable[key].ToString());
            }

            return dict;
        }

        private void ClearLocalPlayerCustomProps()
        {
            var customProps = new Hashtable();
            foreach (var key in _client.LocalPlayer.CustomProperties.Keys)
            {
                customProps.Add(key, null);
            }
            _client.LocalPlayer.SetCustomProperties(customProps);
        }
    }
}