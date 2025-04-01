using UnityEngine;

public class FingerCollisionDetector : MonoBehaviour
{
    public TrackHand handTracker; // �����ֲ��������ϵ� TrackHand �ű�
    public string fingerName; // ��ָ���ƣ����� "Thumb"��"Index"��

    private void OnCollisionEnter(Collision collision)
    {
        // ����ײ�¼����ݸ��ֲ�������
        handTracker.HandleCollisionEnter(collision.gameObject, fingerName);
    }

    private void OnCollisionExit(Collision collision)
    {
        // ����ײ�¼����ݸ��ֲ�������
        handTracker.HandleCollisionExit(collision.gameObject, fingerName);
    }
}