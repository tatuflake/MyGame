using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections;

public class ObjectSwitcher : MonoBehaviour
{
    public AudioClip sound1;         
    public AudioClip sound2;    
    private AudioSource audioSource;

    /// <summary>
    /// �R���g���[������3�̃I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject[] objects;

    /// <summary>
    /// A�{�^���iPrimaryButton�j�̓��̓A�N�V����
    /// </summary>
    [SerializeField]
    private InputActionReference aButtonAction;

    /// <summary>
    /// B�{�^���iSecondaryButton�j�̓��̓A�N�V����
    /// </summary>
    [SerializeField]
    private InputActionReference bButtonAction;

    // ImageA��ImageB��L�������邽�߂�UI Image
    [SerializeField]
    private Image imageA;
    [SerializeField]
    private Image imageB;

    /// <summary>
    /// ���ݗL��������Ă���I�u�W�F�N�g�̃C���f�b�N�X
    /// </summary>
    private int currentIndex = 0;

    // 3�̃R���g���[���[�̎Q�Ƃ�ǉ�
    [SerializeField]
    private RifleController rifleController;
    [SerializeField]
    private SniperRifleController sniperRifleController;
    [SerializeField]
    private ShotgunController shotgunController;

    void Start()
    {
        // �ŏ��ɗ�����Image���\���ɂ��Ă���
        imageA.enabled = false;
        imageB.enabled = false;

        audioSource = GetComponent<AudioSource>();

        // �ŏ��ɃI�u�W�F�N�g1������L��������
        UpdateObjectStates();

        // A�{�^���̃C�x���g���X�i�[��ǉ�
        aButtonAction.action.performed += OnAButtonPressed;

        // B�{�^���̃C�x���g���X�i�[��ǉ�
        bButtonAction.action.performed += OnBButtonPressed;
    }

    void Update()
    {
        // A�{�^����������Ă����ImageA��L����
        if (aButtonAction.action.ReadValue<float>() > 0)
        {
            if (IsAnyWeaponReloading())
            {
                return;
            }
            imageA.enabled = true;
        }
        else
        {
            imageA.enabled = false;
        }

        // B�{�^����������Ă����ImageB��L����
        if (bButtonAction.action.ReadValue<float>() > 0)
        {
            if (IsAnyWeaponReloading())
            {
                return;
            }
            imageB.enabled = true;
        }
        else
        {
            imageB.enabled = false;
        }
    }

    private void OnDestroy()
    {
        // �C�x���g���X�i�[�̉���
        aButtonAction.action.performed -= OnAButtonPressed;
        bButtonAction.action.performed -= OnBButtonPressed;
    }

    /// <summary>
    /// A�{�^���������ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    /// </summary>
    /// <param name="context"></param>
    private void OnAButtonPressed(InputAction.CallbackContext context)
    {
        // �����[�h�����ǂ����m�F���A�����[�h���Ȃ珈���𒆎~
        if (IsAnyWeaponReloading())
        {
            audioSource.PlayOneShot(sound2);
            return;
        }
        audioSource.PlayOneShot(sound1);
        // �������ɐ؂�ւ��i1��2��3��1�j
        currentIndex = (currentIndex + 1) % objects.Length;
        UpdateObjectStates();
    }

    /// <summary>
    /// B�{�^���������ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    /// </summary>
    /// <param name="context"></param>
    private void OnBButtonPressed(InputAction.CallbackContext context)
    {
        // �����[�h�����ǂ����m�F���A�����[�h���Ȃ珈���𒆎~
        if (IsAnyWeaponReloading())
        {
            audioSource.PlayOneShot(sound2);
            return;
        }
        audioSource.PlayOneShot(sound1);
        // �t�����ɐ؂�ւ��i3��2��1��3�j
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length;
        UpdateObjectStates();
    }

    /// <summary>
    /// �I�u�W�F�N�g�̗L��/�������X�V����
    /// </summary>
    private void UpdateObjectStates()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (i == currentIndex)
            {
                objects[i].SetActive(true);  // ���݂̃I�u�W�F�N�g��L����
            }
            else
            {
                objects[i].SetActive(false); // ����ȊO�̃I�u�W�F�N�g�͖�����
            }
        }
    }

    // �ǂ̕���R���g���[���[�������[�h�����ǂ����m�F
    private bool IsAnyWeaponReloading()
    {
        return (rifleController != null && rifleController.IsReloading) ||
               (sniperRifleController != null && sniperRifleController.IsReloading) ||
               (shotgunController != null && shotgunController.IsReloading) ||
               rifleController.IsHeld || sniperRifleController.IsHeld || shotgunController.IsHeld
               ;
    }
}
