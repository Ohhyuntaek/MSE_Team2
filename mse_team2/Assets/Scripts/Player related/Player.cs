using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerServer  // Prevents this script from being exposed externally causing it to be misreferenced.
{
    // class that contains the player information during the whole app playing
    // data won't be removed or initiated even the scene is changed during the game
    // data will be updated or removed, initiated only when the player signup/login/edit information/delete information
    [Serializable]
    public class Player : MonoBehaviour
    {
        public static long privateCode;
        public static string id;
        public static string nickname;
        public static string password;

        public Player(ParsedPlayer pp)
        {
            privateCode = pp.privateCode;
            id = pp.id;
            nickname = pp.nickname;
            password = pp.password;
        }
    }
}
