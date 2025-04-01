using UnityEngine;

public class ImgTipsPosCtrl : MonoBehaviour
{
    private Transform trans;
    /// <summary>
    /// 高度
    /// </summary>
    private float posY = 0;
    /// <summary>
    /// 上升最高位置
    /// </summary>
    private double posYMax = 0;
    private double posYMin = 0;
    private bool isUp;

    // 添加一个变量来控制浮动速度
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
        // 根据 isUp 状态，调整 posY 的值
        if (isUp)
            posY += floatSpeed; // 改为 floatSpeed 控制速度
        else
            posY -= floatSpeed; // 改为 floatSpeed 控制速度

        // 检查是否达到边界，改变方向
        if (posY >= posYMax)
            isUp = false;
        else if (posY <= posYMin)
            isUp = true;

        // 更新对象的局部位置
        trans.localPosition = new Vector3(trans.localPosition.x, posY, trans.localPosition.z);
    }
}
