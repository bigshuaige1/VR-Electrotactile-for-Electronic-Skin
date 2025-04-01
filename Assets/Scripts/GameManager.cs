using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public bool gameStart = false;
    public bool userGameStart = false;

    //public GameObject playerPanel;
    public GameObject assistantPanel;

    public GameObject visual;

    public List<GameObject> places;
    public bool needPlace;
    
    public OVRCameraRig ovrCameraRig;
    private Camera _centerCamera;

    private OVRPassthroughLayer _passthroughLayer;
    
    public GameObject anchorContainer;

    private void Awake()
    {
        Instance = this;
        gameObject.AddComponent<Utils>();
    }

    private void Start()
    {
        gameStart = false;
        userGameStart = false;
        _centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();
        _passthroughLayer = FindObjectOfType<OVRPassthroughLayer>();
        
        if (needPlace)
        {
            for (var i = 0; i < visual.transform.childCount; i++)
            {
                visual.transform.GetChild(i).gameObject.SetActive(false);
            }
            foreach (var obj in places)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            GameStart();
        }

        // FOR TEST
        // StartCoroutine(UserGameStartWithDelay(5));
    }

    public delegate void GameStartEventHandler();
    public event GameStartEventHandler OnGameStart;
    public void GameStart()
    {
        if (gameStart) 
            return;
        
        gameStart = true;

        for (var i = 0; i < visual.transform.childCount; i++)
        {
            visual.transform.GetChild(i).gameObject.SetActive(true);
        }

        _centerCamera.clearFlags = CameraClearFlags.Skybox;

        SetAnchorVisual(false);
        SetPassthroughLayerVisual(false);

        if (OnGameStart != null)
        {
            OnGameStart();
        }
    }
    
    public void TogglePassthroughLayer()
    {
        SetPassthroughLayerVisual(_passthroughLayer.textureOpacity < 1.0f);
    }

    private void SetPassthroughLayerVisual(bool visual)
    {
        _passthroughLayer.textureOpacity = visual ? 1.0f : 0.0f;
    }

    public void ToggleAnchorVisual()
    {
        SetAnchorVisual(!anchorContainer.activeSelf);
    }

    private void SetAnchorVisual(bool visual)
    {
        anchorContainer.SetActive(visual);
    }

    private int _aPanelTriggerCount = 0;
    private Coroutine _aPanelTriggerCoroutine = null;
    public void CountAPanelTrigger()
    {
        _aPanelTriggerCount += 1;

        if (_aPanelTriggerCoroutine != null)
        {
            StopCoroutine(_aPanelTriggerCoroutine);
            _aPanelTriggerCoroutine = null;
        }
        _aPanelTriggerCoroutine = StartCoroutine(APanelTriggerCoroutine(0.2f));
        
        if (_aPanelTriggerCount >= 3)
        {
            assistantPanel.SetActive(!assistantPanel.activeSelf);
        }
    }
    
    private IEnumerator APanelTriggerCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        _aPanelTriggerCount = 0;
    }

    public delegate void UserGameStartEventHandler();
    public event UserGameStartEventHandler OnUserGameStart;
    public void UserGameStart()
    {
        if (userGameStart)
            return;

        userGameStart = true;

        if (OnUserGameStart != null)
        {
            OnUserGameStart();
        }
    }


    /// <summary>
    /// 测试用
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator UserGameStartWithDelay(float delay = 5)
    {
        yield return new WaitForSeconds(delay);

        UserGameStart();
    }
}
