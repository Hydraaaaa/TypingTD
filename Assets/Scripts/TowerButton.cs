using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] TowerCreator m_TowerCreator;
    [SerializeField] Tower m_Tower;
    [SerializeField] Text m_Text;

    void Start()
    {
        m_Text.text = m_Tower.Cost.ToString();
    }

    public void MakeTower()
    {
        m_TowerCreator.MakeTower(m_Tower);
    }
}
