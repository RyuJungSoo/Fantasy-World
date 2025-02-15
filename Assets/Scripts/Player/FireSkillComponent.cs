using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSkillComponent : MonoBehaviour
{
    // �÷��̾� ���� �� ��Ÿ��
    public CenterComponent centerComponent;
    private PlayerComponent playerComponent; // �÷��̾��� ���� ������ �˷��ִ� ������Ʈ
    private Rigidbody2D playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    public float MpUse = 30; // Mp ��뷮
    private float curTime;
    public float skillTime = 8f; // ��ų �ߵ� �ð�
    public float coolTime = 30f; // Fire ���� ��Ÿ��

    public int SkillLevel=0; // ��ų ����
    public bool isOn = false; // ��ų�ߵ� ����
    private GameObject center;

    public Image cool_skill; // ��Ÿ�� �̹���
    public Image continue_skill; // ����Ÿ�� �̹���

    // Start is called before the first frame update
    void Start()
    {
        // ����� ������Ʈ�� ���� ��������
        playerComponent = GetComponent<PlayerComponent>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        center = this.transform.GetChild(1).gameObject;
        this.SkillLevel = SkillLevel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(curTime > 0)
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
        GameObject center = this.gameObject.transform.GetChild(1).gameObject;
        center.SetActive(true);
        center.GetComponent<CenterComponent>().LevelUp(0.2f, 1);
            
        if(isOn == false)
            center.SetActive(false);
        
    }

    public void Skill()
    {
        if (curTime <= 0 && playerComponent.isAttack == false)
        {
            if (Input.GetKey(KeyCode.V) && isOn == false)
            {
                if (GameManager.Instance.playerMp() < MpUse)
                {
                    Debug.Log("Mp ����");
                    return;
                }

                //Debug.Log("���̾�");
                GameManager.Instance.playSoundEffect(1, 5);
                GameManager.Instance.playerMpUse(MpUse);

                playerComponent.isAttack = true;
                isOn = true;
                playerAnimator.SetBool("isWalk", false);
                playerAnimator.SetTrigger("Skill");
                center.SetActive(true);
                curTime = skillTime;
                StartCoroutine(ContinueTime());
            }

        }

        if (!Input.GetKey(KeyCode.LeftControl) && (!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.V) && Input.anyKey || !Input.anyKey))
        {
            playerComponent.isAttack = false;
        }
    }

    IEnumerator CoolTime()
    {
        print("��Ÿ�� �ڷ�ƾ ����");

        while (curTime >= 0)
        {
            cool_skill.fillAmount = (1.0f * (coolTime - curTime)/coolTime);
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
            center.SetActive(false);
            isOn = false;
            curTime = coolTime;
            StartCoroutine(CoolTime());
        }
        playerComponent.isAttack = false;
    }
}
