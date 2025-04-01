using System.Collections;
using System.IO;
using UnityEngine;

public class TrackingHands : MonoBehaviour
{
    public Transform rightHand; // 右手的Transform引用
    public float delaySeconds;  // 每次更新的延迟时间

    private StreamWriter csvWriter; // 用于写入CSV文件的StreamWriter
    private bool isWriting = false;  // 标志位，用于控制是否正在写入

    void Start()
    {
        // 创建文件路径和目录
        string directoryPath = Application.dataPath + "/PositionData";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // 打开CSV文件进行写入
        string filePath = directoryPath + "/PositionData.csv";
        csvWriter = new StreamWriter(filePath, false); // 'false'表示如果文件存在则覆盖
        csvWriter.WriteLine("Time,x,y,z"); // 写入表头

        // 开始写入数据的协程
        StartCoroutine(WriteDataWithDelay());
    }

    IEnumerator WriteDataWithDelay()
    {
        while (true)
        {
            if (!isWriting && rightHand != null)
            {
                isWriting = true; // 标记开始写入

                // 获取当前时间和手的位置
                string timeStamp = Time.time.ToString("F2");
                string positionData = $"{rightHand.position.x:F3},{rightHand.position.y:F3},{rightHand.position.z:F3}";

                // 将时间和位置数据写入CSV文件
                csvWriter.WriteLine($"{timeStamp},{positionData}");

                isWriting = false; // 标记结束写入
            }

            // 每次写入后延迟指定的秒数
            yield return new WaitForSeconds(delaySeconds);
        }
    }

    void OnDestroy()
    {
        // 确保在对象销毁时正确关闭StreamWriter
        if (csvWriter != null)
        {
            csvWriter.Close();
        }
    }
}
