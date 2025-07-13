using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class ItemScaleRecover : MonoBehaviour
{
    // �Q�[���I�u�W�F�N�g�̃X�P�[����ێ�����ϐ�
    private Vector3 m_defalutScale;

    // Start is called before the first frame update
    private void Awake()
    {
        // �V�[���N�����̃Q�[���I�u�W�F�N�g�̃X�P�[����ێ�����
        m_defalutScale = transform.localScale;
    }

    public void RecoverScale()
    {
        // ���^�\�P�b�g������o�����Ƃ��ɃQ�[���I�u�W�F�N�g�̃X�P�[�������ɖ߂�
        transform.localScale = m_defalutScale;
    }
}