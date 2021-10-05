using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] WordList m_WordList;
    [SerializeField] TextMeshPro m_Text;
    [SerializeField] SpriteRenderer m_Renderer;
    [SerializeField] GameObject m_DeathEffect;

    [Space]

    [SerializeField] Gradient m_MatchingTextImpactGradient;
    [SerializeField] Gradient m_CompletelyMatchingTextImpactGradient;
    [SerializeField] Gradient m_MissingTextImpactGradient;

    [Space]

    [SerializeField] float m_MovementSpeed = 1.0f;
    [SerializeField] int m_Health = 5;
    [SerializeField] float m_FlinchFactor = 0.8f;
    [SerializeField] float m_FlinchRecoveryRate = 2.0f;
    [SerializeField] float m_MatchingTextTransitionSpeed = 2.0f;

    public static List<Enemy> Enemies { get; private set; }

    public event Action OnDestinationReached;

    Vector2[] m_Waypoints;

    Vector3 m_MovementDirection;

    int m_CurrentWaypoint;

    int m_CurrentHealth;

    int m_PreviousMatchLength;

    bool m_IsMoving;

    public string Word { get; private set; }

    List<float> m_MatchingLetterColors = new List<float>();

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

    public void Initialize(Vector2[] a_Waypoints)
    {
        m_Waypoints = a_Waypoints;

        m_CurrentWaypoint = 0;

        m_MovementDirection = (new Vector3(a_Waypoints[0].x, a_Waypoints[0].y, transform.position.z) - transform.position).normalized;

        m_IsMoving = true;

        Word = m_WordList.GetRandomWord(m_Health);

        m_Text.text = Word;
        m_CurrentHealth = m_Health;

        m_PreviousMatchLength = 0;
    }

    void Update()
    {
        if (m_IsMoving)
        {
            Move();
        }

        if (m_Renderer.transform.localScale.x < 1)
        {
            float _Scale = Mathf.Clamp01(m_Renderer.transform.localScale.x + Time.deltaTime * m_FlinchRecoveryRate);

            m_Renderer.transform.localScale = new Vector3(_Scale, _Scale, 1);
        }

        for (int i = 0; i < m_MatchingLetterColors.Count; i++)
        {
            m_MatchingLetterColors[i] = Mathf.Clamp01(m_MatchingLetterColors[i] + Time.deltaTime * m_MatchingTextTransitionSpeed);
        }

        UpdateWordColors();
    }

    void Move()
    {
        float _FrameMovement = m_MovementSpeed * Time.deltaTime;
        float _DistanceToWaypoint = Vector2.Distance(transform.position, m_Waypoints[m_CurrentWaypoint]);

        // If the travel distance would allow the enemy to reach the waypoint
        while (_FrameMovement >= _DistanceToWaypoint)
        {
            _FrameMovement -= _DistanceToWaypoint;

            transform.position = m_Waypoints[m_CurrentWaypoint];

            m_CurrentWaypoint++;

            if (m_CurrentWaypoint == m_Waypoints.Length)
            {
                m_IsMoving = false;

                DestinationReached();

                return;
            }

            _DistanceToWaypoint = Vector2.Distance(transform.position, m_Waypoints[m_CurrentWaypoint]);

            m_MovementDirection = (new Vector3(m_Waypoints[m_CurrentWaypoint].x, m_Waypoints[m_CurrentWaypoint].y, transform.position.z) - transform.position).normalized;
        }

        transform.position += m_MovementDirection * _FrameMovement;
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
            Instantiate(m_DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnWordUpdate(string a_Word)
    {
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

            if (_MatchLength > m_PreviousMatchLength)
            {
                float _Scale = m_Renderer.transform.localScale.x * m_FlinchFactor;
                m_Renderer.transform.localScale = new Vector3(_Scale, _Scale, 1);

                m_MatchingLetterColors.Add(0.0f);
            }
            else if (m_PreviousMatchLength > _MatchLength)
            {
                m_MatchingLetterColors.RemoveAt(_MatchLength);
            }

            m_PreviousMatchLength = _MatchLength;

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

        if (m_CurrentHealth < Word.Length)
        {
            _Text += $"<color=#{ColorUtility.ToHtmlStringRGB(m_MissingTextImpactGradient.Evaluate(1))}>";
            _Text += Word.Substring(m_CurrentHealth);
            _Text += "</color>";
        }

        m_Text.text = _Text;
    }
}
