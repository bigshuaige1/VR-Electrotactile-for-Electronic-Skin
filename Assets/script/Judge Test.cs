using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.UI.Image;

public class Judge_Test : MonoBehaviour
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
                Debug.Log("GrayCount为" + grayCount);
                CheckTriggerCount(); // 检查触发次数                            
            }
            // 如果碰撞物体属于乙系列
            else if (other.CompareTag(tagForSeriesB))
            {
                string patternName = GetPatternName(Script.test.name);
                spriteB = currentSprite;  // 记录乙系列物体的 Sprite

                //如果是测试阶段，则将物体变灰
                if (Script.original.name == "问号")
                {
                    SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                    if (spriteRenderer.color != Color.gray)
                    {
                        spriteRenderer.color = Color.gray; // 设置为灰色
                        Debug.Log("方块变灰");
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
        if (grayCount == 10)
        {
            HandleSecondCollision(); // 达到10次触发函数
        }
        else if (grayCount == 20)
        {
            HandleThirdCollision(); // 达到20次触发函数
        }
        else if (grayCount == 30)
        {
            HandleForthCollision(); // 达到30次触发函数
        }
        else if (grayCount == 40)
        {
            HandleFifthCollision(); // 达到40次触发函数
        }
        else if(grayCount == 50)
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
    }

    void HandleResult()
    {
        scoreSheet.gameObject.SetActive(true);

        for(int i = 0; i<17; i++)
        {
            targetObjects[i].gameObject.SetActive(false);
        }

        targetObjects[18].gameObject.SetActive(false);
    }
}

