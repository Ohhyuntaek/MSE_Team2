using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
