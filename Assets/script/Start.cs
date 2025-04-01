using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpriteGroupVisibilityController : MonoBehaviour
{
    public GameObject[] targetObjects;  // 需要控制的物体数组
    public TextMeshPro currentText;         // 用于显示字的 TMP 组件
    public TextMeshPro preliminaryText;         // 用于显示字的 TMP 组件

    void Start()
    {
        //// 遍历所有物体，显示精灵
        //foreach (GameObject ob in targetObjects)
        //{
        //    if (ob != null)
        //    {
        //        ob.SetActive(false);
        //        Debug.Log("Hiding: " + ob.name);
        //    }
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
        // 遍历所有物体，显示精灵
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                ob.SetActive(true);
            }
        }
        Debug.Log("Sprites are now visible.");
        currentText.gameObject.SetActive(false);
        preliminaryText.gameObject.SetActive(true);
    }

    //void OnTriggerExit(Collider other)
    //{
    //    // 遍历所有物体，隐藏精灵
    //    foreach (SpriteRenderer sr in spriteRenderers)
    //    {
    //        if (sr != null)
    //        {
    //            sr.enabled = false;
    //        }
    //    }

    //    Debug.Log("Sprites are now hidden.");
    //}
}
