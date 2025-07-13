using UnityEngine;

public class PlaySoundInArea : MonoBehaviour
{
    /// <summary>
    /// サウンドを再生するためのAudioSourceコンポーネント
    /// </summary>
    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        // サウンドを最初は停止しておく
        if (audioSource != null)
        {
            audioSource.loop = true; // サウンドをループ再生に設定
            audioSource.Stop(); // サウンドを停止
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // プレイヤーがエリアに入ったときにサウンドを再生
        if (collider.gameObject.tag == "Player")
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // サウンドを再生
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // プレイヤーがエリアから出たときにサウンドを停止
        if (collider.gameObject.tag == "Player")
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop(); // サウンドを停止
            }
        }
    }
}
