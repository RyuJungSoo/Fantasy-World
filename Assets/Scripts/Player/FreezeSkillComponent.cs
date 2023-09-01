using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeSkillComponent : MonoBehaviour
{
    // �÷��̾� ���� �� ��Ÿ��
    private PlayerComponent playerComponent; // �÷��̾��� ���� ������ �˷��ִ� ������Ʈ
    private Rigidbody2D playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    public float MpUse = 20; // Mp ��뷮
    private float curTime;
    public float skillTime = 10f; // ��ų �ߵ� �ð�
    public float coolTime = 22f; // Freeze ��ų ��Ÿ��

    public int SkillLevel = 0; // ��ų ����
    public bool isOn = false; // ��ų�ߵ� ����

    public Transform pos; // Freeze �߽���
    public float radius; // Freeze ����

    public ParticleSystem effect;

    public Image cool_skill; // ��Ÿ�� �̹���
    public Image continue_skill; // ����Ÿ�� �̹���

    private Collider2D[] collider2Ds; // ���� ���� ������ �迭;

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
        
    }

    public void Skill()
    {
        if (curTime <= 0 && playerComponent.isAttack == false)
        {


            if (Input.GetKey(KeyCode.X) && isOn == false )
            {
                if (GameManager.Instance.playerMp() < MpUse)
                {
                    Debug.Log("Mp ����");
                    return;
                }

                //Debug.Log("���ڵ�");
                effectPlay();
                GameManager.Instance.playSoundEffect(2, 5);
                GameManager.Instance.playerMpUse(MpUse);
                collider2Ds = Physics2D.OverlapCircleAll(playerComponent.pos.position, radius);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Monster")
                    {
                        //Debug.Log(collider.tag);
                        collider.GetComponent<MonsterComponent>().Freeze();
                    }
                }

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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pos.position, radius);
    }

    void effectPlay()
    {
        effect.gameObject.active = true;
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

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Monster")
            {
                //Debug.Log(collider.tag);
                collider.GetComponent<MonsterComponent>().FreezeEnd();
            }
        }

        print("���ӽð� �ڷ�ƾ �Ϸ�");
        continue_skill.fillAmount = 0;

        if (isOn == true)
        {
            isOn = false;
            curTime = coolTime;
            StartCoroutine(CoolTime());
        }
        playerComponent.isAttack = false;
    }
}
