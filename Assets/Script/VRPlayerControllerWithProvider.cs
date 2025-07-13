using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerControllerWithDash : MonoBehaviour
{
    public float normalSpeed = 2.0f;   // �ʏ�̈ړ����x
    public float dashSpeed = 6.0f;     // �_�b�V�����̑��x

    private Rigidbody playerRigidbody;
    private ActionBasedContinuousMoveProvider moveProvider;
    private bool isDashing = false;

    // �_�b�V���A�N�V���� (�E�R���g���[���[A�{�^��)
    public InputActionProperty dashAction; // A�{�^���Ɋ��蓖��

    private void Start()
    {
        // Rigidbody��MoveProvider���擾
        playerRigidbody = GetComponent<Rigidbody>();
        moveProvider = GetComponent<ActionBasedContinuousMoveProvider>();

        // �����̈ړ����x��ݒ�
        moveProvider.moveSpeed = normalSpeed;
    }

    private void Update()
    {
        // A�{�^���Ń_�b�V��
        if (dashAction.action.WasPressedThisFrame())
        {
            isDashing = true;
            moveProvider.moveSpeed = dashSpeed;  // �_�b�V�����x�ɕύX
        }
        else if (dashAction.action.WasReleasedThisFrame())
        {
            isDashing = false;
            moveProvider.moveSpeed = normalSpeed; // �ʏ푬�x�ɖ߂�
        }
    }
}
