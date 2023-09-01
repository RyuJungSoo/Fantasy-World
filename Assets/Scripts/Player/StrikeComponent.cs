using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeComponent : MonoBehaviour
{

    // 컴포넌트 및 게임 오브젝트
    private PlayerComponent playerComponent; // 플레이어의 관한 정보를 알려주는 컴포넌트
    private Rigidbody2D playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    public float damage = 50;
    public float MpUse = 10; // Mp 사용량
    private float curTime;
    public float coolTime = 5f; // Strike 스킬 쿨타임

    public int SkillLevel = 0; // 스킬 레벨

    public Image cool_skill; // 쿨타임 이미지

    // Start is called before the first frame update
    void Start()
    {
        // 사용할 컴포넌트의 참조 가져오기
        playerComponent = GetComponent<PlayerComponent>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
            Strike();
        if (curTime > 0)
        {
            //Debug.Log("쿨타임 안 찼음");
            curTime -= Time.deltaTime;

        }
    }

    public void LevelUp()
    {
        if (SkillLevel < 10)
        {
            SkillLevel += 1;
            damage += 0.5f;
        }
    }

    public void Strike()
    {
        if (curTime <= 0 && playerComponent.isAttack == false)
        {
            
            if (Input.GetKey(KeyCode.Z))
            {
                if (GameManager.Instance.playerMp() < MpUse)
                {
                    Debug.Log("Mp 부족");
                    return;
                }

                //Debug.Log("스트라이크");
                GameManager.Instance.playSoundEffect(4,5);
                GameManager.Instance.playerMpUse(MpUse);
                
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(playerComponent.pos.position, playerComponent.boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Monster")
                    {
                        //Debug.Log(collider.tag);
                        collider.GetComponent<MonsterComponent>().TakeDamage(damage);
                        
                        collider.transform.Translate(new Vector3(transform.localScale.x * playerComponent.power, 0, 0));

                    }
                }

                
                playerComponent.isAttack = true;
                playerAnimator.SetBool("isWalk", false);
                playerAnimator.SetTrigger("Strike");
                curTime = coolTime;
                StartCoroutine(CoolTime(coolTime));
            }

            else
            {

                //playerComponent.AttackState_false();//playerComponent.isAttack = false;
                //playerAnimator.SetBool("isStrike", false);
            }


        }

        else
        {
            
            //Debug.Log("쿨타임 안 찼음");
            curTime -= Time.deltaTime;
        }

        if (!Input.GetKey(KeyCode.LeftControl) && (!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.V) && Input.anyKey || !Input.anyKey))
        {
            playerComponent.isAttack = false;
        }
    }

    IEnumerator CoolTime(float cool)
    {
        print("쿨타임 코루틴 실행");

        while (cool >= 0)
        {
            cool -= Time.deltaTime;
            cool_skill.fillAmount = (1.0f * (coolTime - cool) / coolTime);
            yield return new WaitForFixedUpdate();
        }
        print("쿨타임 코루틴 완료");
        cool_skill.fillAmount = 0;
    }

}
