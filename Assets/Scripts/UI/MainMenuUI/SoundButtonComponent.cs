using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonComponent : MonoBehaviour
{

    public GameObject UI;
    public void OnClick()
    {
        if (UI.active == false)
            UI.active = true;
        else
            UI.active = false;
    }
}
