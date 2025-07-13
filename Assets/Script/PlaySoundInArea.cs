using UnityEngine;

public class PlaySoundInArea : MonoBehaviour
{
    /// <summary>
    /// �T�E���h���Đ����邽�߂�AudioSource�R���|�[�l���g
    /// </summary>
    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        // �T�E���h���ŏ��͒�~���Ă���
        if (audioSource != null)
        {
            audioSource.loop = true; // �T�E���h�����[�v�Đ��ɐݒ�
            audioSource.Stop(); // �T�E���h���~
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // �v���C���[���G���A�ɓ������Ƃ��ɃT�E���h���Đ�
        if (collider.gameObject.tag == "Player")
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // �T�E���h���Đ�
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // �v���C���[���G���A����o���Ƃ��ɃT�E���h���~
        if (collider.gameObject.tag == "Player")
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop(); // �T�E���h���~
            }
        }
    }
}
