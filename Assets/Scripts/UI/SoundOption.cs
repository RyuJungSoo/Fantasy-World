using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOption : MonoBehaviour
{
    // ����� �ͼ�
    public AudioMixer audioMixer;

    // �����̴�
    public Slider BgmSlider;
    public Slider SfxSlider;
    public bool isUpdate; // ���θ޴� �����ٿ� ����ȭ��Ű�� ���� bool��

    public void OnEnable()
    {
        if (isUpdate)
        {
            float volume;
            audioMixer.GetFloat("BGM", out volume);
            BgmSlider.value = Mathf.Pow(10, volume/20);
            isUpdate = false;
        }
    }

    // ���� ����
    public void SetBgmVolme()
    {
        // �α� ���� �� ����
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
        //audioMixer.SetFloat("BGM", BgmSlider.value);
    }

    public void SetSFXVolme()
    {
        // �α� ���� �� ����
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
        //audioMixer.SetFloat("SFX", SfxSlider.value);
    }
}
