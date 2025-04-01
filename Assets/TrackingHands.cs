using System.Collections;
using System.IO;
using UnityEngine;

public class TrackingHands : MonoBehaviour
{
    public Transform rightHand; // ���ֵ�Transform����
    public float delaySeconds;  // ÿ�θ��µ��ӳ�ʱ��

    private StreamWriter csvWriter; // ����д��CSV�ļ���StreamWriter
    private bool isWriting = false;  // ��־λ�����ڿ����Ƿ�����д��

    void Start()
    {
        // �����ļ�·����Ŀ¼
        string directoryPath = Application.dataPath + "/PositionData";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // ��CSV�ļ�����д��
        string filePath = directoryPath + "/PositionData.csv";
        csvWriter = new StreamWriter(filePath, false); // 'false'��ʾ����ļ������򸲸�
        csvWriter.WriteLine("Time,x,y,z"); // д���ͷ

        // ��ʼд�����ݵ�Э��
        StartCoroutine(WriteDataWithDelay());
    }

    IEnumerator WriteDataWithDelay()
    {
        while (true)
        {
            if (!isWriting && rightHand != null)
            {
                isWriting = true; // ��ǿ�ʼд��

                // ��ȡ��ǰʱ����ֵ�λ��
                string timeStamp = Time.time.ToString("F2");
                string positionData = $"{rightHand.position.x:F3},{rightHand.position.y:F3},{rightHand.position.z:F3}";

                // ��ʱ���λ������д��CSV�ļ�
                csvWriter.WriteLine($"{timeStamp},{positionData}");

                isWriting = false; // ��ǽ���д��
            }

            // ÿ��д����ӳ�ָ��������
            yield return new WaitForSeconds(delaySeconds);
        }
    }

    void OnDestroy()
    {
        // ȷ���ڶ�������ʱ��ȷ�ر�StreamWriter
        if (csvWriter != null)
        {
            csvWriter.Close();
        }
    }
}
