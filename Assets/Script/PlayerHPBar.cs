
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI使うときは忘れずに。
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class PlayerHPBar : MonoBehaviour
{
    //最大HPと現在のHP。
    int maxHp = 300;
    int currentHp;
    //Sliderを入れる
    public Slider slider;

    public GameObject gameoverText;

    public GameObject Sound;
    private AudioSource audioSource = null;


    // 赤いオーバーレイを制御するためのImage
    public Image damageOverlay;
    private Color overlayColor;
    public float overlayFadeDuration = 0.5f;

    void Start()
    {

        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = maxHp;
        Debug.Log("Start currentHp : " + currentHp);

        // 初期のオーバーレイカラーを設定
        if (damageOverlay != null)
        {
            overlayColor = damageOverlay.color;
            overlayColor.a = 0;
            damageOverlay.color = overlayColor;
        }
    }

    public GameManager gamemanager;
    //ColliderオブジェクトのIsTriggerにチェック入れること。
    private void OnTriggerEnter(Collider collider)
    {
        //Enemyタグのオブジェクトに触れると発動
        if (collider.gameObject.tag == "Enemy")
        {
            

            int damage = 10;
            Debug.Log("damage : " + damage);

            //現在のHPからダメージを引く
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);

            //最大HPにおける現在のHPをSliderに反映。
            //int同士の割り算は小数点以下は0になるので、
            //(float)をつけてfloatの変数として振舞わせる。
            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value : " + slider.value);

            StartCoroutine(FlashRedOverlay());

            audioSource = Sound.GetComponent<AudioSource>();
            audioSource.Play();
        }
        if (collider.gameObject.tag == "Recovery")
        {

            //現在のHPからダメージを引く
            currentHp = maxHp;
            Debug.Log("After currentHp : " + currentHp);

            //最大HPにおける現在のHPをSliderに反映。
            //int同士の割り算は小数点以下は0になるので、
            //(float)をつけてfloatの変数として振舞わせる。
            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value : " + slider.value);
        }
        if(slider.value <= 0)
        {
            Destroy(this.gameObject);
            // ゲームオーバーを呼び出す
            gamemanager.GameOver();


        }
    }

    private IEnumerator FlashRedOverlay()
    {
        overlayColor.a = 1;
        damageOverlay.color = overlayColor;

        // 徐々に透明にする
        float elapsedTime = 0f;
        while (elapsedTime < overlayFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            overlayColor.a = Mathf.Lerp(1, 0, elapsedTime / overlayFadeDuration);
            damageOverlay.color = overlayColor;
            yield return null;
        }
    }
}