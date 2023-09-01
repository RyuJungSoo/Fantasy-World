using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionComponent : MonoBehaviour
{
    public bool isHp;
    public bool isMp;

    public float PlusHp = 20;
    public float PlusMp = 20;

    public AudioClip potionSoundEffect;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerComponent playerComponent = collision.GetComponent<PlayerComponent>();
            collision.GetComponent<AudioSource>().PlayOneShot(potionSoundEffect, 1);

            if (isHp == true)
            {
                playerComponent.Hp += PlusHp;
                if (playerComponent.Hp >= playerComponent.maxHp)
                {
                    playerComponent.Hp = playerComponent.maxHp;

                }
                UIManager.Instance.HpSliderUpdate();
            }

            if (isMp == true)
            {
                playerComponent.Mp += PlusMp;
                if (playerComponent.Mp >= playerComponent.maxMp)
                {
                    playerComponent.Mp = playerComponent.maxMp;

                }
                UIManager.Instance.MpSliderUpdate();
            }

            this.gameObject.SetActive(false);
        }
    }

    
}
