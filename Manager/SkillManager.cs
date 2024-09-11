using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // �� ������ �ʿ�

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    public Dictionary<string, bool> SkillStatus { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // �� ��ȯ �� ��ü �ı� ����
            InitializeSkills();  // ��ų �ʱ�ȭ
            SceneManager.sceneLoaded += OnSceneLoaded;  // �� �ε� �̺�Ʈ �ڵ鷯 ���
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  // �ߺ� �ν��Ͻ� ����
        }
    }

    private void OnDestroy()
    {
        // ��ü �ı� �� �̺�Ʈ �ڵ鷯 ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeSkills()
    {
        SkillStatus = new Dictionary<string, bool>
        {
            {"Green", false},
            {"Blue", false},
            {"Red", false},
            {"Brown", false},
            {"Yellow", false}
        };
    }

    public void ToggleSkill(string skill)
    {
        if (SkillStatus.ContainsKey(skill))
        {
            int activeCount = CountActiveSkills();

            if (!SkillStatus[skill] && activeCount >= 3)
            {
                Debug.Log("Cannot activate more than 3 skills.");
                return;
            }

            SkillStatus[skill] = !SkillStatus[skill];
            Debug.Log(skill + " skill is now " + (SkillStatus[skill] ? "activated" : "deactivated"));
        }
        else
        {
            Debug.Log("Skill key not found: " + skill);
        }
    }

    private int CountActiveSkills()
    {
        int count = 0;
        foreach (var entry in SkillStatus)
        {
            if (entry.Value)
            {
                count++;
            }
        }
        return count;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene '{scene.name}' loaded. Active skills are:");
        foreach (var skill in SkillStatus)
        {
            if (skill.Value)  // ��ų�� Ȱ��ȭ�� ���
            {
                Debug.Log(skill.Key);
            }
        }
    }
}
