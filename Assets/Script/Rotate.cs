using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    /// <summary>
    /// X������_�ɉ�]���鑬��
    /// </summary>
    [SerializeField]
    private float rotAngleX = 2.0f;

    /// <summary>
    /// Y������_�ɉ�]���鑬��
    /// </summary>
    [SerializeField]
    private float rotAngleY = 2.0f;

    /// <summary>
    /// Z������_�ɉ�]���鑬��
    /// </summary>
    [SerializeField]
    private float rotAngleZ = 2.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotAngleX, rotAngleY, rotAngleZ);
    }
}
