using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    public int m_QualityLevel;
    public string m_QualityName;

    private int m_LastQualityLevel;
    void Start()
    {
        m_QualityLevel = QualitySettings.GetQualityLevel();
        m_QualityName = QualitySettings.names[m_QualityLevel];
    }

    private void Update()
    {
        if(m_LastQualityLevel != m_QualityLevel)
        {
            m_LastQualityLevel = m_QualityLevel;
            QualitySettings.SetQualityLevel(m_QualityLevel);
            UpdateLevel();
        }
    }

    private void UpdateLevel()
    {
        m_QualityLevel = QualitySettings.GetQualityLevel();
        m_QualityName = QualitySettings.names[m_QualityLevel];
        Debug.Log("Quality Level: " + m_QualityLevel + " | " + m_QualityName);
    }
}
