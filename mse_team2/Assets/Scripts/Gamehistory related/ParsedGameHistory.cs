using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class contains the information of the game history of the player (not static)
[Serializable]
public class ParsedGameHistory
{
    public long privateCode;
    public int easyGame;
    public int easyWin;
    public int hardGame;
    public int hardWin;

    public ParsedGameHistory() {

    }
}
