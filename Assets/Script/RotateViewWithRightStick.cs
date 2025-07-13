using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateViewWithRightStick : MonoBehaviour
{
    // ��]���x
    public float rotationSpeed = 60f;

    // Input Action (�E�X�e�B�b�N�̓��͂��󂯎��)
    [SerializeField]
    private InputActionReference rightStickAction;

    // �J�����܂��̓v���C���[����]������I�u�W�F�N�g
    [SerializeField]
    private Transform playerCameraRig;

    void Update()
    {
        // �E�X�e�B�b�N�̓��͂��擾
        Vector2 rightStickInput = rightStickAction.action.ReadValue<Vector2>();

        // �X�e�B�b�N�̉������iX���j�̓��͂Ɋ�Â��ăJ��������]
        if (rightStickInput.x != 0)
        {
            RotateView(rightStickInput.x);
        }
    }

    // ���_����]�����鏈��
    private void RotateView(float input)
    {
        // ��]�ʂ��v�Z
        float rotationAmount = input * rotationSpeed * Time.deltaTime;

        // �v���C���[�J�����̐e�I�u�W�F�N�g�i���O�j����]
        playerCameraRig.Rotate(0, rotationAmount, 0);
    }
}
