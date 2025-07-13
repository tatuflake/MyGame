using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UIg‚¤‚Æ‚«‚Í–Y‚ê‚¸‚ÉB
using UnityEngine.UI;

public class HPBarDirection : MonoBehaviour
{
    public Canvas canvas;

    void Update()
    {
        //EnemyCanvas‚ğMain Camera‚ÉŒü‚©‚¹‚é
        canvas.transform.rotation =
            Camera.main.transform.rotation;
    }
}