using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to parse winning rate data got from the server for ranking system
[Serializable]
public class WinningRate
{
    public string nickname;
    public double winningRate;

    public WinningRate(){

    }
}
