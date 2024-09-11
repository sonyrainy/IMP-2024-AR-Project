using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ARRaycastManager))]

public class PlaneMonsterSpawner2 : MonoBehaviour   // �������� 1 ���� ���� ��ũ��Ʈ
{
    private ARRaycastManager raycastManager;
    
    [SerializeField] private GameObject prefabFire;   // ���� ���� 1
    [SerializeField] private GameObject prefabWater;   // ���� ���� 2
    [SerializeField] private GameObject prefabElectric;   // ���� ���� 3
    [SerializeField] private GameObject prefabGrass;   // ���� ���� 3
    [SerializeField] private GameObject prefabGround;   // ���� ���� 3

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();   // ARRaycast ��� ���� ����Ʈ

    [HideInInspector]
    public int phase;   // �������� 1�� n��° ������ (ex. 1-1, 1-2 .. 1-n)
    [HideInInspector]
    public bool created; // ����� ���� ���͸� ��� �����ߴ°�? true or false
    [HideInInspector]
    public int monsterCount;    // ������ ���Ͱ� ��� �׾����� ī��Ʈ
    [HideInInspector]
    public Ray ray;     
    [HideInInspector]
    public Pose hitPose;        // ����� ��ġ�� ��ġ�� ����

    public GameObject ClickPlaneText;

    scoreController scoreController;
    void Awake()
    {
        monsterCount = 0;   // �ʱ�ȭ �κе�
        raycastManager = GetComponent<ARRaycastManager>();
        phase = 1;  
        created = false;
        ClickPlaneText.SetActive(true);

        scoreController = FindObjectOfType<scoreController>();
        //scoreController.refresh();
    }

    void Update()
    {
        if (created) {  // ���� �ش� ����� ���� ���͸� ��� �����߰�,
            if (monsterCount == 0) {    // ������ ���Ͱ� ��� �ı��Ǿ� 0���� �����ִٸ�,
                Debug.Log("go next phase"); 
                phase +=1;      // ���� �������.
                created = false;    // ���� ����� �ش��ϴ� ���͸� ���� �������� ���� �����̹Ƿ� false�� ��������
            }
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && !created && phase == 1)     // ��ġ �Ǿ���, 1 �������̰�, ���� ���� ������ ��������,
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // �÷��̾��� ray��
            if (raycastManager.Raycast(ray, hitResults, TrackableType.PlaneWithinPolygon))      // plane�� ��Ҵٸ�, (��ġ�� ���� �÷��� �����)
            {
                Debug.Log("phase 1!");  
                hitPose = hitResults[0].pose;   // ��ġ�� �� ��ġ �����ϰ�,  (�ش� �������������� �� ��ġ�� ������� �ֺ��� ���� ������)
                create1();  // 1������ ���� ����
                ClickPlaneText.SetActive(false);
             }
        }
        else if (!created && phase == 2)    // 2�������̰�, ���� ������ ���ߴٸ�,
        {
            Debug.Log("phase 2!");
            create2();  // 2������ ���� ����
        }
        else if (!created && phase == 3)    // 3�������̰�, ���� ������ ���ߴٸ�,
        {
            Debug.Log("phase 3!");
            create3();  // 3������ ���� ����
        }
        else if (!created && phase == 4)    // 4������� �����Ƿ�
        {
            Debug.Log("Stage 2 Clear!!");  //  �������� 1 Ŭ����!!
            phase += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }


    void create1() 
    {
        GameObject createRandom = null;


        for (int i = 0; i < 3; i++)     // 1������� ���� 3���� �����Ǵµ�
        {
            int type = Random.Range(0, 5);
            switch (type)
            {
                case 0:
                    createRandom = prefabFire; break;

                case 1:
                    createRandom = prefabWater; break;

                case 2:
                    createRandom = prefabElectric; break;

                case 3:
                    createRandom = prefabGrass; break;

                case 4:
                    createRandom = prefabGround; break;

                default: break;
            }

            var x_rand = Random.Range(-0.7f, 0.7f);
            var z_rand = Random.Range(-0.7f, 0.7f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);

            Vector3 spawnPosition = ranpos + yoffset + hitPose.position;
            GameObject monster = Instantiate(createRandom, spawnPosition, Quaternion.identity);  // prefab1P ���� �������� ������ġ�� ����
            Rigidbody rb = monster.GetComponent<Rigidbody>();   
            spawnedObjects.Add(monster);    
            monsterCount++;     // ���������Ƿ� ī��Ʈ +1
        }
      
        created = true; // �ش� �������� ���� �Ϸ������Ƿ� true
    }

    void create2()
    {
        GameObject createRandom = null;

        for (int i = 0; i < 6; i++)     // 2������� ���� 6����
        {
            int type = Random.Range(0, 5);
            switch (type)
            {
                case 0:
                    createRandom = prefabFire; break;

                case 1:
                    createRandom = prefabWater; break;

                case 2:
                    createRandom = prefabElectric; break;

                case 3:
                    createRandom = prefabGrass; break;

                case 4:
                    createRandom = prefabGround; break;

                default: break;
            }

            var x_rand = Random.Range(-0.7f, 0.7f);
            var z_rand = Random.Range(-0.7f, 0.7f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);

            Vector3 spawnPosition = ranpos + yoffset + hitPose.position;
            GameObject monster = Instantiate(createRandom, spawnPosition, Quaternion.identity); // prefab2P ���� �������� ������ġ�� ����
            Rigidbody rb = monster.GetComponent<Rigidbody>();
            spawnedObjects.Add(monster);
            monsterCount++;
        }
        created = true;  
    }
    void create3()
    {
        GameObject createRandom = null;

        for (int i = 0; i < 9; i++)      // 3������� ���� 9����
        {
            int type = Random.Range(0, 5);
            switch (type)
            {
                case 0:
                    createRandom = prefabFire; break;

                case 1:
                    createRandom = prefabWater; break;

                case 2:
                    createRandom = prefabElectric; break;

                case 3:
                    createRandom = prefabGrass; break;

                case 4:
                    createRandom = prefabGround; break;

                default: break;
            }

            var x_rand = Random.Range(-0.7f, 0.7f);
            var z_rand = Random.Range(-0.7f, 0.7f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);

            Vector3 spawnPosition = ranpos + yoffset + hitPose.position;
            GameObject monster = Instantiate(createRandom, spawnPosition, Quaternion.identity);    // prefab3P ���� �������� ������ġ�� ����
            Rigidbody rb = monster.GetComponent<Rigidbody>();
            spawnedObjects.Add(monster);
            monsterCount++; 
        }
        created = true;  
    }


}
