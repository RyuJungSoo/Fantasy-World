using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameManager;
    private static UIManager instance = null;

    // BarUI
    public GameObject Player_HpBar;
    public Text Player_HpBarText;
    public GameObject Player_MpBar;
    public Text Player_MpBarText;
    public GameObject Player_ExpBar;
    public Text Player_ExpBarText;
    private Slider Player_HpSlider;
    private Slider Player_MpSlider;
    private Slider Player_ExpSlider;

    // GameOverUI
    public GameObject GameOverUI;
    public GameObject Lose_Text;
    public GameObject Clear_Text;
    public TextMeshProUGUI PlayTime;
    public TextMeshProUGUI KillCount;
    public Text Player_levelText;

    // SkillUI
    public GameObject LevelUpUI;
    public GameObject SkillArrow;
    public TextMeshProUGUI HpLevel_Text;
    public TextMeshProUGUI MpLevel_Text;
    public TextMeshProUGUI Skill1Level_Text;
    public TextMeshProUGUI Skill2Level_Text;
    public TextMeshProUGUI Skill3Level_Text;
    public TextMeshProUGUI Skill4Level_Text;
    public TextMeshProUGUI StatPoint;

    // BossUI
    public GameObject BossUI;
    public Image BossHpBar;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }

    }


    // UI 매니저 인스턴스에 접근할 수 있는 프로퍼티
    public static UIManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    public void UIReset()
    {
        // 플레이어 Hp바 UI 초기화
        Player_HpSlider = Player_HpBar.GetComponent<Slider>();
        Player_HpSlider.maxValue = GameManager.Instance.playerMaxHp();
        Player_HpSlider.value = GameManager.Instance.playerHp();
        Player_HpBarText.text = GameManager.Instance.playerHp().ToString() + " / " + GameManager.Instance.playerMaxHp().ToString();

        // 플레이어 Mp바 UI 초기화
        Player_MpSlider = Player_MpBar.GetComponent<Slider>();
        Player_MpSlider.maxValue = GameManager.Instance.playerMaxMp();
        Player_MpSlider.value = GameManager.Instance.playerMp();
        Player_MpBarText.text = GameManager.Instance.playerMp().ToString() + " / " + GameManager.Instance.playerMaxMp().ToString();

        // 플레이어 Exp바 UI 초기화
        Player_ExpSlider = Player_ExpBar.GetComponent<Slider>();
        Player_ExpSlider.maxValue = GameManager.Instance.playerMaxExp();
        Player_ExpSlider.value = 0;

        // 플레이어 레벨 텍스트 초기화
        Player_levelText.text = "Lv 0";
    }

    public void HpSliderUpdate()
    {
        Player_HpSlider.maxValue = GameManager.Instance.playerMaxHp();
        Player_HpSlider.value = GameManager.Instance.playerHp();

        if (Player_HpSlider.value < 0)
            Player_HpSlider.value = 0;

        float Hp = Mathf.Round(GameManager.Instance.playerHp());
        Player_HpBarText.text =  Hp.ToString() + " / " + GameManager.Instance.playerMaxHp().ToString();
    }

    public void MpSliderUpdate()
    {
        Player_MpSlider.maxValue = GameManager.Instance.playerMaxMp();
        Player_MpSlider.value = GameManager.Instance.playerMp();
        Player_MpBarText.text = GameManager.Instance.playerMp().ToString() + " / " + GameManager.Instance.playerMaxMp().ToString();
    }

    public void ExpSliderUpdate()
    {
        Player_ExpSlider.value = GameManager.Instance.playerExp();
        Player_ExpSlider.maxValue = GameManager.Instance.playerMaxExp();
    }

    public void LevelUIUpdate()
    {
        if (GameManager.Instance.level() >= 60)
            Player_levelText.text = "Lv MAX";
        else
            Player_levelText.text = "Lv " + GameManager.Instance.level().ToString();
    }

    public void SkillLevelUIUpdate(int index, int level)
    {
        if (index == 0)
        {
            if (level >= 10)
                HpLevel_Text.text = "Lv MAX";
            else
                HpLevel_Text.text = "Lv " + level.ToString();
            HpSliderUpdate();
        }

        else if (index == 1)
        {
            if (level >= 10)
                MpLevel_Text.text = "Lv MAX";
            else
                MpLevel_Text.text = "Lv " + level.ToString();
            MpSliderUpdate();
        }

        else if (index == 2)
        {
            if (level >= 10)
                Skill1Level_Text.text = "Lv MAX";
            else
                Skill1Level_Text.text = "Lv " + level.ToString();
        }

        else if (index == 3)
        {
            if (level >= 10)
                Skill2Level_Text.text = "Lv MAX";
            else
                Skill2Level_Text.text = "Lv " + level.ToString();
        }

        else if (index == 4)
        {
            if (level >= 10)
                Skill3Level_Text.text = "Lv MAX";
            else
                Skill3Level_Text.text = "Lv " + level.ToString();
        }

        else if (index == 5)
        {
            if (level >= 10)
                Skill4Level_Text.text = "Lv MAX";
            else
                Skill4Level_Text.text = "Lv " + level.ToString();
        }
    }

    public void GameOverUI_ON(bool isClear)
    {
        GameOverUI.SetActive(true);

        if (isClear == false)
        {
            GameManager.Instance.ChangeBGM(1, 1);
            Lose_Text.active = true;
        }

        else
        {
            GameManager.Instance.ChangeBGM(2, 1);
            Clear_Text.active = true;
        }

        float Time = GameManager.Instance.GameTime;
        int min = Mathf.FloorToInt(Time / 60);
        int sec = Mathf.FloorToInt(Time % 60);
        PlayTime.text = string.Format("{0:D2} : {1:D2}", min, sec);

        KillCount.text = GameManager.Instance.kill.ToString();
    }

    public void StatPointUIUpdate()
    {
        StatPoint.text = GameManager.Instance.StatPoint.ToString();
    }
    public void LevelUpUI_ON()
    {
        GameManager.Instance.Stop();
        StatPointUIUpdate();
        LevelUpUI.SetActive(true);
        SkillArrow.SetActive(true);
        gameManager.GetComponent<AudioSource>().Pause();
    }

    public void LevelUpUI_OFF(bool isCheat)
    {
        if (isCheat)
        {
            GameManager.Instance.Resume();
            LevelUpUI.SetActive(false);
            SkillArrow.SetActive(false);
            gameManager.GetComponent<AudioSource>().UnPause();
            return;
        }

        else
        {
            if (GameManager.Instance.StatPoint > 0)
                return;
            GameManager.Instance.Resume();
            LevelUpUI.SetActive(false);
            gameManager.GetComponent<AudioSource>().UnPause();
        }
    }

    public void BossUI_ON()
    {
        BossUI.SetActive(true);
    }

    public void BossUI_OFF()
    {
        BossUI.SetActive(false);
    }

    public void MonsterHPUI_OFF()
    {
        GameObject[] objArr = GameObject.FindGameObjectsWithTag("MonsterHpBar");
        for (int i = 0; i < objArr.Length; i++)
        {
            objArr[i].active = false;    
        }
    }

    public void BossHpUIUpdate(float Hp, float MaxHp)
    {
        if (BossUI.active == false)
            return;
        BossHpBar.fillAmount = 1 * Hp / MaxHp;
    }
}
