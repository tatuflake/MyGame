using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTrail : MonoBehaviour
{
    public GameObject trailPrefab;  // �v���n�u���A�T�C��
    public float spawnInterval = 0.1f;  // �R�s�[�𐶐�����Ԋu�i�b�j
    public float trailLifetime = 2.0f;  // �e�R�s�[�����݂��鎞�ԁi�b�j
    public float alpha = 0.5f;  // �������x�i0.0�`1.0�j

    private float timeSinceLastSpawn;

    void Update()
    {
        // �O��̐�������̎��Ԃ��v��
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            // �R�s�[�𐶐�
            GameObject trailObject = Instantiate(trailPrefab, transform.position, transform.rotation);

            // �R�s�[�̓����x��ݒ�
            SetAlpha(trailObject, alpha);

            // trailLifetime��ɃR�s�[��j��
            Destroy(trailObject, trailLifetime);

            // �^�C�}�[�����Z�b�g
            timeSinceLastSpawn = 0f;
        }
    }

    // �I�u�W�F�N�g�̓����x��ݒ肷��w���p�[���\�b�h
    void SetAlpha(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
        }
    }
}
