using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;

    private float refreshRoomTime;  // 房间刷新的时间

    public GameObject[] LinkBtns;
    
#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
    
    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
    }

    /// <summary>
    /// 搜索房间
    /// </summary>
    public void FindServer()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    public void StartHost()
    {
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
    }

    /// <summary>
    /// 退出房间
    /// </summary>
    public void ExitHost()
    {
        if (NetworkServer.active && NetworkClient.isConnected)  // 当房主退出房间
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)  // 非房主退出房间
        {
            NetworkManager.singleton.StopClient();
        }
        networkDiscovery.StopDiscovery();
    }
    
    public void Connect(ServerResponse info)
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        FindServer();
    }

    // Update is called once per frame
    void Update()
    {
        refreshRoomTime += Time.deltaTime;
        if (refreshRoomTime > 1.0f)
        {
            refreshRoomTime = 0.0f;

            for (var i = 0; i < LinkBtns.Length; i++)
            {
                int i2 = i;
                
                LinkBtns[i].SetActive(false);
                var eventWrapper = LinkBtns[i].GetComponent<PointableUnityEventWrapper>();
                eventWrapper.WhenRelease.RemoveAllListeners();
                eventWrapper.WhenRelease.AddListener(delegate
                {
                    LinkBtns[i2].transform.GetChild(2).GetChild(1).GetComponent<AudioTrigger>().PlayAudio();
                });
            }

            var index = 0;
            foreach (var info in discoveredServers.Values)  // 循环读取扫描到的房间信息
            {
                LinkBtns[index].GetComponent<PointableUnityEventWrapper>().WhenRelease.AddListener(delegate
                {
                    Connect(info);
                });  // 将连接房间的方法添加到按钮监听器上
                LinkBtns[index].transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshPro>().text =
                    info.EndPoint.Address.ToString();  // 显示房间IP
                LinkBtns[index].SetActive(true);
                index += 1;
            }
        }
    }
}
 