using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.2f;

    private CameraFollowY follow;   // ← 追加
    private void Awake()
    {
        follow = GetComponent<CameraFollowY>();
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    private System.Collections.IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);

            // ▼ CameraFollowY に揺れ量を渡す
            follow.SetShakeOffset(new Vector3(x, y, 0));

            elapsed += Time.deltaTime;
            yield return null;
        }

        // ▼ 終了時に揺れをゼロに戻す
        follow.SetShakeOffset(Vector3.zero);
    }
}

