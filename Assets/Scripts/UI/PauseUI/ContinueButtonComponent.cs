using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButtonComponent : MonoBehaviour
{
    public GameObject PauseUI;

    public void OnClick()
    {
        PauseUI.active = false;
        GameManager.Instance.Resume();
        GameManager.Instance.GetComponent<AudioSource>().UnPause();
    }
}
