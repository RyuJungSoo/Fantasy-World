using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageButtonComponent : MonoBehaviour
{
    public GameObject UI;
    public bool isPrev;
    public void OnClick()
    {
        if (isPrev == false)
            UI.GetComponent<MainMenuUIController>().NextPage();
        else
            UI.GetComponent<MainMenuUIController>().PrevPage();
    }
}
