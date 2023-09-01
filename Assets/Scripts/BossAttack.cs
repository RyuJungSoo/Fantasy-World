using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    
    private MonsterComponent monsterComponent;
    private Rigidbody2D monsterRig;
    private CapsuleCollider2D monsterCollider;
    private Animator monsterAnimator; // 몬스터의 애니메이터
    public GameObject Center;


    private float curTime;
    public float teleport_Range = 5f;
    public float Warp_Cool = 3f; // 워프 쿨타임
    public float Shoot_Cool = 5f; // 탄막 쿨타임
    public float Slow_Cool = 6f; // 슬로우 쿨타임
    private int Warp_cnt = 0;
    public float ShootPower = 10f;

    public Transform pos; // Slow 중심점
    public float radius = 30; // Slow 범위
    public float SlowTime = 60; // Slow 디버프 시간

    public ParticleSystem effect;


    // Start is called before the first frame update
    void Start()
    {
        
        monsterComponent = GetComponent<MonsterComponent>();
        monsterRig = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();
        monsterCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameover == false)
        {
            if (GameManager.Instance.isBossStart == false)
            {
                
                return;
            }

            if (curTime <= 0 && monsterComponent.isAttacked == false && monsterComponent.isDead == false && monsterComponent.isFreeze == false)
            {
                int index = Random.Range(-2, 6);
                Debug.Log(index);
                monsterComponent.isAttacked = true;

                if (index <= 0 && Warp_cnt <= 4)
                    index = 1;


                if(index == 1)
                    Teleport();

                else if(index == 2 || index == 3 && Warp_cnt > 4)
                {
                    if (Center.active == true)
                        return;
                    Bullet();
                    Warp_cnt = 0;
                    Invoke("BulletShot", 5f);
                }

                else if (index == 5)
                {
                    Slow();
                }
            }

            else
            {
                monsterComponent.isAttacked = false;
                curTime -= Time.deltaTime;
            }
        }
    }


    private void Teleport() // 순간이동 패턴
    {
        Vector2 Player_pos = GameManager.Instance.playerTransform().position;
        Vector2 Warp_pos = Vector2.zero;
        int index = Random.Range(0, 4);

        if (index == 0)
            Warp_pos = Player_pos + new Vector2(Random.Range(3, teleport_Range + 1), Random.Range(3, teleport_Range + 1));
        else if (index == 1)
            Warp_pos = Player_pos + new Vector2(Random.Range(3, teleport_Range + 1), -1 * Random.Range(3, teleport_Range + 1));
        else if (index == 2)
            Warp_pos = Player_pos + new Vector2(-1 * Random.Range(3, teleport_Range + 1), Random.Range(3, teleport_Range + 1));
        else if (index == 3)
            Warp_pos = Player_pos + new Vector2(-1 * Random.Range(3, teleport_Range + 1), -1 * Random.Range(3, teleport_Range + 1));
        //Debug.Log("워프");
        transform.position = Warp_pos;
        curTime = Warp_Cool;
        Warp_cnt++;
    }
     
    private void Bullet() // 탄막 소환 패턴
    {

        GameManager.Instance.Boss_playSoundEffect(1, 5);
        if (monsterComponent.isFreeze == true)
        {
            Warp_cnt = 4; // 중간에 빙결됬을 경우 다시 실행할 수 있게
            return;
        }
        Center.SetActive(true);
        Center.GetComponent<CenterComponent>().Init();
    }

    private void BulletShot() // 탄막 쏘기 패턴
    {

        if (monsterComponent.isFreeze == true)
        {
            Warp_cnt = 4; // 중간에 빙결됬을 경우 다시 실행할 수 있게
            return;
        }

        Debug.Log("보스 탄막 발사");
        monsterAnimator.SetTrigger("Attack2");
        BulletComponent[] bulletComponents = Center.GetComponentsInChildren<BulletComponent>();
        for (int i = 0; i < bulletComponents.Length; i++)
        {
            bulletComponents[i].RotationToShot(ShootPower);
        }
        curTime = Shoot_Cool;
    }

    private void Slow()
    {
        Debug.Log("슬로우");
        GameManager.Instance.Boss_playSoundEffect(2, 5);
        monsterAnimator.SetTrigger("Attack1");
        effect.gameObject.active = true;
        effect.transform.position = transform.position;
        effect.Play();

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(effect.transform.position, radius);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                Debug.Log(collider.tag);
                collider.GetComponent<PlayerComponent>().Slow(SlowTime);
            }
        }
        curTime = Slow_Cool;
    }
}
