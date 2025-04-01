using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class NetServerManager : MonoBehaviour
{
    public enum NetType
    {
        Server, Client
    }

    public NetType netType;
    public string serverIP;

    public float relinkTime;

    public TextMeshPro stateText;

    void InitialNetwork()
    {
        if (netType == NetType.Server)
        {
            // 服务器端开启服务器
            NetworkManager.singleton.StartServer();
        }
        else
        {
            // 客户端
            NetworkManager.singleton.networkAddress = serverIP;
            NetworkManager.singleton.StartClient();
        }
    }
    
    void Start()
    {
        if (netType == NetType.Server)
        {
            // 服务器端开启服务器
            NetworkManager.singleton.StartServer();
        }
        else
        {
            // 客户端
            NetworkManager.singleton.networkAddress = serverIP;
            NetworkManager.singleton.StartClient();
        }
    }

    void Update()
    {
        if (!NetworkManager.singleton.isNetworkActive)
        {
            stateText.text = "Link Server Error";
            relinkTime += Time.deltaTime;
            if (relinkTime > 5)
            {
                relinkTime = 0;
                
                if (netType == NetType.Server)
                {
                    // 服务器端开启服务器
                    NetworkManager.singleton.StartServer();
                }
                else
                {
                    // 客户端
                    NetworkManager.singleton.StartClient();
                }
            }
        }
        else
        {
            stateText.text = "Link Server Success";
        }
    }
}
