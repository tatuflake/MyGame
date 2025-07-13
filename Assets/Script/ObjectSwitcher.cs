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
    /// コントロールする3つのオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject[] objects;

    /// <summary>
    /// Aボタン（PrimaryButton）の入力アクション
    /// </summary>
    [SerializeField]
    private InputActionReference aButtonAction;

    /// <summary>
    /// Bボタン（SecondaryButton）の入力アクション
    /// </summary>
    [SerializeField]
    private InputActionReference bButtonAction;

    // ImageAとImageBを有効化するためのUI Image
    [SerializeField]
    private Image imageA;
    [SerializeField]
    private Image imageB;

    /// <summary>
    /// 現在有効化されているオブジェクトのインデックス
    /// </summary>
    private int currentIndex = 0;

    // 3つのコントローラーの参照を追加
    [SerializeField]
    private RifleController rifleController;
    [SerializeField]
    private SniperRifleController sniperRifleController;
    [SerializeField]
    private ShotgunController shotgunController;

    void Start()
    {
        // 最初に両方のImageを非表示にしておく
        imageA.enabled = false;
        imageB.enabled = false;

        audioSource = GetComponent<AudioSource>();

        // 最初にオブジェクト1だけを有効化する
        UpdateObjectStates();

        // Aボタンのイベントリスナーを追加
        aButtonAction.action.performed += OnAButtonPressed;

        // Bボタンのイベントリスナーを追加
        bButtonAction.action.performed += OnBButtonPressed;
    }

    void Update()
    {
        // Aボタンが押されている間ImageAを有効化
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

        // Bボタンが押されている間ImageBを有効化
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
        // イベントリスナーの解除
        aButtonAction.action.performed -= OnAButtonPressed;
        bButtonAction.action.performed -= OnBButtonPressed;
    }

    /// <summary>
    /// Aボタンが押されたときに呼ばれるメソッド
    /// </summary>
    /// <param name="context"></param>
    private void OnAButtonPressed(InputAction.CallbackContext context)
    {
        // リロード中かどうか確認し、リロード中なら処理を中止
        if (IsAnyWeaponReloading())
        {
            audioSource.PlayOneShot(sound2);
            return;
        }
        audioSource.PlayOneShot(sound1);
        // 順方向に切り替え（1→2→3→1）
        currentIndex = (currentIndex + 1) % objects.Length;
        UpdateObjectStates();
    }

    /// <summary>
    /// Bボタンが押されたときに呼ばれるメソッド
    /// </summary>
    /// <param name="context"></param>
    private void OnBButtonPressed(InputAction.CallbackContext context)
    {
        // リロード中かどうか確認し、リロード中なら処理を中止
        if (IsAnyWeaponReloading())
        {
            audioSource.PlayOneShot(sound2);
            return;
        }
        audioSource.PlayOneShot(sound1);
        // 逆方向に切り替え（3→2→1→3）
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length;
        UpdateObjectStates();
    }

    /// <summary>
    /// オブジェクトの有効/無効を更新する
    /// </summary>
    private void UpdateObjectStates()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (i == currentIndex)
            {
                objects[i].SetActive(true);  // 現在のオブジェクトを有効化
            }
            else
            {
                objects[i].SetActive(false); // それ以外のオブジェクトは無効化
            }
        }
    }

    // どの武器コントローラーもリロード中かどうか確認
    private bool IsAnyWeaponReloading()
    {
        return (rifleController != null && rifleController.IsReloading) ||
               (sniperRifleController != null && sniperRifleController.IsReloading) ||
               (shotgunController != null && shotgunController.IsReloading) ||
               rifleController.IsHeld || sniperRifleController.IsHeld || shotgunController.IsHeld
               ;
    }
}
