using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeScript : MonoBehaviour
{
    // ��Ÿ�� �׽�Ʈ �� �ڵ�
    public Image img_skill;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
            //StartCoroutine(CoolTime(3f));
    }

    IEnumerator CoolTime (float cool)
    {
        print("��Ÿ�� �ڷ�ƾ ����");
  
        while (cool > 1.0f)
        {
            cool -= Time.deltaTime;
            img_skill.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
        print("��Ÿ�� �ڷ�ƾ �Ϸ�");
    }
}
