using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTrail : MonoBehaviour
{
    public GameObject trailPrefab;  // プレハブをアサイン
    public float spawnInterval = 0.1f;  // コピーを生成する間隔（秒）
    public float trailLifetime = 2.0f;  // 各コピーが存在する時間（秒）
    public float alpha = 0.5f;  // 半透明度（0.0～1.0）

    private float timeSinceLastSpawn;

    void Update()
    {
        // 前回の生成からの時間を計測
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            // コピーを生成
            GameObject trailObject = Instantiate(trailPrefab, transform.position, transform.rotation);

            // コピーの透明度を設定
            SetAlpha(trailObject, alpha);

            // trailLifetime後にコピーを破壊
            Destroy(trailObject, trailLifetime);

            // タイマーをリセット
            timeSinceLastSpawn = 0f;
        }
    }

    // オブジェクトの透明度を設定するヘルパーメソッド
    void SetAlpha(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
        }
    }
}
