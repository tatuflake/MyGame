using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkit���g�p

public class SniperRifleController : MonoBehaviour
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
    /// �e�̔��ˊԊu
    /// </summary>
    [SerializeField]
    private float fireRate = 1f; // �X�i�C�p�[���C�t���̂��߁A���ˊԊu�𒷂߂ɐݒ�

    /// <summary>
    /// �}�K�W���̒e��
    /// </summary>
    private int maxAmmo = 5; // ��x�ɔ��˂ł���e��
    private int currentAmmo;
    private int totalAmmo = 50; // ���e��
    private bool isReloading = false;
    private Coroutine firingCoroutine; // ���˃R���[�`���̎Q�Ƃ�ۑ�����

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
            if (currentAmmo > 0 && totalAmmo >= 10) // �e��5���ȏ�c���Ă���ꍇ�̂ݔ���
            {
                for (int i = 0; i < 10; i++) // 10�������ɔ���
                {
                    ShootAmmo();
                }

                // ���C�����Đ�
                audioSource.PlayOneShot(sound1);

                // �e���̔����G�t�F�N�g���Đ�
                StartCoroutine(FlashMuzzle());

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

                currentAmmo--;
                totalAmmo -= 10; // ���e�������炷
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
    /// �e�e�𐶐�����B
    /// </summary>
    private void ShootAmmo()
    {
        if (m_bulletPrefab == null || m_muzzlePos == null)
        {
            Debug.Log(" Inspector �̐ݒ肪�Ԉ���Ă��");
            return;
        }

        // �e�ۂ𐶐�
        GameObject bulletObj = Instantiate(m_bulletPrefab, m_muzzlePos.position, m_muzzlePos.rotation);

        Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(m_muzzlePos.forward * bulletSpeed);
        }
        else
        {
            Debug.LogWarning("�e�ۃI�u�W�F�N�g��Rigidbody������܂���");
        }

        // ���C�����Đ�
        audioSource.PlayOneShot(sound1);


        // �R���g���[���[��U��������
        TriggerHaptics();


        // ��莞�Ԍ�ɒe�ۂ�j������
        Destroy(bulletObj, 5f);
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
            leftController.SendHapticImpulse(1.0f, 0.2f); // ���x 0.5, 0.1�b�̐U��
        }

        if (rightController != null)
        {
            rightController.SendHapticImpulse(1.0f, 0.2f); // ���x 0.5, 0.1�b�̐U��
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
        totalAmmo = 50;
        isReloading = false;
        firingCoroutine = null; // �����[�h��ɔ��˂��\�ɂ��邽�߂ɃR���[�`�������Z�b�g
        ammoText.color = Color.white; // �����[�h������������e�L�X�g�̐F�����ɖ߂�
        UpdateAmmoText(); // �����[�h��̒e����\��
        Debug.Log("�����[�h����");
    }

    /// <summary>
    /// �c�e���̕\�����X�V����B
    /// </summary>
    private void UpdateAmmoText()
    {
        ammoText.text = $"{currentAmmo}";
    }
}
