using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>
    /// �e�̑��x (m/s)
    /// </summary>
    [SerializeField]
    private float m_bulletSpeed = 27.0f;

    // Update is called once per frame
    void Update()
    {
        //�e��O�ɐi�܂���
        transform.position +=
            transform.forward * m_bulletSpeed * Time.deltaTime;
    }
}