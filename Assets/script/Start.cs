using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpriteGroupVisibilityController : MonoBehaviour
{
    public GameObject[] targetObjects;  // ��Ҫ���Ƶ���������
    public TextMeshPro currentText;         // ������ʾ�ֵ� TMP ���
    public TextMeshPro preliminaryText;         // ������ʾ�ֵ� TMP ���

    void Start()
    {
        //// �����������壬��ʾ����
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
        // �����������壬��ʾ����
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
    //    // �����������壬���ؾ���
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
