using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTarget : MonoBehaviour
{

	// ゴール
	[SerializeField] List<Transform> targets;

	// カーソル
	[SerializeField] Transform cursor;

	IEnumerator<Transform> currentTargets;

	Quaternion offsetRotation;

	void Start()
	{
		currentTargets = targets.GetEnumerator();
		currentTargets.MoveNext();
	}

	// Update is called once per frame
	void Update()
	{
		if (Vector3.Distance(transform.position, currentTargets.Current.position) < 3)
		{
			currentTargets.MoveNext();
		}
		cursor.LookAt(currentTargets.Current);
	}
}
