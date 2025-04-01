using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Scene2UDPBSender : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint endPoint;

    public string tagForSeriesA = "Current";  // ��ϵ�������Tag
    public string tagForSeriesB = "Pattern";  // ��ϵ�������Tag
    private byte patternName = 20; // ͼ������

    private bool shouldSend = false; // ���ڱ���Ƿ���Ҫ��������

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
            shouldSend = false; // ����һ�κ����ñ��
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagForSeriesB))
        {
            ChangeSprite script = other.GetComponent<ChangeSprite>();
            if (script == null) return;

            patternName = GetPatternName(script.test.name);
            shouldSend = true; // ���ñ�ǣ��ȴ�����һ֡����
        }
        else if (other.CompareTag(tagForSeriesA))
        {
            SpriteRenderer sprite = other.GetComponent<SpriteRenderer>();
            if (sprite == null) return;

            patternName = GetPatternName(sprite.name);
            shouldSend = true; // ���ñ�ǣ��ȴ�����һ֡����
        }
    }

    // ��ȡͼ������
    private byte GetPatternName(string buttonName)
    {
        switch (buttonName)
        {
            case "��": return 1;
            case "����": return 0;
            case "Ʋ": return 2;
            case "��": return 3;
            case "�˺�": return 5;
            case "�Ӻ�": return 4;
            case "������": return 6;
            case "������": return 7;
            case "Ц��": return 8;
            case "����": return 9;
            case "1A": return 17;
            case "2A": return 18;
            case "4A": return 19;
            default:
                Debug.LogError("δ֪�İ�ť����");
                return 15;
        }
    }

    private void Send()
    {
        byte[] data = new byte[] { patternName };
        udpClient.Send(data, data.Length, endPoint);

        Debug.Log("���͵�patternNameΪ: " + patternName);
    }

    void OnApplicationQuit()
    {
        udpClient?.Close();
    }
}
