using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{
    // �X�R�A�̕ϐ�
    private int score = 0;

    // �X�R�A�\���p��Text UI
    public Text scoreText;

    // �����N�\���p��Text UI
    public Text rankText;

    // �{�X�̃Q�[���I�u�W�F�N�g
    public GameObject boss;



    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
        scoreText.text = "0";

        // ������Ԃł̓X�R�A�ƃ����N�̕\�����\���ɂ���
        scoreText.gameObject.SetActive(false);
        rankText.gameObject.SetActive(false);
    }

    // �G���|���ꂽ�Ƃ��ɃX�R�A�����Z����֐�
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    // �X�R�A�ɉ����ă����N���擾����֐�
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

    // �X�R�A�ƃ����N�̕\�����X�V����֐�
    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score.ToString();
        rankText.text = "Rank: " + GetRank();
    }

    // �{�X���|���ꂽ�Ƃ���UI��\������֐�
    public void OnBossDefeated()
    {
        // �X�R�A�ƃ����N��UI��\������
        scoreText.gameObject.SetActive(true);
        rankText.gameObject.SetActive(true);

        // �\�����X�V����
        UpdateScoreDisplay();
    }
}
