using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnGunHit : MonoBehaviour
{
    /// <summary>
    /// 表示させるテキストのUI要素
    /// </summary>
    [SerializeField]
    private Text displayText;

    void Start()
    {
        // テキストを最初は非表示にしておく
        if (displayText != null)
        {
            displayText.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // 衝突したオブジェクトがGunタグを持っているか確認
        if (collider.gameObject.tag == "Gun")
        {
            // テキストを表示する
            if (displayText != null)
            {
                displayText.enabled = true;
                StartCoroutine(HideTextAfterDelay(1.0f)); // 1秒後にテキストを非表示にする
            }
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        // 指定した時間だけ待機
        yield return new WaitForSeconds(delay);

        // テキストを非表示にする
        if (displayText != null)
        {
            displayText.enabled = false;
        }
    }
}
