using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    private int collisionCount = 0;  // ��ײ������
    public GameObject[] targetObjects;  // ��Ҫ���Ƶ���������
    public Sprite[] pic;//ͼƬ
    public GameObject targetObject;  // ���� SpriteCollisionChecker �ű��Ķ���

    public TextMeshPro preliminaryText;         // ������ʾ�ֵ� TMP ���

    void OnTriggerEnter(Collider collision)
    {
        collisionCount++;

        // ���ݲ�ͬ����ײ������ִ�в�ͬ�Ĳ���
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
        // ������ִ�е�һ����ײʱ�������߼�
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // ��ȡ�����������и�����MyChildScript�ű������
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // ���������ҵ��Ľű�������޸�public����
                foreach (ChangeSprite script in childScripts)
                {
                    script.original = pic[10]; // ��myPublicVariable����Ϊ10,�ʺ�
                    script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
                    if (i == 0 || i == 4 || i == 5 || i == 3 || i == 9||i==15)
                    {
                        script.test = pic[1];//����
                    }
                    else if (i == 1 || i == 2 || i == 6 || i == 7 || i == 8||i==14)
                    {
                        script.test = pic[0];//��
                    }
                }
            }
            i++;
        }
        //����ĳЩ����������ĳЩ����
        targetObjects[10].SetActive(false);
        targetObjects[11].SetActive(false);
        targetObjects[12].SetActive(false);
        targetObjects[13].SetActive(false);
        targetObjects[14].SetActive(true);
        targetObjects[15].SetActive(true);
        //����ű�
        // ��ȡĿ������ϵ� SpriteCollisionChecker �ű�
        Judge_Test checker_test = targetObject.GetComponent<Judge_Test>();
        Judge checker = targetObject.GetComponent<Judge>();

        if (checker_test != null)
        {
            // ���� SpriteCollisionChecker �ű�
            checker_test.enabled = true;  // ���ýű�
            Debug.Log("SpriteCollisionChecker is now active.");

            // �������Ҫ���ýű�������ʹ��:
            // checker.enabled = false;
        }

        else if(checker != null)
        {
            // ���� SpriteCollisionChecker �ű�
            checker.enabled = true;  // ���ýű�
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
        // ������ִ�еڶ�����ײʱ�������߼�
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // ��ȡ�����������и�����MyChildScript�ű������
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // ���������ҵ��Ľű�������޸�public����
                foreach (ChangeSprite script in childScripts)
                {

                    if (i == 0 || i == 2 || i == 3 || i == 5 || i == 8 )
                    {
                        script.test = pic[3];//��
                    }
                    else if (i == 1 || i == 4 || i == 6 || i == 7 || i == 9)
                    {
                        script.test = pic[2];//Ʋ
                    }
                    else if(i==14)
                    {
                        script.original = pic[3]; // ��Judge1ͼ������Ϊ��
                        script.test = pic[3];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
                    }
                    else if (i == 15)
                    {
                        script.original = pic[2]; // ��Judge2ͼ������ΪƲ
                        script.test = pic[2];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
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
        // �����μ����ϵ���ײ�߼���
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // ��ȡ�����������и�����MyChildScript�ű������
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // ���������ҵ��Ľű�������޸�public����
                foreach (ChangeSprite script in childScripts)
                {

                    if (i == 0 || i == 1 || i == 4 || i == 6 || i == 7)
                    {
                        script.test = pic[4];//�˺�
                    }
                    else if (i == 2 || i == 3 || i == 5 || i == 8 || i == 9)
                    {
                        script.test = pic[5];//�Ӻ�
                    }
                    else if (i == 14)
                    {
                        script.original = pic[5]; // ��Judge1ͼ������Ϊ�Ӻ�
                        script.test = pic[5];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
                    }
                    else if (i == 15)
                    {
                        script.original = pic[4]; // ��Judge2ͼ������Ϊ�˺�
                        script.test = pic[4];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
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
        // ���Ĵε���ײ�߼���
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // ��ȡ�����������и�����MyChildScript�ű������
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // ���������ҵ��Ľű�������޸�public����
                foreach (ChangeSprite script in childScripts)
                {

                    if (i == 0 || i == 3 || i == 4 || i == 6 || i == 7)
                    {
                        script.test = pic[6];//������
                    }
                    else if (i == 1 || i == 2 || i == 5 || i == 8 || i == 9)
                    {
                        script.test = pic[7];//������
                    }
                    else if (i == 14)
                    {
                        script.original = pic[6]; // ��Judge1ͼ������Ϊ������
                        script.test = pic[6];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
                    }
                    else if (i == 15)
                    {
                        script.original = pic[7]; // ��Judge2ͼ������Ϊ������
                        script.test = pic[7];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
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
        // �����μ����ϵ���ײ�߼���
        int i = 0;
        foreach (GameObject ob in targetObjects)
        {
            if (ob != null)
            {
                // ��ȡ�����������и�����MyChildScript�ű������
                ChangeSprite[] childScripts = ob.GetComponentsInChildren<ChangeSprite>();

                // ���������ҵ��Ľű�������޸�public����
                foreach (ChangeSprite script in childScripts)
                {

                    if (i == 0 || i == 3 || i == 6 || i == 7 || i == 8)
                    {
                        script.test = pic[8];//Ц��
                    }
                    else if (i == 1 || i == 2 || i == 4 || i == 5 || i == 9)
                    {
                        script.test = pic[9];//����
                    }
                    else if (i == 14)
                    {
                        script.original = pic[8]; // ��Judge1ͼ������ΪЦ��
                        script.test = pic[8];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
                    }
                    else if (i == 15)
                    {
                        script.original = pic[9]; // ��Judge2ͼ������Ϊ����
                        script.test = pic[9];
                        script.Next();// �ı�ÿ��Sprite Renderer��spriteͼ��
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
