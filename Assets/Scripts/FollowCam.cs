using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target; // ī�޶� ���� ���
    float offsetX; // ī�޶�� ��� ������ X�� ������

    void Start()
    {
        // target�� �������� ������, ī�޶� �ƹ��͵� ������ �ʵ��� ��
        if (target == null)
            return;

        // ī�޶��� ���� ��ġ�� ����� X ��ġ ���̸� ���������� ����
        offsetX = transform.position.x - target.position.x;
    }

    void Update()
    {
        // target�� null�̶�� ī�޶� �ƹ��͵� ������ ����
        if (target == null)
            return;

        // ���� ī�޶� ��ġ�� ����
        Vector3 pos = transform.position;
        // ī�޶��� X ��ġ�� ����� X ��ġ�� �������� ���� ����
        pos.x = target.position.x + offsetX;
        // ���� ���ο� ��ġ�� ī�޶� �̵�
        transform.position = pos;
    }
}
