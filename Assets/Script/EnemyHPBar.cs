
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI�g���Ƃ��͖Y�ꂸ�ɁB
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    public int maxHp;
    int currentHp;
    //Slider������
    public Slider slider;
    public int scorepoint;
    
    private GameObject scoreText;//*

    public AudioClip sound1;


    [SerializeField] private Transform burstVFXPrefab;

    private bool isDead = false;

    public GameObject Sound;
    private AudioSource audioSource = null;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText");//*
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        Debug.Log("Start currentHp : " + currentHp);

        audioSource = GetComponent<AudioSource>();
    }

    //Collider�I�u�W�F�N�g��IsTrigger�Ƀ`�F�b�N����邱�ƁB
    private void OnTriggerEnter(Collider collider)
    {
        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (collider.gameObject.tag == "Gun")
        {
            if (isDead) return;

            audioSource.PlayOneShot(sound1);
            
            int damage = 10;
            Debug.Log("damage : " + damage);

            //���݂�HP����_���[�W������
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);

            //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f�B
            //int���m�̊���Z�͏����_�ȉ���0�ɂȂ�̂ŁA
            //(float)������float�̕ϐ��Ƃ��ĐU���킹��B
            slider.value = (float)currentHp / (float)maxHp;
            Debug.Log("slider.value : " + slider.value);
        }

        if (slider.value <= 0)
        {
            isDead = true;
            ScoreManager scoreManager = scoreText.GetComponent<ScoreManager>();
            scoreManager.score += scorepoint;

            audioSource = Sound.GetComponent<AudioSource>();
            audioSource.Play();

            Destroy(this.gameObject);
            Transform burstVFXTransform = Instantiate(burstVFXPrefab, transform.position, Quaternion.identity);
            
        }
    }
}