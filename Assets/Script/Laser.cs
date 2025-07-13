using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("player");
        transform.LookAt(player.transform);
    }

    void Update()
    {
        transform.Translate(0, 0, 1);
        Destroy(gameObject, 1);
    }
}
