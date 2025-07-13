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
            // GameOverテキストを呼び出す
            gameoverText.SetActive(true);
            // GameRestartを呼び出して6秒間待つ
            Invoke("GameRestart", 12);
        }

        public void GameRestart()
        {
            // 現在のシーンを取得してロードする
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.name);
        }
}

