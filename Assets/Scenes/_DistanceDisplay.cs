using UnityEngine;
using TMPro;  // TextMeshPro用

public class DistanceDisplay : MonoBehaviour
{
    public Transform box;          // 落下中のボックス
    public Transform targetLine;   // 判定ライン
    public TextMeshProUGUI distanceText;

    void Update()
    {
        if (box == null || targetLine == null) return;

        // Y軸距離（絶対値）
        //float distance = Mathf.Abs(box.position.y - targetLine.position.y);

        float distance = box.position.y - targetLine.position.y;

        // 小数1桁で表示（例：2.3m）
        distanceText.text = $"地上までの距離：{distance:F1}m";
    }
}

