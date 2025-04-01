using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Scene2UDPBSender : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint endPoint;

    public string tagForSeriesA = "Current";  // 甲系列物体的Tag
    public string tagForSeriesB = "Pattern";  // 乙系列物体的Tag
    private byte patternName = 20; // 图案名称

    private bool shouldSend = false; // 用于标记是否需要发送数据

    void Start()
    {
        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
    }

    private void Update()
    {
        if (shouldSend)
        {
            Send();
            shouldSend = false; // 发送一次后重置标记
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagForSeriesB))
        {
            ChangeSprite script = other.GetComponent<ChangeSprite>();
            if (script == null) return;

            patternName = GetPatternName(script.test.name);
            shouldSend = true; // 设置标记，等待在下一帧发送
        }
        else if (other.CompareTag(tagForSeriesA))
        {
            SpriteRenderer sprite = other.GetComponent<SpriteRenderer>();
            if (sprite == null) return;

            patternName = GetPatternName(sprite.name);
            shouldSend = true; // 设置标记，等待在下一帧发送
        }
    }

    // 获取图案名称
    private byte GetPatternName(string buttonName)
    {
        switch (buttonName)
        {
            case "竖": return 1;
            case "减号": return 0;
            case "撇": return 2;
            case "捺": return 3;
            case "乘号": return 5;
            case "加号": return 4;
            case "正方形": return 6;
            case "长方形": return 7;
            case "笑脸": return 8;
            case "哭脸": return 9;
            case "1A": return 17;
            case "2A": return 18;
            case "4A": return 19;
            default:
                Debug.LogError("未知的按钮名称");
                return 15;
        }
    }

    private void Send()
    {
        byte[] data = new byte[] { patternName };
        udpClient.Send(data, data.Length, endPoint);

        Debug.Log("发送的patternName为: " + patternName);
    }

    void OnApplicationQuit()
    {
        udpClient?.Close();
    }
}
