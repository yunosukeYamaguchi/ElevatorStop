using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    private Coroutine slowDownCoroutine;

    public GameObject window_fail;
    public GameObject monitor;
    private bool start = false;

    void Start()
    {
        window_fail.SetActive(false);
        monitor.SetActive(true);
        start = false;

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (start)
        {
            for (KeyCode key = KeyCode.Keypad0; key <= KeyCode.Keypad9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    OnPlayerPressedStop();
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPlayerPressedStop();
        }

        if (!start)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(PlayStartThenFall());
                monitor.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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

        start = true;
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
        slowDownCoroutine = StartCoroutine(SlowDownMovement());

        // 風音フェードアウト
        StartCoroutine(FadeOutWind());
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

            oneShotSource.PlayOneShot(collisionClip);

            cameraShake?.ShakeCamera();

            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;

            if (slowDownCoroutine != null)
            {
                StopCoroutine(slowDownCoroutine);
                slowDownCoroutine = null;
                // 即停止したときの処理
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }

            window_fail.SetActive(true);
            GameManager.Fail();
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

        bool isOverlap = false;

        if (50f > GameManager.distance)
        {
            isOverlap = true;
        }

        if (isOverlap)
        {
            Debug.Log("FallingBox 成功");
            GameManager.Success();
        }
        else
        {
            Debug.Log("FallingBox 失敗");
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

        // 成功/失敗判定実行
        JudgeSuccessOrFail();
    }
}
