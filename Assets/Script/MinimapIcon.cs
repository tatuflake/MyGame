using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MinimapIcon : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;              // �~�j�}�b�v�p�J����
    [SerializeField] private Transform iconTarget;              // �A�C�R���ɑΉ�����I�u�W�F�N�g�i���������j
    [SerializeField] private float rangeRadiusOffset = 1.0f;    // �\���͈͂̃I�t�Z�b�g

    // �K�v�ȃR���|�[�l���g
    private SpriteRenderer spriteRenderer;

    private float minimapRangeRadius;   // �~�j�}�b�v�̕\���͈�
    private float defaultPosY;          // �A�C�R���̃f�t�H���gY���W
    const float normalAlpha = 1.0f;     // �͈͓��̃A���t�@�l
    const float outRangeAlpha = 0.5f;   // �͈͊O�̃A���t�@�l

    private void Start()
    {
        minimapRangeRadius = minimapCamera.orthographicSize;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultPosY = transform.position.y;
    }

    private void Update()
    {
        DispIcon();
    }

    /// <summary>
    /// �A�C�R���\�����X�V����
    /// </summary>
    private void DispIcon()
    {
        // �A�C�R����\��������W
        var iconPos = new Vector3(iconTarget.position.x, defaultPosY, iconTarget.position.z);

        // �~�j�}�b�v�͈͓��̏ꍇ�͂��̂܂ܕ\������
        if (CheckInsideMap())
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, normalAlpha);
            transform.position = iconPos;
            return;
        }

        // �}�b�v�͈͊O�̏ꍇ�A�~�j�}�b�v�[�܂ł̃x�N�g�������߂Ĕ������ŕ\������
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, outRangeAlpha);
        var centerPos = new Vector3(minimapCamera.transform.position.x, defaultPosY, minimapCamera.transform.position.z);
        var offset = iconPos - centerPos;
        transform.position = centerPos + Vector3.ClampMagnitude(offset, minimapRangeRadius - rangeRadiusOffset);
    }

    /// <summary>
    /// �I�u�W�F�N�g���~�j�}�b�v�͈͓��ɂ��邩�m�F����
    /// </summary>
    /// <returns>�~�j�}�b�v�͈͓��̏ꍇ�Atrue��Ԃ�</returns>
    private bool CheckInsideMap()
    {
        var cameraPos = minimapCamera.transform.position;
        var targetPos = iconTarget.position;

        // ���������Ŕ��肷�邽�߁Ay��0�����ɂ���
        cameraPos.y = targetPos.y = 0;

        return Vector3.Distance(cameraPos, targetPos) <= minimapRangeRadius - rangeRadiusOffset;
    }
}
