using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using System;

public class IKFootSolver : MonoBehaviour
{
    public Transform body;            // 脚の付け根部分などの基準となるオブジェクトのTransform
    public float footSpacing = 0.3f;  // 足の位置の間隔
    public float stepDistance = 0.5f; // 一歩で移動する距離の閾値
    public float stepHeight = 0.3f;   // 足が持ち上がる高さ
    public float speed = 5f;          // ステップのスピード
    public LayerMask terrainLayer;    // 地面のレイヤー

    private Vector3 currentPosition;
    private Vector3 newPosition;
    private Vector3 oldPosition;
    private float lerp = 1;

    void Start()
    {
        currentPosition = transform.position;
        newPosition = transform.position;
        oldPosition = transform.position;
    }

    void Update()
    {
        transform.position = currentPosition;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && lerp >= 1)
            {
                lerp = 0;
                newPosition = info.point;
            }
        }

        if (lerp < 1)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = footPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }
}

