using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChangeOnSelect : MonoBehaviour
{
    public Color selectedColor = Color.red; // �I�����̐F
    private Color originalColor; // ���̐F��ێ�
    private Renderer objectRenderer;

    void Start()
    {
        // Renderer�R���|�[�l���g���擾
        objectRenderer = GetComponent<Renderer>();
        // ���̐F���擾
        originalColor = objectRenderer.material.color;

        // XR Grab Interactable�̃C�x���g���t�b�N
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    // �I�u�W�F�N�g���I�����ꂽ�Ƃ��ɌĂ΂��
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        ChangeColor(selectedColor);
    }

    // �I�u�W�F�N�g���I���������ꂽ�Ƃ��ɌĂ΂��
    public void OnSelectExited(SelectExitEventArgs args)
    {
        ChangeColor(originalColor);
    }

    // �F��ύX���郁�\�b�h
    private void ChangeColor(Color newColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }
}
