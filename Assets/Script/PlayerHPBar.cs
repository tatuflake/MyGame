
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI�g���Ƃ��͖Y�ꂸ�ɁB
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class PlayerHPBar : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    int maxHp = 300;
    int currentHp;
    //Slider������
    public Slider slider;

    public GameObject gameoverText;

    public GameObject Sound;
    private AudioSource audioSource = null;


    // �Ԃ��I�[�o�[���C�𐧌䂷�邽�߂�Image
    public Image damageOverlay;
    private Color overlayColor;
    public float overlayFadeDuration = 0.5f;

    void Start()
    {

        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        Debug.Log("Start currentHp : " + currentHp);

        // �����̃I�[�o�[���C�J���[��ݒ�
        if (damageOverlay != null)
        {
            overlayColor = damageOverlay.color;
            overlayColor.a = 0;
            damageOverlay.color = overlayColor;
        }
    }

    public GameManager gamemanager;
    //Collider�I�u�W�F�N�g��IsTrigger�Ƀ`�F�b�N����邱�ƁB
    private void OnTriggerEnter(Collider collider)
    {
        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (collider.gameObject.tag == "Enemy")
        {
            

            int damage = 10;
            Debug.Log("damage : " + damage);

            //���݂�HP����_���[�W������
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);

            //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f�B
            //int���m�̊���Z�͏����_�ȉ���0�ɂȂ�̂ŁA
            //(float)������float�̕ϐ��Ƃ��ĐU���킹��B
            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value : " + slider.value);

            StartCoroutine(FlashRedOverlay());

            audioSource = Sound.GetComponent<AudioSource>();
            audioSource.Play();
        }
        if (collider.gameObject.tag == "Recovery")
        {

            //���݂�HP����_���[�W������
            currentHp = maxHp;
            Debug.Log("After currentHp : " + currentHp);

            //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f�B
            //int���m�̊���Z�͏����_�ȉ���0�ɂȂ�̂ŁA
            //(float)������float�̕ϐ��Ƃ��ĐU���킹��B
            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value : " + slider.value);
        }
        if(slider.value <= 0)
        {
            Destroy(this.gameObject);
            // �Q�[���I�[�o�[���Ăяo��
            gamemanager.GameOver();


        }
    }

    private IEnumerator FlashRedOverlay()
    {
        overlayColor.a = 1;
        damageOverlay.color = overlayColor;

        // ���X�ɓ����ɂ���
        float elapsedTime = 0f;
        while (elapsedTime < overlayFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            overlayColor.a = Mathf.Lerp(1, 0, elapsedTime / overlayFadeDuration);
            damageOverlay.color = overlayColor;
            yield return null;
        }
    }
}