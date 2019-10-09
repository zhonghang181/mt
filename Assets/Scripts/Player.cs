using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public enum FaceType
    {
        Up, Down, Left, Right,
    }
    static protected Player s_Instance;
    static public Player Instance
    {
        get { return s_Instance; }
    }

    public FaceType faceType = FaceType.Up;
    public AudioClip audioClipFootStep;
    public PlayerData PlayerData
    {
        get { return m_PlayerData; }
    }
    public PlayerData m_PlayerData = new PlayerData();

    protected Animator m_Animator;
    protected Rigidbody2D m_Rigidbody2D;
    protected Collider2D m_Collider2D;
    protected AudioSource m_AudioSource;
    public PlayerController2D m_PlayerController2D;

    protected bool m_InPause;
    protected Vector2 m_MoveVector;
    protected bool m_MovingCheckSwitch;
    protected ContactFilter2D m_ContactFilter;
    protected RaycastHit2D[] m_HitBuffer = new RaycastHit2D[3];

    protected readonly int m_HashMovingPara = Animator.StringToHash("Moving");
    protected readonly int m_HashMoveXPara = Animator.StringToHash("MoveX");
    protected readonly int m_HashMoveYPara = Animator.StringToHash("MoveY");
    protected readonly int m_HashStepLeftPara = Animator.StringToHash("StepLeft");
    protected readonly int m_HashFacePara = Animator.StringToHash("Face");

    // =========== MonoBehaviour ===========
    private void Awake()
    {
        s_Instance = this;
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();
        m_AudioSource = GetComponent<AudioSource>();
        m_PlayerController2D = GetComponent<PlayerController2D>();

        SceneLinkedSMB<Player>.Initialise(m_Animator, this);

        Physics2D.queriesStartInColliders = false;
        m_ContactFilter.layerMask = LayerMask.NameToLayer("Element");
        m_ContactFilter.useLayerMask = true;
        m_ContactFilter.useTriggers = false;
    }

    void Update()
    {
        if (PlayerInput.Instance.Pause.Down)
        {
            if (!m_InPause)
            {
                Pause();
            } else
            {
                Resume();
            }
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log("MoveVector | " + m_MoveVector);
        //m_PlayerController2D.Move(m_MoveVector);
        //m_Animator.SetBool(m_HashMovingPara, !Mathf.Approximately(m_MoveVector.x , 0f) || !Mathf.Approximately(m_MoveVector.y, 0f));

        if (m_MovingCheckSwitch)
        {
            var count = Physics2D.Raycast(transform.position, Face2Direction(), m_ContactFilter, m_HitBuffer, 1f);
            Debug.Log("HitCount | " + count);
            Debug.Log("FaceType | " + faceType);
        }
    }

    // =========== Protected functions ===========
    Vector2 Face2Direction()
    {
        Vector2 dir = Vector2.up;
        switch (faceType)
        {
            case FaceType.Up:
                dir = Vector2.up;
                break;
            case FaceType.Down:
                dir = Vector2.down;
                break;
            case FaceType.Left:
                dir = Vector2.left;
                break;
            case FaceType.Right:
                dir = Vector2.right;
                break;
        }
        return dir;
    }
    IEnumerator ResumeCoroutine()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Pause");
        PlayerInput.Instance.GainControl();

        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        m_InPause = false;
    }

    // =========== Public functions ===========
    public void Pause()
    {
        m_InPause = true;
        Time.timeScale = 0;

        PlayerInput.Instance.ReleaseControl(false);
        PlayerInput.Instance.Pause.GainControl();

        SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
    }

    public void Resume()
    {
        if (Time.timeScale > 0)
        {
            return;
        }

        StartCoroutine(ResumeCoroutine());
    }

    public void UpdateMoving()
    {
        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, m_MoveVector.x + PlayerInput.Instance.Horizontal.Value, Time.deltaTime);
        m_MoveVector.y = Mathf.MoveTowards(m_MoveVector.y, m_MoveVector.y + PlayerInput.Instance.Vertical.Value, Time.deltaTime);
    }

    public void UpdateFace()
    {
        var horizontal = PlayerInput.Instance.Horizontal.Value;
        var vertical = PlayerInput.Instance.Vertical.Value;
        if (!Mathf.Approximately(horizontal, 0))
        {
            if (Mathf.Approximately(horizontal, 1))
            {
                faceType = FaceType.Right;
            } else
            {
                faceType = FaceType.Left;
            }
        }

        if (!Mathf.Approximately(vertical, 0))
        {
            if (Mathf.Approximately(vertical, 1))
            {
                faceType = FaceType.Up;
            }
            else
            {
                faceType = FaceType.Down;
            }
        }
        m_Animator.SetFloat(m_HashFacePara, (float)faceType);
    }

    public void TryMoving()
    {
        var horizontal = PlayerInput.Instance.Horizontal.Value;
        var vertical = PlayerInput.Instance.Vertical.Value;
        m_MovingCheckSwitch = Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0;
    }

    public void OpenDoor(Door door)
    {
        int keyIndex = (int)door.GetDoorType();
        if (!door.IsOpened() && m_PlayerData.GetKeyNum(keyIndex) > 0)
        {
            m_PlayerData.UpdateKeys(keyIndex, -1);
            door.Open();
            m_AudioSource.PlayOneShot(door._audioClip, 0.5f);
        }
    }

    // Called by Animation Event
    public void FootStep()
    {
        m_AudioSource.PlayOneShot(audioClipFootStep);
    }
}
