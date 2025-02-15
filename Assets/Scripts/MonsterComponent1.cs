using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent1 : MonoBehaviour
{
    // 스탯
    public float speed = 3f;
    public float Hp = 100;
    public float maxHp = 100;
    public float Damage = 5;
    public float meleeDamage = 2;

    // 변수
    public bool isRangeAttack; // 트리거 공격 처리인지 melee 범위 감지 공격 처리인지 유무
    public int itemDropMode = 1;
    public bool isExpMax;
    public bool isAttack = false;
    public bool isAttacked = false;
    public bool isDead = false;
    public bool isFreeze = false;
    private float curTime;
    public float coolTime = 1f; // 일반 공격 쿨타임
    public Transform pos;
    public Vector2 boxSize;
    private float FreezeTimer;

    // 컴포넌트 및 게임오브젝트
    private GameObject player;
    private Rigidbody2D monsterRig;
    private Animator monsterAnimator; // 몬스터의 애니메이터
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

        // 게임 오버시 애니메이션 idle로 돌아가기
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
            //monsterRig.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); // 속도 반영이 잘 안 되는 거 같음.

            Vector2 dirVec = player.GetComponent<Rigidbody2D>().position - monsterRig.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            monsterRig.MovePosition(monsterRig.position + nextVec);
        }

        else
            monsterAnimator.SetBool("isWalk", false);
    }

    private void RangeAttack() // 직접 공격 데미지 처리
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
            
            monsterRig.velocity = new Vector3(0, 0); // 관성 지우기
            Transform dropTransform = transform;
            GameManager.Instance.ExpJewelDrop(isDead, dropTransform, isExpMax); // 중복 함수 호출을 막기 위해 isDead를 flag처럼 사용함.
            GameManager.Instance.ItemDrop(isDead, itemDropMode, dropTransform); // 중복 함수 호출을 막기 위해 isDead를 flag처럼 사용함.
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

    private void OnCollisionStay2D(Collision2D collision) // 일반 공격 데미지 처리
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerComponent>().TakeDamage(Time.deltaTime*Damage);
        }
    }

    private void OnDrawGizmos() // 공격 범위 시각화 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
