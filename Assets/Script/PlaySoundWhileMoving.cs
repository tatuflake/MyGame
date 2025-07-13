using UnityEngine;

public class PlaySoundWhileMoving : MonoBehaviour
{
    /// <summary>
    /// サウンドを再生するためのAudioSourceコンポーネント
    /// </summary>
    [SerializeField]
    private AudioSource audioSource;

    /// <summary>
    /// プレイヤーの前回フレームの位置
    /// </summary>
    private Vector3 lastPosition;

    /// <summary>
    /// プレイヤーの移動検出のための閾値
    /// </summary>
    [SerializeField]
    private float movementThreshold = 0.01f;

    void Start()
    {
        // 最初の位置を記憶
        lastPosition = transform.position;

        // サウンドを最初は停止しておく
        if (audioSource != null)
        {
            audioSource.loop = true; // サウンドをループ再生に設定
            audioSource.Stop(); // サウンドを停止
        }
    }

    void Update()
    {
        // 現在の位置と前フレームの位置を比較
        Vector3 currentPosition = transform.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

        // プレイヤーが移動しているかどうかをチェック
        if (audioSource != null)
        {
            if (distanceMoved > movementThreshold) // 閾値を超える移動があれば音を再生
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play(); // サウンドを再生
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop(); // サウンドを停止
                }
            }
        }

        // 現在の位置を次のフレームのために保存
        lastPosition = currentPosition;
    }
}
