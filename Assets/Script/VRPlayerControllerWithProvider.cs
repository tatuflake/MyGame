using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerControllerWithDash : MonoBehaviour
{
    public float normalSpeed = 2.0f;   // 通常の移動速度
    public float dashSpeed = 6.0f;     // ダッシュ時の速度

    private Rigidbody playerRigidbody;
    private ActionBasedContinuousMoveProvider moveProvider;
    private bool isDashing = false;

    // ダッシュアクション (右コントローラーAボタン)
    public InputActionProperty dashAction; // Aボタンに割り当て

    private void Start()
    {
        // RigidbodyとMoveProviderを取得
        playerRigidbody = GetComponent<Rigidbody>();
        moveProvider = GetComponent<ActionBasedContinuousMoveProvider>();

        // 初期の移動速度を設定
        moveProvider.moveSpeed = normalSpeed;
    }

    private void Update()
    {
        // Aボタンでダッシュ
        if (dashAction.action.WasPressedThisFrame())
        {
            isDashing = true;
            moveProvider.moveSpeed = dashSpeed;  // ダッシュ速度に変更
        }
        else if (dashAction.action.WasReleasedThisFrame())
        {
            isDashing = false;
            moveProvider.moveSpeed = normalSpeed; // 通常速度に戻す
        }
    }
}
