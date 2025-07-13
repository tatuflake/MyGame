using System.Collections;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public AudioClip spawnSound;    // オブジェクトが発生した時のサウンド
    public AudioClip impactSound;   // オブジェクトが刺さった時のサウンド
    public GameObject bulletPrefab; // 弾のプレハブ
    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isFixed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // オブジェクトが発生した時のサウンドを再生
        audioSource.PlayOneShot(spawnSound);

        // 重力を適用して真下に落下させる
        rb.useGravity = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isFixed)
        {
            // オブジェクトが何かに衝突した時に音を再生
            audioSource.PlayOneShot(impactSound);

            // オブジェクトを固定
            rb.isKinematic = true; // 物理演算を無効にして固定
            isFixed = true;

            // 2秒後にオブジェクトを消失させる
            StartCoroutine(DestroyAfterDelay(2f));
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 弾を四方八方に発射する処理
        FireBulletsInAllDirections();

        // オブジェクトを消失させる
        Destroy(gameObject);
    }

    void FireBulletsInAllDirections()
    {
        int bulletCount = 10;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * Vector3.forward;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = direction * 10f; // 弾の速度を設定

            // 0.1秒後に弾を消失させる
            Destroy(bullet, 0.1f);
        }
    }
}
