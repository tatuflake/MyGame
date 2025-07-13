using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UIを使用するために必要
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkitを使用

public class RifleController : MonoBehaviour
{
    public AudioClip sound1;         // 発砲音
    public AudioClip reloadSound;    // リロード音
    private AudioSource audioSource;

    [SerializeField]
    private GameObject m_bulletPrefab = null;
    [SerializeField]
    private Transform m_muzzlePos = null;
    [SerializeField]
    private float bulletSpeed = 1000f;
    [SerializeField]
    private float fireRate = 0.1f;

    private int maxAmmo = 30;
    private int currentAmmo;
    private bool isReloading = false;
    private Coroutine firingCoroutine;

    [SerializeField]
    private Text ammoText;

    // 新しいフィールド
    [SerializeField]
    private Light muzzleFlashLight;
    [SerializeField]
    private float muzzleFlashDuration = 0.05f;
    [SerializeField]
    private GameObject shellPrefab;
    [SerializeField]
    private Transform shellEjectPoint;
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
        audioSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;

        // XRGrabInteractableコンポーネントを取得
        grabInteractable = GetComponent<XRGrabInteractable>();

        // イベントリスナーを設定
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        UpdateAmmoText();
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        IsHeld = true; // プレイヤーに持たれている状態
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        IsHeld = false; // プレイヤーに離された状態
    }


    public void Activate()
    {

        if (!isReloading && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
    }

    public void Deactivate()
    {

        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }

        if (!isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            if (currentAmmo > 0)
            {
                ShootAmmo();
                currentAmmo--;
                UpdateAmmoText();
            }
            else
            {
                yield break;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void ShootAmmo()
    {
        if (m_bulletPrefab == null || m_muzzlePos == null)
        {
            Debug.Log("Inspector の設定が間違ってるで");
            return;
        }

        GameObject bulletObj = Instantiate(m_bulletPrefab, m_muzzlePos.position, m_muzzlePos.rotation);
        Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(m_muzzlePos.forward * bulletSpeed);
        }

        audioSource.PlayOneShot(sound1);

        StartCoroutine(FlashMuzzle());
        TriggerHaptics();

        if (shellPrefab != null && shellEjectPoint != null)
        {
            GameObject shell = Instantiate(shellPrefab, shellEjectPoint.position, shellEjectPoint.rotation);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();
            if (shellRb != null)
            {
                shellRb.AddForce(shellEjectPoint.right * shellEjectForce);
            }
            Destroy(shell, 5f);
        }

        Destroy(bulletObj, 5f);
    }

    private IEnumerator FlashMuzzle()
    {
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(muzzleFlashDuration);
        muzzleFlashLight.enabled = false;
    }

    private void TriggerHaptics()
    {
        if (leftController != null)
        {
            leftController.SendHapticImpulse(0.5f, 0.1f);
        }

        if (rightController != null)
        {
            rightController.SendHapticImpulse(0.5f, 0.1f);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("リロード中...");
        ammoText.color = Color.red;
        audioSource.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(2f);
        currentAmmo = maxAmmo;
        isReloading = false;
        ammoText.color = Color.white;
        UpdateAmmoText();
        Debug.Log("リロード完了");
    }

    private void UpdateAmmoText()
    {
        ammoText.text = currentAmmo.ToString();
    }
}
