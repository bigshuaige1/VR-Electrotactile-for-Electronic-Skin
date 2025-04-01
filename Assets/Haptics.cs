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
            // ��ȡManus Haptic Feature
            m_MANUSOpenXRHapticsFeature = OpenXRSettings.Instance.GetFeature<MANUSOpenXRHapticsFeature>();

            // ���Featureδ���ã���ʾ�û�
            if (m_MANUSOpenXRHapticsFeature == null || !m_MANUSOpenXRHapticsFeature.enabled)
            {
                Debug.LogError("Manus Haptic Extension feature group is not enabled. Please enable it through the XR-Plug-in Management OpenXR menu in the Project Settings.");
            }

            // ���ó�ʼ��
            DisableVibration();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collided!");

            // ������
            TriggerVibration();
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log("Exited!");

            // �뿪��ײʱ������
            DisableVibration();
        }

        private void DisableVibration()
        {
            if (m_MANUSOpenXRHapticsFeature != null && m_MANUSOpenXRHapticsFeature.enabled)
            {
                // ������ǿ��Ϊ0
                m_MANUSOpenXRHapticsFeature.VibrateHand(0, XrHandEXT.XR_HAND_LEFT_EXT, new FingerAmplitudes(0, 0, 0, 0, 0));
            }
        }

        private void TriggerVibration()
        {
            if (m_MANUSOpenXRHapticsFeature != null && m_MANUSOpenXRHapticsFeature.enabled)
            {
                // �����񶯣�����ʱ��100ms��ǿ��Ϊ1
                m_MANUSOpenXRHapticsFeature.VibrateHand(10000000000, XrHandEXT.XR_HAND_LEFT_EXT, new FingerAmplitudes(1, 1, 1, 1, 1));
            }
        }
    }
}