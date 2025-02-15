using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeScript : MonoBehaviour
{
    // 쿨타임 테스트 용 코드
    public Image img_skill;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
            //StartCoroutine(CoolTime(3f));
    }

    IEnumerator CoolTime (float cool)
    {
        print("쿨타임 코루틴 실행");
  
        while (cool > 1.0f)
        {
            cool -= Time.deltaTime;
            img_skill.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
        print("쿨타임 코루틴 완료");
    }
}
