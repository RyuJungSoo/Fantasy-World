using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �÷��̾� ĳ���͸� �����ϱ� ���� ����� �Է��� ����
// ������ �Է°��� �ٸ� ������Ʈ�� ����� �� �ֵ��� ����
public class PlayerInput : MonoBehaviour
{
    public string moveVAxisName = "Vertical"; // ���Ʒ� �������� ���� �Է��� �̸�
    public string moveHAxisName = "Horizontal"; // �յ� �������� ���� �Է��� �̸�
    /*public string attackButtonName = "Attack"; // ������ ���� �Է� ��ư �̸�
    public string skill1ButtonName = "Skill1"; // ��ų1�� ���� �Է� ��ư �̸�
    public string skill2ButtonName = "Skill2"; // ��ų2�� ���� �Է� ��ư �̸�
    public string skill3ButtonName = "Skill3"; // ��ų3�� ���� �Է� ��ư �̸�
    public string skill4ButtonName = "Skill4"; // ��ų4�� ���� �Է� ��ư �̸�*/

    // �� �Ҵ��� ���ο����� ����
    public float move_V { get; private set; } // ������ ���Ʒ� ������ �Է°�
    public float move_H { get; private set; } // ������ �յ� ������ �Է°�
    /*public bool attack { get; private set; } // ������ ���� �Է°�
    public bool skill1 { get; private set; } // ������ ��ų1 �Է°�
    public bool skill2 { get; private set; } // ������ ��ų2 �Է°�
    public bool skill3 { get; private set; } // ������ ��ų3 �Է°�
    public bool skill4 { get; private set; } // ������ ��ų4 �Է°�*/


    // Update is called once per frame
    void Update()
    {
        // ���ӿ��� ���¿����� ����� �Է��� �������� ����
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            move_V = 0;
            /*move_H = 0;
            attack = false;
            skill1 = false;
            skill2 = false;
            skill3 = false;
            skill4 = false;*/
            return;
        }

        // move_V�� ���� �Է� ����
        move_V = Input.GetAxis(moveVAxisName);
        // move_H�� ���� �Է� ����
        move_H = Input.GetAxis(moveHAxisName);
        /*// attack�� ���� �Է� ����
        attack = Input.GetButton(attackButtonName);
        // skill1�� ���� �Է� ����
        skill1 = Input.GetButton(skill1ButtonName);
        // skill2�� ���� �Է� ����
        skill2 = Input.GetButton(skill2ButtonName);
        // skill3�� ���� �Է� ����
        skill3 = Input.GetButton(skill3ButtonName);
        // skill4�� ���� �Է� ����
        skill4 = Input.GetButton(skill4ButtonName);*/

        
    }


}
