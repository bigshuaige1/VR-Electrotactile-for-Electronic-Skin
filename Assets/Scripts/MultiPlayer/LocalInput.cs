using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInput : MonoBehaviour
{
    public static LocalInput Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Transform head, leftHand, rightHand;
    public Transform rootHead, rootLeftHand, rootRightHand;

    public SkinnedMeshRenderer leftHandMesh, rightHandMesh;

    private void Update()
    {
        rootHead.position = head.position;
        rootHead.rotation = head.rotation;
        
        rootLeftHand.position = leftHand.position;
        rootLeftHand.rotation = leftHand.rotation;
        
        rootRightHand.position = rightHand.position;
        rootRightHand.rotation = rightHand.rotation;
    }
}
