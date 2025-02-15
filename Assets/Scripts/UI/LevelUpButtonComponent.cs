using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpButtonComponent : MonoBehaviour
{
    public int SkillIndex = 0;
    private bool flag;

    public void OnClick()
    {
        flag = GameManager.Instance.skillLevelUp(SkillIndex);

        if (flag == true)
        {
            GameManager.Instance.StatPoint--;
            UIManager.Instance.StatPointUIUpdate();
            UIManager.Instance.LevelUpUI_OFF(false);
        }
    }
}
