using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    // 任意の初期化処理
    public void Initialize()
    {
        Debug.Log(gameObject.name + " が初期化されました。");
        // ここにスクリプトの初期化ロジックを追加します。
    }
}

