using System.Collections;
using System.Collections.Generic;
using Mirror;
using NetSync;
using UnityEngine;

public class HandAnimSync : NetworkBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public List<Transform> handJoint = new List<Transform>();  // 作为其他玩家的可视化展示，来源为 VRPlayer 中的手
    public List<Transform> localHandJoint = new List<Transform>();  // 本机玩家的数据，来源为本地 CameraRig 中的手

    public enum HandDir
    {
        Left,
        Right
    }

    public HandDir handDir = HandDir.Left;
    
    [System.Serializable]
    public class HandData  // List 的中转数据类，转为 JSON 进行同步。Unity 内置的 JSON 转换无法做 List 转换
    {
        public SyncTransform[] syncJointData;
    }

    public HandData handData = new HandData();

    [SyncVar] public string handDataJson = "";
    
    void Start()
    {
        if (isLocalPlayer)  // 本机玩家获取本地的手部数据
        {
            Transform[] bones = handDir == HandDir.Left
                ? LocalInput.Instance.leftHandMesh.bones
                : LocalInput.Instance.rightHandMesh.bones;
            
            foreach (var obj in bones)
                localHandJoint.Add(obj);
        }
        
        // 本机玩家与其他玩家都获取 VRPlayer 中的首部数据，用于在 update 中更新
        foreach (var obj in skinnedMesh.bones)
            handJoint.Add(obj);
        handData.syncJointData = new SyncTransform[handJoint.Count];
    }

    void Update()
    {
        if (isLocalPlayer)  // 本机玩家将手部数据保存为 JSON
        {
            for (var i = 0; i < handJoint.Count; i++)
            {
                if (handData.syncJointData[i] == null)
                    handData.syncJointData[i] = new SyncTransform();
                else
                    handData.syncJointData[i].SyncObjectToServer(localHandJoint[i]);  // 每个手部节点数据以 SyncTransform 数据类型保存
            }
            handDataJson = JsonUtility.ToJson(handData);
        }
        else  // 保存的 JSON 同步加载为其他玩家视角里的非本机玩家可视化
        {
            // 被 [SyncVar] 修饰的变量会在所有客户端同步
            handData = JsonUtility.FromJson<HandData>(handDataJson);
            if (handData != null && handData.syncJointData != null)
            {
                for (var i = 0; i < handData.syncJointData.Length; i++)
                {
                    if (handData.syncJointData[i] != null)
                        handData.syncJointData[i].SyncObjectToLocal(handJoint[i]);  // 每个手部节点数据从 SyncTransform 数据类型读取
                }
            }
        }
    }
}
