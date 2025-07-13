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
    private Image imageA;  // �I�u�W�F�N�g1�ɑΉ�����ImageA
    [SerializeField]
    private Image imageB;  // �I�u�W�F�N�g2�ɑΉ�����ImageB
    [SerializeField]
    private Image imageC;  // �I�u�W�F�N�g3�ɑΉ�����ImageC

    // XR Grab Interactable
    private XRGrabInteractable grabInteractable1;
    private XRGrabInteractable grabInteractable2;
    private XRGrabInteractable grabInteractable3;

    void Start()
    {
        // �e�I�u�W�F�N�g��XRGrabInteractable�R���|�[�l���g���擾
        grabInteractable1 = object1.GetComponent<XRGrabInteractable>();
        grabInteractable2 = object2.GetComponent<XRGrabInteractable>();
        grabInteractable3 = object3.GetComponent<XRGrabInteractable>();

        // �ŏ��ɑS�ẴC���[�W���\���ɂ��Ă���
        imageA.enabled = false;
        imageB.enabled = false;
        imageC.enabled = false;
    }

    void Update()
    {
        // �I�u�W�F�N�g1���L�����v���C���[�Ɏ�����Ă��Ȃ��Ƃ��AImageA��L����
        if (object1.activeSelf && !grabInteractable1.isSelected)
        {
            imageA.enabled = true;
        }
        else
        {
            imageA.enabled = false;
        }

        // �I�u�W�F�N�g2���L�����v���C���[�Ɏ�����Ă��Ȃ��Ƃ��AImageB��L����
        if (object2.activeSelf && !grabInteractable2.isSelected)
        {
            imageB.enabled = true;
        }
        else
        {
            imageB.enabled = false;
        }

        // �I�u�W�F�N�g3���L�����v���C���[�Ɏ�����Ă��Ȃ��Ƃ��AImageC��L����
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
