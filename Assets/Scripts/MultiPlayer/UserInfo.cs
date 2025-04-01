using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static UserInfo Instance;

    private void Awake()
    {
        Instance = this;
    }

    public string playerName;
}
