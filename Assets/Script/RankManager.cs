using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{
    // スコアの変数
    private int score = 0;

    // スコア表示用のText UI
    public Text scoreText;

    // ランク表示用のText UI
    public Text rankText;

    // ボスのゲームオブジェクト
    public GameObject boss;



    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
        scoreText.text = "0";

        // 初期状態ではスコアとランクの表示を非表示にする
        scoreText.gameObject.SetActive(false);
        rankText.gameObject.SetActive(false);
    }

    // 敵が倒されたときにスコアを加算する関数
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    // スコアに応じてランクを取得する関数
    private string GetRank()
    {
        if (score > 9000)
        {
            return "SS";
        }
        else if (score > 5000)
        {
            return "S";
        }
        else if (score > 3000)
        {
            return "A";
        }
        else if (score > 1000)
        {
            return "B";
        }
        else
        {
            return "C";
        }
    }

    // スコアとランクの表示を更新する関数
    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score.ToString();
        rankText.text = "Rank: " + GetRank();
    }

    // ボスが倒されたときにUIを表示する関数
    public void OnBossDefeated()
    {
        // スコアとランクのUIを表示する
        scoreText.gameObject.SetActive(true);
        rankText.gameObject.SetActive(true);

        // 表示を更新する
        UpdateScoreDisplay();
    }
}
