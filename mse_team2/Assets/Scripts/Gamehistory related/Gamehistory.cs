using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that contains the player's game history information during the whole app playing
// data won't be removed or initiated even the scenen is changed during the game
// data will be updated or removed, initiated only when the player signup/login/finish game/delete information
[Serializable]
public class Gamehistory : MonoBehaviour
{
    public static long privateCode;
    public static int easyGame;
    public static int easyWin;
    public static int hardGame;
    public static int hardWin;

    public Gamehistory(ParsedGameHistory pgh)
    {
        privateCode = pgh.privateCode;
        easyGame = pgh.easyGame;
        easyWin = pgh.easyWin;
        hardGame = pgh.hardGame;
        hardWin = pgh.hardWin;
    }
}
