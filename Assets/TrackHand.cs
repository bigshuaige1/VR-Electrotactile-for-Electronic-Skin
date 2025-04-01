using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class TrackHand : MonoBehaviour
{
    // 手指关节
    public GameObject[] thumbJoints; // thumb1, thumb2, thumb3
    public GameObject[] indexJoints; // index1, index2, index3
    public GameObject[] middleJoints; // middle1, middle2, middle3
    public GameObject[] ringJoints; // ring1, ring2, ring3
    public GameObject[] pinkyJoints; // pinky1, pinky2, pinky3

    // 可交互的物体
    public GameObject[] Objects;

    // 文件路径
    private string noCollisionFilePath; // 无碰撞时的文件路径
    private string[] collisionFilePaths; // 碰撞时的文件路径
    private bool isNoCollisionHeaderWritten = false; // 无碰撞文件的表头是否已写入
    private bool[] isCollisionHeaderWritten; // 碰撞文件的表头是否已写入

    // 数据缓冲区和写入间隔
    private List<string> noCollisionDataBuffer = new List<string>(); // 无碰撞数据缓冲区
    private List<string>[] collisionDataBuffers; // 碰撞数据缓冲区
    private float lastWriteTime = 0f; // 上次写入时间
    private float writeInterval = 1f; // 写入间隔（秒）

    // 缓存Transform组件
    private Transform[] thumbTransforms;
    private Transform[] indexTransforms;
    private Transform[] middleTransforms;
    private Transform[] ringTransforms;
    private Transform[] pinkyTransforms;

    // 碰撞状态
    private bool isColliding = false;
    private int currentCollidingObjectIndex = -1;

    void Start()
    {
        // 初始化文件路径
        noCollisionFilePath = Application.dataPath + "/NoCollisionHandData.csv";
        collisionFilePaths = new string[Objects.Length];
        isCollisionHeaderWritten = new bool[Objects.Length];
        collisionDataBuffers = new List<string>[Objects.Length];

        for (int i = 0; i < Objects.Length; i++)
        {
            collisionFilePaths[i] = Application.dataPath + $"/CollisionHandData_Object{i + 1}.csv";
            isCollisionHeaderWritten[i] = false;
            collisionDataBuffers[i] = new List<string>();
        }

        // 如果文件已存在，先删除旧文件
        if (File.Exists(noCollisionFilePath))
        {
            File.Delete(noCollisionFilePath);
        }
        for (int i = 0; i < Objects.Length; i++)
        {
            if (File.Exists(collisionFilePaths[i]))
            {
                File.Delete(collisionFilePaths[i]);
            }
        }

        // 缓存Transform组件
        thumbTransforms = CacheTransforms(thumbJoints);
        indexTransforms = CacheTransforms(indexJoints);
        middleTransforms = CacheTransforms(middleJoints);
        ringTransforms = CacheTransforms(ringJoints);
        pinkyTransforms = CacheTransforms(pinkyJoints);
    }

    void Update()
    {
        // 每隔一段时间写入数据
        if (Time.time - lastWriteTime >= writeInterval)
        {
            WriteBuffersToFile();
            lastWriteTime = Time.time;
        }

        // 记录数据
        if (isColliding)
        {
            // 记录碰撞时的数据
            string formattedData = GetFormattedHandData();
            Vector3 objectPosition = Objects[currentCollidingObjectIndex].transform.position;
            Vector3 objectRotation = Objects[currentCollidingObjectIndex].transform.eulerAngles;

            string extraData = string.Format(",Object_X: {0:F4},Object_Y: {1:F4},Object_Z: {2:F4},Object_RX: {3:F4},Object_RY: {4:F4},Object_RZ: {5:F4}",
                objectPosition.x, objectPosition.y, objectPosition.z,
                objectRotation.x, objectRotation.y, objectRotation.z);

            collisionDataBuffers[currentCollidingObjectIndex].Add(formattedData + extraData);
        }
        else
        {
            // 记录无碰撞时的数据
            noCollisionDataBuffer.Add(GetFormattedHandData());
        }
    }

    public void HandleCollisionEnter(GameObject collidedObject, string fingerName)
    {
        int objectIndex = GetObjectIndex(collidedObject);
        if (objectIndex != -1)
        {
            isColliding = true;
            currentCollidingObjectIndex = objectIndex;
            Debug.Log($"{fingerName} collided with {collidedObject.name}");
        }
    }

    public void HandleCollisionExit(GameObject collidedObject, string fingerName)
    {
        int objectIndex = GetObjectIndex(collidedObject);
        if (objectIndex != -1)
        {
            isColliding = false;
            currentCollidingObjectIndex = -1;
            Debug.Log($"{fingerName} exited collision with {collidedObject.name}");
        }
    }

    int GetObjectIndex(GameObject obj)
    {
        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] == obj)
            {
                return i;
            }
        }
        return -1;
    }

    string GetFormattedHandData()
    {
        StringBuilder dataBuilder = new StringBuilder();
        dataBuilder.Append(Time.time.ToString("F4"));

        AppendJointData(thumbTransforms, "Thumb", dataBuilder);
        AppendJointData(indexTransforms, "Index", dataBuilder);
        AppendJointData(middleTransforms, "Middle", dataBuilder);
        AppendJointData(ringTransforms, "Ring", dataBuilder);
        AppendJointData(pinkyTransforms, "Pinky", dataBuilder);

        return dataBuilder.ToString();
    }

    void AppendJointData(Transform[] joints, string fingerName, StringBuilder dataBuilder)
    {
        for (int i = 0; i < joints.Length; i++)
        {
            Vector3 position = joints[i].position;
            Vector3 angles = joints[i].eulerAngles;

            dataBuilder.AppendFormat(",{0}{1}_X: {2:F4},{0}{1}_Y: {3:F4},{0}{1}_Z: {4:F4},{0}{1}_RX: {5:F4},{0}{1}_RY: {6:F4},{0}{1}_RZ: {7:F4}",
                fingerName, i + 1, position.x, position.y, position.z, angles.x, angles.y, angles.z);
        }
    }

    void WriteBuffersToFile()
    {
        // 写入无碰撞数据
        if (noCollisionDataBuffer.Count > 0)
        {
            WriteToCSV(noCollisionFilePath, noCollisionDataBuffer, ref isNoCollisionHeaderWritten);
            noCollisionDataBuffer.Clear();
        }

        // 写入碰撞数据
        for (int i = 0; i < collisionDataBuffers.Length; i++)
        {
            if (collisionDataBuffers[i].Count > 0)
            {
                WriteToCSV(collisionFilePaths[i], collisionDataBuffers[i], ref isCollisionHeaderWritten[i]);
                collisionDataBuffers[i].Clear();
            }
        }
    }

    void WriteToCSV(string filePath, List<string> dataBuffer, ref bool isHeaderWritten)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            if (!isHeaderWritten)
            {
                writer.WriteLine(GetCSVHeader());
                isHeaderWritten = true;
            }
            foreach (string data in dataBuffer)
            {
                writer.WriteLine(data);
            }
        }
    }

    string GetCSVHeader()
    {
        StringBuilder headerBuilder = new StringBuilder();
        headerBuilder.Append("Time");

        AppendJointHeader(thumbTransforms.Length, "Thumb", headerBuilder);
        AppendJointHeader(indexTransforms.Length, "Index", headerBuilder);
        AppendJointHeader(middleTransforms.Length, "Middle", headerBuilder);
        AppendJointHeader(ringTransforms.Length, "Ring", headerBuilder);
        AppendJointHeader(pinkyTransforms.Length, "Pinky", headerBuilder);

        headerBuilder.Append(",Object_X,Object_Y,Object_Z,Object_RX,Object_RY,Object_RZ");

        return headerBuilder.ToString();
    }

    void AppendJointHeader(int jointCount, string fingerName, StringBuilder headerBuilder)
    {
        for (int i = 1; i <= jointCount; i++)
        {
            headerBuilder.AppendFormat(",{0}{1}_X,{0}{1}_Y,{0}{1}_Z,{0}{1}_RX,{0}{1}_RY,{0}{1}_RZ", fingerName, i);
        }
    }

    Transform[] CacheTransforms(GameObject[] joints)
    {
        Transform[] transforms = new Transform[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            transforms[i] = joints[i].transform;
        }
        return transforms;
    }
}