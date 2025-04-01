using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ChangeSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] pic;//ͼƬ
    public Sprite original;
    public Sprite test;//����ͼ��
    public string tagForSeriesA = "Judge";  // ��ϵ�������Tag
    public string tagForSeriesB = "Pattern";


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��ȡ SpriteRenderer ���
        original = pic[0];
        spriteRenderer.sprite = original;

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on " + gameObject.name);
            return;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag(tagForSeriesA))//����������жϰ��������������
        {
            return;
        }
        //Debug.Log("Sprite Trigger detected with: " + other.gameObject.name);
        if (spriteRenderer != null && pic != null)
        {

            spriteRenderer.sprite = pic[1];
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = original; // ��ѡ�����˳�ʱ�ָ�ΪĬ��
            //Debug.Log("Sprite reset.");
        }
    }

    public void Next()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��ȡ SpriteRenderer ���
        spriteRenderer.sprite = original;
    }
}
