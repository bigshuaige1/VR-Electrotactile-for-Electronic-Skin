using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.XR.OpenXR.Features.Manus.MANUSOpenXRHapticsFeature;

namespace UnityEngine.XR.OpenXR.Features.Manus
{
    public class Haptics : MonoBehaviour
    {
        private MANUSOpenXRHapticsFeature m_MANUSOpenXRHapticsFeature;

        private void Start()
        {
            // 获取Manus Haptic Feature
            m_MANUSOpenXRHapticsFeature = OpenXRSettings.Instance.GetFeature<MANUSOpenXRHapticsFeature>();

            // 如果Feature未启用，提示用户
            if (m_MANUSOpenXRHapticsFeature == null || !m_MANUSOpenXRHapticsFeature.enabled)
            {
                Debug.LogError("Manus Haptic Extension feature group is not enabled. Please enable it through the XR-Plug-in Management OpenXR menu in the Project Settings.");
            }

            // 禁用初始振动
            DisableVibration();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collided!");

            // 触发振动
            TriggerVibration();
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log("Exited!");

            // 离开碰撞时禁用振动
            DisableVibration();
        }

        private void DisableVibration()
        {
            if (m_MANUSOpenXRHapticsFeature != null && m_MANUSOpenXRHapticsFeature.enabled)
            {
                // 设置振动强度为0
                m_MANUSOpenXRHapticsFeature.VibrateHand(0, XrHandEXT.XR_HAND_LEFT_EXT, new FingerAmplitudes(0, 0, 0, 0, 0));
            }
        }

        private void TriggerVibration()
        {
            if (m_MANUSOpenXRHapticsFeature != null && m_MANUSOpenXRHapticsFeature.enabled)
            {
                // 触发振动，持续时间100ms，强度为1
                m_MANUSOpenXRHapticsFeature.VibrateHand(10000000000, XrHandEXT.XR_HAND_LEFT_EXT, new FingerAmplitudes(1, 1, 1, 1, 1));
            }
        }
    }
}