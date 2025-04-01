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
    private Sprite lastCollisionSprite = null;  // ��¼��һ����ײ�����Sprite
    private Sprite spriteA = null;  // ��¼��ϵ�������Sprite
    private Sprite spriteB = null;  // ��¼��ϵ�������Sprite

    // ��ϵ�к���ϵ�еı�ǩ��ȷ��������������ȷ�ı�ǩ
    public string tagForSeriesA = "Judge";  // ��ϵ�������Tag
    public string tagForSeriesB = "Pattern";  // ��ϵ�������Tag

    public TextMeshPro scoreText;         // ������ʾ������ TMP ���
    private double score = 0;  // ����

    private int triggerCount = 0; // ��¼��������
    public GameObject[] targetObjects;  // ��Ҫ���Ƶ���������
    public Sprite[] pic;//ͼƬ

    private int grayCount = 0;


    void Start()
    {
        this.enabled = false; // ���õ�ǰ�ű�
    }

    void OnTriggerEnter(Collider other)
    {
        // ��ȡ��ǰ��ײ����� SpriteRenderer ���
        ChangeSprite Script = other.GetComponent<ChangeSprite>();

        if (Script != null)
        {
            Debug.Log(other.gameObject.name);
            Sprite currentSprite = Script.test;

            // �����ײ�������ڼ�ϵ�У�ͨ��Tag�жϣ�
            if (other.CompareTag(tagForSeriesA))
            {
                spriteA = currentSprite;  // ��¼��ϵ������� Sprite
                Debug.Log("Sprite A Name: " + spriteA.name);
                Debug.Log("GrayCountΪ" + grayCount);
                CheckTriggerCount(); // ��鴥������                            
            }
            // �����ײ����������ϵ��
            else if (other.CompareTag(tagForSeriesB))
            {
                string patternName = GetPatternName(Script.test.name);
                spriteB = currentSprite;  // ��¼��ϵ������� Sprite

                //����ǲ��Խ׶Σ���������
                if (Script.original.name == "�ʺ�")
                {
                    SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                    if (spriteRenderer.color != Color.gray)
                    {
                        spriteRenderer.color = Color.gray; // ����Ϊ��ɫ
                        Debug.Log("������");
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
            case "��": return "2";
            case "����": return "1";
            case "Ʋ": return "3";
            case "��": return "4";
            case "�˺�": return "6";
            case "�Ӻ�": return "5";
            case "������": return "7";
            case "������": return "8";
            case "Ц��": return "9";
            case "����": return "10";
            case "4A": return "2";
            case "1A": return "2";
            case "2A": return "2";
            default:
                Debug.LogError("δ֪�İ�ť����");
                return null;
        }
    }

    private void CheckTriggerCount()
    {
        if (grayCount == 10)
        {
            HandleSecondCollision(); // �ﵽ10�δ�������
        }
        else if (grayCount == 20)
        {
            HandleThirdCollision(); // �ﵽ20�δ�������
        }
        else if (grayCount == 30)
        {
            HandleForthCollision(); // �ﵽ30�δ�������
        }
        else if (grayCount == 40)
        {
            HandleFifthCollision(); // �ﵽ40�δ�������
        }
        else if(grayCount == 50)
        {
            HandleResult();//50,������
        }
    }

    void HandleSecondCollision()
    {
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
                    if (i == 0 || i == 2 || i == 3 || i == 5 || i == 8)
                    {
                        script.test = pic[3];//��
                    }
                    else if (i == 1 || i == 4 || i == 6 || i == 7 || i == 9)
                    {
                        script.test = pic[2];//Ʋ
                    }
                    else if (i == 14)
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


    void HandleThirdCollision()
    {
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

    void HandleForthCollision()
    {
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

    void HandleFifthCollision()
    {
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

