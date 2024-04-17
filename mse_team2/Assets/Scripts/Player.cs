using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection.Emit;

[Serializable]
public class Player
{
    private string ID;
    private string Nickname;
    private string Password;

    private int totalGameNumber;
    private int winGameNumber;

    private int easymodeTotalGameNumber;
    private int easymodeWinGameNumber;

    private int hardmodeTotalGameNumber;
    private int hardmodeWinGameNumber;


    public void SetID(string ID)
    {
        this.ID = ID;
    }
    
    public void SetNickname(string Nickname)
    {
        this.Nickname = Nickname;
    }

    public void SetPassword(string Password)
    {
        this.Password = Password;
    }
}
