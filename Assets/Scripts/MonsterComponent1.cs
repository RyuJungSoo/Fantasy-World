using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent1 : MonoBehaviour
{
    // ����
    public float speed = 3f;
    public float Hp = 100;
    public float maxHp = 100;
    public float Damage = 5;
    public float meleeDamage = 2;

    // ����
    public bool isRangeAttack; // Ʈ���� ���� ó������ melee ���� ���� ���� ó������ ����
    public int itemDropMode = 1;
    public bool isExpMax;
    public bool isAttack = false;
    public bool isAttacked = false;
    public bool isDead = false;
    public bool isFreeze = false;
    private float curTime;
    public float coolTime = 1f; // �Ϲ� ���� ��Ÿ��
    public Transform pos;
    public Vector2 boxSize;
    private float FreezeTimer;

    // ������Ʈ �� ���ӿ�����Ʈ
    private GameObject player;
    private Rigidbody2D monsterRig;
    private Animator monsterAnimator; // ������ �ִϸ�����
    private Renderer monsterRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        monsterRig = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();
        monsterRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
            return;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (GameManager.Instance.isGameover == false && isDead == false && isFreeze == false)
        {
            Move(distance);
            if(isRangeAttack == true)
                RangeAttack();
        }

        // ���� ������ �ִϸ��̼� idle�� ���ư���
        else
        {
            monsterAnimator.SetBool("isWalk", false);
            monsterAnimator.SetBool("isAttack", false);
            if (isFreeze == true)
            {
                FreezeTimer -= Time.deltaTime;
                if (FreezeTimer <= 0)
                {
                    monsterRenderer.material.color = Color.white;
                    isFreeze = false;
                }
            }
        }

    }

    private void Move(float distance)
    {
        if (isAttack == false && isAttacked == false && isDead == false)
        {
            if(distance > 0)
                monsterAnimator.SetBool("isWalk", true);
            if (transform.position.x <= player.transform.position.x)
                transform.localScale = new Vector2(1, 1);
            else
                transform.localScale = new Vector2(-1, 1);

            //transform.LookAt(player.transform);
            //monsterRig.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); // �ӵ� �ݿ��� �� �� �Ǵ� �� ����.

            Vector2 dirVec = player.GetComponent<Rigidbody2D>().position - monsterRig.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            monsterRig.MovePosition(monsterRig.position + nextVec);
        }

        else
            monsterAnimator.SetBool("isWalk", false);
    }

    private void RangeAttack() // ���� ���� ������ ó��
    {
        if (curTime <= 0 && isAttacked == false)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            if(collider2Ds.Length > 0 )
            { 
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Player")
                    {
                        isAttack = true;
                        monsterAnimator.SetBool("isAttack", true);
                        //Debug.Log(collider.tag);
                        collider.GetComponent<PlayerComponent>().TakeDamage(meleeDamage);
                        curTime = coolTime;
                    }
                }
                
            }
        }

        else
        {
            isAttack = false;
            monsterAnimator.SetBool("isAttack", false);
            curTime -= Time.deltaTime;
        }
    }

    private void Object_OFF()
    {
        gameObject.SetActive(false);
    }

    public void Object_ON()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Collider2D>().enabled = true;
        Hp = maxHp;
        isAttack = false;
        isAttacked = false;
        isDead = false;
        isFreeze = false;
    }

    private void DeadCheck()
    {
        if (Hp <= 0)
        {
            GameManager.Instance.kill += 1;
            if(isFreeze == true)
                monsterRenderer.material.color = Color.white;
            
            monsterRig.velocity = new Vector3(0, 0); // ���� �����
            Transform dropTransform = transform;
            GameManager.Instance.ExpJewelDrop(isDead, dropTransform, isExpMax); // �ߺ� �Լ� ȣ���� ���� ���� isDead�� flagó�� �����.
            GameManager.Instance.ItemDrop(isDead, itemDropMode, dropTransform); // �ߺ� �Լ� ȣ���� ���� ���� isDead�� flagó�� �����.
            isDead = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
            monsterAnimator.SetTrigger("Die");
            Invoke("Object_OFF", 1.25f);
        }
    }

    public void TakeDamage(float damage)
    {
        isAttacked = true;
        Hp -= damage;
        DeadCheck();
        isAttacked = false;
    }

    public void Freeze(float FreezeTime)
    {
        monsterRenderer.material.color = Color.cyan;
        isFreeze = true;
        FreezeTimer = FreezeTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            isAttacked = true;
            Hp -= collision.GetComponent<BulletComponent>().damage;
            DeadCheck();
            isAttacked = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) // �Ϲ� ���� ������ ó��
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerComponent>().TakeDamage(Time.deltaTime*Damage);
        }
    }

    private void OnDrawGizmos() // ���� ���� �ð�ȭ 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
