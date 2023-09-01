using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThunderSkillComponent : MonoBehaviour
{
    // �÷��̾� ���� �� ��Ÿ��
    private PlayerComponent playerComponent; // �÷��̾��� ���� ������ �˷��ִ� ������Ʈ
    private Rigidbody2D playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    public float MpUse = 15; // Mp ��뷮
    public float damage = 3; // ���� ������
    private float curTime;
    public float skillTime = 10f; // ��ų �ߵ� �ð�
    public float coolTime = 20f; // Thunder ��ų ��Ÿ��

    public int SkillLevel = 0; // ��ų ����
    public bool isOn = false; // ��ų�ߵ� ����

    public float radius = 2; // Thunder ����
    public ParticleSystem effect;

    public Image cool_skill; // ��Ÿ�� �̹���
    public Image continue_skill; // ����Ÿ�� �̹���

    // Start is called before the first frame update
    void Start()
    {
        // ����� ������Ʈ�� ���� ��������
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
            //Debug.Log("��Ÿ�� �� á��");
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
                    Debug.Log("Mp ����");
                    return;
                }

                //Debug.Log("���");
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
            //Debug.Log("��Ÿ�� �� á��");
            curTime -= Time.deltaTime;

        }

        if (!Input.GetKey(KeyCode.LeftControl) && (!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.V) && Input.anyKey || !Input.anyKey))
        {
            playerComponent.isAttack = false;
        }
    }

    private void OnDrawGizmos() // ���� ���� �ð�ȭ 
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
        print("��Ÿ�� �ڷ�ƾ ����");

        while (curTime >= 0)
        {
            cool_skill.fillAmount = (1.0f * (coolTime - curTime) / coolTime);
            yield return new WaitForFixedUpdate();
        }
        print("��Ÿ�� �ڷ�ƾ �Ϸ�");
        cool_skill.fillAmount = 0;
    }

    IEnumerator ContinueTime()
    {
        print("���ӽð� �ڷ�ƾ ����");

        while (curTime >= 0)
        {

            continue_skill.fillAmount = (1.0f * (skillTime - curTime) / skillTime);
            yield return new WaitForFixedUpdate();
        }
        print("���ӽð� �ڷ�ƾ �Ϸ�");
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
