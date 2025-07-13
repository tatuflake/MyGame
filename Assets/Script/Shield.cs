using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Gun"))
        {

            Destroy(other.gameObject);
        }
    }
}
