using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
	// OnTriggerStay�C�x���g���Ɏ��s�������֐���o�^����ϐ��i������Collider�����j
	[SerializeField]
	private UnityEvent<Collider> onTriggerStayEvent = new UnityEvent<Collider>();

	//  OnTriggerExit�C�x���g���Ɏ��s�������֐���o�^����ϐ��i������Collider�����j
	[SerializeField]
	private UnityEvent<Collider> onTriggerExitEvent = new UnityEvent<Collider>();

	/// <summary>
	/// Is Trigger��ON�ő���GameObject��Collider���ɂ���Ƃ��ɌĂ΂ꑱ����
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerStay(Collider other)
	{
		// Inspector�^�u��onTriggerStayEvent�Ŏw�肳�ꂽ���������s����
		onTriggerStayEvent.Invoke(other);
	}

	/// <summary>
	/// Is Trigger��ON�ő���GameObject��Collider����o���Ƃ��ɌĂ΂��
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other)
	{
		// Inspector�^�u��onTriggerExitEvent�Ŏw�肳�ꂽ���������s����
		onTriggerExitEvent.Invoke(other);
	}
}
