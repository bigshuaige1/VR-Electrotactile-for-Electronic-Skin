using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnTrigger : MonoBehaviour
{
    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        // ����Ƿ�ɹ���ȡ�����
        if (objRenderer == null)
        {
            Debug.LogError("Renderer not found on " + gameObject.name);
            return; // ���û���ҵ������ִֹͣ��
        }
        objRenderer.material = new Material(objRenderer.material); // �������ʸ���
    }

    void OnTriggerEnter(Collider other)
    {
        // ��ײ����ʱ����
        //Debug.Log("Trigger detected with: " + other.gameObject.name);
        Color color = objRenderer.material.color;
        color.a = 0.7f; // ��ð�͸��
        objRenderer.material.color = color;
        //Debug.Log("Trigger detected, color changed!");
    }

    void OnTriggerExit(Collider other)
    {
        Color color = objRenderer.material.color;
        color.a = 0.3f; // �ָ���ȫ��͸��
        objRenderer.material.color = color;
        //Debug.Log("Trigger exited, color restored.");
    }
}