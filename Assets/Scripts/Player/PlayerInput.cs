using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �÷��̾� ĳ���͸� �����ϱ� ���� ����� �Է��� ����
// ������ �Է°��� �ٸ� ������Ʈ�� ����� �� �ֵ��� ����
public class PlayerInput : MonoBehaviour
{
    public string moveVAxisName = "Vertical"; // ���Ʒ� �������� ���� �Է��� �̸�
    public string moveHAxisName = "Horizontal"; // �յ� �������� ���� �Է��� �̸�

    // �� �Ҵ��� ���ο����� ����
    public float move_V { get; private set; } // ������ ���Ʒ� ������ �Է°�
    public float move_H { get; private set; } // ������ �յ� ������ �Է°�

    // Update is called once per frame
    void Update()
    {
        // ���ӿ��� ���¿����� ����� �Է��� �������� ����
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            move_V = 0;
            return;
        }

        // move_V�� ���� �Է� ����
        move_V = Input.GetAxis(moveVAxisName);
        // move_H�� ���� �Է� ����
        move_H = Input.GetAxis(moveHAxisName);
    }
}
