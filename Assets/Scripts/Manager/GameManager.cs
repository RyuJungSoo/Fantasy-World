using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public bool isLive; // 일시 중지 여부
    public bool isBossStart; // 보스 시작 여부
    public bool isGameover { get; private set; } // 게임오버 유무
    public AudioClip[] audioClips;
    public AudioClip[] BGMClips;

    // 게임오브젝트
    private GameObject player;

    // 컴포넌트
    private PlayerComponent playerComponent;
    private AudioSource BgmAudio;
    public AudioSource playerAudio;
    public AudioSource bossAudio;
    public PoolManager pool;

    // 변수
    public float GameTime;
    public int kill;
    public int StatPoint = 0;

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

    void Update()
    {
        if(isGameover == false)
            GameTime += Time.deltaTime;

    }

    // 게임 매니저 인스턴스에 접근할 수 있는 프로퍼티
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    private void Start()
    {
        // 오브젝트 초기화
        player = GameObject.Find("Player");

        // 컴포넌트 초기화
        playerComponent = player.GetComponent<PlayerComponent>();
        playerAudio = player.GetComponent<AudioSource>();
        BgmAudio = GetComponent<AudioSource>();
        // UI 초기화
        UIManager.Instance.UIReset();
    }

    // 플레이어 transform 반환
    public Transform playerTransform()
    {
        return player.transform;
    }

    // 플레이어 효과음 출력
    public void playSoundEffect(int index, float volume)
    {
        playerAudio.PlayOneShot(audioClips[index], volume);
    }
    public void Boss_playSoundEffect(int index, float volume)
    {
        bossAudio.PlayOneShot(audioClips[index], volume);
    }
    public void ChangeBGM(int index, float volume)
    {
        BgmAudio.clip = BGMClips[index];
        BgmAudio.volume = volume;
        BgmAudio.Play();
    }

    // 플레이어 Hp 반환
    public float playerHp()
    {
        return playerComponent.Hp;
    }
    // 플레이어 MaxHp 반환
    public float playerMaxHp()
    {
        return playerComponent.maxHp;
    }
    // 플레이어 Mp 반환
    public float playerMp()
    {
        return playerComponent.Mp;
    }
    // 플레이어 MaxMp 반환
    public float playerMaxMp()
    {
        return playerComponent.maxMp;
    }
    // 플레이어 Mp 사용 처리
    public void playerMpUse(float MpUse)
    {
        playerComponent.Mp -= MpUse;
        if (playerComponent.Mp < 0)
            playerComponent.Mp = 0;
        UIManager.Instance.MpSliderUpdate();
    }

    // 플레이어 Exp 반환
    public float playerExp()
    {
        return playerComponent.Exp;
    }
    // 플레이어 maxExp 반환
    public float playerMaxExp()
    {
        return playerComponent.maxExp;
    }
    public void playerSetExp(float Exp)
    {
        playerComponent.Exp = Exp;
    }
    // 플레이어 레벨 반환
    public int level()
    {
        return playerComponent.level;
    }



    // 아이템
    public void ExpJewelDrop(bool DeadFlag, Transform pos, bool isMax)
    {
        if (DeadFlag == false)
        {
            //GameObject ExpJewel = Instantiate(itemPrefebs[0], pos.position, Quaternion.identity);
            GameObject ExpJewel;
            if (isMax == false)
                ExpJewel = GameManager.Instance.pool.itemGet(0, pos);
            else
                ExpJewel = GameManager.Instance.pool.itemGet(5, pos);
        }
    }

    public void levelUp()
    {
        StatPoint += 1;
        playerComponent.level += 1;
        playerComponent.power += 0.01f;
        playerComponent.damage += 0.5f;
        playerComponent.maxExp += 5;
        UIManager.Instance.LevelUIUpdate();
    }

    public bool skillLevelUp(int index)
    {
        if (index == 0)
        {
            if (playerComponent.Hplevel < 10)
            {
                playerComponent.Hplevel += 1;
                playerComponent.maxHp += 10;
            }
            else
                return false;
            UIManager.Instance.SkillLevelUIUpdate(index, playerComponent.Hplevel);
        }
        else if (index == 1)
        {
            if (playerComponent.Mplevel < 10)
            {
                playerComponent.Mplevel += 1;
                playerComponent.maxMp += 10;
            }
            else
                return false;
            UIManager.Instance.SkillLevelUIUpdate(index, playerComponent.Mplevel);
        }
        else if (index == 2)
        {
            FireSkillComponent fireSkillComponent = player.GetComponent<FireSkillComponent>();
            if (fireSkillComponent.SkillLevel < 10)
                fireSkillComponent.LevelUp();
            else
                return false;
            UIManager.Instance.SkillLevelUIUpdate(index, player.GetComponent<FireSkillComponent>().SkillLevel);
        }
        else if (index == 3)
        {
            FreezeSkillComponent freeszeSkillComponent =  player.GetComponent<FreezeSkillComponent>();
            if (freeszeSkillComponent.SkillLevel < 10)
                freeszeSkillComponent.LevelUp();
            else
                return false;
            UIManager.Instance.SkillLevelUIUpdate(index, player.GetComponent<FreezeSkillComponent>().SkillLevel);
        }
        else if (index == 4)
        {
            ThunderSkillComponent thunderSkillComponent = player.GetComponent<ThunderSkillComponent>();
            if (thunderSkillComponent.SkillLevel < 10)
                thunderSkillComponent.LevelUp();
            else
                return false;
            UIManager.Instance.SkillLevelUIUpdate(index, player.GetComponent<ThunderSkillComponent>().SkillLevel);
        }
        else if (index == 5)
        {
            player.GetComponent<StrikeComponent>().LevelUp();
            UIManager.Instance.SkillLevelUIUpdate(index, player.GetComponent<StrikeComponent>().SkillLevel);
        }
        return true;
    }

    public void LevelMaxCheat()
    {
        // 레벨 MAX
        while (playerComponent.level < 60)
            levelUp();
        
        // Hp MAX
        while (playerComponent.Hplevel < 10)
        {
            playerComponent.Hplevel += 1;
            playerComponent.maxHp += 10;
        }
        UIManager.Instance.SkillLevelUIUpdate(0, playerComponent.Hplevel);
        playerComponent.Hp = playerComponent.maxHp;
        UIManager.Instance.HpSliderUpdate();

        // Mp MAX
        while (playerComponent.Mplevel < 10)
        {
            playerComponent.Mplevel += 1;
            playerComponent.maxMp += 10;
        }
        UIManager.Instance.SkillLevelUIUpdate(1, playerComponent.Mplevel);
        playerComponent.Mp = playerComponent.maxMp;
        UIManager.Instance.MpSliderUpdate();

        // 버닝 MAX
        FireSkillComponent fireSkillComponent = player.GetComponent<FireSkillComponent>();
        while (fireSkillComponent.SkillLevel < 10)
            fireSkillComponent.LevelUp();
        UIManager.Instance.SkillLevelUIUpdate(2, player.GetComponent<FireSkillComponent>().SkillLevel);

        // 블리자드 MAX
        FreezeSkillComponent freeszeSkillComponent = player.GetComponent<FreezeSkillComponent>();
        while (freeszeSkillComponent.SkillLevel < 10)
            freeszeSkillComponent.LevelUp();
        UIManager.Instance.SkillLevelUIUpdate(3, player.GetComponent<FreezeSkillComponent>().SkillLevel);

        // 라이트닝 MAX
        ThunderSkillComponent thunderSkillComponent = player.GetComponent<ThunderSkillComponent>();
        while (thunderSkillComponent.SkillLevel < 10)
            thunderSkillComponent.LevelUp();
        UIManager.Instance.SkillLevelUIUpdate(4, player.GetComponent<ThunderSkillComponent>().SkillLevel);

        // 스트라이크 MAX
        StrikeComponent strikeComponent = player.GetComponent<StrikeComponent>();
        while (strikeComponent.SkillLevel < 10)
            strikeComponent.LevelUp();
        UIManager.Instance.SkillLevelUIUpdate(5, player.GetComponent<StrikeComponent>().SkillLevel);
        
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void ItemDrop(bool DeadFlag, int DropMode, Transform pos)
    {
        // DropMode가 1이면 Max 아닌 아이템만 랜덤으로 뜸
        // DropMode가 2이면 Max 아이템만 랜덤으로 뜸
        // DropMode가 3이면 모든 아이템이 랜덤으로 뜸

        int RandomNum = Random.Range(1, 100);
        
        if (DeadFlag == false)
        {
            if (DropMode == 1)
            {
                if (RandomNum >= 85)
                {
                    //GameObject MpPotion = Instantiate(itemPrefebs[2], pos.position+RandomVector, Quaternion.identity);
                    GameObject MpPotion = GameManager.Instance.pool.itemGet(2, pos);
                }
                else if (RandomNum >= 75)
                {
                    //GameObject HpPotion = Instantiate(itemPrefebs[1], pos.position+RandomVector, Quaternion.identity);
                    GameObject HpPotion = GameManager.Instance.pool.itemGet(1, pos);
                }

            }

            else if (DropMode == 2)
            {
                if (RandomNum >= 85)
                {
                    //GameObject MpPotion = Instantiate(itemPrefebs[4], pos.position + RandomVector, Quaternion.identity);
                    GameObject MpPotion_Max = GameManager.Instance.pool.itemGet(4, pos);
                }
                else if (RandomNum >= 75)
                {
                    //GameObject HpPotion = Instantiate(itemPrefebs[3], pos.position + RandomVector, Quaternion.identity);
                    GameObject HpPotion_Max = GameManager.Instance.pool.itemGet(3, pos);
                }

            }

            else if (DropMode == 3)
            {
                if (RandomNum >= 95)
                {
                    //GameObject MpPotion_Max = Instantiate(itemPrefebs[4], pos.position + RandomVector, Quaternion.identity);
                    GameObject MpPotion_Max = GameManager.Instance.pool.itemGet(4, pos);
                }
                else if (RandomNum >= 85)
                {
                    //GameObject HpPotion_Max = Instantiate(itemPrefebs[3], pos.position + RandomVector, Quaternion.identity);
                    GameObject HpPotion_Max = GameManager.Instance.pool.itemGet(3, pos);
                }

                else if (RandomNum >= 75)
                {
                    //GameObject MpPotion = Instantiate(itemPrefebs[2], pos.position + RandomVector, Quaternion.identity);
                    GameObject MpPotion = GameManager.Instance.pool.itemGet(2, pos);
                }
                else if (RandomNum >= 65)
                {
                    //GameObject HpPotion = Instantiate(itemPrefebs[1], pos.position + RandomVector, Quaternion.identity);
                    GameObject MpPotion = GameManager.Instance.pool.itemGet(2, pos);
                }
            }
        }
    }

    public void GameClear()
    {
        EndGame();
        GameObject.Find("PoolManager").active = false;
        UIManager.Instance.MonsterHPUI_OFF();

        // 플레이어 처리
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("isWalk", false);
        animator.SetTrigger("Clear");

        // UI 처리
        Invoke("Invoke_UI", 3f);
    }

    private void Invoke_UI()
    {
        UIManager.Instance.GameOverUI_ON(true);
    }

    public void EndGame()
    {
        isGameover = true;
        Debug.Log("Game Over");
    }
}
