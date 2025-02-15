using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public bool isLive; // �Ͻ� ���� ����
    public bool isBossStart; // ���� ���� ����
    public bool isGameover { get; private set; } // ���ӿ��� ����
    public AudioClip[] audioClips;
    public AudioClip[] BGMClips;

    // ���ӿ�����Ʈ
    private GameObject player;

    // ������Ʈ
    private PlayerComponent playerComponent;
    private AudioSource BgmAudio;
    public AudioSource playerAudio;
    public AudioSource bossAudio;
    public PoolManager pool;

    // ����
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

    // ���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ
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
        // ������Ʈ �ʱ�ȭ
        player = GameObject.Find("Player");

        // ������Ʈ �ʱ�ȭ
        playerComponent = player.GetComponent<PlayerComponent>();
        playerAudio = player.GetComponent<AudioSource>();
        BgmAudio = GetComponent<AudioSource>();
        // UI �ʱ�ȭ
        UIManager.Instance.UIReset();
    }

    // �÷��̾� transform ��ȯ
    public Transform playerTransform()
    {
        return player.transform;
    }

    // �÷��̾� ȿ���� ���
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

    // �÷��̾� Hp ��ȯ
    public float playerHp()
    {
        return playerComponent.Hp;
    }
    // �÷��̾� MaxHp ��ȯ
    public float playerMaxHp()
    {
        return playerComponent.maxHp;
    }
    // �÷��̾� Mp ��ȯ
    public float playerMp()
    {
        return playerComponent.Mp;
    }
    // �÷��̾� MaxMp ��ȯ
    public float playerMaxMp()
    {
        return playerComponent.maxMp;
    }
    // �÷��̾� Mp ��� ó��
    public void playerMpUse(float MpUse)
    {
        playerComponent.Mp -= MpUse;
        if (playerComponent.Mp < 0)
            playerComponent.Mp = 0;
        UIManager.Instance.MpSliderUpdate();
    }

    // �÷��̾� Exp ��ȯ
    public float playerExp()
    {
        return playerComponent.Exp;
    }
    // �÷��̾� maxExp ��ȯ
    public float playerMaxExp()
    {
        return playerComponent.maxExp;
    }
    public void playerSetExp(float Exp)
    {
        playerComponent.Exp = Exp;
    }
    // �÷��̾� ���� ��ȯ
    public int level()
    {
        return playerComponent.level;
    }



    // ������
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
        // ���� MAX
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

        // ���� MAX
        FireSkillComponent fireSkillComponent = player.GetComponent<FireSkillComponent>();
        while (fireSkillComponent.SkillLevel < 10)
            fireSkillComponent.LevelUp();
        UIManager.Instance.SkillLevelUIUpdate(2, player.GetComponent<FireSkillComponent>().SkillLevel);

        // ���ڵ� MAX
        FreezeSkillComponent freeszeSkillComponent = player.GetComponent<FreezeSkillComponent>();
        while (freeszeSkillComponent.SkillLevel < 10)
            freeszeSkillComponent.LevelUp();
        UIManager.Instance.SkillLevelUIUpdate(3, player.GetComponent<FreezeSkillComponent>().SkillLevel);

        // ����Ʈ�� MAX
        ThunderSkillComponent thunderSkillComponent = player.GetComponent<ThunderSkillComponent>();
        while (thunderSkillComponent.SkillLevel < 10)
            thunderSkillComponent.LevelUp();
        UIManager.Instance.SkillLevelUIUpdate(4, player.GetComponent<ThunderSkillComponent>().SkillLevel);

        // ��Ʈ����ũ MAX
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
        // DropMode�� 1�̸� Max �ƴ� �����۸� �������� ��
        // DropMode�� 2�̸� Max �����۸� �������� ��
        // DropMode�� 3�̸� ��� �������� �������� ��

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

        // �÷��̾� ó��
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("isWalk", false);
        animator.SetTrigger("Clear");

        // UI ó��
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
