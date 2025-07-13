using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private InputActionProperty jumpButton; // �W�����v�{�^���̃A�N�V����
    [SerializeField] private float jumpHeight = 3f;          // �W�����v�̍���
    [SerializeField] private CharacterController cc;         // CharacterController�̎Q��
    [SerializeField] private LayerMask groundLayers;         // �n�ʃ��C���[

    private float gravity = Physics.gravity.y;               // �d��
    private Vector3 movement;                                // �ړ��x�N�g��

    private void Update()
    {
        // �n�ʂɐڐG���Ă��邩�ǂ������m�F
        bool isGrounded = IsGrounded();

        // �n�ʂɐڐG���Ă��ăW�����v�{�^���������ꂽ��W�����v
        if (jumpButton.action.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }

        movement.y += gravity * Time.deltaTime;

        // CharacterController�ňړ�
        cc.Move(movement * Time.deltaTime);
    }

    // �W�����v����
    private void Jump()
    {
        movement.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
    }

    // �n�ʔ���
    private bool IsGrounded()
    {
        // SphereCast�܂���CheckSphere�Œn�ʂ��m�F
        return Physics.CheckSphere(transform.position, 0.2f, groundLayers);
    }
}
