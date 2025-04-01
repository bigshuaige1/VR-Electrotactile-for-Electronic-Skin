using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NetSync
{
    [System.Serializable]
    public class SyncTransform
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;

        /// <summary>
        /// 将本地数据转为中转数据，转发到服务器
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SyncObjectToServer(Transform obj)
        {
            localPosition = obj.localPosition;
            localRotation = obj.localRotation;
            localScale = obj.localScale;

            string jsonData = JsonUtility.ToJson(this);
            return jsonData;
        }

        public void SyncObjectToLocal(Transform obj, string jsonData)
        {
            SyncTransform syncTransform = JsonUtility.FromJson<SyncTransform>(jsonData);
            localPosition = syncTransform.localPosition;
            localRotation = syncTransform.localRotation;
            localScale = syncTransform.localScale;

            obj.localPosition = localPosition;
            obj.localRotation = localRotation;
            obj.localScale = localScale;
        }
        
        public void SyncObjectToLocal(Transform obj)
        {
            obj.localPosition = localPosition;
            obj.localRotation = localRotation;
            obj.localScale = localScale;
        }
    }
}
