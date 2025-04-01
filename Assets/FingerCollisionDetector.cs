using UnityEngine;

public class FingerCollisionDetector : MonoBehaviour
{
    public TrackHand handTracker; // 引用手部根对象上的 TrackHand 脚本
    public string fingerName; // 手指名称（例如 "Thumb"、"Index"）

    private void OnCollisionEnter(Collision collision)
    {
        // 将碰撞事件传递给手部根对象
        handTracker.HandleCollisionEnter(collision.gameObject, fingerName);
    }

    private void OnCollisionExit(Collision collision)
    {
        // 将碰撞事件传递给手部根对象
        handTracker.HandleCollisionExit(collision.gameObject, fingerName);
    }
}