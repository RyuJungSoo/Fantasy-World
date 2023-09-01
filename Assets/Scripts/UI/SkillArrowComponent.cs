using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArrowComponent : MonoBehaviour
{
    public GameObject[] Skills;
    public AudioClip moveAudio;
    private int index = 0;
    public AudioSource audioSource;

    private void OnEnable()
    {
        index = 0;
        ArrowMove();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Skills[index].transform.GetComponentInChildren<LevelUpButtonComponent>().OnClick();
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
            if (index >= Skills.Length)
                index = 0;
            ArrowMove();
            audioSource.PlayOneShot(moveAudio);
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
            if (index < 0)
                index = Skills.Length - 1;
            ArrowMove();
            audioSource.PlayOneShot(moveAudio);
        }

        

    }

    private void ArrowMove()
    {
        float x = Skills[index].GetComponent<RectTransform>().anchoredPosition.x;
        float y = Skills[index].GetComponent<RectTransform>().anchoredPosition.y + 51.16292f;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);


    }
}
