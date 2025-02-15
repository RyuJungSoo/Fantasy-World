using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOption : MonoBehaviour
{
    // 오디오 믹서
    public AudioMixer audioMixer;

    // 슬라이더
    public Slider BgmSlider;
    public Slider SfxSlider;
    public bool isUpdate; // 메인메뉴 볼륨바와 동기화시키기 위한 bool값

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

    // 볼륨 조절
    public void SetBgmVolme()
    {
        // 로그 연산 값 전달
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
        //audioMixer.SetFloat("BGM", BgmSlider.value);
    }

    public void SetSFXVolme()
    {
        // 로그 연산 값 전달
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
        //audioMixer.SetFloat("SFX", SfxSlider.value);
    }
}
