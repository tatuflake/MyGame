using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ReturnToOriginalPosition : MonoBehaviour
{
    // 追従するオブジェクト
    [SerializeField]
    private GameObject followTarget;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    // オブジェクトが戻るまでの時間
    [SerializeField]
    private float returnDuration = 1f;

    void Start()
    {
        // XRGrabInteractable コンポーネントを取得
        grabInteractable = GetComponent<XRGrabInteractable>();

        // オブジェクトが持たれたときと離されたときのイベントを登録
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        // イベントリスナーの解除
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // オブジェクトが持たれたときの処理
    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true; // プレイヤーに持たれている状態にする
        StopAllCoroutines(); // 追従や戻る処理を停止
    }

    // オブジェクトが離されたときの処理
    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false; // プレイヤーに持たれていない状態にする
        StartCoroutine(FollowTarget()); // 追従を開始
    }

    // 追従するコルーチン
    private IEnumerator FollowTarget()
    {
        while (!isHeld)
        {
            // 追従先のオブジェクトが設定されていればその位置と回転に追従する
            if (followTarget != null)
            {
                // 現在の位置と回転を補間して追従する
                transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime / returnDuration);
                transform.rotation = Quaternion.Lerp(transform.rotation, followTarget.transform.rotation, Time.deltaTime / returnDuration);
            }

            yield return null; // 次のフレームまで待機
        }
    }
}
