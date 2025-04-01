using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnTrigger : MonoBehaviour
{
    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        // 检查是否成功获取到组件
        if (objRenderer == null)
        {
            Debug.LogError("Renderer not found on " + gameObject.name);
            return; // 如果没有找到组件，停止执行
        }
        objRenderer.material = new Material(objRenderer.material); // 创建材质副本
    }

    void OnTriggerEnter(Collider other)
    {
        // 碰撞发生时触发
        //Debug.Log("Trigger detected with: " + other.gameObject.name);
        Color color = objRenderer.material.color;
        color.a = 0.7f; // 变得半透明
        objRenderer.material.color = color;
        //Debug.Log("Trigger detected, color changed!");
    }

    void OnTriggerExit(Collider other)
    {
        Color color = objRenderer.material.color;
        color.a = 0.3f; // 恢复完全不透明
        objRenderer.material.color = color;
        //Debug.Log("Trigger exited, color restored.");
    }
}