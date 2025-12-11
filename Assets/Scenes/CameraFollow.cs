using UnityEngine;

public class CameraFollowY : MonoBehaviour
{
    public Transform target = null; 
    public float smoothSpeed = 0.1f;

    private float initialYOffset;
    private Vector3 shakeOffset = Vector3.zero;  // ← 追加（シェイク用）

    void Start()
    {
        initialYOffset = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetY = target.position.y + initialYOffset;
        float newY = Mathf.Lerp(transform.position.y, targetY, smoothSpeed);

        // ▼ shakeOffset を加算してカメラの揺れを反映
        transform.position = new Vector3(
            transform.position.x + shakeOffset.x,
            newY + shakeOffset.y,
            transform.position.z
        );
    }

    // ▼ CameraShake から呼び出される（値を渡す窓口）
    public void SetShakeOffset(Vector3 offset)
    {
        shakeOffset = offset;
    }
}

