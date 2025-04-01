using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    private int collisionCount = 0;  // 碰撞计数器
    public GameObject[] targetObjects;  // 需要控制的物体数组
    public Sprite[] pic;//图片
    public GameObject targetObject;  // 包含 SpriteCollisionChecker 脚本的对象

    public TextMeshPro preliminaryText;         // 用于显示字的 TMP 组件

    void OnTriggerEnter(Collider collision)
    {
        collisionCount++;

        // 根据不同的碰撞次数，执行不同的操作
        switch (collisionCount)
        {
            case 1:
                HandleFirstCollision(collision);
                preliminaryText.gameObject.SetActive(false);


                break;
            //case 2:
            //    HandleSecondCollision(collision);
            //    break;
            //case 3:
            //    HandleThirdCollision(collision);
            //    break;
            //case 4:
            //    HandleForthCollision(collision);
            //    break;
            //case 5:
            //    HandleFifthCollision(collision);
            //    break;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        targetObjects[16].SetActive(false);
    }

    void HandleFirstCollision(Collider collision)
    {
        Debug.Log("First collision with: " + collision.gameObject.name);
        // 在这里执行第一次碰撞时的特殊逻辑
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
                    script.original = pic[10]; // 将myPublicVariable设置为10,问号
                    script.Next();// 改变每个Sprite Renderer的sprite图案
                    if (i == 0 || i == 4 || i == 5 || i == 3 || i == 9||i==15)
                    {
                        script.test = pic[1];//减号
                    }
                    else if (i == 1 || i == 2 || i == 6 || i == 7 || i == 8||i==14)
                    {
                        script.test = pic[0];//竖
                    }
                }
            }
            i++;
        }
        //激活某些部件，隐藏某些部件
        targetObjects[10].SetActive(false);
        targetObjects[11].SetActive(false);
        targetObjects[12].SetActive(false);
        targetObjects[13].SetActive(false);
        targetObjects[14].SetActive(true);
        targetObjects[15].SetActive(true);
        //激活脚本
        // 获取目标对象上的 SpriteCollisionChecker 脚本
        Judge_Test checker_test = targetObject.GetComponent<Judge_Test>();
        Judge checker = targetObject.GetComponent<Judge>();

        if (checker_test != null)
        {
            // 激活 SpriteCollisionChecker 脚本
            checker_test.enabled = true;  // 启用脚本
            Debug.Log("SpriteCollisionChecker is now active.");

            // 如果你想要禁用脚本，可以使用:
            // checker.enabled = false;
        }

        else if(checker != null)
        {
            // 激活 SpriteCollisionChecker 脚本
            checker.enabled = true;  // 启用脚本
            Debug.Log("SpriteCollisionChecker is now active.");
        }

        else
        {
            Debug.LogError("SpriteCollisionChecker script not found on target object.");
        }
    }

    void HandleSecondCollision(Collider collision)
    {
        Debug.Log("Second collision with: " + collision.gameObject.name);
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

                    if (i == 0 || i == 2 || i == 3 || i == 5 || i == 8 )
                    {
                        script.test = pic[3];//捺
                    }
                    else if (i == 1 || i == 4 || i == 6 || i == 7 || i == 9)
                    {
                        script.test = pic[2];//撇
                    }
                    else if(i==14)
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

    void HandleThirdCollision(Collider collision)
    {
        Debug.Log("Another collision (" + collisionCount + " times) with: " + collision.gameObject.name);
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

    void HandleForthCollision(Collider collision)
    {
        Debug.Log("Another collision (" + collisionCount + " times) with: " + collision.gameObject.name);
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

    void HandleFifthCollision(Collider collision)
    {
        Debug.Log("Another collision (" + collisionCount + " times) with: " + collision.gameObject.name);
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
}
