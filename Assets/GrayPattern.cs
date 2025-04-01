using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayPattern : MonoBehaviour
{
    public string tagForSeriesB = "Pattern";  // ��ϵ�������Tag
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

    private void OnTriggerEnter(Collider other)
    {
            //Debug.Log(other.name);
            if (other.CompareTag(tagForSeriesB))
            {
                ChangeSprite script = other.GetComponent<ChangeSprite>();
                if (script == null) return;

                string patternName = GetPatternName(script.test.name);

                //����ǲ��Խ׶Σ���������
                if (script.original.name == "�ʺ�")
                {
                    SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.gray; // ����Ϊ��ɫ
                    }
                }
            }
    }
}
