using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatComponent : MonoBehaviour
{

    public AudioClip Cheaton_Sound;

    private void Update()
    {

        if (Input.GetKey(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.J) && Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("ġƮ ON");
                GetComponent<AudioSource>().PlayOneShot(Cheaton_Sound);
                GameManager.Instance.LevelMaxCheat();
                UIManager.Instance.LevelUpUI_OFF(true);
            }
        }
    }
}
