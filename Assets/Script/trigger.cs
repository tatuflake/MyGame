using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject HPBar;
    public string playerTag = "Player";  // �v���C���[�̃^�O���w��

    private Collider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
        {
            Debug.LogError("���̃I�u�W�F�N�g�ɂ�Collider�R���|�[�l���g���K�v�ł��B");
        }
    }

    void Update()
    {
        if (triggerCollider != null)
        {
            GameObject player = GameObject.FindWithTag(playerTag);
            if (player != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();
                if (playerCollider != null && triggerCollider.bounds.Intersects(playerCollider.bounds))
                {
                    // �v���C���[���G�ꂽ�Ɣ��f���ꂽ�ꍇ�̏���
                    targetObject.SetActive(!targetObject.activeSelf);
                    HPBar.SetActive(!HPBar.activeSelf);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
