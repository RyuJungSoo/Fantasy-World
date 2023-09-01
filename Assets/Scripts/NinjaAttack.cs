using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // OnDrawGizmos
#endif

public class NinjaAttack : MonoBehaviour
{
    private MonsterComponent monsterComponent;
    private CapsuleCollider2D monsterCollider;
    private Animator monsterAnimator; // ������ �ִϸ�����

    // ����
    public bool isMove = false;
    private float curTime;
    public float coolTime = 5f; // �Ϲ� ���� ��Ÿ��
    public float PauseTime = 0.2f; // ���� ���� �� ��
    public float radius = 20f; // ���� �Ÿ�
    public float angleRange = 80f;
    public float damage = 20f;

    //public float AttackSpeed = 4f;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3f;


    private Vector3 target_pos;

    void Start()
    {
        monsterComponent = GetComponent<MonsterComponent>();
        monsterAnimator = GetComponent<Animator>();
        monsterCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {

        if (GameManager.Instance.isGameover == false)
        {

            Pattern();
            if (isMove == true)
            {
                transform.position = Vector3.SmoothDamp(transform.position, target_pos, ref velocity, smoothTime);
                if ((transform.position.x >= target_pos.x - 0.1f && transform.position.x <= target_pos.x + 0.1f) || (transform.position.y >= target_pos.y - 0.1f && transform.position.y <= target_pos.y + 0.1f))
                {
                    Invoke("MoveStop", 0.5f);
                    
                }
            }
        }
    }


    private bool PlayerCheck()
    {
        Vector2 interV = monsterComponent.player.transform.position - transform.position;
        if (interV.magnitude <= radius)
        {
            // '����-�÷��̾� ����'�� '���� ���� ����'�� ���� 
            float dot = Vector2.Dot(interV.normalized, new Vector2(transform.localScale.x, 0));
            // �� ���� ��� ���� �����̹Ƿ� ���� �ᰡ�� cos�� ���� ���ؼ� theta�� ����
            float theta = Mathf.Acos(dot);
            // angleRange�� ���ϱ� ���� degree�� ��ȯ
            float degree = Mathf.Rad2Deg * theta;

            // �þ߰� �Ǻ�
            if (degree <= angleRange / 2f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    private void Pattern()
    {
        if (curTime <= 0 && monsterComponent.isAttacked == false && monsterComponent.isDead == false && monsterComponent.isFreeze == false)
        {

            if (PlayerCheck() == true)
            {

                monsterComponent.isAttack = true;
                monsterCollider.isTrigger = true;
                monsterAnimator.SetTrigger("Batto");
                Invoke("MoveStart", PauseTime);





                curTime = coolTime;

            }


        }

        else
        {
            curTime -= Time.deltaTime;
        }


    }



    void MoveStart()
    {
        target_pos = monsterComponent.player.transform.position;
        isMove = true;        
        monsterAnimator.SetTrigger("Attack");

    }

    void MoveStop()
    {
        isMove = false;
        monsterComponent.isAttack = false;
        monsterCollider.isTrigger = false;
        monsterAnimator.SetTrigger("Return");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() // ���� ���� �ð�ȭ 
    {

        Handles.color = new Color(255f, 255f, 0f, 0.2f);
        // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
        Handles.DrawSolidArc(transform.position, Vector3.forward, new Vector3(transform.localScale.x, 0, 0), angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.forward, new Vector3(transform.localScale.x, 0, 0), -angleRange / 2, radius);
    }
#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerComponent>().TakeDamage(damage);
        }
    }
}
