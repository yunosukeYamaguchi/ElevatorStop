using UnityEngine;
using System.Collections;

public class FallingBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isStopped = false;        // プレイヤーによる停止処理したか
    private bool hasTouchedGround = false; // 地面に衝突したか

    [Header("判定用オブジェクト")]
    public Collider2D bottomLine;
    public Collider2D targetLine;

    [Header("Audio Sources")]
    public AudioSource loopSource;      // 風音(wind)
    public AudioSource oneShotSource;   // start, stop, collision

    [Header("Audio Clips")]
    public AudioClip startClip;
    public AudioClip windClip;
    public AudioClip stopClip;
    public AudioClip collisionClip;

    public GameManager GameManager;
    public CameraShake cameraShake;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        StartCoroutine(PlayStartThenFall());
    }

    IEnumerator PlayStartThenFall()
    {
        oneShotSource.PlayOneShot(startClip);
        yield return new WaitForSeconds(startClip.length);

        StartFalling();
    }

    void StartFalling()
    {
        rb.isKinematic = false;

        loopSource.clip = windClip;
        loopSource.loop = true;
        loopSource.volume = 1f;
        loopSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPlayerPressedStop();
        }
    }

    // -------------------------------------------------------
    // ★ プレイヤーが Stop を押した時の処理
    // -------------------------------------------------------
    void OnPlayerPressedStop()
    {
        if (isStopped) return;

        // 衝突後 → 無効
        if (hasTouchedGround)
        {
            Debug.Log("衝突後 → stop.mp3 は再生しない");
            return;
        }

        // stop.mp3 再生（即時）
        oneShotSource.PlayOneShot(stopClip);
        Debug.Log("stop.mp3 再生開始");

        isStopped = true;

        // 5秒かけて停止
        StartCoroutine(SlowDownMovement());

        // 風音フェードアウト
        StartCoroutine(FadeOutWind());

        // 成功/失敗判定実行
        JudgeSuccessOrFail();
    }

    // -------------------------------------------------------
    // ★ 衝突処理
    // -------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasTouchedGround) return;

        if (collision.collider.CompareTag("Ground"))
        {
            hasTouchedGround = true;

            loopSource.Stop();

            // stop を押していない時だけ衝突音を鳴らす
            if (!isStopped)
            {
                oneShotSource.PlayOneShot(collisionClip);
            }

            cameraShake?.ShakeCamera();

            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;

            // stop を押していないまま衝突 → 失敗確定
            if (!isStopped)
            {
                GameManager.Fail();
            }
        }
    }

    // -------------------------------------------------------
    // ★ success / fail 判定
    // -------------------------------------------------------
    void JudgeSuccessOrFail()
    {
        if (bottomLine == null || targetLine == null)
        {
            Debug.LogWarning("bottomLine または targetLine が設定されていません！");
            return;
        }

        bool isOverlap = bottomLine.IsTouching(targetLine);

        if (isOverlap)
        {
            Debug.Log("成功！");
            GameManager.Success();
        }
        else
        {
            Debug.Log("失敗！");
            GameManager.Fail();
        }
    }


    // -------------------------------------------------------
    // wind フェードアウト
    // -------------------------------------------------------
    IEnumerator FadeOutWind()
    {
        float v = loopSource.volume;

        while (v > 0f)
        {
            v -= Time.deltaTime;
            loopSource.volume = v;
            yield return null;
        }

        loopSource.Stop();
    }

    // -------------------------------------------------------
    // ★ 5秒かけて減速 → 停止
    // -------------------------------------------------------
    IEnumerator SlowDownMovement()
    {
        float duration = 0.5f;
        float time = 0f;

        Vector2 startVelocity = rb.linearVelocity;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            rb.linearVelocity = Vector2.Lerp(startVelocity, Vector2.zero, t);

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
    }
}
