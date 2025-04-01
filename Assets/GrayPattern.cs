using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayPattern : MonoBehaviour
{
    public string tagForSeriesB = "Pattern";  // 乙系列物体的Tag
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

    private void OnTriggerEnter(Collider other)
    {
            //Debug.Log(other.name);
            if (other.CompareTag(tagForSeriesB))
            {
                ChangeSprite script = other.GetComponent<ChangeSprite>();
                if (script == null) return;

                string patternName = GetPatternName(script.test.name);

                //如果是测试阶段，则将物体变灰
                if (script.original.name == "问号")
                {
                    SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.gray; // 设置为灰色
                    }
                }
            }
    }
}
