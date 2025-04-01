using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class VirtualWorldManager : MonoBehaviour
{
    public GameObject envRoot;
    public float groundDelta = -0.02f;

    public MRUK mruk;
    
    // Start is called before the first frame update
    void Awake()
    {
        envRoot.SetActive(false);
    }

    public void InitEnv()
    {
        List<MRUKAnchor> anchors = mruk.GetCurrentRoom().Anchors;
        MRUKAnchor floor = anchors.Find(anchor => anchor.Label == MRUKAnchor.SceneLabels.FLOOR);
        MRUKAnchor window = anchors.Find(anchor => anchor.Label == MRUKAnchor.SceneLabels.WINDOW_FRAME);
        Debug.Log("floor: " + floor.transform.position);
        Debug.Log("window: " + window.transform.position);
    }
}
