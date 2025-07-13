using UnityEngine;
using UnityEngine.InputSystem; // Input Systemを使用
using UnityEngine.XR.Interaction.Toolkit;

public class DisableControllerStick : MonoBehaviour
{
    [SerializeField]
    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private InputActionReference moveAction; // スティック動作用のInputAction
    [SerializeField]
    private InputActionReference rotateAction; // 回転動作用のInputAction

    void Start()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        // イベントリスナーの登録
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        // イベントリスナーの解除
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // オブジェクトが持たれたときにスティック動作を無効にする
    private void OnGrab(SelectEnterEventArgs args)
    {
        DisableStickInput();
    }

    // オブジェクトが離されたときにスティック動作を有効にする
    private void OnRelease(SelectExitEventArgs args)
    {
        EnableStickInput();
    }

    // スティックの入力を無効化
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

    // スティックの入力を有効化
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
