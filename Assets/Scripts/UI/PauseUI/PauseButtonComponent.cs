using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonComponent : MonoBehaviour
{
    public GameObject PauseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnClick();
    }
    public void OnClick()
    {
        if (GameManager.Instance.isGameover == true)
            return;

        if (PauseUI.activeSelf == true)
        {
            PauseUI.active = false;
            GameManager.Instance.Resume();
            GameManager.Instance.GetComponent<AudioSource>().UnPause();
            return;
        }

        PauseUI.active = true;
        GameManager.Instance.Stop();
        GameManager.Instance.GetComponent<AudioSource>().Pause();
    }
}
