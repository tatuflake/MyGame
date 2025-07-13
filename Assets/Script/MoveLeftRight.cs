using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    /// <summary>
    /// ���E�ɓ����͈͂̍ő勗��
    /// </summary>
    [SerializeField]
    private float moveDistance = 5.0f;

    /// <summary>
    /// �ړ����x
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2.0f;

    /// <summary>
    /// �I�u�W�F�N�g�̏����ʒu
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// ���݂̈ړ������i1: �E, -1: ���j
    /// </summary>
    [SerializeField]
    private int moveDirection = 1;

    void Start()
    {
        // �I�u�W�F�N�g�̏����ʒu��ۑ�
        startPosition = transform.position;
    }

    void Update()
    {
        // �V�����ʒu���v�Z
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance) * moveDirection;
        transform.position = startPosition + new Vector3(offset, 0, 0);

        // �ړ����ő�͈͂ɒB����������𔽓]
        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            moveDirection *= -1;
        }
    }
}
