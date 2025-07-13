using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSequenceManagerWithLine : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;  // 複数のオブジェクトを設定
    [SerializeField]
    private Text distanceText;  // 距離表示用のUIテキスト
    [SerializeField]
    private GameObject playerObject;  // プレイヤーとして設定するオブジェクト
    [SerializeField]
    private LineRenderer lineRenderer; // ラインレンダラー

    private int currentIndex = 0;  // 現在有効化されているオブジェクトのインデックス

    void Start()
    {
        // 全オブジェクトを無効化し、最初のオブジェクトのみを有効化
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == 0);
        }

        // 距離テキストの初期化
        distanceText.enabled = false;  // 最初は非表示

        // ラインレンダラーの初期化
        lineRenderer.positionCount = 2;  // 始点と終点の2つの頂点を使用
    }

    void Update()
    {
        if (objects[currentIndex].activeSelf)
        {
            // オブジェクトとプレイヤーの距離を計測してテキストに反映
            float distance = Vector3.Distance(playerObject.transform.position, objects[currentIndex].transform.position);
            distanceText.text = "Distance: " + distance.ToString("F2") + "m";

            // テキストをオブジェクトの上部に表示し、常にプレイヤーを向くようにする
            Vector3 textPosition = objects[currentIndex].transform.position + Vector3.up * 2.0f;  // オブジェクトの上部に配置
            distanceText.transform.position = textPosition;
            distanceText.transform.LookAt(playerObject.transform);
            distanceText.transform.Rotate(0, 180, 0);  // 正面を向くように回転

            // テキストを表示する
            distanceText.enabled = true;

            // ラインレンダラーでプレイヤーとオブジェクトの間に線を引く
            lineRenderer.SetPosition(0, playerObject.transform.position);  // ラインの始点をプレイヤーの位置に設定
            lineRenderer.SetPosition(1, objects[currentIndex].transform.position);  // ラインの終点をオブジェクトの位置に設定
        }
    }

    // オブジェクトに触れた際に呼び出されるメソッド
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && objects[currentIndex].activeSelf)
        {
            // 現在のオブジェクトを無効化し、次のオブジェクトを有効化
            objects[currentIndex].SetActive(false);
            currentIndex++;

            if (currentIndex < objects.Length)
            {
                objects[currentIndex].SetActive(true);
            }

            // 全てのオブジェクトが有効化されたら、テキストとラインを非表示
            if (currentIndex >= objects.Length)
            {
                distanceText.enabled = false;
                lineRenderer.enabled = false;
            }
        }
    }
}