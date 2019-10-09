using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Serializable]
    public class InputButton
    {
        public KeyCode Key;
        public bool Down { get; protected set; } 
        public bool Held { get; protected set; }
        public bool Up { get; protected set; }
        public bool Enabled
        {
            get { return m_Enabled; }
        }
        protected bool m_Enabled;
        bool m_AfterFixedUpdateDown;
        bool m_AfterFixedUpdateHeld;
        bool m_AfterFixedUpdateUp;

        public InputButton(KeyCode key) { Key = key; }

        public void Get(bool fixedUpdateHappened)
        {
            if (!m_Enabled)
            {
                return;
            }

            if (fixedUpdateHappened)
            {
                Down = Input.GetKeyDown(Key);
                Held = Input.GetKey(Key);
                Up = Input.GetKeyUp(Key);

                m_AfterFixedUpdateDown = Down;
                m_AfterFixedUpdateHeld = Held;
                m_AfterFixedUpdateUp = Up;
            } else
            {
                Down = Input.GetKeyDown(Key) || m_AfterFixedUpdateDown;
                Held = Input.GetKey(Key) || m_AfterFixedUpdateHeld;
                Up = Input.GetKeyUp(Key) || m_AfterFixedUpdateUp;

                m_AfterFixedUpdateDown |= Down;
                m_AfterFixedUpdateHeld |= Held;
                m_AfterFixedUpdateUp |= Up;
            }
        }

        public void GainControl()
        {
            m_Enabled = true;
        }

        public IEnumerator ReleaseControl(bool resetValues)
        {
            m_Enabled = false;

            if (!resetValues)
                yield break;

            Up |= Down;
            Down = false;
            Held = false;

            m_AfterFixedUpdateDown = false;
            m_AfterFixedUpdateHeld = false;
            m_AfterFixedUpdateUp = false;

            yield return null;

            Up = false;
        }
    }

    [Serializable]
    public class InputAxis
    {
        public KeyCode positive;
        public KeyCode negative;
        public float Value { get; protected set; }
        public bool ReceivingInput { get; protected set; }
        protected bool m_GettingInput = true;

        public InputAxis(KeyCode positive, KeyCode negative)
        {
            this.positive = positive;
            this.negative = negative;
        }

        public void Get()
        {
            if (!m_GettingInput)
                return;

            bool positiveHeld = Input.GetKey(positive);
            bool negativeHeld = Input.GetKey(negative);

            if (positiveHeld == negativeHeld)
                Value = 0f;
            else if (positiveHeld)
                Value = 1f;
            else
                Value = -1f;

            ReceivingInput = positiveHeld || negativeHeld;
        }

        public void GainControl()
        {
            m_GettingInput = true;
        }

        public void ReleaseControl(bool resetValues)
        {
            m_GettingInput = false;
            if (resetValues)
            {
                Value = 0f;
                ReceivingInput = false;
            }
        }
    }
    #region Static-Instance
    protected static PlayerInput s_Instance;
    public static PlayerInput Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<PlayerInput>();

            if (s_Instance != null)
                return s_Instance;

            Create();

            return s_Instance;
        }
    }

    public static void Create()
    {
        var obj = new GameObject("PlayerInput");
        s_Instance = obj.AddComponent<PlayerInput>();
    }
    #endregion

    #region Static-Properties
    #endregion

    public bool HaveControl
    {
        get { return Instance.m_HaveControl; }
    }

    public InputButton Pause = new InputButton(KeyCode.Escape);
    public InputAxis Horizontal = new InputAxis(KeyCode.D, KeyCode.A);
    public InputAxis Vertical = new InputAxis(KeyCode.W, KeyCode.S);
    protected bool m_HaveControl = true;
    protected bool m_FixedUpdateHappend;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        GainControl();
    }

    private void Update()
    {
        GetInputs(m_FixedUpdateHappend || Mathf.Approximately(Time.timeScale, 0));

        m_FixedUpdateHappend = false;
    }

    private void FixedUpdate()
    {
        m_FixedUpdateHappend = true;
    }

    protected void GetInputs(bool fixedUpdateHappend)
    {
        Pause.Get(fixedUpdateHappend);
        Horizontal.Get();
        Vertical.Get();
    }

    public void GainControl()
    {
        m_HaveControl = true;
        Pause.GainControl();
        Horizontal.GainControl();
        Vertical.GainControl();
    }

    public void ReleaseControl(bool resetValues = true)
    {
        m_HaveControl = false;
        StartCoroutine(Pause.ReleaseControl(resetValues));
        Horizontal.ReleaseControl(resetValues);
        Vertical.ReleaseControl(resetValues);
    }
}
