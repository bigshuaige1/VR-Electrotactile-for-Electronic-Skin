using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.UI.Image;

public class Judge : MonoBehaviour
{
    public GameObject scoreSheet;
    private Sprite lastCollisionSprite = null;  // 记录上一个碰撞物体的Sprite
    private Sprite spriteA = null;  // 记录甲系列物体的Sprite
    private Sprite spriteB = null;  // 记录乙系列物体的Sprite

    // 甲系列和乙系列的标签，确保物体设置了正确的标签
    public string tagForSeriesA = "Judge";  // 甲系列物体的Tag
    public string tagForSeriesB = "Pattern";  // 乙系列物体的Tag

    public TextMeshPro scoreText;         // 用于显示分数的 TMP 组件
    private double score = 0;  // 分数

    private int triggerCount = 0; // 记录触发次数
    public GameObject[] targetObjects;  // 需要控制的物体数组
    public Sprite[] pic;//图片

    private int grayCount = 0;

    private float timeElapsedBetweenCollisions = 0.0f; // 用于累加的总时间
    private float lastCollisionTime = 0.0f; // 用于记录上一次碰撞乙系列物体的时间
    private float group1Reactiontime = 0.0f;
    private float group2Reactiontime = 0.0f;
    private float group3Reactiontime = 0.0f;
    private float group4Reactiontime = 0.0f;
    private float group5Reactiontime = 0.0f;

    private int Acc = 0;
    private float group1Acc = 0;//记录准确度
    private float group2Acc = 0;
    private float group3Acc = 0;
    private float group4Acc = 0;
    private float group5Acc = 0;

    public TextMeshPro[] Text;//评分单里面的text


    void Start()
    {
        this.enabled = false; // 禁用当前脚本
    }

    void OnTriggerEnter(Collider other)
    {
        // 获取当前碰撞物体的 SpriteRenderer 组件
        ChangeSprite Script = other.GetComponent<ChangeSprite>();

        if (Script != null)
        {
            Debug.Log(other.gameObject.name);
            Sprite currentSprite = Script.test;

            // 如果碰撞物体属于甲系列（通过Tag判断）
            if (other.CompareTag(tagForSeriesA))
            {
                spriteA = currentSprite;  // 记录甲系列物体的 Sprite
                Debug.Log("Sprite A Name: " + spriteA.name);

                float currentTime = Time.time;
                float timeDifference = currentTime - lastCollisionTime;
                timeElapsedBetweenCollisions += timeDifference; // 累加时间
                Debug.Log("Time difference between collisions: " + timeDifference);
                lastCollisionTime = 0; // 重置为 0，等待下一次乙系列物体的碰撞

                // 当甲系列物体的 Sprite 存在且乙系列物体的 Sprite 也已记录时，进行比较
                if (spriteA != null && spriteB != null)
                {
                    if (spriteA.name == "竖" || spriteA.name == "减号")
                    {
                        AddScore(1.5);//判断正确加一分
                    }
                    else if (spriteA.name == "撇" || spriteA.name == "捺")
                    {
                        AddScore(1);//判断正确加一分
                    }
                    else if (spriteA.name == "加号" || spriteA.name == "乘号")
                    {
                        AddScore(2.75);//判断正确加一分
                    }
                    else if (spriteA.name == "正方形" || spriteA.name == "长方形")
                    {
                        AddScore(3.06);//判断正确加一分
                    }
                    else if (spriteA.name == "笑脸" || spriteA.name == "哭脸")
                    {
                        AddScore(4.28);//判断正确加一分
                    }
                    Acc++;//判断正确准确度加一

                    // 增加触发次数
                    triggerCount++;
                    CheckTriggerCount(); // 检查触发次数                            
                }
            }
            // 如果碰撞物体属于乙系列
            else if (other.CompareTag(tagForSeriesB))
            {
                string patternName = GetPatternName(Script.test.name);
                spriteB = currentSprite;  // 记录乙系列物体的 Sprite

                // 记录碰撞乙系列物体的时间
                lastCollisionTime = Time.time;

                //如果是测试阶段，则将物体变灰
                if (Script.original.name == "问号")
                {
                    SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.gray; // 设置为灰色

                        grayCount++;
                    }
                }
            }
        }

