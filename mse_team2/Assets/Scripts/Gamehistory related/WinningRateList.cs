using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to parse ranking list gotten from server
[Serializable]
public class WinningRateList
{
    public List<WinningRate> Items;

    public WinningRateList() {
    }

}
