using UnityEngine;
using TMPro;  // TextMeshPro用

public class GameManager : MonoBehaviour
{
    public Transform bottomLine;  // ボックス下のライン
    public Transform targetLine;  // 判定用の地面ライン
    public float lineThickness = 2f; // ラインの縦幅

    public Transform box;          // 落下中のボックス
    //public Transform targetLine;   // 判定ライン
    public TextMeshProUGUI distanceText;

    public float distance;

    public bool gameEnd = false;

    void Update()
    {
        if (box == null || targetLine == null) return;

        // Y軸距離（絶対値）
        //float distance = Mathf.Abs(box.position.y - targetLine.position.y);

        distance = box.position.y - targetLine.position.y;

        if(!gameEnd)
        {
            // 小数1桁で表示（例：2.3m）
            distanceText.text = $"5m以内で止めろ！\n\n地上まで\n{(distance / 10):F1}m";
        }
    }

    // // ✔ ボタンを押した直後に呼び出される
    // public void CheckOverlap()
    // {
    //     if (IsOverlap())
    //     {
    //         Success();
    //     }
    //     else
    //     {
    //         Fail();
    //     }
    // }


    // // 重なり判定の本体
    // private bool IsOverlap()
    // {
    //     // bottomLine の上下
    //     float bottomTop = bottomLine.position.y + lineThickness / 2f;
    //     float bottomBottom = bottomLine.position.y - lineThickness / 2f;

    //     // targetLine の上下
    //     float targetTop = targetLine.position.y + lineThickness / 2f;
    //     float targetBottom = targetLine.position.y - lineThickness / 2f;

    //     // 重なっている or 接している場合 → true
    //     bool overlap =
    //         !(bottomBottom > targetTop || bottomTop < targetBottom);

    //     return overlap;
    // }

    // ▼ 成功
    public void Success()
    {
        Debug.Log("成功！（少しでも接している or 重なっている）");
        gameEnd = true;

        // 成功演出を入れたいならここに追加
        distanceText.text = $"脱出成功！\n\n地上まで\n{((distance - 0.7f) / 10):F1}m";
    }

    // ▼ 失敗
    public void Fail()
    {
        Debug.Log("失敗！（重なっていない or 地面に触れた）");
        gameEnd = true;

        // 失敗演出を入れたいならここに追加
        distanceText.text = $"脱出失敗！\n\n地上まで\n{((distance - 0.7f) / 10):F1}m";
    }
}



