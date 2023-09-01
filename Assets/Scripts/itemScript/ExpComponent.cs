using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpComponent : MonoBehaviour
{
    public float PlusExp = 2;
    public AudioClip ExpSoundEffect;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {


            collision.GetComponent<AudioSource>().PlayOneShot(ExpSoundEffect, 3);

            if (GameManager.Instance.level() < 60)
            {
                collision.GetComponent<PlayerComponent>().Exp += PlusExp;

                if (GameManager.Instance.playerExp() >= GameManager.Instance.playerMaxExp())
                {
                    // 앞으로 올려야 할 레벨 계산
                    int levelPoint = 0;
                    float curExpMax = GameManager.Instance.playerMaxExp();
                    float curExp = GameManager.Instance.playerExp();
                    while (curExp >= curExpMax)
                    {
                        curExp -= curExpMax;
                        curExpMax += 5;
                        levelPoint++;
                    }

                    // 만약 레벨 업 후 60레벨을 넘으면 강제로 60레벨에서 멈추도록 함.
                    if (GameManager.Instance.level() + levelPoint > 60)
                        levelPoint = 60 - GameManager.Instance.level();
                    while (levelPoint > 0)
                    {
                        GameManager.Instance.levelUp();
                        levelPoint--;
                    }

                    // 경험치 남은 거 처리
                    if (GameManager.Instance.level() < 60)
                        GameManager.Instance.playerSetExp(curExp);
                    else
                        GameManager.Instance.playerSetExp(0);
                    UIManager.Instance.LevelUpUI_ON();
                }
                UIManager.Instance.ExpSliderUpdate();
            }

            this.gameObject.SetActive(false);
        }
    }

    
}
