using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMchange : MonoBehaviour
{
    private GameObject bgm;
    private GameObject bossbgm;
    private AudioSource audioSource = null;
    private Collider triggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.Find("BGM");
        bossbgm = GameObject.Find("bossBGM");
        audioSource = GetComponent<AudioSource>();
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
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();
                if (playerCollider != null && triggerCollider.bounds.Intersects(playerCollider.bounds))
                {
                    // �v���C���[���G�ꂽ�Ɣ��f���ꂽ�ꍇ�̏���
                    ChangeBGM();
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void ChangeBGM()
    {
        audioSource = bgm.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource = bossbgm.GetComponent<AudioSource>();
        audioSource.Play();
    }
}
