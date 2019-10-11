using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPanel : MonoBehaviour
{
    protected static FightPanel s_Instance;
    public static FightPanel Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<FightPanel>();

            if (s_Instance != null)
                return s_Instance;

            Create();

            return s_Instance;
        }
    }

    public static void Create()
    {
        var prefab = Resources.Load<FightPanel>("FightPanel");
        s_Instance = Instantiate(prefab);
    }

    public TMPro.TextMeshProUGUI monsterAtk;
    public TMPro.TextMeshProUGUI monsterDef;
    public TMPro.TextMeshProUGUI monsterHp;

    public TMPro.TextMeshProUGUI heroAtk;
    public TMPro.TextMeshProUGUI heroDef;
    public TMPro.TextMeshProUGUI heroHp;

    public GameObject aliveNode;

    PlayerData player;
    MonsterData monster;
    bool m_IsPlayerAction;

    // =========== MonoBehaivor ===========
    private void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        aliveNode.SetActive(false);

        m_IsPlayerAction = true;
    }

    void Start()
    {
    }

    private void Update()
    {
        if (aliveNode.activeSelf)
        {
            if (monster.hp <= 0 || player.hp <= 0)
            {
                StartCoroutine(Hide());
            }
        }
    }

    // =========== Public Functions ===========
    public void Show(PlayerData player, MonsterData monster)
    {
        this.player = player;
        this.monster = monster;

        heroAtk.text = player.atk.ToString();
        heroDef.text = player.def.ToString();
        heroHp.text = player.hp.ToString();

        monsterAtk.text = monster.atk.ToString();
        monsterDef.text = monster.def.ToString();
        monsterHp.text = monster.hp.ToString();

        aliveNode.SetActive(true);
        PlayerInput.Instance.ReleaseControl();
        
        StartCoroutine(PlayerAtk());
        StartCoroutine(MonsterAtk());
    }

    // =========== Private Functions ===========
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(0.5f);

        aliveNode.SetActive(false);
        PlayerInput.Instance.GainControl();
    }

    IEnumerator MonsterAtk()
    {
        while (monster.hp > 0 && player.hp > 0)
        {
            if (!m_IsPlayerAction)
            {
                var damage = monster.atk - player.def;
                player.UpdateHp(-damage);
                heroHp.text = player.hp.ToString();

                if (player.hp <= 0)
                {
                    Debug.Log("player dead");
                    break;
                }

                yield return new WaitForSeconds(0.5f);
                m_IsPlayerAction = true;
            } else
            {
                yield return null;
            }
        }
    }

    IEnumerator PlayerAtk()
    {
        while (monster.hp > 0 && player.hp > 0)
        {
            if (m_IsPlayerAction)
            {
                var damage = player.atk - monster.def;
                monster.UpdateHp(-damage);
                monsterHp.text = monster.hp.ToString();

                if (monster.hp <= 0)
                {
                    Debug.Log("monster dead");
                    break;
                }

                yield return new WaitForSeconds(0.5f);
                m_IsPlayerAction = false;
            } else
            {
                yield return null;
            }
        }
    }
}
