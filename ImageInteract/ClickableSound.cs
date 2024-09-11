using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
           // Debug.LogError("AudioSource component is missing on this object");
        }
    }

    void OnMouseDown()
    {
        // Ŭ�� �̺�Ʈ �߻� �� �Ҹ� ���
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
