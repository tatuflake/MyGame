using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectImageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject object1;
    [SerializeField]
    private GameObject object2;
    [SerializeField]
    private GameObject object3;

    [SerializeField]
    private Image imageA;  // オブジェクト1に対応するImageA
    [SerializeField]
    private Image imageB;  // オブジェクト2に対応するImageB
    [SerializeField]
    private Image imageC;  // オブジェクト3に対応するImageC

    // XR Grab Interactable
    private XRGrabInteractable grabInteractable1;
    private XRGrabInteractable grabInteractable2;
    private XRGrabInteractable grabInteractable3;

    void Start()
    {
        // 各オブジェクトのXRGrabInteractableコンポーネントを取得
        grabInteractable1 = object1.GetComponent<XRGrabInteractable>();
        grabInteractable2 = object2.GetComponent<XRGrabInteractable>();
        grabInteractable3 = object3.GetComponent<XRGrabInteractable>();

        // 最初に全てのイメージを非表示にしておく
        imageA.enabled = false;
        imageB.enabled = false;
        imageC.enabled = false;
    }

    void Update()
    {
        // オブジェクト1が有効かつプレイヤーに持たれていないとき、ImageAを有効化
        if (object1.activeSelf && !grabInteractable1.isSelected)
        {
            imageA.enabled = true;
        }
        else
        {
            imageA.enabled = false;
        }

        // オブジェクト2が有効かつプレイヤーに持たれていないとき、ImageBを有効化
        if (object2.activeSelf && !grabInteractable2.isSelected)
        {
            imageB.enabled = true;
        }
        else
        {
            imageB.enabled = false;
        }

        // オブジェクト3が有効かつプレイヤーに持たれていないとき、ImageCを有効化
        if (object3.activeSelf && !grabInteractable3.isSelected)
        {
            imageC.enabled = true;
        }
        else
        {
            imageC.enabled = false;
        }
    }
}
