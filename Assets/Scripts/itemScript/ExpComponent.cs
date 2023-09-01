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
                    // ������ �÷��� �� ���� ���
                    int levelPoint = 0;
                    float curExpMax = GameManager.Instance.playerMaxExp();
                    float curExp = GameManager.Instance.playerExp();
                    while (curExp >= curExpMax)
                    {
                        curExp -= curExpMax;
                        curExpMax += 5;
                        levelPoint++;
                    }

                    // ���� ���� �� �� 60������ ������ ������ 60�������� ���ߵ��� ��.
                    if (GameManager.Instance.level() + levelPoint > 60)
                        levelPoint = 60 - GameManager.Instance.level();
                    while (levelPoint > 0)
                    {
                        GameManager.Instance.levelUp();
                        levelPoint--;
                    }

                    // ����ġ ���� �� ó��
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
