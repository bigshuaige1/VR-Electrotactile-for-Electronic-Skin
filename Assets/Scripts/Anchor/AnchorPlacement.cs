using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class AnchorPlacement : MonoBehaviour
{
    /// <summary>
    /// 锚点对象。子物体1为具体物体，子物体2为信息UI
    /// </summary>
    public List<GameObject> anchorPrefabs;
    private int _selectedIndex;

    private List<GameObject> _anchorPreviews;
    private GameObject _anchorPreviewContainer;
    
    /// <summary>
    /// 锚点预览与右控制器的偏置位置
    /// </summary>
    public Vector3 anchorPreviewOffset;

    private List<GameObject> _anchorContainers;
    private Dictionary<string, List<OVRSpatialAnchor>> _anchorDict;

    private List<string> _anchorNames;

    private OVRCameraRig _cameraRig;

    private Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;

    private void Awake()
    {
        _selectedIndex = 0;
        _cameraRig = FindObjectOfType<OVRCameraRig>();

        // 初始化锚点预览
        _anchorPreviews = new List<GameObject>();
        _anchorPreviewContainer = new GameObject();
        _anchorPreviewContainer.name = "Anchor Preview Container";
        _anchorPreviewContainer.transform.SetParent(_cameraRig.rightControllerAnchor);

        // 初始化锚点容器
        _anchorContainers = new List<GameObject>();

        // 初始化锚点记录器
        _anchorDict = new Dictionary<string, List<OVRSpatialAnchor>>();

        // 初始化所有锚点的命名
        _anchorNames = new List<string>();

        // 根据所给的锚点预制体不同种类初始化
        foreach (GameObject anchorPrefab in anchorPrefabs)
        {
            GameObject prefab = Instantiate(anchorPrefab);
            prefab.transform.GetChild(0).gameObject.SetActive(false);  // 将预览件的画布删除
            Destroy(prefab.GetComponent<BoxCollider>());

            // 初始化锚点预览
            _anchorPreviews.Add(prefab);
            prefab.transform.SetParent(_anchorPreviewContainer.transform);

            // 初始化锚点容器
            GameObject anchorContainer = new GameObject("[Container] " + anchorPrefab.name);
            _anchorContainers.Add(anchorContainer);

            // 初始化锚点记录器
            _anchorDict[anchorPrefab.name] = new List<OVRSpatialAnchor>();

            // 所有锚点的命名
            _anchorNames.Add(anchorPrefab.name);
        }

        ResetPreview();
    }

    private void Update()
    {
        _anchorPreviewContainer.transform.localPosition = anchorPreviewOffset;
    }

    /// <summary>
    /// 创建当前选中的预览对应的锚点
    /// </summary>
    public void CreateSpatialAnchor()
    {
        GameObject prefab = Instantiate(anchorPrefabs[_selectedIndex], _anchorPreviews[_selectedIndex].transform.position, _anchorPreviews[_selectedIndex].transform.rotation);

        OVRSpatialAnchor workingAnchor = prefab.AddComponent<OVRSpatialAnchor>();
        StartCoroutine(AnchorCreated(workingAnchor, anchorPrefabs[_selectedIndex].name));
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor, string gameObjectName)
    {
        while (!workingAnchor.Created && !workingAnchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

        _anchorDict[gameObjectName].Add(workingAnchor);
        Guid anchorGuid = workingAnchor.Uuid;
        
        workingAnchor.transform.SetParent(_anchorContainers[_selectedIndex].transform);
        workingAnchor.gameObject.AddComponent<Rigidbody>();

        Canvas anchorCanvas = workingAnchor.GetComponentInChildren<Canvas>();
        TextMeshProUGUI uuidText = anchorCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI statusText = anchorCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        uuidText.text = anchorGuid.ToString();
        statusText.text = "Not Saved";
    }

    /// <summary>
    /// 下一个预览
    /// </summary>
    public void IndexPlus()
    {
        SetSelectedIndex((_selectedIndex + 1) % anchorPrefabs.Count);
    }

    /// <summary>
    /// 上一个预览
    /// </summary>
    public void IndexMinus()
    {
        SetSelectedIndex((_selectedIndex - 1 + anchorPrefabs.Count) % anchorPrefabs.Count);
    }

    private void SetSelectedIndex(int index)
    {
        _selectedIndex = index;
        ResetPreview();
    }

    public void PreviewOffsetCloser()
    {
        anchorPreviewOffset.z = Mathf.Max(0, anchorPreviewOffset.z - 0.01f);
    }

    public void PreviewOffsetFarther()
    {
        anchorPreviewOffset.z = Mathf.Min(2.0f, anchorPreviewOffset.z + 0.01f);
    }

    /// <summary>
    /// 重置锚点预览。启动时以及 selectedIndex 改变时调用
    /// </summary>
    private void ResetPreview()
    {
        foreach (GameObject anchorPreview in _anchorPreviews)
        {
            anchorPreview.SetActive(false);
        }
        _anchorPreviews[_selectedIndex].SetActive(true);
    }

    /// <summary>
    /// 保存当前空间锚点状态。会首先清楚所有锚点记录
    /// </summary>
    public async void SaveCurrent()
    {
        ClearAllUuidsFromPlayerPrefs();

        foreach (string anchorName in _anchorNames)
        {
            string numKey = getPlayerPrefsNumKey(anchorName);
            PlayerPrefs.SetInt(numKey, _anchorDict[anchorName].Count);
            for (int i = 0; i < _anchorDict[anchorName].Count; i++)
            {
                OVRSpatialAnchor anchor = _anchorDict[anchorName][i];

                OVRResult<OVRAnchor.SaveResult> result = await anchor.SaveAnchorAsync();
                if (result.Success)
                {
                    string uuidKey = getPlayerPrefsKey(anchorName, i);
                    PlayerPrefs.SetString(uuidKey, anchor.Uuid.ToString());

                    var textComponents = anchor.GetComponentsInChildren<TextMeshProUGUI>();
                    if (textComponents.Length > 1)
                    {
                        var statusText = textComponents[1];
                        statusText.text = "Saved";
                    }
                }
            }
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 清除当前空间中所有锚点。不会清除锚点记录
    /// </summary>
    public void ClearCurrentAllAnchors()
    {
        foreach (string anchorName in _anchorNames)
        {
            foreach (OVRSpatialAnchor spatialAnchor in _anchorDict[anchorName])
            {
                Destroy(spatialAnchor.gameObject);
            }
            _anchorDict[anchorName].Clear();
        }
    }

    /// <summary>
    /// 保存上一个创建的锚点
    /// </summary>
    //public async void SaveLastCreatedAnchor()
    //{
    //    if (lastCreatedAnchor == null) 
    //        return;

    //    OVRResult<OVRAnchor.SaveResult> result = await lastCreatedAnchor.SaveAnchorAsync();
    //    if (result.Success)
    //    {
    //        TextMeshProUGUI statusText = lastCreatedAnchorCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    //        statusText.text = "Saved";
    //    }

    //    SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
    //}

    /// <summary>
    /// 根据 uuid 保存锚点
    /// </summary>
    /// <param name="uuid"></param>
    //void SaveUuidToPlayerPrefs(Guid uuid)
    //{
    //    string numKey = getPlayerPrefsNumKey(lastCreatedIndex);
    //    int numUuids = PlayerPrefs.HasKey(numKey) ? PlayerPrefs.GetInt(numKey) : 0;
    //    string uuidKey = getPlayerPrefsKey(lastCreatedIndex, numUuids);
    //    PlayerPrefs.SetString(uuidKey, uuid.ToString());
    //    PlayerPrefs.SetInt(numKey, ++numUuids);
    //    PlayerPrefs.Save();
    //}

    /// <summary>
    /// 撤销保存上一个保存的锚点
    /// </summary>
    //public async void UnsaveLastCreatedAnchor()
    //{
    //    if (lastCreatedAnchor == null)
    //        return;

    //    OVRResult<OVRAnchor.EraseResult> result = await lastCreatedAnchor.EraseAnchorAsync();
    //    if (result.Success)
    //    {
    //        TextMeshProUGUI statusText = lastCreatedAnchorCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    //        statusText.text = "Not Saved";
    //    }
    //}

    /// <summary>
    /// 撤销保存所有锚点
    /// </summary>
    //public void UnsaveAllAnchors()
    //{
    //    foreach (string anchorName in _anchorNames)
    //    {
    //        foreach (OVRSpatialAnchor item in _anchorDict[anchorName])
    //        {
    //            UnsaveAnchor(item);
    //        }
    //        _anchorDict[anchorName].Clear();
    //    }
    //    // clear all uuids
    //    ClearAllUuidsFromPlayerPrefs();
    //}

    /// <summary>
    /// 根据锚点撤销保存锚点
    /// </summary>
    /// <param name="anchor">锚点对象</param>
    //private async void UnsaveAnchor(OVRSpatialAnchor anchor)
    //{
    //    OVRResult<OVRAnchor.EraseResult> result = await anchor.EraseAnchorAsync();
    //    if (result.Success)
    //    {
    //        var textComponents = anchor.GetComponentsInChildren<TextMeshProUGUI>();
    //        if (textComponents.Length > 1)
    //        {
    //            var statusText = textComponents[1];
    //            statusText.text = "Not Saved";
    //        }
    //    }
    //}

    /// <summary>
    /// 清楚所有锚点 uuid 记录
    /// </summary>
    private void ClearAllUuidsFromPlayerPrefs()
    {
        foreach (string anchorName in _anchorNames)
        {
            string numKey = getPlayerPrefsNumKey(anchorName);
            if (PlayerPrefs.HasKey(numKey))
            {
                int numUuids = PlayerPrefs.GetInt(numKey);
                for (int i = 0; i < numUuids; i++)
                {
                    string uuidKey = getPlayerPrefsKey(anchorName, i);
                    PlayerPrefs.DeleteKey(uuidKey);
                }
                PlayerPrefs.DeleteKey(numKey);
                PlayerPrefs.Save();
            }
        }
        //PlayerPrefs.Save();
    }

    string getPlayerPrefsNumKey(int index)
    {
        return "numUuids_[" + anchorPrefabs[index].name + "]";
    }

    string getPlayerPrefsNumKey(string anchorName)
    {
        return "numUuids_[" + anchorName + "]";
    }

    string getPlayerPrefsKey(int index, int num)
    {
        return "uuid_[" + anchorPrefabs[index].name + "]_" + num;
    }

    string getPlayerPrefsKey(string anchorName, int num)
    {
        return "uuid_[" + anchorName + "]_" + num;
    }

    /// <summary>
    /// 重置锚点记录器。仅当主动加载锚点时调用
    /// </summary>
    /// <returns></returns>
    //private void ResetAnchorDict()
    //{
    //    foreach (string anchorName in _anchorNames)
    //    {
    //        foreach (OVRSpatialAnchor spatialAnchor in _anchorDict[anchorName])
    //        {
    //            Destroy(spatialAnchor);
    //        }
    //        _anchorDict[anchorName].Clear();
    //    }
    //}

    /// <summary>
    /// 加载存储在本地的锚点前做的一些操作
    /// </summary>
    void BeforeLoadAnchor()
    {
        // 重置锚点记录器
        //ResetAnchorDict();
        ClearCurrentAllAnchors();

        // 清空锚点容器
    }


    /* *********************************************** LOAD ANCHOR *********************************************** */

    public async void LoadAnchorsByUuid()
    {
        BeforeLoadAnchor();

        for (int i = 0; i < _anchorNames.Count; i++)
        {
            string anchorName = _anchorNames[i];
            GameObject prefab = anchorPrefabs[i];

            string numKey = getPlayerPrefsNumKey(anchorName);
            int numUuids = PlayerPrefs.HasKey(numKey) ? PlayerPrefs.GetInt(numKey) : 0;

            if (numUuids == 0)
                continue;

            List<Guid> uuids = new List<Guid>();
            for (int j = 0; j < numUuids; j++)
            {
                string uuidKey = getPlayerPrefsKey(anchorName, j);
                Guid currentUuid = new Guid(PlayerPrefs.GetString(uuidKey));
                uuids.Add(currentUuid);
            }

            List<OVRSpatialAnchor.UnboundAnchor> unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
            var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(uuids, unboundAnchors);

            if (result.Success)
            {
                foreach (var unboundAnchor in result.Value)
                {
                    bool localizeSuccess = await unboundAnchor.LocalizeAsync();
                    if (localizeSuccess)
                    {
                        Pose pose = unboundAnchor.Pose;
                        GameObject spatialAnchorGameObject = Instantiate(prefab, pose.position, pose.rotation);
                        spatialAnchorGameObject.transform.SetParent(_anchorContainers[i].transform);
                        OVRSpatialAnchor spatialAnchor = spatialAnchorGameObject.AddComponent<OVRSpatialAnchor>();
                        unboundAnchor.BindTo(spatialAnchor);

                        _anchorDict[anchorName].Add(spatialAnchor);

                        var textComponents = spatialAnchor.GetComponentsInChildren<TextMeshProUGUI>();
                        textComponents[0].text = spatialAnchor.Uuid.ToString();
                        textComponents[1].text = "Loaded";
                    }
                }
            }
        }
    }
}
