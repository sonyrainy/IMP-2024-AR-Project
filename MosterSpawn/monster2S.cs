using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monster2S : MonoBehaviour  // ���� �����鿡 �� ��ũ��Ʈ
{
    [HideInInspector]
    public float hp;    // ���� HP
                        // ���Ͱ� ���� �� ����� AudioSource
    public AudioClip deathSound;




    private float range = 1f;   // ���� �Ʒ����� ray�� ��Ÿ�
    PlaneMonsterSpawner1 MonsterSpawner1;   // PlaneMonsterSpawner1 ��ũ��Ʈ �ޱ����� ����
    PlaneMonsterSpawner2 MonsterSpawner2;
    PlaneMonsterSpawner3 MonsterSpawner3;

    public LayerMask planeMask;     // plane ����ũ
    scoreController scoreController; // ���� ����

    private float minTime = 2f;
    private float maxTime = 4f;

    private Renderer rend;
    private float timer;
    private Color originalColor;
    public bool isDeffense = false;

    private float speed = 0.1f;
    private float changeDirectionInterval = 2f;

    private Vector3 targetDirection;
    private float timer2;

    private Rigidbody rb;

    private Transform playerCamera; // �÷��̾��� ���� ī�޶�
    void Awake()
    {
        hp = 60;    // hp 10���� �ʱ⼳��
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            MonsterSpawner1 = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlaneMonsterSpawner1>();  //  "XR Origin (XR Rig)" ������Ʈ ���� PlaneMonsterSpawner1 ��ũ��Ʈ�� ������

        }
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            MonsterSpawner2 = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlaneMonsterSpawner2>();  //  "XR Origin (XR Rig)" ������Ʈ ���� PlaneMonsterSpawner1 ��ũ��Ʈ�� ������

        }
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            MonsterSpawner3 = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlaneMonsterSpawner3>();  //  "XR Origin (XR Rig)" ������Ʈ ���� PlaneMonsterSpawner1 ��ũ��Ʈ�� ������

        }

        //  rend = GetComponent<Renderer>();
        //  originalColor = rend.material.color;
        timer = Random.Range(minTime, maxTime);

        SetRandomDirection();
        timer2 = changeDirectionInterval;
    }

    private void Start()
    {
        scoreController = FindObjectOfType<scoreController>();
        rb = GetComponent<Rigidbody>();
        transform.Find("shield").gameObject.SetActive(false);
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (playerCamera != null)
        {
            // ���Ϳ� ī�޶��� ��ġ ���̸� ���մϴ�.
            Vector3 direction = playerCamera.position - transform.position;
            // y�� ������ ���� �����ϰ� ������ ���� ���� �����մϴ�.
            direction.y = 0f;
            // ���� ���͸� ȸ�������� ��ȯ�մϴ�.
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // ������ ȸ�� ���� �����մϴ�.
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
        }

        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                if (isDeffense)
                {
                    // rend.material.color = originalColor;
                    transform.Find("shield").gameObject.SetActive(false);

                }
                else
                {
                    //  rend.material.color = Color.gray;
                    transform.Find("shield").gameObject.SetActive(true);
                }
                isDeffense = !isDeffense;
                timer = Random.Range(minTime, maxTime);
            }

        }

        if (SceneManager.GetActiveScene().buildIndex == 7 || SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            timer2 -= Time.deltaTime;

            if (timer2 <= 0f)
            {
                SetRandomDirection();
                timer2 = changeDirectionInterval;
            }

            transform.Translate(targetDirection * speed * Time.deltaTime);
        }




        if (hp <= 0)
        {     // ���� HP�� 0�̵Ǹ�..
            Debug.Log("monster die");
            SoundManager.instance.SFXPlay("dieSound", deathSound);
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                MonsterSpawner1.monsterCount -= 1;  // ���Ͱ� �׾����Ƿ� monsterCount -1

            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                MonsterSpawner2.monsterCount -= 1;  // ���Ͱ� �׾����Ƿ� monsterCount -1

            }
            else if (SceneManager.GetActiveScene().buildIndex == 7)
            {
                MonsterSpawner3.monsterCount -= 1;  // ���Ͱ� �׾����Ƿ� monsterCount -1

            }


            scoreController.GetScore();
            Destroy(this.gameObject);   // �ش� ���� destroy
        }
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);   // �߶��� �����ϱ� ���� ���� �Ʒ������� ray
        RaycastHit hit;


        if (!Physics.Raycast(ray, out hit, range, planeMask))   // ray�� plane�� �����ʴ´ٸ�..
        {
            var x_rand = Random.Range(-0.5f, 0.5f);
            var z_rand = Random.Range(-0.5f, 0.5f);
            Vector3 ranpos = new Vector3(x_rand, 0, z_rand);
            Vector3 yoffset = new Vector3(0, 0.1f, 0);
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                Vector3 respawnPosition = ranpos + yoffset + MonsterSpawner1.hitPose.position;  // �������� ���۽� ��ġ�ߴ� plane �ֺ��� ���� ���� ���
                gameObject.transform.position = respawnPosition;    // ��ġ�� �װ����� �ٽ� �Űܼ� �߶��� ����
                Debug.Log("Monster respawn.");
            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                Vector3 respawnPosition = ranpos + yoffset + MonsterSpawner2.hitPose.position;  // �������� ���۽� ��ġ�ߴ� plane �ֺ��� ���� ���� ���
                gameObject.transform.position = respawnPosition;    // ��ġ�� �װ����� �ٽ� �Űܼ� �߶��� ����
                Debug.Log("Monster respawn.");
            }
            else if (SceneManager.GetActiveScene().buildIndex == 7)
            {

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                Vector3 respawnPosition = ranpos + yoffset + MonsterSpawner3.hitPose.position;  // �������� ���۽� ��ġ�ߴ� plane �ֺ��� ���� ���� ���
                gameObject.transform.position = respawnPosition;    // ��ġ�� �װ����� �ٽ� �Űܼ� �߶��� ����
                Debug.Log("Monster respawn.");
            }

        }
    }

    void SetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        targetDirection = new Vector3(randomX, 0f, randomZ).normalized;
    }


}
