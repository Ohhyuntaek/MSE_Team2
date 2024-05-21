using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection.Emit;

// class contains the information of the player (not static)
[Serializable]
public class ParsedPlayer
{
    public long privateCode;
    public string id;
    public string nickname;
    public string password;

    public ParsedPlayer(){

    }
    
}
 