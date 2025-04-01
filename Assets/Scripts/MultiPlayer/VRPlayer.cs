using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using NetSync;

public class VRPlayer : NetworkBehaviour
{
    [SyncVar] public string playerName;

    [SyncVar] public bool isMaster;  // 是否为房主

    public Transform playerPanel;

    public TextMeshPro nameTest;

    [SyncVar] public uint playerID;

    public Transform head, leftHand, rightHand;
    public SyncTransform headData = new SyncTransform();
    public SyncTransform leftHandData = new SyncTransform();
    public SyncTransform rightHandData = new SyncTransform();
    [SyncVar] public string headDataJson = "";
    [SyncVar] public string leftHandDataJson = "";
    [SyncVar] public string rightHandDataJson = "";

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)  // 玩家为本地用户
        {
            playerName = UserInfo.Instance.playerName;
            if (isServer)
                isMaster = true;

            playerID = GetComponent<NetworkIdentity>().netId;
            
            head.gameObject.SetActive(false);
            leftHand.GetChild(0).gameObject.SetActive(false);
            rightHand.GetChild(0).gameObject.SetActive(false);
            playerPanel.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        nameTest.text = playerName + playerID;

        if (isLocalPlayer)  // 判断是否为本地用户还是其他玩家。若为本地用户，则吧本地获取到的输入转为json上传，其他玩家则是将服务器同步过来的json数据转为中转数据（SyncTransform）再驱动到本地的物体上
        {
            headDataJson = headData.SyncObjectToServer(LocalInput.Instance.rootHead);
            leftHandDataJson = leftHandData.SyncObjectToServer(LocalInput.Instance.rootLeftHand);
            rightHandDataJson = rightHandData.SyncObjectToServer(LocalInput.Instance.rootRightHand);
        }
        else
        {
            if (headDataJson != string.Empty) 
                headData.SyncObjectToLocal(head, headDataJson);
            if (leftHandDataJson != string.Empty) 
                leftHandData.SyncObjectToLocal(leftHand, leftHandDataJson);
            if (rightHandDataJson != string.Empty) 
                rightHandData.SyncObjectToLocal(rightHand, rightHandDataJson);
            playerPanel.transform.position = head.position + new Vector3(0, 0.15f, 0);

            Vector3 dir = LocalInput.Instance.head.position - playerPanel.position;
            dir.y = 0f;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (dir != Vector3.zero)
                rotation = Quaternion.LookRotation(dir);
            playerPanel.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180, 0f);
        }
    }
}
