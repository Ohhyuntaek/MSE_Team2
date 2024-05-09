using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerServer  // Prevents this script from being exposed externally causing it to be misreferenced.
{
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
