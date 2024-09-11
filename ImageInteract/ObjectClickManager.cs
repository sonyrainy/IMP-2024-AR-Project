using UnityEngine;

public class ObjectClickManager : MonoBehaviour
{
    public GameObject fireBtn;
    public GameObject waterBtn;
    public GameObject electricBtn;
    public GameObject grassBtn;
    public GameObject groundBtn;

    void Start()
    {
        UpdateButtonStates();  // �� �ε� �� ��ư ���� �ʱ�ȭ
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string tag = hit.transform.tag;
                if (SkillManager.Instance.SkillStatus.ContainsKey(tag))
                {
                    SkillManager.Instance.ToggleSkill(tag);
                    UpdateButtonStates();  // ��ų ���� ���� �� UI ������Ʈ
                }
            }
        }
    }

    void UpdateButtonStates()
    {
        // ��ų ��ư�� Ȱ��ȭ ���¸� SkillManager�� ��ų ���¿� ���߾� ������Ʈ
        fireBtn.SetActive(SkillManager.Instance.SkillStatus["Red"]);
        waterBtn.SetActive(SkillManager.Instance.SkillStatus["Blue"]);
        electricBtn.SetActive(SkillManager.Instance.SkillStatus["Yellow"]);
        grassBtn.SetActive(SkillManager.Instance.SkillStatus["Green"]);
        groundBtn.SetActive(SkillManager.Instance.SkillStatus["Brown"]);
    }
}
