using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트가 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour
{
    public string moveVAxisName = "Vertical"; // 위아래 움직임을 위한 입력축 이름
    public string moveHAxisName = "Horizontal"; // 앞뒤 움직임을 위한 입력축 이름

    // 값 할당은 내부에서만 가능
    public float move_V { get; private set; } // 감지된 위아래 움직임 입력값
    public float move_H { get; private set; } // 감지된 앞뒤 움직임 입력값

    // Update is called once per frame
    void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않음
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            move_V = 0;
            return;
        }

        // move_V에 관한 입력 감지
        move_V = Input.GetAxis(moveVAxisName);
        // move_H에 관한 입력 감지
        move_H = Input.GetAxis(moveHAxisName);
    }
}
