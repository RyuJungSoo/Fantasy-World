using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // OnDrawGizmos
#endif

public class MonsterBullet : MonoBehaviour
{
    private MonsterComponent monsterComponent;
    private Animator monsterAnimator; // 몬스터의 애니메이터
    private PlayerComponent playerComponent;

    // 변수
    private float curTime;
    public float coolTime = 3f; // 일반 공격 쿨타임
    public float radius = 20f; // 사정 거리
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

    private void BulletAttack() // 탄알 소환 및 발사
    {

        if (curTime <= 0 && monsterComponent.isAttacked == false && monsterComponent.isDead == false && monsterComponent.isFreeze == false)
        {


            if (PlayerCheck() == true)
            {
                

                monsterComponent.isAttack = true;
                monsterAnimator.SetTrigger("Attack");
                Fire();


                Debug.Log("탄알 발사");
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
            // '몬스터-플레이어 벡터'와 '몬스터 정면 벡터'를 내적 
            float dot = Vector2.Dot(interV.normalized, new Vector2(transform.localScale.x, 0));
            // 두 벡터 모두 단위 벡터이므로 내적 결가에 cos의 역을 취해서 theta를 구함
            theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
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
    private void OnDrawGizmos() // 공격 범위 시각화 
    {

        Handles.color = new Color(255f, 255f, 255f, 0.2f);
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.forward, new Vector3(transform.localScale.x, 0, 0), angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.forward, new Vector3(transform.localScale.x, 0, 0), -angleRange / 2, radius);
    }
#endif
}
