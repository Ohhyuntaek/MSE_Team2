using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for game result information
// this will be used when the game result is sent to server part after game ends
[Serializable]
public class GameResultInfo : MonoBehaviour
{
    public long privateCode;
    public string gameMode;
    public string result;

    public void setPrivateCode(long privateCode){
        this.privateCode = privateCode;
    }
    public void setGameMode(string gameMode){
        this.gameMode = gameMode;
    }
    public void setResult(string result){
        this.result = result;
    }
}
