using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateViewWithRightStick : MonoBehaviour
{
    // 回転速度
    public float rotationSpeed = 60f;

    // Input Action (右スティックの入力を受け取る)
    [SerializeField]
    private InputActionReference rightStickAction;

    // カメラまたはプレイヤーを回転させるオブジェクト
    [SerializeField]
    private Transform playerCameraRig;

    void Update()
    {
        // 右スティックの入力を取得
        Vector2 rightStickInput = rightStickAction.action.ReadValue<Vector2>();

        // スティックの横方向（X軸）の入力に基づいてカメラを回転
        if (rightStickInput.x != 0)
        {
            RotateView(rightStickInput.x);
        }
    }

    // 視点を回転させる処理
    private void RotateView(float input)
    {
        // 回転量を計算
        float rotationAmount = input * rotationSpeed * Time.deltaTime;

        // プレイヤーカメラの親オブジェクト（リグ）を回転
        playerCameraRig.Rotate(0, rotationAmount, 0);
    }
}
