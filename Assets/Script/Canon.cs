using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public Transform player;
    public GameObject canonball;
    private int count = 0;
    public AudioClip sound1;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerStay(Collider col)
    {

        if (col.gameObject.name == "player")
        {
            transform.LookAt(player);

            count++;
            
            if (count % 20 == 0)
            {
                Instantiate(canonball, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(sound1);
            }
        }
    }
}
