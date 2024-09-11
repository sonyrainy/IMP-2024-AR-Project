using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Raycast : MonoBehaviour    // �÷��̾��� ��ġ raycast ��� ��ũ��Ʈ
{
    public LayerMask monsterMask;   // ���� ���̾�

    private float range = 20f;
    private bool touched;   // ��ġ�ߴ°�?
    private float damage = 10f;
    private float effective = 1.0f;
    private string currentSkill;

    public ParticleSystem hitFire; // �¾��� �� ����� ����Ʈ ������
    public ParticleSystem hitWater;
    public ParticleSystem hitElectric;
    public ParticleSystem hitGround;
    public ParticleSystem hitGrass;

    public AudioSource fireSound;
    public AudioSource waterSound;
    public AudioSource electricSound;
    public AudioSource groundSound;
    public AudioSource grassSound;
    public AudioSource deffenseSound;

    private void Awake()
    {

    }

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)   // ��ġ�ϸ�
        {
            touched = true; // true��
        }

    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (touched && Physics.Raycast(ray, out hit, range, monsterMask))   // ���͸� ��ġ�ѰŶ��,
        {
            if (hit.collider.tag == "fire")
            {
                
                switch (CurrentSkill.currentSkill)
                {
                    case "water":
                        effective = 2.0f; break;

                    case "grass":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "water")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "electric":
                        effective = 2.0f; break;

                    case "fire":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "electric")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "ground":
                        effective = 2.0f; break;

                    case "water":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "ground")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "grass":
                        effective = 2.0f; break;

                    case "electric":
                        effective = 0.5f; break;

                    default: break;
                }
            }
            if (hit.collider.tag == "grass")
            {
                switch (CurrentSkill.currentSkill)
                {
                    case "fire":
                        effective = 2.0f; break;

                    case "ground":
                        effective = 0.5f; break;

                    default: break;
                }
            }

            damage *= effective;
            if (hit.collider.gameObject.GetComponent<monster2S>().isDeffense == false)
            {
                hit.collider.gameObject.GetComponent<monster2S>().hp -= damage;
                hit.collider.gameObject.GetComponent<HPUI>().HpSlider.value -= damage;

                Debug.Log(damage + " ��ŭ�� �������� �־����ϴ�!");
                switch (CurrentSkill.currentSkill)
                {
                    case "fire":
                        fireSound.Play();
                        break;
                    case "water":
                        waterSound.Play();
                        break;
                    case "electric":
                        electricSound.Play();
                        break;
                    case "grass":
                        grassSound.Play();
                        break;
                    case "ground":
                        groundSound.Play();
                        break;

                }
                PlayHitEffect(hit.point); // �浹 ������ �����Ͽ� �¾��� �� ����Ʈ�� ����մϴ�.
            }
            else if (hit.collider.gameObject.GetComponent<monster2S>().isDeffense == true)
            {
                Debug.Log("��밡 �������Դϴ�!");
                deffenseSound.Play();
            }
            damage = 10f;
            effective = 1.0f;


        }




        touched = false;    // ��ġ ���� �ʱ�ȭ   
    }


    private void PlayHitEffect(Vector3 position)
    {
        // ����Ʈ�� �����ϰ� ������ ��ġ�� ����մϴ�.
        switch (CurrentSkill.currentSkill)
        {
            case "fire":
                Instantiate(hitFire, position, Quaternion.identity); break;
                Debug.Log("���� ������");

            case "water":
                Instantiate(hitWater, position, Quaternion.identity); break;

            case "electric":
                Instantiate(hitElectric, position, Quaternion.identity); break;

            case "grass":
                Instantiate(hitGrass, position, Quaternion.identity); break;

            case "ground":
                Instantiate(hitGround, position, Quaternion.identity); break;

            default: break;
        }

    }
}
