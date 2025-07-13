using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject HPBar;
    public string playerTag = "Player";  // プレイヤーのタグを指定

    private Collider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
        {
            Debug.LogError("このオブジェクトにはColliderコンポーネントが必要です。");
        }
    }

    void Update()
    {
        if (triggerCollider != null)
        {
            GameObject player = GameObject.FindWithTag(playerTag);
            if (player != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();
                if (playerCollider != null && triggerCollider.bounds.Intersects(playerCollider.bounds))
                {
                    // プレイヤーが触れたと判断された場合の処理
                    targetObject.SetActive(!targetObject.activeSelf);
                    HPBar.SetActive(!HPBar.activeSelf);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
