using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeComponent : MonoBehaviour
{

    // ������Ʈ �� ���� ������Ʈ
    private PlayerComponent playerComponent; // �÷��̾��� ���� ������ �˷��ִ� ������Ʈ
    private Rigidbody2D playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    public float damage = 50;
    public float MpUse = 10; // Mp ��뷮
    private float curTime;
    public float coolTime = 5f; // Strike ��ų ��Ÿ��

    public int SkillLevel = 0; // ��ų ����

    public Image cool_skill; // ��Ÿ�� �̹���

    // Start is called before the first frame update
    void Start()
    {
        // ����� ������Ʈ�� ���� ��������
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
            //Debug.Log("��Ÿ�� �� á��");
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
                    Debug.Log("Mp ����");
                    return;
                }

                //Debug.Log("��Ʈ����ũ");
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
            
            //Debug.Log("��Ÿ�� �� á��");
            curTime -= Time.deltaTime;
        }

        if (!Input.GetKey(KeyCode.LeftControl) && (!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.V) && Input.anyKey || !Input.anyKey))
        {
            playerComponent.isAttack = false;
        }
    }

    IEnumerator CoolTime(float cool)
    {
        print("��Ÿ�� �ڷ�ƾ ����");

        while (cool >= 0)
        {
            cool -= Time.deltaTime;
            cool_skill.fillAmount = (1.0f * (coolTime - cool) / coolTime);
            yield return new WaitForFixedUpdate();
        }
        print("��Ÿ�� �ڷ�ƾ �Ϸ�");
        cool_skill.fillAmount = 0;
    }

}
