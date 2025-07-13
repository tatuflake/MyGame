using UnityEngine;
using UnityEngine.InputSystem; // Input System���g�p
using UnityEngine.XR.Interaction.Toolkit;

public class DisableControllerStick : MonoBehaviour
{
    [SerializeField]
    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private InputActionReference moveAction; // �X�e�B�b�N����p��InputAction
    [SerializeField]
    private InputActionReference rotateAction; // ��]����p��InputAction

    void Start()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        // �C�x���g���X�i�[�̓o�^
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        // �C�x���g���X�i�[�̉���
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // �I�u�W�F�N�g�������ꂽ�Ƃ��ɃX�e�B�b�N����𖳌��ɂ���
    private void OnGrab(SelectEnterEventArgs args)
    {
        DisableStickInput();
    }

    // �I�u�W�F�N�g�������ꂽ�Ƃ��ɃX�e�B�b�N�����L���ɂ���
    private void OnRelease(SelectExitEventArgs args)
    {
        EnableStickInput();
    }

    // �X�e�B�b�N�̓��͂𖳌���
    private void DisableStickInput()
    {
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }

        if (rotateAction != null)
        {
            rotateAction.action.Disable();
        }
    }

    // �X�e�B�b�N�̓��͂�L����
    private void EnableStickInput()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }

        if (rotateAction != null)
        {
            rotateAction.action.Enable();
        }
    }
}
