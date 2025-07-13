using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunController : MonoBehaviour
{
    public AudioClip sound1;         // ���C��
    public AudioClip reloadSound;    // �����[�h��
    private AudioSource audioSource;

    /// <summary>
    /// �e�e�̃v���n�u�B
    /// ���C�����ۂɁA���̃I�u�W�F�N�g��e�Ƃ��Ď��̉�����B
    /// </summary>
    [SerializeField]
    private GameObject m_bulletPrefab = null;

    /// <summary>
    /// �e���̈ʒu�B
    /// �e�e�����̉����鎞�̈ʒu������̐ݒ�ȂǂɎg�p����B
    /// </summary>
    [SerializeField]
    private Transform m_muzzlePos = null;

    /// <summary>
    /// �e�e�̏����x�B
    /// </summary>
    [SerializeField]
    private float bulletSpeed = 1000f;

    /// <summary>
    /// �U�e�̐�
    /// </summary>
    [SerializeField]
    private int pelletCount = 10;

    /// <summary>
    /// �U�e�̔��ˊp�x�͈̔́i�L����j
    /// </summary>
    [SerializeField]
    private float spreadAngle = 10f;

    /// <summary>
    /// �e�̔��ˊԊu
    /// </summary>
    [SerializeField]
    private float fireRate = 0.5f; // �V���b�g�K���̂��߁A���ˊԊu�𒷂߂ɐݒ�

    /// <summary>
    /// �}�K�W���̒e��
    /// </summary>
    private int maxAmmo = 8; // �V���b�g�K���̒e�������Ȃ߂ɐݒ�
    private int currentAmmo;
    private bool isReloading = false;
    private Coroutine firingCoroutine;

    /// <summary>
    /// �e���\���̃e�L�X�gUI
    /// </summary>
    [SerializeField]
    private Text ammoText; // UI Text�ւ̎Q��

    /// <summary>
    /// �e���̔������C�g�B
    /// </summary>
    [SerializeField]
    private Light muzzleFlashLight;

    /// <summary>
    /// �e�������鎞��
    /// </summary>
    [SerializeField]
    private float muzzleFlashDuration = 0.05f;

    /// <summary>
    /// ��䰂̃v���n�u�B
    /// </summary>
    [SerializeField]
    private GameObject shellPrefab;

    /// <summary>
    /// ��䰂̔r�o���̈ʒu�B
    /// </summary>
    [SerializeField]
    private Transform shellEjectPoint;

    /// <summary>
    /// ��䰂̔r�o���x�B
    /// </summary>
    [SerializeField]
    private float shellEjectForce = 150f;

    [SerializeField]
    private XRBaseController leftController;

    [SerializeField]
    private XRBaseController rightController;

    // XRGrabInteractable �̎Q��
    private XRGrabInteractable grabInteractable;

    // �����[�h�����ǂ������O������Q�Ƃł���悤�Ƀv���p�e�B��ǉ�
    public bool IsReloading => isReloading;

    // �v���C���[�Ɏ�����Ă��邩�ǂ����������v���p�e�B
    public bool IsHeld { get; private set; } = false;

    void Start()
    {
        //Component���擾
        audioSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo; // �����e����ݒ�

        // XRGrabInteractable�R���|�[�l���g���擾
        grabInteractable = GetComponent<XRGrabInteractable>();

        // �C�x���g���X�i�[��ݒ�
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        UpdateAmmoText(); // �����̒e����\��
    }


    public void OnGrab(SelectEnterEventArgs args)
    {
        IsHeld = true; // �v���C���[�Ɏ�����Ă�����
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        IsHeld = false; // �v���C���[�ɗ����ꂽ���
    }

    /// <summary>
    /// VR�R���g���[���[�̃g���K�[������ꂽ���ɌĂяo���B
    /// </summary>
    public void Activate()
    {
        if (!isReloading && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
    }

    /// <summary>
    /// VR�R���g���[���[�̃g���K�[�������ꂽ���ɌĂяo���B
    /// </summary>
    public void Deactivate()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null; // �R���[�`������~�������Ƃ��������߂�null�ɂ���
        }

        if (!isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    /// <summary>
    /// �e��A���Ŕ��˂���R���[�`���B
    /// </summary>
    private IEnumerator FireContinuously()
    {
        while (true)
        {
            if (currentAmmo > 0)
            {
                ShootPellets(); // �V���b�g�K���̎U�e�𔭎�
                currentAmmo--;
                UpdateAmmoText(); // �e�����X�V
            }
            else
            {
                yield break; // �e���Ȃ��ꍇ�͔��˂��~
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    /// <summary>
    /// �U�e�𔭎˂���B
    /// </summary>
    private void ShootPellets()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            // �e�ۂ𐶐�
            GameObject bulletObj = Instantiate(m_bulletPrefab);

            // �e�̈ʒu���A�e���̈ʒu�Ɠ���ɂ���B
            bulletObj.transform.position = m_muzzlePos.position;

            // �����_���Ȋp�x�̕΍��𐶐��i���E����я㉺�̕΍��j
            float randomYaw = Random.Range(-spreadAngle, spreadAngle); // ���E�����̊p�x
            float randomPitch = Random.Range(-spreadAngle, spreadAngle); // �㉺�����̊p�x
            Quaternion rotation = Quaternion.Euler(randomPitch, randomYaw, 0);

            // �e�̌������A�e���̌����Ƀ����_���ȕ΍������������̂ɂ���B
            bulletObj.transform.rotation = m_muzzlePos.rotation * rotation;

            // �e��Rigidbody��t�����āA�O���ɗ͂�������
            Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(bulletObj.transform.forward * bulletSpeed);
            }
            else
            {
                Debug.LogWarning("�e�ۃI�u�W�F�N�g��Rigidbody������܂���");
            }

            // ��莞�Ԍ�ɒe�ۂ�j������
            Destroy(bulletObj, 2f);
        }

        // ���C�����Đ�
        audioSource.PlayOneShot(sound1);

        // �e���̔����G�t�F�N�g���Đ�
        StartCoroutine(FlashMuzzle());

        // �R���g���[���[��U��������
        TriggerHaptics();

        // ��䰂𐶐����Ĕr�o
        if (shellPrefab != null && shellEjectPoint != null)
        {
            GameObject shell = Instantiate(shellPrefab, shellEjectPoint.position, shellEjectPoint.rotation);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();
            if (shellRb != null)
            {
                shellRb.AddForce(shellEjectPoint.right * shellEjectForce);
            }
            Destroy(shell, 5f); // ��莞�Ԍ�ɖ�䰂�j��
        }
    }

    /// <summary>
    /// �e��������G�t�F�N�g���Đ�����B
    /// </summary>
    private IEnumerator FlashMuzzle()
    {
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(muzzleFlashDuration);
        muzzleFlashLight.enabled = false;
    }

    /// <summary>
    /// �R���g���[���[�̐U���𔭐�������B
    /// </summary>
    private void TriggerHaptics()
    {
        if (leftController != null)
        {
            leftController.SendHapticImpulse(0.7f, 0.15f); // ���x 0.5, 0.1�b�̐U��
        }

        if (rightController != null)
        {
            rightController.SendHapticImpulse(0.7f, 0.15f); // ���x 0.5, 0.1�b�̐U��
        }
    }

    /// <summary>
    /// �����[�h�������s���B
    /// </summary>
    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("�����[�h��...");
        ammoText.color = Color.red; // �����[�h���̓e�L�X�g��Ԃ�
        audioSource.PlayOneShot(reloadSound);

        // �����[�h���Ԃ�ҋ@ (��: 2�b)
        yield return new WaitForSeconds(2f);

        currentAmmo = maxAmmo;
        isReloading = false;
        ammoText.color = Color.white; // �����[�h������������e�L�X�g�̐F�����ɖ߂�
        UpdateAmmoText(); // �����[�h��̒e����\��
        Debug.Log("�����[�h����");
    }

    /// <summary>
    /// �c�e���̕\�����X�V����B
    /// </summary>
    private void UpdateAmmoText()
    {
        ammoText.text = currentAmmo.ToString();
    }
}
