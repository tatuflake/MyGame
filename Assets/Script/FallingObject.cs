using System.Collections;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public AudioClip spawnSound;    // �I�u�W�F�N�g�������������̃T�E���h
    public AudioClip impactSound;   // �I�u�W�F�N�g���h���������̃T�E���h
    public GameObject bulletPrefab; // �e�̃v���n�u
    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isFixed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // �I�u�W�F�N�g�������������̃T�E���h���Đ�
        audioSource.PlayOneShot(spawnSound);

        // �d�͂�K�p���Đ^���ɗ���������
        rb.useGravity = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isFixed)
        {
            // �I�u�W�F�N�g�������ɏՓ˂������ɉ����Đ�
            audioSource.PlayOneShot(impactSound);

            // �I�u�W�F�N�g���Œ�
            rb.isKinematic = true; // �������Z�𖳌��ɂ��ČŒ�
            isFixed = true;

            // 2�b��ɃI�u�W�F�N�g������������
            StartCoroutine(DestroyAfterDelay(2f));
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // �e���l�������ɔ��˂��鏈��
        FireBulletsInAllDirections();

        // �I�u�W�F�N�g������������
        Destroy(gameObject);
    }

    void FireBulletsInAllDirections()
    {
        int bulletCount = 10;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * Vector3.forward;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = direction * 10f; // �e�̑��x��ݒ�

            // 0.1�b��ɒe������������
            Destroy(bullet, 0.1f);
        }
    }
}
