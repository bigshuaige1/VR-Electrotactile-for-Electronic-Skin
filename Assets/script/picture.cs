using UnityEngine;

public class ImgTipsPosCtrl : MonoBehaviour
{
    private Transform trans;
    /// <summary>
    /// �߶�
    /// </summary>
    private float posY = 0;
    /// <summary>
    /// �������λ��
    /// </summary>
    private double posYMax = 0;
    private double posYMin = 0;
    private bool isUp;

    // ���һ�����������Ƹ����ٶ�
    public float floatSpeed = 0.0001f;

    private void Awake()
    {
        trans = GetComponent<Transform>();
        posY = trans.localPosition.y;
        posYMax = posY + 0.01;
        posYMin = posY - 0.01;
    }

    void Update()
    {
        // ���� isUp ״̬������ posY ��ֵ
        if (isUp)
            posY += floatSpeed; // ��Ϊ floatSpeed �����ٶ�
        else
            posY -= floatSpeed; // ��Ϊ floatSpeed �����ٶ�

        // ����Ƿ�ﵽ�߽磬�ı䷽��
        if (posY >= posYMax)
            isUp = false;
        else if (posY <= posYMin)
            isUp = true;

        // ���¶���ľֲ�λ��
        trans.localPosition = new Vector3(trans.localPosition.x, posY, trans.localPosition.z);
    }
}
