using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkitを使用

public class SniperRifleController : MonoBehaviour
{
    public AudioClip sound1;         // 発砲音
    public AudioClip reloadSound;    // リロード音
    private AudioSource audioSource;

    /// <summary>
    /// 銃弾のプレハブ。
    /// 発砲した際に、このオブジェクトを弾として実体化する。
    /// </summary>
    [SerializeField]
    private GameObject m_bulletPrefab = null;

    /// <summary>
    /// 銃口の位置。
    /// 銃弾を実体化する時の位置や向きの設定などに使用する。
    /// </summary>
    [SerializeField]
    private Transform m_muzzlePos = null;

    /// <summary>
    /// 銃弾の初速度。
    /// </summary>
    [SerializeField]
    private float bulletSpeed = 1000f;

    /// <summary>
    /// 弾の発射間隔
    /// </summary>
    [SerializeField]
    private float fireRate = 1f; // スナイパーライフルのため、発射間隔を長めに設定

    /// <summary>
    /// マガジンの弾数
    /// </summary>
    private int maxAmmo = 5; // 一度に発射できる弾数
    private int currentAmmo;
    private int totalAmmo = 50; // 総弾数
    private bool isReloading = false;
    private Coroutine firingCoroutine; // 発射コルーチンの参照を保存する

    /// <summary>
    /// 弾数表示のテキストUI
    /// </summary>
    [SerializeField]
    private Text ammoText; // UI Textへの参照

    /// <summary>
    /// 銃口の発光ライト。
    /// </summary>
    [SerializeField]
    private Light muzzleFlashLight;

    /// <summary>
    /// 銃口が光る時間
    /// </summary>
    [SerializeField]
    private float muzzleFlashDuration = 0.05f;

    /// <summary>
    /// 薬莢のプレハブ。
    /// </summary>
    [SerializeField]
    private GameObject shellPrefab;

    /// <summary>
    /// 薬莢の排出口の位置。
    /// </summary>
    [SerializeField]
    private Transform shellEjectPoint;

    /// <summary>
    /// 薬莢の排出速度。
    /// </summary>
    [SerializeField]
    private float shellEjectForce = 150f;

    [SerializeField]
    private XRBaseController leftController;

    [SerializeField]
    private XRBaseController rightController;

    // XRGrabInteractable の参照
    private XRGrabInteractable grabInteractable;

    // リロード中かどうかを外部から参照できるようにプロパティを追加
    public bool IsReloading => isReloading;

    // プレイヤーに持たれているかどうかを示すプロパティ
    public bool IsHeld { get; private set; } = false;

    void Start()
    {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo; // 初期弾数を設定

        // XRGrabInteractableコンポーネントを取得
        grabInteractable = GetComponent<XRGrabInteractable>();

        // イベントリスナーを設定
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        UpdateAmmoText(); // 初期の弾数を表示
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        IsHeld = true; // プレイヤーに持たれている状態
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        IsHeld = false; // プレイヤーに離された状態
    }

    /// <summary>
    /// VRコントローラーのトリガーが握られた時に呼び出す。
    /// </summary>
    public void Activate()
    {
        if (!isReloading && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
    }

    /// <summary>
    /// VRコントローラーのトリガーが離された時に呼び出す。
    /// </summary>
    public void Deactivate()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null; // コルーチンが停止したことを示すためにnullにする
        }

        if (!isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    /// <summary>
    /// 弾を連続で発射するコルーチン。
    /// </summary>
    private IEnumerator FireContinuously()
    {
        while (true)
        {
            if (currentAmmo > 0 && totalAmmo >= 10) // 弾が5発以上残っている場合のみ発射
            {
                for (int i = 0; i < 10; i++) // 10発同時に発射
                {
                    ShootAmmo();
                }

                // 発砲音を再生
                audioSource.PlayOneShot(sound1);

                // 銃口の発光エフェクトを再生
                StartCoroutine(FlashMuzzle());

                // 薬莢を生成して排出
                if (shellPrefab != null && shellEjectPoint != null)
                {
                    GameObject shell = Instantiate(shellPrefab, shellEjectPoint.position, shellEjectPoint.rotation);
                    Rigidbody shellRb = shell.GetComponent<Rigidbody>();
                    if (shellRb != null)
                    {
                        shellRb.AddForce(shellEjectPoint.right * shellEjectForce);
                    }
                    Destroy(shell, 5f); // 一定時間後に薬莢を破棄
                }

                currentAmmo--;
                totalAmmo -= 10; // 総弾数を減らす
                UpdateAmmoText(); // 弾数を更新
            }
            else
            {
                yield break; // 弾がない場合は発射を停止
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    /// <summary>
    /// 銃弾を生成する。
    /// </summary>
    private void ShootAmmo()
    {
        if (m_bulletPrefab == null || m_muzzlePos == null)
        {
            Debug.Log(" Inspector の設定が間違ってるで");
            return;
        }

        // 弾丸を生成
        GameObject bulletObj = Instantiate(m_bulletPrefab, m_muzzlePos.position, m_muzzlePos.rotation);

        Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(m_muzzlePos.forward * bulletSpeed);
        }
        else
        {
            Debug.LogWarning("弾丸オブジェクトにRigidbodyがありません");
        }

        // 発砲音を再生
        audioSource.PlayOneShot(sound1);


        // コントローラーを振動させる
        TriggerHaptics();


        // 一定時間後に弾丸を破棄する
        Destroy(bulletObj, 5f);
    }

    /// <summary>
    /// 銃口が光るエフェクトを再生する。
    /// </summary>
    private IEnumerator FlashMuzzle()
    {
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(muzzleFlashDuration);
        muzzleFlashLight.enabled = false;
    }

    /// <summary>
    /// コントローラーの振動を発生させる。
    /// </summary>
    private void TriggerHaptics()
    {
        if (leftController != null)
        {
            leftController.SendHapticImpulse(1.0f, 0.2f); // 強度 0.5, 0.1秒の振動
        }

        if (rightController != null)
        {
            rightController.SendHapticImpulse(1.0f, 0.2f); // 強度 0.5, 0.1秒の振動
        }
    }

    /// <summary>
    /// リロード処理を行う。
    /// </summary>
    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("リロード中...");
        ammoText.color = Color.red; // リロード中はテキストを赤に
        audioSource.PlayOneShot(reloadSound);

        // リロード時間を待機 (例: 2秒)
        yield return new WaitForSeconds(2f);

        currentAmmo = maxAmmo;
        totalAmmo = 50;
        isReloading = false;
        firingCoroutine = null; // リロード後に発射を可能にするためにコルーチンをリセット
        ammoText.color = Color.white; // リロードが完了したらテキストの色を元に戻す
        UpdateAmmoText(); // リロード後の弾数を表示
        Debug.Log("リロード完了");
    }

    /// <summary>
    /// 残弾数の表示を更新する。
    /// </summary>
    private void UpdateAmmoText()
    {
        ammoText.text = $"{currentAmmo}";
    }
}