        else
        {
        }   
    }

    private string GetPatternName(string buttonName)
    {
        switch (buttonName)
        {
            case "竖": return "2";
            case "减号": return "1";
            case "撇": return "3";
            case "捺": return "4";
            case "乘号": return "6";
            case "加号": return "5";
            case "正方形": return "7";
            case "长方形": return "8";
            case "笑脸": return "9";
            case "哭脸": return "10";
            case "4A": return "2";
            case "1A": return "2";
            case "2A": return "2";
            default:
                Debug.LogError("未知的按钮名称");
                return null;
        }
    }

    private void CheckTriggerCount()
    {
        if (triggerCount == 20)
        {
            HandleThirdCollision(); // 达到20次触发函数
        }
        else if (triggerCount == 10)
        {
            HandleSecondCollision(); // 达到10次触发函数
        }
        else if (triggerCount == 30)
        {
            HandleForthCollision(); // 达到30次触发函数
        }
        else if (triggerCount == 40)
        {
            HandleFifthCollision(); // 达到40次触发函数
        }
        else if(grayCount == 10 || triggerCount == 50)
        {
            HandleResult();//50,计算结果
        }
    }

    void HandleSecondCollision()
    {
        // 在这里执行第二次碰撞时的特殊逻辑
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // 获取所有子物体中附加了MyChildScript脚本的组件
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // 遍历所有找到的脚本组件并修改public变量
                foreach (ChangeSprite script in childScripts)
                {
                    if (i == 0 || i == 2 || i == 3 || i == 5 || i == 8)
                    {
                        script.test = pic[3];//捺
                    }
                    else if (i == 1 || i == 4 || i == 6 || i == 7 || i == 9)
                    {
                        script.test = pic[2];//撇
                    }
                    else if (i == 14)
                    {
                        script.original = pic[3]; // 将Judge1图案设置为捺
                        script.test = pic[3];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                    else if (i == 15)
                    {
                        script.original = pic[2]; // 将Judge2图案设置为撇
                        script.test = pic[2];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                }
                SpriteRenderer spriteRenderer = ob.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }
            i++;
        }

        group1Reactiontime = timeElapsedBetweenCollisions / 10;//记录时间
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group1 average time:" + group1Reactiontime);
        group1Acc = Acc;
        Debug.Log("group1 acc:" + group1Acc);
        Acc = 0;

    }


    void HandleThirdCollision()
    {
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // 获取所有子物体中附加了MyChildScript脚本的组件
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // 遍历所有找到的脚本组件并修改public变量
                foreach (ChangeSprite script in childScripts)
                {

                    if (i == 0 || i == 1 || i == 4 || i == 6 || i == 7)
                    {
                        script.test = pic[4];//乘号
                    }
                    else if (i == 2 || i == 3 || i == 5 || i == 8 || i == 9)
                    {
                        script.test = pic[5];//加号
                    }
                    else if (i == 14)
                    {
                        script.original = pic[5]; // 将Judge1图案设置为加号
                        script.test = pic[5];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                    else if (i == 15)
                    {
                        script.original = pic[4]; // 将Judge2图案设置为乘号
                        script.test = pic[4];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                }
                SpriteRenderer spriteRenderer = ob.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }
            i++;
        }

        group2Reactiontime = timeElapsedBetweenCollisions / 10;//记录时间
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group2 average time:" + group2Reactiontime);
        group2Acc = Acc;
        Acc = 0;

    }

    void HandleForthCollision()
    {
        // 第四次的碰撞逻辑，
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // 获取所有子物体中附加了MyChildScript脚本的组件
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // 遍历所有找到的脚本组件并修改public变量
                foreach (ChangeSprite script in childScripts)
                {

                    if (i == 0 || i == 3 || i == 4 || i == 6 || i == 7)
                    {
                        script.test = pic[6];//正方形
                    }
                    else if (i == 1 || i == 2 || i == 5 || i == 8 || i == 9)
                    {
                        script.test = pic[7];//长方形
                    }
                    else if (i == 14)
                    {
                        script.original = pic[6]; // 将Judge1图案设置为正方形
                        script.test = pic[6];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                    else if (i == 15)
                    {
                        script.original = pic[7]; // 将Judge2图案设置为长方形
                        script.test = pic[7];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                }
                SpriteRenderer spriteRenderer = ob.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }
            i++;
        }

        group3Reactiontime = timeElapsedBetweenCollisions / 10;//记录时间
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group3 average time:" + group3Reactiontime);
        group3Acc = Acc;
        Acc = 0;

    }

    void HandleFifthCollision()
    {
        // 第三次及以上的碰撞逻辑，
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // 获取所有子物体中附加了MyChildScript脚本的组件
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // 遍历所有找到的脚本组件并修改public变量
                foreach (ChangeSprite script in childScripts)
                {

                    if (i == 0 || i == 3 || i == 6 || i == 7 || i == 8)
                    {
                        script.test = pic[8];//笑脸
                    }
                    else if (i == 1 || i == 2 || i == 4 || i == 5 || i == 9)
                    {
                        script.test = pic[9];//哭脸
                    }
                    else if (i == 14)
                    {
                        script.original = pic[8]; // 将Judge1图案设置为笑脸
                        script.test = pic[8];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                    else if (i == 15)
                    {
                        script.original = pic[9]; // 将Judge2图案设置为哭脸
                        script.test = pic[9];
                        script.Next();// 改变每个Sprite Renderer的sprite图案
                    }
                }
                SpriteRenderer spriteRenderer = ob.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }
            i++;
        }

        group4Reactiontime = timeElapsedBetweenCollisions / 10;//记录时间
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group4 average time:" + group4Reactiontime);
        group4Acc = Acc;
        Acc = 0;

    }

    void HandleResult()
    {
        group5Reactiontime = timeElapsedBetweenCollisions / 10;//记录时间
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group4 average time:" + group4Reactiontime);
        group5Acc = Acc;
        Acc = 0;
        int i = 0;
        for (i = 0; i < 10; i++)
        {
            targetObjects[i].SetActive(false);//把图案给消失
        }
        targetObjects[14].SetActive(false);
        targetObjects[15].SetActive(false);
        targetObjects[16].SetActive(false);
        targetObjects[18].SetActive(false);//文字消失
        targetObjects[17].SetActive(true);//激活评分表
        Text[0].text = score.ToString();//总分数
        Text[1].text = group1Reactiontime.ToString();
        Text[2].text = (group1Acc / 10).ToString();
        Text[3].text = group2Reactiontime.ToString();
        Text[4].text = (group2Acc / 10).ToString();
        Text[5].text = group3Reactiontime.ToString();
        Text[6].text = (group3Acc / 10).ToString();
        Text[7].text = group4Reactiontime.ToString();
        Text[8].text = (group4Acc / 10).ToString();
        Text[9].text = group5Reactiontime.ToString();
        Text[10].text = (group5Acc / 10).ToString();
        if (score < 61.18)
        {
            targetObjects[19].SetActive(true);
        }
        else if (score >= 61.18 && score <= 100.04)
        {
            targetObjects[20].SetActive(true);
        }
        else
        {
            targetObjects[21].SetActive(true);
        }
    }

    // 加分函数
    private void AddScore(double points)
    {
        score += points;  // 增加分数
        //UpdateScoreText(); // 更新显示
    }

    // 更新 TextMeshPro 分数显示
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();  // 更新分数文本
        }
        else
        {
            Debug.LogWarning("ScoreText (TextMeshProUGUI) is not assigned in the inspector.");
        }
    }
}

