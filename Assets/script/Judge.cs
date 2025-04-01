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

    private float timeElapsedBetweenCollisions = 0.0f; // �����ۼӵ���ʱ��
    private float lastCollisionTime = 0.0f; // ���ڼ�¼��һ����ײ��ϵ�������ʱ��
    private float group1Reactiontime = 0.0f;
    private float group2Reactiontime = 0.0f;
    private float group3Reactiontime = 0.0f;
    private float group4Reactiontime = 0.0f;
    private float group5Reactiontime = 0.0f;

    private int Acc = 0;
    private float group1Acc = 0;//��¼׼ȷ��
    private float group2Acc = 0;
    private float group3Acc = 0;
    private float group4Acc = 0;
    private float group5Acc = 0;

    public TextMeshPro[] Text;//���ֵ������text


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

                float currentTime = Time.time;
                float timeDifference = currentTime - lastCollisionTime;
                timeElapsedBetweenCollisions += timeDifference; // �ۼ�ʱ��
                Debug.Log("Time difference between collisions: " + timeDifference);
                lastCollisionTime = 0; // ����Ϊ 0���ȴ���һ����ϵ���������ײ

                // ����ϵ������� Sprite ��������ϵ������� Sprite Ҳ�Ѽ�¼ʱ�����бȽ�
                if (spriteA != null && spriteB != null)
                {
                    if (spriteA.name == "��" || spriteA.name == "����")
                    {
                        AddScore(1.5);//�ж���ȷ��һ��
                    }
                    else if (spriteA.name == "Ʋ" || spriteA.name == "��")
                    {
                        AddScore(1);//�ж���ȷ��һ��
                    }
                    else if (spriteA.name == "�Ӻ�" || spriteA.name == "�˺�")
                    {
                        AddScore(2.75);//�ж���ȷ��һ��
                    }
                    else if (spriteA.name == "������" || spriteA.name == "������")
                    {
                        AddScore(3.06);//�ж���ȷ��һ��
                    }
                    else if (spriteA.name == "Ц��" || spriteA.name == "����")
                    {
                        AddScore(4.28);//�ж���ȷ��һ��
                    }
                    Acc++;//�ж���ȷ׼ȷ�ȼ�һ

                    // ���Ӵ�������
                    triggerCount++;
                    CheckTriggerCount(); // ��鴥������                            
                }
            }
            // �����ײ����������ϵ��
            else if (other.CompareTag(tagForSeriesB))
            {
                string patternName = GetPatternName(Script.test.name);
                spriteB = currentSprite;  // ��¼��ϵ������� Sprite

                // ��¼��ײ��ϵ�������ʱ��
                lastCollisionTime = Time.time;

                //����ǲ��Խ׶Σ���������
                if (Script.original.name == "�ʺ�")
                {
                    SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.gray; // ����Ϊ��ɫ

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
        if (triggerCount == 20)
        {
            HandleThirdCollision(); // �ﵽ20�δ�������
        }
        else if (triggerCount == 10)
        {
            HandleSecondCollision(); // �ﵽ10�δ�������
        }
        else if (triggerCount == 30)
        {
            HandleForthCollision(); // �ﵽ30�δ�������
        }
        else if (triggerCount == 40)
        {
            HandleFifthCollision(); // �ﵽ40�δ�������
        }
        else if(grayCount == 10 || triggerCount == 50)
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

        group1Reactiontime = timeElapsedBetweenCollisions / 10;//��¼ʱ��
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

        group2Reactiontime = timeElapsedBetweenCollisions / 10;//��¼ʱ��
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group2 average time:" + group2Reactiontime);
        group2Acc = Acc;
        Acc = 0;

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

        group3Reactiontime = timeElapsedBetweenCollisions / 10;//��¼ʱ��
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group3 average time:" + group3Reactiontime);
        group3Acc = Acc;
        Acc = 0;

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

        group4Reactiontime = timeElapsedBetweenCollisions / 10;//��¼ʱ��
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group4 average time:" + group4Reactiontime);
        group4Acc = Acc;
        Acc = 0;

    }

    void HandleResult()
    {
        group5Reactiontime = timeElapsedBetweenCollisions / 10;//��¼ʱ��
        timeElapsedBetweenCollisions = 0.0f;
        Debug.Log("group4 average time:" + group4Reactiontime);
        group5Acc = Acc;
        Acc = 0;
        int i = 0;
        for (i = 0; i < 10; i++)
        {
            targetObjects[i].SetActive(false);//��ͼ������ʧ
        }
        targetObjects[14].SetActive(false);
        targetObjects[15].SetActive(false);
        targetObjects[16].SetActive(false);
        targetObjects[18].SetActive(false);//������ʧ
        targetObjects[17].SetActive(true);//�������ֱ�
        Text[0].text = score.ToString();//�ܷ���
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

    // �ӷֺ���
    private void AddScore(double points)
    {
        score += points;  // ���ӷ���
        //UpdateScoreText(); // ������ʾ
    }

    // ���� TextMeshPro ������ʾ
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();  // ���·����ı�
        }
        else
        {
            Debug.LogWarning("ScoreText (TextMeshProUGUI) is not assigned in the inspector.");
        }
    }
}

