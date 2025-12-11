using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform bottomLine;  // ボックス下のライン
    public Transform targetLine;  // 判定用の地面ライン
    public float lineThickness = 2f; // ラインの縦幅

    // ✔ ボタンを押した直後に呼び出される
    public void CheckOverlap()
    {
        if (IsOverlap())
        {
            Success();
        }
        else
        {
            Fail();
        }
    }

    // 🎯 重なり判定の本体
    private bool IsOverlap()
    {
        // bottomLine の上下
        float bottomTop = bottomLine.position.y + lineThickness / 2f;
        float bottomBottom = bottomLine.position.y - lineThickness / 2f;

        // targetLine の上下
        float targetTop = targetLine.position.y + lineThickness / 2f;
        float targetBottom = targetLine.position.y - lineThickness / 2f;

        // 🎯重なっている or 接している場合 → true
        bool overlap =
            !(bottomBottom > targetTop || bottomTop < targetBottom);

        return overlap;
    }

    // ▼ 成功
    public void Success()
    {
        Debug.Log("🎉 成功！（少しでも接している or 重なっている）");
        // 成功演出を入れたいならここに追加
    }

    // ▼ 失敗
    public void Fail()
    {
        Debug.Log("💥 失敗！（重なっていない or 地面に触れた）");
        // 失敗演出を入れたいならここに追加
    }
}



