using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;

        public void GameOver()
        {
            // GameOver�e�L�X�g���Ăяo��
            gameoverText.SetActive(true);
            // GameRestart���Ăяo����6�b�ԑ҂�
            Invoke("GameRestart", 12);
        }

        public void GameRestart()
        {
            // ���݂̃V�[�����擾���ă��[�h����
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.name);
        }
}

