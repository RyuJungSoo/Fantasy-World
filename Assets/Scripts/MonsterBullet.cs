using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // OnDrawGizmos
#endif

public class MonsterBullet : MonoBehaviour
{
    private MonsterComponent monsterComponent;
    private Animator monsterAnimator; // ������ �ִϸ�����
    private PlayerComponent playerComponent;

    // ����
    private float curTime;
    public float coolTime = 3f; // �Ϲ� ���� ��Ÿ��
    public float radius = 20f; // ���� �Ÿ�
    public float angleRange = 80f;
    public float damage = 15f;
    public float speed = 5f;
    public int per = 0;
    private Vector2 interV;
    private float theta;

    // Start is called before the first frame update
    void Start()
    {
        monsterComponent = GetComponent<MonsterComponent>();
        monsterAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isGameover == false)
            BulletAttack();
    }

    private void BulletAttack() // ź�� ��ȯ �� �߻�
    {

        if (curTime <= 0 && monsterComponent.isAttacked == false && monsterComponent.isDead == false && monsterComponent.isFreeze == false)
        {


            if (PlayerCheck() == true)
            {
                

                monsterComponent.isAttack = true;
                monsterAnimator.SetTrigger("Attack");
                Fire();


                Debug.Log("ź�� �߻�");
                curTime = coolTime;
                 

            }


        }

        else
        {
            monsterComponent.isAttack = false;
            curTime -= Time.deltaTime;
        }
    }

    private void Fire()
    {
        GameObject bullet;


        bullet = GameManager.Instance.pool.Get(1);
        bullet.transform.position = transform.position + new Vector3(transform.localScale.x * 2f, 0, 0);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector2.up, interV.normalized);
        bullet.GetComponent<BulletComponent>().Init(damage, per, interV.normalized*speed);
        bullet.transform.localScale = transform.localScale;
    }

    private bool PlayerCheck()
    {
        interV = monsterComponent.player.transform.position - transform.position;
        if (interV.magnitude <= radius)
        {
            // '����-�÷��̾� ����'�� '���� ���� ����'�� ���� 
            float dot = Vector2.Dot(interV.normalized, new Vector2(transform.localScale.x, 0));
            // �� ���� ��� ���� �����̹Ƿ� ���� �ᰡ�� cos�� ���� ���ؼ� theta�� ����
            theta = Mathf.Acos(dot);
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

#if UNITY_EDITOR
    private void OnDrawGizmos() // ���� ���� �ð�ȭ 
    {

        Handles.color = new Color(255f, 255f, 255f, 0.2f);
        // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
        Handles.DrawSolidArc(transform.position, Vector3.forward, new Vector3(transform.localScale.x, 0, 0), angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.forward, new Vector3(transform.localScale.x, 0, 0), -angleRange / 2, radius);
    }
#endif
}
