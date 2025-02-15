using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    public GameObject[] PageObject;
    public GameObject PrevButton;
    private int curIndex = 0;

    public void NextPage()
    {
        if (curIndex == 0)
            PrevButton.active = true;

        PageObject[curIndex++].active = false;

        if (curIndex >= PageObject.Length)
            curIndex = 0;
        PageObject[curIndex].active = true;
    }

    public void PrevPage()
    {
        PageObject[curIndex--].active = false;

        if (curIndex < 0)
            curIndex = PageObject.Length - 1;
        PageObject[curIndex].active = true;
    }

    public void PageReset()
    {
        PageObject[curIndex].active = false;
        curIndex = 0;
        PageObject[curIndex].active = true;
    }
}
