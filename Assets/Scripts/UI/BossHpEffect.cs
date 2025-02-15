using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpEffect : MonoBehaviour
{
    private Image BossHpBar;
    private SpriteRenderer spriteRenderer;
    private float a = 0;

    private void Awake()
    {
        BossHpBar = GetComponent<Image>();
        spriteRenderer = GameObject.Find("Boss").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.Instance.isBossStart == false)
        {
            if (BossHpBar.fillAmount == 0)
            {
                a += Time.unscaledDeltaTime;
                spriteRenderer.color = new Color(1, 1, 1, a);
            }

            if (a >= 1)
            {
                BossHpBar.fillAmount += 1 * Time.unscaledDeltaTime;
            }

            if (BossHpBar.fillAmount >= 1 && a >= 1)
            {
                BossHpBar.fillAmount = 1;
                spriteRenderer.color = new Color(1, 1, 1, 1);
                BossStart();
            }
        }
    }

    void BossStart()
    {
        GameManager.Instance.isBossStart = true;
        GameManager.Instance.Resume();
        enabled = false;
    }
}
