using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーン管理用

public class CountdownManager : MonoBehaviour
{
    // カウントダウンタイマーの設定
    public float countdownTime = 300f; // 5分（300秒）
    private float remainingTime;
    private bool isCountingDown = false;
    private bool isPaused = false;

    // Playerオブジェクトをインスペクターで設定可能にする
    public GameObject playerObject;

    // カウントダウンが開始されるコライダーオブジェクト
    public GameObject targetObjectToActivate;
    public GameObject objectToPauseCountdown;
    private Collider triggerCollider;

    // カウントダウン表示用のUIテキスト
    public Text countdownText;

    // ScoreManagerの参照
    public ScoreManager scoreManager;

    void Start()
    {
        remainingTime = countdownTime; // カウントダウンの初期化

        // カウントダウンテキストを初期化
        UpdateCountdownText();

        triggerCollider = GetComponent<Collider>();

        if (triggerCollider == null)
        {
            Debug.LogError("このオブジェクトにはColliderコンポーネントが必要です。");
        }
    }

    void Update()
    {
        if (triggerCollider != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();
                if (playerCollider != null && triggerCollider.bounds.Intersects(playerCollider.bounds))
                {
                    // プレイヤーが触れたと判断された場合の処理
                    // カウントダウン開始
                    isCountingDown = true;
                }
            }
        }

        // カウントダウンがアクティブであり、停止していない場合は時間を減らす
        if (isCountingDown && !isPaused)
        {
            remainingTime -= Time.deltaTime;

            // カウントダウンが終了した場合
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                ActivateObjectAndDisplayScore();

                // 10秒後にゲームをリセット
                Invoke("ResetGame", 10f);
            }

            // カウントダウンのUIを更新
            UpdateCountdownText();
        }

        // 他のオブジェクトがアクティブな場合はカウントダウンを停止
        if (objectToPauseCountdown.activeSelf)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }


    // 5分後にオブジェクトを有効化し、スコアとランクを表示
    private void ActivateObjectAndDisplayScore()
    {
        // ターゲットオブジェクトを有効化
        if (targetObjectToActivate != null)
        {
            targetObjectToActivate.SetActive(true);
        }

        // ScoreManagerのOnBossDefeatedメソッドを呼び出す
        if (scoreManager != null)
        {
            scoreManager.OnBossDefeated();
        }

        // カウントダウンを停止
        isCountingDown = false;
    }

    // カウントダウンのUIテキストを更新する
    private void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime % 60F);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 10秒後にゲームをリセットする処理
    private void ResetGame()
    {
        // 現在のシーンを再読み込みすることでゲームをリセット
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
