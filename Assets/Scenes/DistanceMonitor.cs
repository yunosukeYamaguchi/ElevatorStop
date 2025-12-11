using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistanceMonitor : MonoBehaviour
{
    public Transform box;              // 落下するボックス
    public float hideDistance = 0.0f;  // この距離以下で UI を非表示（今回は使わない）
    public GameObject distancePanel;   // 表示したい UI

    private float startY;              // 初期位置

    void Start()
    {
        startY = box.position.y;

        // 最初から表示
        distancePanel.SetActive(true);
    }

    void Update()
    {
        // UI を非表示にする処理をしない
        // float currentDistance = startY - box.position.y;
        // if (currentDistance >= hideDistance)
        // {
        //     distancePanel.SetActive(false);
        // }
    }
}


