using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;
    /// <summary>
    /// �e�e�̃v���n�u�B
    /// ���C�����ۂɁA���̃I�u�W�F�N�g��e�Ƃ��Ď��̉�����B
    /// </summary>
    [SerializeField]
    private GameObject m_bulletPrefab = null;

    /// <summary>
    /// �e���̈ʒu�B
    /// �e�e�����̉����鎞�̈ʒu������̐ݒ�ȂǂɎg�p����B
    /// </summary>
    [SerializeField]
    private Transform m_muzzlePos = null;

    void Start()
    {
        //Component���擾
        audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// VR�R���g���[���[�̃g���K�[������ꂽ���ɌĂяo���B
    /// </summary>
    public void Activate()
    {
        ShootAmmo();
    }

    /// <summary>
    /// �e�e�𐶐�����B
    /// </summary>
    private void ShootAmmo()
    {
        //�e�̃v���n�u���e���ʒu���ݒ肳��Ă��Ȃ���Ώ������s�킸�A��B���łɐ���B
        if (m_bulletPrefab == null ||
            m_muzzlePos == null)
        {
            Debug.Log(" Inspector �̐ݒ肪�Ԉ���Ă��ww m9(^�D^)�߷ެ� ");
            return;
        }

        //�e�𐶐�����B
        GameObject bulletObj = Instantiate(m_bulletPrefab);

        //�e�̈ʒu���A�e���̈ʒu�Ɠ���ɂ���B
        bulletObj.transform.position = m_muzzlePos.position;

        //�e�̌������A�e���̌����Ɠ���ɂ���B
        bulletObj.transform.rotation = m_muzzlePos.rotation;

        audioSource.PlayOneShot(sound1);
    }
}
