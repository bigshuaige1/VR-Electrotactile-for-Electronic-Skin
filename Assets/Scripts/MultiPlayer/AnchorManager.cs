using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using UnityEngine;
using Oculus.Interaction.Input;
using System;
using TMPro;

public class AnchorManager : MonoBehaviour
{
    public SpatialAnchorCoreBuildingBlock _spatialAnchorCore;
    public GameObject AnchorPrefab;
    public Transform RightHandIndexTip;

    public Hand rightHand;
    public bool isPinch;

    public TextMeshPro ShowInfo;
    public List<OVRSpatialAnchor> Anchors = new List<OVRSpatialAnchor>();

    private List<Guid> _uuids = new List<Guid>();
    private const string NumUuidsPlayerPref = "numUuids";

    public GameObject anchorContainer;

    public GameObject rightControllerAnchor;
    private GameObject anchorPrefab;
    
    public void SpawnSpatialAnchor(Vector3 position, Quaternion rotation)
    {
        _spatialAnchorCore.InstantiateSpatialAnchor(AnchorPrefab, position, rotation);
    }

    void SpawnSpatialAnchor()
    {
        SpawnSpatialAnchor(RightHandIndexTip.position, RightHandIndexTip.rotation);
    }
    
    void LoadAnchorsFromDefaultLocalStorage()
    {
        var uuids = GetAnchorAnchorUuidFromLocalStorage();
        if (uuids == null) return;
        _spatialAnchorCore.LoadAndInstantiateAnchors(AnchorPrefab, uuids);
    }
    
    internal List<Guid> GetAnchorAnchorUuidFromLocalStorage()
    {
        // Get number of saved anchor uuids
        if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
        {
            Reset();
            Debug.Log($"[{nameof(SpatialAnchorLocalStorageManagerBuildingBlock)}] Anchor not found.");
            return null;
        }

        // Load unbounded anchors
        _uuids.Clear();
        var playerUuidCount = PlayerPrefs.GetInt(NumUuidsPlayerPref);
        for (int i = 0; i < playerUuidCount; ++i)
        {
            var uuidKey = "uuid" + i;
            if (!PlayerPrefs.HasKey(uuidKey))
                continue;

            var currentUuid = PlayerPrefs.GetString(uuidKey);
            _uuids.Add(new Guid(currentUuid));
        }

        return _uuids;
    }

    public void Reset()
    {
        PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
    }
    
    void SaveAnchorUuidToLocalStorage(OVRSpatialAnchor anchor, OVRSpatialAnchor.OperationResult result)
    {
        if (result != OVRSpatialAnchor.OperationResult.Success)
        {
            return;
        }
        
        Anchors.Add(anchor);
        anchor.transform.SetParent(anchorContainer.transform);

        if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
        {
            PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
        }

        int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
        PlayerPrefs.SetString("uuid" + playerNumUuids, anchor.Uuid.ToString());
        PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumUuids);
    }

    void RemoveAnchorFromLocalStorage(OVRSpatialAnchor anchor, OVRSpatialAnchor.OperationResult result)
    {
        var uuid = anchor.Uuid;
        if (result == OVRSpatialAnchor.OperationResult.Failure)
            return;
        
        Anchors.Clear();

        var playerUuidCount = PlayerPrefs.GetInt(NumUuidsPlayerPref, 0);
        for (int i = 0; i < playerUuidCount; i++)
        {
            var key = "uuid" + i;
            var value = PlayerPrefs.GetString(key, "");
            if (value.Equals(uuid.ToString()))
            {
                var lastKey = "uuid" + (playerUuidCount - 1);
                var lastValue = PlayerPrefs.GetString(lastKey);
                PlayerPrefs.SetString(key, lastValue);
                PlayerPrefs.DeleteKey(lastKey);

                playerUuidCount--;
                if (playerUuidCount < 0) playerUuidCount = 0;
                PlayerPrefs.SetInt(NumUuidsPlayerPref, playerUuidCount);
                break;
            }
        }
    }

    public void ResetAnchor()
    {
        _spatialAnchorCore.EraseAllAnchors();
    }

    void GetLoadAnchors(List<OVRSpatialAnchor> anchors)
    {
        Anchors = anchors;
        foreach (var anchor in anchors)
        {
            anchor.transform.SetParent(anchorContainer.transform);
        }
    }
    
    void Start()
    {
        _spatialAnchorCore.OnAnchorCreateCompleted.AddListener(SaveAnchorUuidToLocalStorage);
        _spatialAnchorCore.OnAnchorEraseCompleted.AddListener(RemoveAnchorFromLocalStorage);
        _spatialAnchorCore.OnAnchorsLoadCompleted.AddListener(GetLoadAnchors);
        LoadAnchorsFromDefaultLocalStorage();

        anchorPrefab = rightControllerAnchor.transform.GetChild(1).gameObject;
    }

    public Transform Root;
    private Vector3 _anchor1Pos, _anchor2Pos;

    void Update()
    {
        if (Anchors.Count == 0)
        {
            ShowInfo.text = "NO ANCHOR";
        }
        else if (Anchors.Count == 1)
        {
            ShowInfo.text = "ONLY ONE ANCHOR";
        } 
        else if (Anchors.Count == 2)
        {
            ShowInfo.text = "ANCHOR COMPLETE";
        }
        else if (Anchors.Count > 2)
        {
            ShowInfo.text = "ANCHOR OVERFLOW";
        }

        anchorPrefab.SetActive(Anchors.Count < 2);
        
        // 判断锚点是1还是2
        if (Anchors.Count == 2)
        {
            if (Anchors[0].Uuid.ToString() == PlayerPrefs.GetString("uuid0"))
            {
                _anchor1Pos = Anchors[0].gameObject.transform.position;
                _anchor2Pos = Anchors[1].gameObject.transform.position;
            }
            if (Anchors[1].Uuid.ToString() == PlayerPrefs.GetString("uuid0"))
            {
                _anchor1Pos = Anchors[1].gameObject.transform.position;
                _anchor2Pos = Anchors[0].gameObject.transform.position;
            }
        }

        Root.position = new Vector3(_anchor1Pos.x, 0, _anchor1Pos.z);
        Vector3 dir = new Vector3(_anchor2Pos.x, 0, _anchor2Pos.z) - new Vector3(_anchor1Pos.x, 0, _anchor1Pos.z);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        if (dir != Vector3.zero)
            rotation = Quaternion.LookRotation(dir);
        Quaternion adjustRotation = Quaternion.Euler(0, -90, 0);
        Root.rotation = rotation * adjustRotation;

        if (rightHand.GetIndexFingerIsPinching())
        {
            if (!isPinch)
            {
                isPinch = true;
                
                if (Anchors.Count < 2)
                    SpawnSpatialAnchor();
            }
        }
        else
            isPinch = false;
    }
}
