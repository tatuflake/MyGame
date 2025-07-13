using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShellController : MonoBehaviour
{
    private AudioSource audioSource;

    /// <summary>
    /// �n�ʂɐG�ꂽ�Ƃ��̃T�E���h
    /// </summary>
    [SerializeField]
    private AudioClip impactSound;

    /// <summary>
    /// �������x������ȉ��̂Ƃ��ɂ͉���炳�Ȃ�
    /// </summary>
    [SerializeField]
    private float minimumVelocity = 2.0f;

    private Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // ����������Đ�����Ȃ��悤�Ɉ�x�����Đ����邽�߂̐ݒ�
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �Փ˂̋������m�F���A臒l�ȏ�̏ꍇ�̂݉����Đ�
        if (rb.velocity.magnitude > minimumVelocity)
        {
            if (impactSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(impactSound);
            }
        }
    }
}
