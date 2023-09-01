using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThunderSkillComponent : MonoBehaviour
{
    // 플레이어 관련 및 쿨타임
    private PlayerComponent playerComponent; // 플레이어의 관한 정보를 알려주는 컴포넌트
    private Rigidbody2D playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    public float MpUse = 15; // Mp 사용량
    public float damage = 3; // 번개 데미지
    private float curTime;
    public float skillTime = 10f; // 스킬 발동 시간
    public float coolTime = 20f; // Thunder 스킬 쿨타임

    public int SkillLevel = 0; // 스킬 레벨
    public bool isOn = false; // 스킬발동 여부

    public float radius = 2; // Thunder 범위
    public ParticleSystem effect;

    public Image cool_skill; // 쿨타임 이미지
    public Image continue_skill; // 지속타임 이미지

    // Start is called before the first frame update
    void Start()
    {
        // 사용할 컴포넌트의 참조 가져오기
        playerComponent = GetComponent<PlayerComponent>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        this.SkillLevel = SkillLevel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (curTime > 0)
        {
            //Debug.Log("쿨타임 안 찼음");
            curTime -= Time.deltaTime;

        }

        
        Skill();
    }

    public void LevelUp()
    {
        
            SkillLevel += 1;
            skillTime += 0.5f;
            radius += 0.2f;
        
    }

    public void Skill()
    {
        if (curTime <= 0 && playerComponent.isAttack == false)
        {


            if (Input.GetKey(KeyCode.C) && isOn == false )
            {
                if (GameManager.Instance.playerMp() < MpUse)
                {
                    Debug.Log("Mp 부족");
                    return;
                }

                //Debug.Log("썬더");
                effectPlay();
                GameManager.Instance.playSoundEffect(3, 5);
                GameManager.Instance.playerMpUse(MpUse);

                playerComponent.isAttack = true;
                isOn = true;
                playerAnimator.SetBool("isWalk", false);
                playerAnimator.SetTrigger("Skill");
                curTime = skillTime;
                StartCoroutine(ContinueTime());
            }

            else
            {
                

            }

        }



        if (isOn == true)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(playerComponent.pos.position, radius);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster")
                {
                    //Debug.Log(collider.tag);
                    
                    collider.GetComponent<MonsterComponent>().TakeDamage(damage);
                    collider.transform.Translate((collider.transform.position - transform.position).normalized*0.7f);
                }
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

    private void OnDrawGizmos() // 공격 범위 시각화 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(effect.transform.position, radius);
    }

    void effectPlay()
    {
        effect.gameObject.active = true;
        effect.gameObject.transform.localScale = new Vector3(radius, radius);
        effect.transform.position = transform.position;
        effect.Play();
    }

    IEnumerator CoolTime()
    {
        print("쿨타임 코루틴 실행");

        while (curTime >= 0)
        {
            cool_skill.fillAmount = (1.0f * (coolTime - curTime) / coolTime);
            yield return new WaitForFixedUpdate();
        }
        print("쿨타임 코루틴 완료");
        cool_skill.fillAmount = 0;
    }

    IEnumerator ContinueTime()
    {
        print("지속시간 코루틴 실행");

        while (curTime >= 0)
        {

            continue_skill.fillAmount = (1.0f * (skillTime - curTime) / skillTime);
            yield return new WaitForFixedUpdate();
        }
        print("지속시간 코루틴 완료");
        continue_skill.fillAmount = 0;

        if (isOn == true)
        {
            effect.gameObject.active = false;
            isOn = false;
            curTime = coolTime;
            StartCoroutine(CoolTime());
        }
        playerComponent.isAttack = false;
    }
}
