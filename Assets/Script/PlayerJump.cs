using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private InputActionProperty jumpButton; // ジャンプボタンのアクション
    [SerializeField] private float jumpHeight = 3f;          // ジャンプの高さ
    [SerializeField] private CharacterController cc;         // CharacterControllerの参照
    [SerializeField] private LayerMask groundLayers;         // 地面レイヤー

    private float gravity = Physics.gravity.y;               // 重力
    private Vector3 movement;                                // 移動ベクトル

    private void Update()
    {
        // 地面に接触しているかどうかを確認
        bool isGrounded = IsGrounded();

        // 地面に接触していてジャンプボタンが押されたらジャンプ
        if (jumpButton.action.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }

        movement.y += gravity * Time.deltaTime;

        // CharacterControllerで移動
        cc.Move(movement * Time.deltaTime);
    }

    // ジャンプ処理
    private void Jump()
    {
        movement.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
    }

    // 地面判定
    private bool IsGrounded()
    {
        // SphereCastまたはCheckSphereで地面を確認
        return Physics.CheckSphere(transform.position, 0.2f, groundLayers);
    }
}
