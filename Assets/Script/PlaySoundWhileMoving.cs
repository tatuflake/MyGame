using UnityEngine;

public class PlaySoundWhileMoving : MonoBehaviour
{
    /// <summary>
    /// �T�E���h���Đ����邽�߂�AudioSource�R���|�[�l���g
    /// </summary>
    [SerializeField]
    private AudioSource audioSource;

    /// <summary>
    /// �v���C���[�̑O��t���[���̈ʒu
    /// </summary>
    private Vector3 lastPosition;

    /// <summary>
    /// �v���C���[�̈ړ����o�̂��߂�臒l
    /// </summary>
    [SerializeField]
    private float movementThreshold = 0.01f;

    void Start()
    {
        // �ŏ��̈ʒu���L��
        lastPosition = transform.position;

        // �T�E���h���ŏ��͒�~���Ă���
        if (audioSource != null)
        {
            audioSource.loop = true; // �T�E���h�����[�v�Đ��ɐݒ�
            audioSource.Stop(); // �T�E���h���~
        }
    }

    void Update()
    {
        // ���݂̈ʒu�ƑO�t���[���̈ʒu���r
        Vector3 currentPosition = transform.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

        // �v���C���[���ړ����Ă��邩�ǂ������`�F�b�N
        if (audioSource != null)
        {
            if (distanceMoved > movementThreshold) // 臒l�𒴂���ړ�������Ή����Đ�
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play(); // �T�E���h���Đ�
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop(); // �T�E���h���~
                }
            }
        }

        // ���݂̈ʒu�����̃t���[���̂��߂ɕۑ�
        lastPosition = currentPosition;
    }
}
