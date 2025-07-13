using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float approachDistance = 10f;  // プレイヤーに近づく距離
    public float keepDistance = 3f;       // プレイヤーから離れる距離
    public float moveSpeed = 5f;          // 移動速度
    public float dashSpeed = 10f;         // 高速移動の速度
    public GameObject bulletPrefab;       // 弾のプレハブ
    public Transform bulletSpawnPoint;    // 弾の生成位置
    public GameObject fallingObjectPrefab; // ダメージを与えるオブジェクトのプレハブ
    public float fallingObjectRadius = 5f; // ダメージオブジェクトが降る範囲
    public float fallingObjectHeight = 10f; // ダメージオブジェクトの初期生成高さ

    public AudioClip chargeSound;         // 突進時のサウンド
    public AudioClip bulletSound;         // 弾を撃つ時のサウンド
    public AudioClip fallingObjectSound;  // ダメージオブジェクト発生時のサウンド
    private AudioSource audioSource;

    private bool isAttacking = false;
    private bool isDashing = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SideDashRoutine()); // 左右ダッシュのルーチンを開始
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // プレイヤーの方向を向く
        Vector3 lookDirection = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * moveSpeed);

        // プレイヤーが近すぎる場合は距離を取る
        if (distanceToPlayer < keepDistance && !isDashing)
        {
            Vector3 direction = transform.position - player.position;
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
        // プレイヤーが一定距離に近づいた場合は近づく
        else if (distanceToPlayer < approachDistance && !isDashing)
        {
            Vector3 direction = player.position - transform.position;
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }

        // 攻撃していない場合は攻撃を開始
        if (!isAttacking && distanceToPlayer < approachDistance)
        {
            StartCoroutine(AttackPattern());
        }
    }

    IEnumerator AttackPattern()
    {
        isAttacking = true;

        // ランダムに攻撃パターンを選択
        int attackChoice = Random.Range(1, 4);

        switch (attackChoice)
        {
            case 1:
                // 突進
                StartCoroutine(ChargeAttack());
                break;
            case 2:
                // 3段階の扇状の弾を発射
                StartCoroutine(ThreePhaseBulletAttack());
                break;
            case 3:
                // ランダムにダメージオブジェクトを降らせる
                StartCoroutine(RandomFallingObjects());
                break;
        }

        yield return new WaitForSeconds(2f); // 攻撃間隔
        isAttacking = false;
    }

    IEnumerator ChargeAttack()
    {
        // サウンドを再生
        audioSource.PlayOneShot(chargeSound);

        Vector3 chargeDirection = (player.position - transform.position).normalized;
        float chargeSpeed = moveSpeed * 6f; // 突進速度を調整
        float chargeDuration = 1f;

        for (float t = 0; t < chargeDuration; t += Time.deltaTime)
        {
            transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ThreePhaseBulletAttack()
    {
        // サウンドを再生
        audioSource.PlayOneShot(bulletSound);

        // 3段階で弾を発射する
        for (int i = 0; i < 3; i++)
        {
            float angleOffset = 0f;

            // 1回目は45度、2回目は-45度、3回目は水平
            if (i == 0)
            {
                angleOffset = 45f;
            }
            else if (i == 1)
            {
                angleOffset = -45f;
            }
            else if (i == 2)
            {
                angleOffset = 0f;
            }

            // 10発の弾を120度の扇状に発射
            float startAngle = angleOffset - 60f; // 120度の扇の開始角度
            float angleStep = 120f / 9f; // 9個のステップで10発の弾を発射

            for (int j = 0; j < 10; j++)
            {
                float angle = startAngle + (j * angleStep);
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Vector3 direction = rotation * transform.forward;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = direction * 10f;
            }

            // 0.5秒間隔を設ける
            yield return new WaitForSeconds(0.5f);
        }
    }




    IEnumerator RandomFallingObjects()
    {
        // サウンドを再生
        audioSource.PlayOneShot(fallingObjectSound);

        int objectCount = 10; // 降らせるオブジェクトの数

        for (int i = 0; i < objectCount; i++)
        {
            // ランダムな位置を決定
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-fallingObjectRadius, fallingObjectRadius), fallingObjectHeight, Random.Range(-fallingObjectRadius, fallingObjectRadius));

            // ダメージオブジェクトを生成
            GameObject fallingObject = Instantiate(fallingObjectPrefab, randomPosition, Quaternion.identity);

            // オブジェクトが刺さった時に固定される処理は FallingObject スクリプト内にある
            yield return new WaitForSeconds(0.1f); // 次のオブジェクトを降らせる間隔
        }
    }

    IEnumerator SideDashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f); // 8秒待機

            isDashing = true;
            Vector3 dashDirection = (Random.value > 0.5f) ? transform.right : -transform.right;

            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                transform.position += dashDirection * dashSpeed * Time.deltaTime;
                yield return null;
            }

            isDashing = false;
        }
    }
}
