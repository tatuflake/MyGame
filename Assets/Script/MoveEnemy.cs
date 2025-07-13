using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveEnemy : MonoBehaviour
{

	private NavMeshAgent navMeshAgent;

	// Start is called before the first frame update
	void Start()
	{
		// このスクリプトを設定したGameObjectのNavMeshAgentコンポーネントを取得
		navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
	}

	// CollisionDetectorクラスに作ったonTriggerStayEventにセットする。
	public void OnDetectObject(Collider collider)
	{
		// 検知したオブジェクトに"Player"タグが付いてれば、そのオブジェクトを追いかける
		if (collider.gameObject.tag == "Player")
		{
			// 対象のオブジェクトを追いかける
			navMeshAgent.destination = collider.gameObject.transform.position;
		}

	}

	// CollisionDetectorクラスに作ったonTriggerExitEventにセットする。 
	public void OnLoseObject(Collider collider)
	{
		// 検知したオブジェクトに"Player"タグが付いてれば、その場で止まる
		if (collider.gameObject.tag == "Player")
		{
			// その場で止まる（目的地を今の自分自身の場所にすることにより止めている）
			navMeshAgent.destination = this.gameObject.transform.position;
		}
	}
}