using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text scoreText;
    public Text rankText;
    public GameObject boss;
    public int score = 0;

    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
        scoreText.text = "0";
        rankText.gameObject.SetActive(false);
    }

    void Update()
    {
        scoreText.text = score.ToString();
    }

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

    private void UpdateScoreDisplay()
    {
        rankText.text = GetRank();
    }

    public void OnBossDefeated()
    {
        // スコアとランクのUIを表示する
        rankText.gameObject.SetActive(true);

        // 表示を更新する
        UpdateScoreDisplay();
    }
}