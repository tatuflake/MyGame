using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnGunHit : MonoBehaviour
{
    /// <summary>
    /// �\��������e�L�X�g��UI�v�f
    /// </summary>
    [SerializeField]
    private Text displayText;

    void Start()
    {
        // �e�L�X�g���ŏ��͔�\���ɂ��Ă���
        if (displayText != null)
        {
            displayText.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // �Փ˂����I�u�W�F�N�g��Gun�^�O�������Ă��邩�m�F
        if (collider.gameObject.tag == "Gun")
        {
            // �e�L�X�g��\������
            if (displayText != null)
            {
                displayText.enabled = true;
                StartCoroutine(HideTextAfterDelay(1.0f)); // 1�b��Ƀe�L�X�g���\���ɂ���
            }
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        // �w�肵�����Ԃ����ҋ@
        yield return new WaitForSeconds(delay);

        // �e�L�X�g���\���ɂ���
        if (displayText != null)
        {
            displayText.enabled = false;
        }
    }
}
