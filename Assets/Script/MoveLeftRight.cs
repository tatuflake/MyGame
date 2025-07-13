using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    /// <summary>
    /// 左右に動く範囲の最大距離
    /// </summary>
    [SerializeField]
    private float moveDistance = 5.0f;

    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2.0f;

    /// <summary>
    /// オブジェクトの初期位置
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// 現在の移動方向（1: 右, -1: 左）
    /// </summary>
    [SerializeField]
    private int moveDirection = 1;

    void Start()
    {
        // オブジェクトの初期位置を保存
        startPosition = transform.position;
    }

    void Update()
    {
        // 新しい位置を計算
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance) * moveDirection;
        transform.position = startPosition + new Vector3(offset, 0, 0);

        // 移動が最大範囲に達したら方向を反転
        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            moveDirection *= -1;
        }
    }
}
