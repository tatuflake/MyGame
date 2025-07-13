using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSequenceManagerWithLine : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;  // �����̃I�u�W�F�N�g��ݒ�
    [SerializeField]
    private Text distanceText;  // �����\���p��UI�e�L�X�g
    [SerializeField]
    private GameObject playerObject;  // �v���C���[�Ƃ��Đݒ肷��I�u�W�F�N�g
    [SerializeField]
    private LineRenderer lineRenderer; // ���C�������_���[

    private int currentIndex = 0;  // ���ݗL��������Ă���I�u�W�F�N�g�̃C���f�b�N�X

    void Start()
    {
        // �S�I�u�W�F�N�g�𖳌������A�ŏ��̃I�u�W�F�N�g�݂̂�L����
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == 0);
        }

        // �����e�L�X�g�̏�����
        distanceText.enabled = false;  // �ŏ��͔�\��

        // ���C�������_���[�̏�����
        lineRenderer.positionCount = 2;  // �n�_�ƏI�_��2�̒��_���g�p
    }

    void Update()
    {
        if (objects[currentIndex].activeSelf)
        {
            // �I�u�W�F�N�g�ƃv���C���[�̋������v�����ăe�L�X�g�ɔ��f
            float distance = Vector3.Distance(playerObject.transform.position, objects[currentIndex].transform.position);
            distanceText.text = "Distance: " + distance.ToString("F2") + "m";

            // �e�L�X�g���I�u�W�F�N�g�̏㕔�ɕ\�����A��Ƀv���C���[�������悤�ɂ���
            Vector3 textPosition = objects[currentIndex].transform.position + Vector3.up * 2.0f;  // �I�u�W�F�N�g�̏㕔�ɔz�u
            distanceText.transform.position = textPosition;
            distanceText.transform.LookAt(playerObject.transform);
            distanceText.transform.Rotate(0, 180, 0);  // ���ʂ������悤�ɉ�]

            // �e�L�X�g��\������
            distanceText.enabled = true;

            // ���C�������_���[�Ńv���C���[�ƃI�u�W�F�N�g�̊Ԃɐ�������
            lineRenderer.SetPosition(0, playerObject.transform.position);  // ���C���̎n�_���v���C���[�̈ʒu�ɐݒ�
            lineRenderer.SetPosition(1, objects[currentIndex].transform.position);  // ���C���̏I�_���I�u�W�F�N�g�̈ʒu�ɐݒ�
        }
    }

    // �I�u�W�F�N�g�ɐG�ꂽ�ۂɌĂяo����郁�\�b�h
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && objects[currentIndex].activeSelf)
        {
            // ���݂̃I�u�W�F�N�g�𖳌������A���̃I�u�W�F�N�g��L����
            objects[currentIndex].SetActive(false);
            currentIndex++;

            if (currentIndex < objects.Length)
            {
                objects[currentIndex].SetActive(true);
            }

            // �S�ẴI�u�W�F�N�g���L�������ꂽ��A�e�L�X�g�ƃ��C�����\��
            if (currentIndex >= objects.Length)
            {
                distanceText.enabled = false;
                lineRenderer.enabled = false;
            }
        }
    }
}