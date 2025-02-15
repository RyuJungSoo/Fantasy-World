using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    // 스탯
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

    // 변수
    public bool isAttack = false;
    public bool isAttacked = false;
    public bool isSlow = false;
    private float curTime;
    public float coolTime = 0; // 일반 공격 쿨타임
    public Transform pos;
    public Vector2 boxSize;
    public Transform center; // 파이어볼 회전 중심점
    private float OriginSpeed; // 슬로우 해제용 임시변수

    // 컴포넌트 및 게임 오브젝트
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody2D playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    

    // Start is called before the first frame update
    void Start()
    {
        // 사용할 컴포넌트의 참조 가져오기
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

        // 움직임 실행
        if(isAttack == false && playerRigidbody.bodyType != RigidbodyType2D.Static)
            Move();
        // 공격 실행
        Attack();
    }

    // 입력값에 따라 캐릭터를 움직임
    private void Move()
    {
        // 상대적으로 이동할 거리 계산
        float x = playerInput.move_H * (speed+level*0.1f) * Time.deltaTime;
        float y = playerInput.move_V * (speed+level*0.1f) * Time.deltaTime;

        // 애니메이션 변경
        if (playerInput.move_H != 0 || playerInput.move_V != 0)
        {
            playerAnimator.SetBool("isWalk", true);
        }

        else
        {
            playerAnimator.SetBool("isWalk", false);
        }

        // 좌우 뒤집기
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

        // 리지드바디를 이용해 게임 오브젝트 위치 변경
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
            // 사망 처리 시 주변 몬스터들에게 가해졌던 관성 없애기 (이렇게 안 하면 움직이면서 사망했을 때 몬스터들이 튕겨나가버림)
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position,new Vector2(5,5), 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster" && collider.GetComponent<MonsterComponent>().isDead == false)
                {
                    collider.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
                }
            }
            playerRigidbody.bodyType = RigidbodyType2D.Static;
            
            GetComponent<BoxCollider2D>().enabled = false; // 충돌 콜라이더 끄기
            GameManager.Instance.playSoundEffect(5, 5);
            GameManager.Instance.EndGame();
            playerAnimator.SetTrigger("Die");
            Invoke("ObjectHide", 1.25f);
        }

        isAttacked = false;
    }


    public void Slow(float SlowTime) // 슬로우 디버프
    {
        if (isSlow)
            return;

        isSlow = true;
        GetComponent<SpriteRenderer>().color = new Color(255/255f,30/255f,219/255f); // 유니티에서는 색상을 0부터 1사이의 실수값으로 변환해서 색상을 섞어넣음
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

    private void OnDrawGizmos() // 공격 범위 시각화 
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
