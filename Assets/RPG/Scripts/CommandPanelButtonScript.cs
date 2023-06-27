using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandPanelButtonScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //�@�{�^����I���������ɕ\������摜
    private Image selectedImage;
    //�@�I���������̉���AudioSource
    private AudioSource audioSource;

    void Awake()
    {
        selectedImage = transform.Find("Image").GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //�@�A�N�e�B�u�ɂȂ��������g��EventSystem�őI������Ă�����
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            selectedImage.enabled = true;
            audioSource.Play();
        }
        else
        {
            selectedImage.enabled = false;
        }
    }

    //�@�{�^�����I�����ꂽ���Ɏ��s
    public void OnSelect(BaseEventData eventData)
    {
        selectedImage.enabled = true;
        audioSource.Play();
    }
    //�@�{�^�����I���������ꂽ���Ɏ��s
    public void OnDeselect(BaseEventData eventData)
    {
        selectedImage.enabled = false;
    }
}