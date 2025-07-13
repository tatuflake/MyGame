
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI使うときは忘れずに。
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    //最大HPと現在のHP。
    public int maxHp;
    int currentHp;
    //Sliderを入れる
    public Slider slider;
    public int scorepoint;
    
    private GameObject scoreText;//*

    public AudioClip sound1;


    [SerializeField] private Transform burstVFXPrefab;

    private bool isDead = false;

    public GameObject Sound;
    private AudioSource audioSource = null;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText");//*
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = maxHp;
        Debug.Log("Start currentHp : " + currentHp);

        audioSource = GetComponent<AudioSource>();
    }

    //ColliderオブジェクトのIsTriggerにチェック入れること。
    private void OnTriggerEnter(Collider collider)
    {
        //Enemyタグのオブジェクトに触れると発動
        if (collider.gameObject.tag == "Gun")
        {
            if (isDead) return;

            audioSource.PlayOneShot(sound1);
            
            int damage = 10;
            Debug.Log("damage : " + damage);

            //現在のHPからダメージを引く
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);

            //最大HPにおける現在のHPをSliderに反映。
            //int同士の割り算は小数点以下は0になるので、
            //(float)をつけてfloatの変数として振舞わせる。
            slider.value = (float)currentHp / (float)maxHp;
            Debug.Log("slider.value : " + slider.value);
        }

        if (slider.value <= 0)
        {
            isDead = true;
            ScoreManager scoreManager = scoreText.GetComponent<ScoreManager>();
            scoreManager.score += scorepoint;

            audioSource = Sound.GetComponent<AudioSource>();
            audioSource.Play();

            Destroy(this.gameObject);
            Transform burstVFXTransform = Instantiate(burstVFXPrefab, transform.position, Quaternion.identity);
            
        }
    }
}