using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShellController : MonoBehaviour
{
    private AudioSource audioSource;

    /// <summary>
    /// 地面に触れたときのサウンド
    /// </summary>
    [SerializeField]
    private AudioClip impactSound;

    /// <summary>
    /// 落下速度がこれ以下のときには音を鳴らさない
    /// </summary>
    [SerializeField]
    private float minimumVelocity = 2.0f;

    private Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // 音が複数回再生されないように一度だけ再生するための設定
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突の強さを確認し、閾値以上の場合のみ音を再生
        if (rb.velocity.magnitude > minimumVelocity)
        {
            if (impactSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(impactSound);
            }
        }
    }
}
