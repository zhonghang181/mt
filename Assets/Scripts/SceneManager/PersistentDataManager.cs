using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    protected static PersistentDataManager s_Instance;
    public static PersistentDataManager Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<PersistentDataManager>();

            if (s_Instance != null)
                return s_Instance;

            Create();

            return s_Instance;
        }
    }

    public static void Create()
    {
        GameObject obj = new GameObject("PersistentDataManager");
        s_Instance = obj.AddComponent<PersistentDataManager>();
    }

    // =========== Properties ===========
    protected HashSet<IDataPersister> m_DataPersisters = new HashSet<IDataPersister>();
    protected Dictionary<string, Data> m_Store = new Dictionary<string, Data>();
    event System.Action schedule = null;


    // =========== MonoBehavior ===========
    private void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    private void Update()
    {
        if (schedule != null)
        {
            Debug.Log("Persistent Update");
            schedule();
            schedule = null;
        }
    }

    // =========== Public Functions ===========
    public void Register(IDataPersister persister)
    {
        schedule += () =>
        {
            m_DataPersisters.Add(persister);
        };
    }

    public void Unregister(IDataPersister persister)
    {
        schedule += () => m_DataPersisters.Remove(persister);
    }

    public void SaveAllData()
    {
        foreach (var dp in m_DataPersisters)
        {
            m_Store[dp.GetDataTag()] = dp.SaveData();
        }
    }

    public void LoadAllData()
    {
        schedule += () =>
        {
            foreach (var dp in m_DataPersisters)
            {
                var dataTag = dp.GetDataTag();
                if (m_Store.ContainsKey(dataTag))
                {
                    dp.LoadData(m_Store[dataTag]);
                }
            }
        };
    }

    public void ClearPersisters()
    {
        m_DataPersisters.Clear();
    }
}
