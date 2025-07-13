using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �V�[���Ǘ��p

public class CountdownManager : MonoBehaviour
{
    // �J�E���g�_�E���^�C�}�[�̐ݒ�
    public float countdownTime = 300f; // 5���i300�b�j
    private float remainingTime;
    private bool isCountingDown = false;
    private bool isPaused = false;

    // Player�I�u�W�F�N�g���C���X�y�N�^�[�Őݒ�\�ɂ���
    public GameObject playerObject;

    // �J�E���g�_�E�����J�n�����R���C�_�[�I�u�W�F�N�g
    public GameObject targetObjectToActivate;
    public GameObject objectToPauseCountdown;
    private Collider triggerCollider;

    // �J�E���g�_�E���\���p��UI�e�L�X�g
    public Text countdownText;

    // ScoreManager�̎Q��
    public ScoreManager scoreManager;

    void Start()
    {
        remainingTime = countdownTime; // �J�E���g�_�E���̏�����

        // �J�E���g�_�E���e�L�X�g��������
        UpdateCountdownText();

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
                    // �J�E���g�_�E���J�n
                    isCountingDown = true;
                }
            }
        }

        // �J�E���g�_�E�����A�N�e�B�u�ł���A��~���Ă��Ȃ��ꍇ�͎��Ԃ����炷
        if (isCountingDown && !isPaused)
        {
            remainingTime -= Time.deltaTime;

            // �J�E���g�_�E�����I�������ꍇ
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                ActivateObjectAndDisplayScore();

                // 10�b��ɃQ�[�������Z�b�g
                Invoke("ResetGame", 10f);
            }

            // �J�E���g�_�E����UI���X�V
            UpdateCountdownText();
        }

        // ���̃I�u�W�F�N�g���A�N�e�B�u�ȏꍇ�̓J�E���g�_�E�����~
        if (objectToPauseCountdown.activeSelf)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }


    // 5����ɃI�u�W�F�N�g��L�������A�X�R�A�ƃ����N��\��
    private void ActivateObjectAndDisplayScore()
    {
        // �^�[�Q�b�g�I�u�W�F�N�g��L����
        if (targetObjectToActivate != null)
        {
            targetObjectToActivate.SetActive(true);
        }

        // ScoreManager��OnBossDefeated���\�b�h���Ăяo��
        if (scoreManager != null)
        {
            scoreManager.OnBossDefeated();
        }

        // �J�E���g�_�E�����~
        isCountingDown = false;
    }

    // �J�E���g�_�E����UI�e�L�X�g���X�V����
    private void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime % 60F);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 10�b��ɃQ�[�������Z�b�g���鏈��
    private void ResetGame()
    {
        // ���݂̃V�[�����ēǂݍ��݂��邱�ƂŃQ�[�������Z�b�g
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
