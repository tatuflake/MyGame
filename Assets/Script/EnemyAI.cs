using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float approachDistance = 10f;  // �v���C���[�ɋ߂Â�����
    public float keepDistance = 3f;       // �v���C���[���痣��鋗��
    public float moveSpeed = 5f;          // �ړ����x
    public float dashSpeed = 10f;         // �����ړ��̑��x
    public GameObject bulletPrefab;       // �e�̃v���n�u
    public Transform bulletSpawnPoint;    // �e�̐����ʒu
    public GameObject fallingObjectPrefab; // �_���[�W��^����I�u�W�F�N�g�̃v���n�u
    public float fallingObjectRadius = 5f; // �_���[�W�I�u�W�F�N�g���~��͈�
    public float fallingObjectHeight = 10f; // �_���[�W�I�u�W�F�N�g�̏�����������

    public AudioClip chargeSound;         // �ːi���̃T�E���h
    public AudioClip bulletSound;         // �e�������̃T�E���h
    public AudioClip fallingObjectSound;  // �_���[�W�I�u�W�F�N�g�������̃T�E���h
    private AudioSource audioSource;

    private bool isAttacking = false;
    private bool isDashing = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SideDashRoutine()); // ���E�_�b�V���̃��[�`�����J�n
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �v���C���[�̕���������
        Vector3 lookDirection = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * moveSpeed);

        // �v���C���[���߂�����ꍇ�͋��������
        if (distanceToPlayer < keepDistance && !isDashing)
        {
            Vector3 direction = transform.position - player.position;
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
        // �v���C���[����苗���ɋ߂Â����ꍇ�͋߂Â�
        else if (distanceToPlayer < approachDistance && !isDashing)
        {
            Vector3 direction = player.position - transform.position;
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }

        // �U�����Ă��Ȃ��ꍇ�͍U�����J�n
        if (!isAttacking && distanceToPlayer < approachDistance)
        {
            StartCoroutine(AttackPattern());
        }
    }

    IEnumerator AttackPattern()
    {
        isAttacking = true;

        // �����_���ɍU���p�^�[����I��
        int attackChoice = Random.Range(1, 4);

        switch (attackChoice)
        {
            case 1:
                // �ːi
                StartCoroutine(ChargeAttack());
                break;
            case 2:
                // 3�i�K�̐��̒e�𔭎�
                StartCoroutine(ThreePhaseBulletAttack());
                break;
            case 3:
                // �����_���Ƀ_���[�W�I�u�W�F�N�g���~�点��
                StartCoroutine(RandomFallingObjects());
                break;
        }

        yield return new WaitForSeconds(2f); // �U���Ԋu
        isAttacking = false;
    }

    IEnumerator ChargeAttack()
    {
        // �T�E���h���Đ�
        audioSource.PlayOneShot(chargeSound);

        Vector3 chargeDirection = (player.position - transform.position).normalized;
        float chargeSpeed = moveSpeed * 6f; // �ːi���x�𒲐�
        float chargeDuration = 1f;

        for (float t = 0; t < chargeDuration; t += Time.deltaTime)
        {
            transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ThreePhaseBulletAttack()
    {
        // �T�E���h���Đ�
        audioSource.PlayOneShot(bulletSound);

        // 3�i�K�Œe�𔭎˂���
        for (int i = 0; i < 3; i++)
        {
            float angleOffset = 0f;

            // 1��ڂ�45�x�A2��ڂ�-45�x�A3��ڂ͐���
            if (i == 0)
            {
                angleOffset = 45f;
            }
            else if (i == 1)
            {
                angleOffset = -45f;
            }
            else if (i == 2)
            {
                angleOffset = 0f;
            }

            // 10���̒e��120�x�̐��ɔ���
            float startAngle = angleOffset - 60f; // 120�x�̐�̊J�n�p�x
            float angleStep = 120f / 9f; // 9�̃X�e�b�v��10���̒e�𔭎�

            for (int j = 0; j < 10; j++)
            {
                float angle = startAngle + (j * angleStep);
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Vector3 direction = rotation * transform.forward;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = direction * 10f;
            }

            // 0.5�b�Ԋu��݂���
            yield return new WaitForSeconds(0.5f);
        }
    }




    IEnumerator RandomFallingObjects()
    {
        // �T�E���h���Đ�
        audioSource.PlayOneShot(fallingObjectSound);

        int objectCount = 10; // �~�点��I�u�W�F�N�g�̐�

        for (int i = 0; i < objectCount; i++)
        {
            // �����_���Ȉʒu������
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-fallingObjectRadius, fallingObjectRadius), fallingObjectHeight, Random.Range(-fallingObjectRadius, fallingObjectRadius));

            // �_���[�W�I�u�W�F�N�g�𐶐�
            GameObject fallingObject = Instantiate(fallingObjectPrefab, randomPosition, Quaternion.identity);

            // �I�u�W�F�N�g���h���������ɌŒ肳��鏈���� FallingObject �X�N���v�g���ɂ���
            yield return new WaitForSeconds(0.1f); // ���̃I�u�W�F�N�g���~�点��Ԋu
        }
    }

    IEnumerator SideDashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f); // 8�b�ҋ@

            isDashing = true;
            Vector3 dashDirection = (Random.value > 0.5f) ? transform.right : -transform.right;

            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                transform.position += dashDirection * dashSpeed * Time.deltaTime;
                yield return null;
            }

            isDashing = false;
        }
    }
}
