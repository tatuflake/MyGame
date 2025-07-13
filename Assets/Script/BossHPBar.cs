using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHPBar : MonoBehaviour
{
    public int maxHp;
    int currentHp;
    public Slider slider;
    public GameObject gameclearText;
    public int scorepoint;
    private GameObject scoreText;

    public AudioClip sound1;
    public AudioClip sound2;

    public GameObject Sound;
    private AudioSource audioSource = null;

    [SerializeField] private Transform burstVFXPrefab;

    private bool isDead = false;

    // HPが半分を切った時に有効にするゲームオブジェクトのリスト
    public List<GameObject> objectsToActivate;

    public GameObject targetObject_D;

    private bool halfHpActivated = false; // HPが半分を切ったかをチェックするフラグ
    private bool isInvincible = false;    // ボスが無敵状態かどうかを示すフラグ

    public List<Collider> hitColliders;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText");
        slider.value = 1;
        currentHp = maxHp;
        audioSource = GetComponent<AudioSource>();
    }

    public GameManager gamemanager;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Gun")
        {
            if (isDead || isInvincible) return;

            audioSource.PlayOneShot(sound1);

            int damage = 10;

            currentHp = currentHp - damage;
            slider.value = (float)currentHp / (float)maxHp;

            // HPが半分を切った時に無敵状態にしてゲームオブジェクトを有効にする
            if (!halfHpActivated && currentHp <= maxHp / 2)
            {
                halfHpActivated = true;
                StartCoroutine(ActivateInvincibilityAndSpawnObjects());
            }
        }

        if (slider.value <= 0)
        {
            isDead = true;
            ScoreManager scoreManager = scoreText.GetComponent<ScoreManager>();
            scoreManager.score += scorepoint;

            audioSource = Sound.GetComponent<AudioSource>();
            audioSource.Play();

            targetObject_D.SetActive(!targetObject_D.activeSelf);

            FindObjectOfType<ScoreManager>().OnBossDefeated();
            Destroy(this.gameObject);
            Transform burstVFXTransform = Instantiate(burstVFXPrefab, transform.position, Quaternion.identity);
            gamemanager.GameOver();
        }
    }

    // HPが半分を切った時にゲームオブジェクトを有効にし、10秒間無敵状態にするコルーチン
    private IEnumerator ActivateInvincibilityAndSpawnObjects()
    {
        // サウンドを5回連続で再生
        for (int i = 0; i < 5; i++)
        {
            audioSource.PlayOneShot(sound2);
            yield return new WaitForSeconds(0.1f); // 少し間隔を空けて再生
        }

        // 無敵状態にする
        isInvincible = true;

        // ゲームオブジェクトを有効にする
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        // 10秒間待機する
        yield return new WaitForSeconds(10f);

        // 無敵状態を解除する
        isInvincible = false;
    }
}
