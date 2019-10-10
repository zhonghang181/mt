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

    protected bool m_InPause;
    protected bool m_MovingCheckSwitch;
    bool m_ResetPosition;
    protected ContactFilter2D m_ContactFilter;
    protected RaycastHit2D[] m_HitBuffer = new RaycastHit2D[3];
    Vector2 m_NextPosition;
    Vector2 m_PrevPosition;

    protected readonly int m_HashMovingPara = Animator.StringToHash("Moving");
    protected readonly int m_HashMoveXPara = Animator.StringToHash("MoveX");
    protected readonly int m_HashMoveYPara = Animator.StringToHash("MoveY");
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

        SceneLinkedSMB<Player>.Initialise(m_Animator, this);

        Physics2D.queriesStartInColliders = false;
        m_ContactFilter.layerMask = LayerMask.NameToLayer("Everything");
        m_ContactFilter.useLayerMask = true;
        m_ContactFilter.useTriggers = false;

        m_PrevPosition = m_Rigidbody2D.position;
        m_NextPosition = m_Rigidbody2D.position;
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
        if (m_ResetPosition)
        {
            m_ResetPosition = false;
            m_Rigidbody2D.MovePosition(m_NextPosition);
        } else
        {
            m_Rigidbody2D.MovePosition(Vector2.MoveTowards(m_Rigidbody2D.position, m_NextPosition, 4 * Time.fixedDeltaTime));
        }

        if (m_MovingCheckSwitch)
        {
            m_MovingCheckSwitch = false;
            var count = Physics2D.Raycast(m_Rigidbody2D.position, Face2Direction(), m_ContactFilter, m_HitBuffer, 1f);
            if (count == 0)
            {
                m_NextPosition = GetNextPosition();
                m_Animator.SetBool(m_HashMovingPara, true);
            } else
            {
                m_PrevPosition = m_NextPosition;
                m_Animator.SetBool(m_HashMovingPara, false);
                OnCollider();
            }
        }
    }

    // =========== Private functions ===========
    void OnCollider()
    {
        var obj = m_HitBuffer[0];
        if (obj.collider.gameObject.CompareTag("Door"))
        {
            var door = obj.collider.gameObject.GetComponent<Door>();
            int keyIndex = (int)door.GetDoorType();
            if (!door.IsOpened() && m_PlayerData.GetKeyNum(keyIndex) > 0)
            {
                m_PlayerData.UpdateKeys(keyIndex, -1);
                door.Open();
                m_AudioSource.PlayOneShot(door._audioClip, 0.5f);
            }
        }
    }

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

    Vector2 GetNextPosition()
    {
        Vector2 cur = m_PrevPosition;
        Vector2 next = cur;
        switch (faceType)
        {
            case FaceType.Up:
                next = cur + Vector2.up;
                break;
            case FaceType.Down:
                next = cur + Vector2.down;
                break;
            case FaceType.Left:
                next = cur + Vector2.left;
                break;
            case FaceType.Right:
                next = cur + Vector2.right;
                break;
        }
        return next;
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
        m_PrevPosition = m_NextPosition;
        m_ResetPosition = true;

        var horizontal = PlayerInput.Instance.Horizontal.Value;
        var vertical = PlayerInput.Instance.Vertical.Value;
        m_MovingCheckSwitch = Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0;
        if (!m_MovingCheckSwitch)
        {
            m_Animator.SetBool(m_HashMovingPara, false);
        }
    }

    // Called by Animation Event
    public void FootStep()
    {
        m_AudioSource.PlayOneShot(audioClipFootStep);
    }
}
