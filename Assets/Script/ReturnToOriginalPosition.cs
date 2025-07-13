using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ReturnToOriginalPosition : MonoBehaviour
{
    // �Ǐ]����I�u�W�F�N�g
    [SerializeField]
    private GameObject followTarget;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    // �I�u�W�F�N�g���߂�܂ł̎���
    [SerializeField]
    private float returnDuration = 1f;

    void Start()
    {
        // XRGrabInteractable �R���|�[�l���g���擾
        grabInteractable = GetComponent<XRGrabInteractable>();

        // �I�u�W�F�N�g�������ꂽ�Ƃ��Ɨ����ꂽ�Ƃ��̃C�x���g��o�^
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        // �C�x���g���X�i�[�̉���
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // �I�u�W�F�N�g�������ꂽ�Ƃ��̏���
    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true; // �v���C���[�Ɏ�����Ă����Ԃɂ���
        StopAllCoroutines(); // �Ǐ]��߂鏈�����~
    }

    // �I�u�W�F�N�g�������ꂽ�Ƃ��̏���
    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false; // �v���C���[�Ɏ�����Ă��Ȃ���Ԃɂ���
        StartCoroutine(FollowTarget()); // �Ǐ]���J�n
    }

    // �Ǐ]����R���[�`��
    private IEnumerator FollowTarget()
    {
        while (!isHeld)
        {
            // �Ǐ]��̃I�u�W�F�N�g���ݒ肳��Ă���΂��̈ʒu�Ɖ�]�ɒǏ]����
            if (followTarget != null)
            {
                // ���݂̈ʒu�Ɖ�]���Ԃ��ĒǏ]����
                transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime / returnDuration);
                transform.rotation = Quaternion.Lerp(transform.rotation, followTarget.transform.rotation, Time.deltaTime / returnDuration);
            }

            yield return null; // ���̃t���[���܂őҋ@
        }
    }
}
