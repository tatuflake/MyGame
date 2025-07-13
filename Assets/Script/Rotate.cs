using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    /// <summary>
    /// XŽ²‚ðŠî“_‚É‰ñ“]‚·‚é‘¬‚³
    /// </summary>
    [SerializeField]
    private float rotAngleX = 2.0f;

    /// <summary>
    /// YŽ²‚ðŠî“_‚É‰ñ“]‚·‚é‘¬‚³
    /// </summary>
    [SerializeField]
    private float rotAngleY = 2.0f;

    /// <summary>
    /// ZŽ²‚ðŠî“_‚É‰ñ“]‚·‚é‘¬‚³
    /// </summary>
    [SerializeField]
    private float rotAngleZ = 2.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotAngleX, rotAngleY, rotAngleZ);
    }
}
