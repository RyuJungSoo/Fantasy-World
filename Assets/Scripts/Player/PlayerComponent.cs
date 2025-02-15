using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    // ����
    public float speed = 4f;
    public int level = 0;
    public int Hplevel = 0;
    public int Mplevel = 0;
    public float Hp;
    public float maxHp;
    public float Mp;
    public float maxMp;
    public float Exp = 0;
    public float maxExp = 10;
    public float power;
    public float damage = 10;

    // ����
    public bool isAttack = false;
    public bool isAttacked = false;
    public bool isSlow = false;
    private float curTime;
    public float coolTime = 0; // �Ϲ� ���� ��Ÿ��
    public Transform pos;
    public Vector2 boxSize;
    public Transform center; // ���̾ ȸ�� �߽���
    private float OriginSpeed; // ���ο� ������ �ӽú���

    // ������Ʈ �� ���� ������Ʈ
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody2D playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    

    // Start is called before the first frame update
    void Start()
    {
        // ����� ������Ʈ�� ���� ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        this.level = level;
        this.Hplevel = Hplevel;
        this.Mplevel = Mplevel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.isGameover == true)
        {
            playerAnimator.SetBool("isAttack", false);
            return;
        }

        // ������ ����
        if(isAttack == false && playerRigidbody.bodyType != RigidbodyType2D.Static)
            Move();
        // ���� ����
        Attack();
    }

    // �Է°��� ���� ĳ���͸� ������
    private void Move()
    {
        // ��������� �̵��� �Ÿ� ���
        float x = playerInput.move_H * (speed+level*0.1f) * Time.deltaTime;
        float y = playerInput.move_V * (speed+level*0.1f) * Time.deltaTime;

        // �ִϸ��̼� ����
        if (playerInput.move_H != 0 || playerInput.move_V != 0)
        {
            playerAnimator.SetBool("isWalk", true);
        }

        else
        {
            playerAnimator.SetBool("isWalk", false);
        }

        // �¿� ������
        if (playerInput.move_H > 0)
        {
            transform.localScale = new Vector2(1,1);
            center.localScale = new Vector2(1, 1);
        }

        else if(playerInput.move_H < 0)
        {
            transform.localScale = new Vector2(-1, 1);
            center.localScale = new Vector2(-1, 1);
        }

        // ������ٵ� �̿��� ���� ������Ʈ ��ġ ����
        playerRigidbody.MovePosition(playerRigidbody.position + new Vector2(x,y));
    }

    private void Attack()
    {
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                StartCoroutine(AttackSound());
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Monster" && collider.GetComponent<MonsterComponent>().isDead == false)
                    {
                        //Debug.Log(collider.tag);
                        collider.GetComponent<MonsterComponent>().TakeDamage(damage);
                        if(collider.GetComponent<MonsterComponent>().isFreeze == false)
                            collider.transform.Translate(new Vector3(transform.localScale.x * 0.5f,0,0));
                        else
                            collider.transform.Translate(new Vector3(transform.localScale.x * 0.3f, 0, 0));
                    }
                }
                    isAttack = true;
                    playerAnimator.SetBool("isAttack", true);
                    curTime = coolTime;
            }
        }

        else
        {
            playerAnimator.SetBool("isAttack", false);
            curTime -= Time.deltaTime;
        }

        if (!Input.GetKey(KeyCode.LeftControl) && (!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.V) &&Input.anyKey || !Input.anyKey))
        {
            isAttack = false;
        }
    }



    private void ObjectHide()
    {
        gameObject.SetActive(false);
        UIManager.Instance.GameOverUI_ON(false);
    }


    public void TakeDamage(float damage)
    {
        isAttacked = true;

        Hp -= damage;
        if (Hp < 0)
            Hp = 0;
        UIManager.Instance.HpSliderUpdate();
        if (Hp < 0.5f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            transform.GetChild(1).gameObject.active = false;
            // ��� ó�� �� �ֺ� ���͵鿡�� �������� ���� ���ֱ� (�̷��� �� �ϸ� �����̸鼭 ������� �� ���͵��� ƨ�ܳ�������)
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position,new Vector2(5,5), 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" && collider.GetComponent<MonsterComponent>().isDead == false)
                {
                    collider.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
                }
            }
            playerRigidbody.bodyType = RigidbodyType2D.Static;
            
            GetComponent<BoxCollider2D>().enabled = false; // �浹 �ݶ��̴� ����
            GameManager.Instance.playSoundEffect(5, 5);
            GameManager.Instance.EndGame();
            playerAnimator.SetTrigger("Die");
            Invoke("ObjectHide", 1.25f);
        }

        isAttacked = false;
    }


    public void Slow(float SlowTime) // ���ο� �����
    {
        if (isSlow)
            return;

        isSlow = true;
        GetComponent<SpriteRenderer>().color = new Color(255/255f,30/255f,219/255f); // ����Ƽ������ ������ 0���� 1������ �Ǽ������� ��ȯ�ؼ� ������ �������
        OriginSpeed = speed;
        speed = OriginSpeed - 0.5f;
        Invoke("SlowStop", SlowTime);
    }

    private void SlowStop()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        speed = OriginSpeed;
        isSlow = false;
    }

    private void OnDrawGizmos() // ���� ���� �ð�ȭ 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    IEnumerator AttackSound()
    {
        if (!GameManager.Instance.playerAudio.isPlaying)
        {
            GameManager.Instance.playerAudio.Play();
        }
        yield return new WaitForSeconds(0.05f);
    }
}
