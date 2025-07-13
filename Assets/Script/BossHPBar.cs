using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHPBar : MonoBehaviour
{
    public int maxHp;
    int currentHp;
    public Slider slider;
    public GameObject gameclearText;
    public int scorepoint;
    private GameObject scoreText;

    public AudioClip sound1;
    public AudioClip sound2;

    public GameObject Sound;
    private AudioSource audioSource = null;

    [SerializeField] private Transform burstVFXPrefab;

    private bool isDead = false;

    // HP��������؂������ɗL���ɂ���Q�[���I�u�W�F�N�g�̃��X�g
    public List<GameObject> objectsToActivate;

    public GameObject targetObject_D;

    private bool halfHpActivated = false; // HP��������؂��������`�F�b�N����t���O
    private bool isInvincible = false;    // �{�X�����G��Ԃ��ǂ����������t���O

    public List<Collider> hitColliders;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText");
        slider.value = 1;
        currentHp = maxHp;
        audioSource = GetComponent<AudioSource>();
    }

    public GameManager gamemanager;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Gun")
        {
            if (isDead || isInvincible) return;

            audioSource.PlayOneShot(sound1);

            int damage = 10;

            currentHp = currentHp - damage;
            slider.value = (float)currentHp / (float)maxHp;

            // HP��������؂������ɖ��G��Ԃɂ��ăQ�[���I�u�W�F�N�g��L���ɂ���
            if (!halfHpActivated && currentHp <= maxHp / 2)
            {
                halfHpActivated = true;
                StartCoroutine(ActivateInvincibilityAndSpawnObjects());
            }
        }

        if (slider.value <= 0)
        {
            isDead = true;
            ScoreManager scoreManager = scoreText.GetComponent<ScoreManager>();
            scoreManager.score += scorepoint;

            audioSource = Sound.GetComponent<AudioSource>();
            audioSource.Play();

            targetObject_D.SetActive(!targetObject_D.activeSelf);

            FindObjectOfType<ScoreManager>().OnBossDefeated();
            Destroy(this.gameObject);
            Transform burstVFXTransform = Instantiate(burstVFXPrefab, transform.position, Quaternion.identity);
            gamemanager.GameOver();
        }
    }

    // HP��������؂������ɃQ�[���I�u�W�F�N�g��L���ɂ��A10�b�Ԗ��G��Ԃɂ���R���[�`��
    private IEnumerator ActivateInvincibilityAndSpawnObjects()
    {
        // �T�E���h��5��A���ōĐ�
        for (int i = 0; i < 5; i++)
        {
            audioSource.PlayOneShot(sound2);
            yield return new WaitForSeconds(0.1f); // �����Ԋu���󂯂čĐ�
        }

        // ���G��Ԃɂ���
        isInvincible = true;

        // �Q�[���I�u�W�F�N�g��L���ɂ���
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        // 10�b�ԑҋ@����
        yield return new WaitForSeconds(10f);

        // ���G��Ԃ���������
        isInvincible = false;
    }
}
