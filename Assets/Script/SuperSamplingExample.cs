using UnityEngine;
using UnityEngine.XR;

public class SuperSamplingExample : MonoBehaviour
{
    void Start()
    {
        // 解像度スケールを1.5倍に設定
        XRSettings.eyeTextureResolutionScale = 3.0f;
    }
}
