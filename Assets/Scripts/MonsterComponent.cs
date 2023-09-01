using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent : MonoBehaviour
{
    // ����
    public float speed = 3f;
    public float Hp = 100;
    public float maxHp = 100;
    public float Damage = 2;


    // ����
    public int itemDropMode = 1;
    public bool isBoss = false;
    public bool isExpMax;
    public bool isAttack = false;
    public bool isAttacked = false;
    public bool isDead = false;
    public bool isFreeze = false;
    //private float curTime;
    //private float FreezeTimer;
    private float distance;

    // ������Ʈ �� ���ӿ�����Ʈ
    public GameObject player;
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
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (GameManager.Instance.isGameover == false && isDead == false && isFreeze == false)
        {


            Move(distance);
            
        }

        // ���� ������ �ִϸ��̼� idle�� ���ư���
        else
        {
            monsterAnimator.SetBool("isWalk", false);

        }

    }



    private void Move(float distance)
    {
        if (isAttack == false && isAttacked == false && isDead == false)
        {

            
            if(distance > 0)
                monsterAnimator.SetBool("isWalk", true);

            float Abs_x = Mathf.Abs(transform.localScale.x);
            float Abs_y = Mathf.Abs(transform.localScale.y);

            if (transform.position.x <= player.transform.position.x)
                transform.localScale = new Vector2(1*Abs_x, 1*Abs_y);
            else
                transform.localScale = new Vector2(-1*Abs_x, 1*Abs_y);


            Vector2 dirVec = player.GetComponent<Rigidbody2D>().position - monsterRig.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            monsterRig.MovePosition(monsterRig.position + nextVec);
        }

        else
            monsterAnimator.SetBool("isWalk", false);
    }

    private void Object_OFF()
    {
        //Destroy(this.gameObject);
        gameObject.SetActive(false);
    }

    public void Object_ON()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Collider2D>().enabled = true;
        Hp = maxHp;
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
            if (isBoss == false)
            {
                GameManager.Instance.ExpJewelDrop(isDead, dropTransform, isExpMax); // �ߺ� �Լ� ȣ���� ���� ���� isDead�� flagó�� �����.
                GameManager.Instance.ItemDrop(isDead, itemDropMode, dropTransform); // �ߺ� �Լ� ȣ���� ���� ���� isDead�� flagó�� �����.
            }
            isDead = true;
            if (isBoss == true) // ���� Ŭ���� ó��
            {
                UIManager.Instance.BossUI_OFF();
                GameManager.Instance.GameClear();
            }
            GetComponent<CapsuleCollider2D>().enabled = false;
            monsterAnimator.SetTrigger("Die");
            Invoke("Object_OFF", 1.25f);
        }
    }

    public void TakeDamage(float damage)
    {
        isAttacked = true;

        Hp -= damage;
        if (isBoss == true)
        {
            UIManager.Instance.BossHpUIUpdate(Hp, maxHp);
        }
        DeadCheck();
        
        isAttacked = false;
    }

    public void Freeze()
    {
        monsterRenderer.material.color = Color.cyan;
        isFreeze = true;

    }

    public void FreezeEnd()
    {
        monsterRenderer.material.color = Color.white;
        isFreeze = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && collision.GetComponent<BulletComponent>().isPlayer == true)
        {
            transform.Translate((transform.position - collision.transform.position).normalized * 0.5f);
            isAttacked = true;
            Hp -= collision.GetComponent<BulletComponent>().damage;
            DeadCheck();
            isAttacked = false;
        }

        
    }

    private void OnCollisionStay2D(Collision2D collision) // �Ϲ� ���� ������ ó��
    {
        if (collision.gameObject.CompareTag("Player") && isFreeze == false )
        {
            collision.gameObject.GetComponent<PlayerComponent>().TakeDamage(Time.deltaTime*Damage);
            if (isBoss == true)
            {

                Hp += Time.deltaTime * Damage;
                if (Hp >= maxHp)
                    Hp = maxHp;
                UIManager.Instance.BossHpUIUpdate(Hp, maxHp);
            }
        }
    }

}
