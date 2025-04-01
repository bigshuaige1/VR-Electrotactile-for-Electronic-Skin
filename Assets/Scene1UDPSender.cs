using Mirror;
using Oculus.Interaction.HandGrab;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Scene1UDPBSender : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint endPoint;

    public HandGrabInteractor handGrab;
    public List<GameObject> mahjongTiles;   // �齫�����б�
    private int selectedIndex = -1; // ��ǰ�齫����
    private bool iftransport1 = true;
    private bool iftransport2 = true;
    private bool iftransport3 = true;
    private bool iftransport4 = true;

    void Start()
    {
        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
    }

    void Update()
    {
        UpdateSelectedIndex();

        //if (iftransport)
        //{
        //    // ������תΪ�ֽ�����
        //    byte[] data = Encoding.ASCII.GetBytes(selectedIndex.ToString());

        //    // ��������
        //    udpClient.Send(data, data.Length, endPoint);

        //    iftransport = false;

        //    Debug.Log("���͵�SelectedIndexΪ: " + selectedIndex);
        //}
    }
     
    private void Send1()
    {

            // ������תΪ�ֽ�����
            byte[] data = Encoding.ASCII.GetBytes(selectedIndex.ToString());
            byte[] buffer = Encoding.ASCII.GetBytes("-1".ToString());

            // ��������
            udpClient.Send(data, data.Length, endPoint);
            udpClient.Send(buffer, buffer.Length, endPoint);

            // ��������
            udpClient.Send(data, data.Length, endPoint);
            udpClient.Send(buffer, buffer.Length, endPoint);

            iftransport1 = false;

            Debug.Log("���͵�SelectedIndexΪ: " + selectedIndex);
    }

    private void Send2()
    {

        // ������תΪ�ֽ�����
        byte[] data = Encoding.ASCII.GetBytes(selectedIndex.ToString());
        byte[] buffer = Encoding.ASCII.GetBytes("-1".ToString());

        // ��������
        udpClient.Send(data, data.Length, endPoint);
        udpClient.Send(buffer, buffer.Length, endPoint);

        iftransport2 = false;

        Debug.Log("���͵�SelectedIndexΪ: " + selectedIndex);
    }

    private void Send3()
    {

        // ������תΪ�ֽ�����
        byte[] data = Encoding.ASCII.GetBytes(selectedIndex.ToString());
        byte[] buffer = Encoding.ASCII.GetBytes("-1".ToString());

        // ��������
        udpClient.Send(data, data.Length, endPoint);
        udpClient.Send(buffer, buffer.Length, endPoint);

        iftransport3 = false;

        Debug.Log("���͵�SelectedIndexΪ: " + selectedIndex);
    }
    private void Send4()
    {

        // ������תΪ�ֽ�����
        byte[] data = Encoding.ASCII.GetBytes(selectedIndex.ToString());
        byte[] buffer = Encoding.ASCII.GetBytes("-1".ToString());

        // ��������
        udpClient.Send(data, data.Length, endPoint);
        udpClient.Send(buffer, buffer.Length, endPoint);

        iftransport4 = false;

        Debug.Log("���͵�SelectedIndexΪ: " + selectedIndex);
    }

    void OnApplicationQuit()
    {
        udpClient?.Close();
    }

    void UpdateSelectedIndex()
    {
        //ץȡ�齫���
        if (handGrab.State == Oculus.Interaction.InteractorState.Select)
        {
            GameObject grabbedObject = handGrab.SelectedInteractable.gameObject;

            if(grabbedObject.name == "majiang 1")
            {
                selectedIndex = 0;
                if (iftransport1 == true)
                {
                    Send1();
                }
            }

            else if (grabbedObject.name == "majiang 2")
            {
                selectedIndex = 1;
                if (iftransport2 == true)
                {
                    Send2();
                }
            }

            else if (grabbedObject.name == "majiang 3")
            {
                selectedIndex = 2;
                if (iftransport3 == true)
                {
                    Send3();
                }
            }

            else if (grabbedObject.name == "majiang 4")
            {
                selectedIndex = 3;
                if (iftransport4 == true)
                {
                    Send4();
                }
            }
        }

        else
        {
            selectedIndex = -1;
        }
    }
}