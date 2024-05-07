using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Network;
using System.Threading.Tasks;

public class ImplementNetworkConnection : NetworkConnection
{
    public override void ConnectToServer(string userName, Dictionary<string, string> customParams)
    {
        throw new System.NotImplementedException();
    }

    public override void CreateRoom(string roomName, int maxPlayers, bool isPrivate, Dictionary<string, string> customParams)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IEnumerable<RoomData>> GetRoomList()
    {
        throw new System.NotImplementedException();
    }

    public override void JoinQuickMatch(int maxPlayers, Dictionary<string, string> customParams)
    {
        throw new System.NotImplementedException();
    }

    public override void JoinRoomByID(string roomID)
    {
        throw new System.NotImplementedException();
    }

    public override void JoinRoomByName(string roomName)
    {
        throw new System.NotImplementedException();
    }

    public override void LeaveRoom()
    {
        throw new System.NotImplementedException();
    }

    public override void SendMatchState(long opCode, IDictionary<string, string> actionParams)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
