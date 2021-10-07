using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> Enemies { get; private set; }

    public event System.Action OnDestinationReached;
    public event System.Action OnDeath;

    public Transform OffsetTransform => m_OffsetTransform;

    public string Word { get; private set; }

    public int Health
    {
        get { return m_CurrentHealth; }
        set { SetHealth(value); }
    }

    public bool IsTargetable
    {
        get
        {
            if (m_IsMoving)
            {
                return m_Waypoints[m_CurrentWaypoint].AreEnemiesTargetable;
            }
            else
            {
                return false;
            }
        }
    }

    [SerializeField] WordList m_WordList;
    [SerializeField] TextMeshPro m_Text;
    [SerializeField] SpriteRenderer m_Renderer;
    [SerializeField] GameObject m_DeathEffect;
    [SerializeField] Transform m_OffsetTransform;
    [SerializeField] BoxCollider2D m_OffsetCollider;

    [Space]

    [SerializeField] Gradient m_MatchingTextImpactGradient;
    [SerializeField] Gradient m_CompletelyMatchingTextImpactGradient;
    [SerializeField] Gradient m_MissingTextImpactGradient;

    [Space]

    [SerializeField] float m_MovementSpeed = 1.0f;
    [SerializeField] int m_Health = 5;
    [SerializeField] float m_FlinchFactor = 0.8f;
    [SerializeField] float m_FlinchRecoveryRate = 2.0f;
    [SerializeField] float m_TextTransitionSpeed = 2.0f;
    [SerializeField] float m_MovementStrictness = 5.0f;
    [SerializeField] float m_SpacingForce = 0.25f;
    [SerializeField] LayerMask m_SpacingLayerMask;

    Waypoint[] m_Waypoints;

    Vector2 m_CurrentPosition;
    Vector2 m_MovementDirection;

    Vector2 m_OffsetPosition;

    int m_CurrentWaypoint;

    int m_CurrentHealth;

    int m_PreviousMatchLength;

    bool m_IsMoving;

    List<float> m_MatchingLetterColors = new List<float>();
    List<float> m_MissingLetterColors = new List<float>();

    void Awake()
    {
        if (Enemies == null)
        {
            Enemies = new List<Enemy>();
        }

        Enemies.Add(this);

        TypingBox.OnWordUpdate += OnWordUpdate;
        TypingBox.OnWordSubmission += OnWordSubmission;
    }

    void OnDestroy()
    {
        Enemies.Remove(this);

        TypingBox.OnWordUpdate -= OnWordUpdate;
        TypingBox.OnWordSubmission -= OnWordSubmission;
    }

    public void Initialize(Waypoint[] a_Waypoints)
    {
        m_Waypoints = a_Waypoints;

        m_CurrentWaypoint = 0;

        m_CurrentPosition = transform.position;
        m_MovementDirection = (new Vector2(a_Waypoints[0].Position.x, a_Waypoints[0].Position.y) - m_CurrentPosition).normalized;

        m_IsMoving = true;

        Word = m_WordList.GetRandomWord(m_Health);

        m_Text.text = Word;
        m_CurrentHealth = m_Health;

        m_PreviousMatchLength = 0;

        m_OffsetPosition = Random.insideUnitCircle * 0.75f;
        m_OffsetTransform.localPosition = m_OffsetPosition;

        m_OffsetCollider.size = new Vector2(m_Text.preferredWidth, m_Text.preferredHeight);
    }

    void Update()
    {
        if (m_IsMoving)
        {
            Move();

            // Move the visual element

            // See if our word is overlapping any other words
            Collider2D[] _Colliders = Physics2D.OverlapBoxAll(m_OffsetTransform.position, m_OffsetCollider.size, 0, m_SpacingLayerMask);

            Vector3 _Spacing = Vector3.zero;

            // For each overlapped word, push us away from the origin point of the other enemy
            for (int i = 0; i < _Colliders.Length; i++)
            {
                _Spacing += (m_OffsetTransform.position - _Colliders[i].transform.position).normalized * m_SpacingForce;
            }

            // Combine follow point with spacing direction
            // Since the follow point direction isn't normalized, if we're far away enough, the follow force will overpower the spacing force
            Vector2 _Direction = new Vector2
            (
                (m_CurrentPosition.x + m_OffsetPosition.x - m_OffsetTransform.position.x) * m_MovementStrictness + _Spacing.x,
                (m_CurrentPosition.y + m_OffsetPosition.y - m_OffsetTransform.position.y) * m_MovementStrictness + _Spacing.y
            );

            // Scale combined movement with deltaTime
            _Direction *= Time.deltaTime;

            m_OffsetTransform.position += new Vector3(_Direction.x, _Direction.y, 0);
        }

        // localScale will be below one if the enemy has flinched from matching keyboard input
        if (m_OffsetTransform.localScale.x < 1)
        {
            float _Scale = Mathf.Clamp01(m_OffsetTransform.localScale.x + Time.deltaTime * m_FlinchRecoveryRate);

            m_OffsetTransform.localScale = new Vector3(_Scale, _Scale, 1);
        }

        // Mature any red letters into either yellow or green
        for (int i = 0; i < m_MatchingLetterColors.Count; i++)
        {
            m_MatchingLetterColors[i] = Mathf.Clamp01(m_MatchingLetterColors[i] + Time.deltaTime * m_TextTransitionSpeed);
        }

        for (int i = 0; i < m_MissingLetterColors.Count; i++)
        {
            m_MissingLetterColors[i] = Mathf.Clamp01(m_MissingLetterColors[i] + Time.deltaTime * m_TextTransitionSpeed);
        }

        UpdateWordColors();
    }

    void Move()
    {
        float _FrameMovement = m_MovementSpeed * Time.deltaTime;
        float _DistanceToWaypoint = Vector2.Distance(m_CurrentPosition, m_Waypoints[m_CurrentWaypoint].Position);

        // While the travel distance would allow the enemy to reach the waypoint
        while (_FrameMovement >= _DistanceToWaypoint)
        {
            // Subtract the remaining waypoint distance from the frame movement
            _FrameMovement -= _DistanceToWaypoint;

            m_CurrentPosition = m_Waypoints[m_CurrentWaypoint].Position;

            m_CurrentWaypoint++;

            if (m_CurrentWaypoint == m_Waypoints.Length)
            {
                m_IsMoving = false;

                DestinationReached();

                return;
            }

            _DistanceToWaypoint = Vector2.Distance(m_CurrentPosition, m_Waypoints[m_CurrentWaypoint].Position);

            m_MovementDirection = (new Vector2(m_Waypoints[m_CurrentWaypoint].Position.x, m_Waypoints[m_CurrentWaypoint].Position.y) - m_CurrentPosition).normalized;
        }

        m_CurrentPosition += m_MovementDirection * _FrameMovement;
    }

    void DestinationReached()
    {
        OnDestinationReached?.Invoke();
        Destroy(gameObject);
    }

    void OnWordSubmission(string a_Word)
    {
        if (a_Word.StartsWith(Word.Substring(0, m_CurrentHealth)))
        {
            Kill();
        }
    }

    void OnWordUpdate(string a_Word)
    {
        // If our word begins with the keyboard input
        // Or the keyboard input begins with our entire word
        if (Word.StartsWith(a_Word) &&
            a_Word.Length > 0 ||
            a_Word.StartsWith(Word.Substring(0, m_CurrentHealth)))
        {
            int _MatchLength;

            if (a_Word.Length < m_CurrentHealth)
            {
                _MatchLength = a_Word.Length;
            }
            else
            {
                _MatchLength = m_CurrentHealth;
            }

            while (_MatchLength > m_PreviousMatchLength)
            {
                float _Scale = m_OffsetTransform.localScale.x * m_FlinchFactor;
                m_OffsetTransform.localScale = new Vector3(_Scale, _Scale, 1);

                m_MatchingLetterColors.Add(0.0f);

                m_PreviousMatchLength++;
            }

            if (m_PreviousMatchLength > _MatchLength)
            {
                m_MatchingLetterColors.RemoveAt(_MatchLength);

                m_PreviousMatchLength = _MatchLength;
            }

            m_Text.sortingOrder = 101;
        }
        else
        {
            m_Text.sortingOrder = 100;

            m_PreviousMatchLength = 0;
            m_MatchingLetterColors.Clear();
        }

        UpdateWordColors();
    }

    void UpdateWordColors()
    {
        string _Text = "";

        if (m_PreviousMatchLength == m_CurrentHealth)
        {
            for (int i = 0; i < m_MatchingLetterColors.Count; i++)
            {
                _Text += $"<color=#{ColorUtility.ToHtmlStringRGB(m_CompletelyMatchingTextImpactGradient.Evaluate(m_MatchingLetterColors[i]))}>{Word[i]}</color>";
            }
        }
        else
        {
            for (int i = 0; i < m_MatchingLetterColors.Count; i++)
            {
                _Text += $"<color=#{ColorUtility.ToHtmlStringRGB(m_MatchingTextImpactGradient.Evaluate(m_MatchingLetterColors[i]))}>{Word[i]}</color>";
            }

            _Text += Word.Substring(m_PreviousMatchLength, m_CurrentHealth - m_PreviousMatchLength);
        }

        for (int i = 0; i < m_MissingLetterColors.Count; i++)
        {
            _Text += $"<color=#{ColorUtility.ToHtmlStringRGB(m_MissingTextImpactGradient.Evaluate(m_MissingLetterColors[i]))}>";
            _Text += Word[m_CurrentHealth + i];
            _Text += "</color>";
        }

        m_Text.text = _Text;
    }

    void SetHealth(int a_Health)
    {
        if (a_Health < 0)
        {
            a_Health = 0;
        }

        while (m_CurrentHealth > a_Health)
        {
            m_MissingLetterColors.Insert(0, 0.0f);
            m_CurrentHealth--;
        }

        while (m_CurrentHealth < a_Health)
        {
            m_MissingLetterColors.RemoveAt(0);
            m_CurrentHealth++;
        }

        while (m_PreviousMatchLength > a_Health)
        {
            m_MatchingLetterColors.RemoveAt(m_PreviousMatchLength - 1);

            m_PreviousMatchLength--;
        }

        if (a_Health == 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        OnDeath?.Invoke();

        Instantiate(m_DeathEffect, m_OffsetTransform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
