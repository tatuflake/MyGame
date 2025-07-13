using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class ItemScaleRecover : MonoBehaviour
{
    // ゲームオブジェクトのスケールを保持する変数
    private Vector3 m_defalutScale;

    // Start is called before the first frame update
    private void Awake()
    {
        // シーン起動時のゲームオブジェクトのスケールを保持する
        m_defalutScale = transform.localScale;
    }

    public void RecoverScale()
    {
        // 小型ソケットから取り出したときにゲームオブジェクトのスケールを元に戻す
        transform.localScale = m_defalutScale;
    }
}