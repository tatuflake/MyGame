using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChangeOnSelect : MonoBehaviour
{
    public Color selectedColor = Color.red; // 選択時の色
    private Color originalColor; // 元の色を保持
    private Renderer objectRenderer;

    void Start()
    {
        // Rendererコンポーネントを取得
        objectRenderer = GetComponent<Renderer>();
        // 元の色を取得
        originalColor = objectRenderer.material.color;

        // XR Grab Interactableのイベントをフック
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    // オブジェクトが選択されたときに呼ばれる
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        ChangeColor(selectedColor);
    }

    // オブジェクトが選択解除されたときに呼ばれる
    public void OnSelectExited(SelectExitEventArgs args)
    {
        ChangeColor(originalColor);
    }

    // 色を変更するメソッド
    private void ChangeColor(Color newColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }
}
