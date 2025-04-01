using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ChangeSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] pic;//图片
    public Sprite original;
    public Sprite test;//测试图案
    public string tagForSeriesA = "Judge";  // 甲系列物体的Tag
    public string tagForSeriesB = "Pattern";


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取 SpriteRenderer 组件
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
        if (this.CompareTag(tagForSeriesA))//如果碰到是判断按件，不触发这个
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
            spriteRenderer.sprite = original; // 可选择在退出时恢复为默认
            //Debug.Log("Sprite reset.");
        }
    }

    public void Next()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取 SpriteRenderer 组件
        spriteRenderer.sprite = original;
    }
}
