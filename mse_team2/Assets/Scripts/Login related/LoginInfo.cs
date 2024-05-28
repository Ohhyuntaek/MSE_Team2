using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// this class handles login information to communicate with server part
[Serializable]
public class LoginInfo
{
    public string ID;
    public string Password;

    public void SetID(string ID)
    {
        this.ID = ID;
    }

    public void SetPassword(string Password)
    {
        this.Password = Password;
    }
}
