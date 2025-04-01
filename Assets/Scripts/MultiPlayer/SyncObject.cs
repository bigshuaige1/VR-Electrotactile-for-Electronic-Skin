using System.Collections;
using System.Collections.Generic;
using Mirror;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class SyncObject : NetworkBehaviour
{
    [SyncVar] public bool isInteracting;  // 当前物体是否正在被交互

    public bool isLocalInteracting;

    [SyncVar] public Vector3 localPosition;
    [SyncVar] public Quaternion localRotation;
    [SyncVar] public Vector3 localScale;

    // 获取交互组件，在有玩家交互时将交互组件关闭，防止多个人操作一个物体
    public GrabInteractable[] grabInteractables;
    public HandGrabInteractable[] handGrabInteractables;
    
    public void StartSync()  // 本地开始交互时调用
    {
        isLocalInteracting = true;
    }

    public void EndSync()  // 本地结束交互时调用
    {
        isLocalInteracting = false;
    }
    
    [Command(requiresAuthority = false)]
    public void StartSyncToServer()  // 开始交互时调用。添加了允许客户端调用服务器端的指令，本地和其他用户的交互动作都可以调用
    {
        isInteracting = true;
    }

    [Command(requiresAuthority = false)]
    public void EndSyncToServer()
    {
        isInteracting = false;
    }

    [Command(requiresAuthority = false)]  // 添加标签，让客户端可以调用服务端方法
    void SyncData(Vector3 position, Quaternion rotation, Vector3 scale)  // 将本地操作时物体发生的姿态变换上传至服务器，同步至其他玩家
    {
        localPosition = position;
        localRotation = rotation;
        localScale = scale;
    }

    void SyncToObject()
    {
        Transform p = transform.parent;
        p.localPosition = localPosition;
        p.localRotation = localRotation;
        p.localScale = localScale;
    }

    void HandleInteractable()
    {
        if (isInteracting && !isLocalInteracting)
        {
            foreach (var grabInteractable in grabInteractables)
                grabInteractable.enabled = false;
            foreach (var handGrabInteractable in handGrabInteractables)
                handGrabInteractable.enabled = false;
        }
        else
        {
            foreach (var grabInteractable in grabInteractables)
                grabInteractable.enabled = true;
            foreach (var handGrabInteractable in handGrabInteractables)
                handGrabInteractable.enabled = true;
        }
    }

    void Start()
    {
        Transform p = transform.parent;
        localPosition = p.localPosition;
        localRotation = p.localRotation;
        localScale = p.localScale;
    }

    void Update()
    {
        HandleInteractable();
        
        if (isLocalInteracting)
        {
            Transform p = transform.parent;
            SyncData(p.localPosition, p.localRotation, p.localScale);
        }
        else  // 其他玩家处从服务器接收数据
        {
            SyncToObject();
        }
    }
}
