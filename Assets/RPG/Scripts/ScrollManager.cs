using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    //�@�A�C�e���{�^���\���p�R���e���c
    private Transform content;
    //�@�X�N���[�������ǂ���
    private bool changeScrollValue;
    //�@�X�N���[���̖ړI�̒l
    private float destinationValue;
    //�@�X�N���[���X�s�[�h
    [SerializeField]
    private float scrollSpeed = 1000f;
    //�@���ŃX�N���[������l
    [SerializeField]
    private float scrollValue = 415f;
    //�@�A�C�e���ꗗ�̃X�N���[���̃f�t�H���g�l
    private Vector3 defaultScrollValue;

    //�@�O�ɑI�����Ă����{�^��
    public static GameObject PreSelectedButton { get; set; }

    void Awake()
    {
        content = transform;
        defaultScrollValue = content.transform.position;
    }

    void Update()
    {

        if (!changeScrollValue)
        {
            return;
        }

        //�@���X�ɖړI�̒l�ɕω�������
        content.transform.localPosition = new Vector3(content.transform.localPosition.x, Mathf.MoveTowards(content.transform.localPosition.y, destinationValue, scrollSpeed * Time.deltaTime), content.transform.localPosition.z);

        //�@������x�ړ�������ړI�n�ɐݒ�
        if (Mathf.Abs(content.transform.localPosition.y - destinationValue) < 0.2f)
        {
            changeScrollValue = false;
            content.transform.localPosition = new Vector3(0f, destinationValue, 0f);
        }
    }

    //�@���ɃX�N���[��
    public void ScrollDown(Transform button)
    {
        if (changeScrollValue)
        {
            changeScrollValue = false;
            content.transform.localPosition = new Vector3(content.transform.localPosition.x, destinationValue, content.transform.localPosition.z);
        }

        if (ScrollManager.PreSelectedButton != null
            && button.position.y > ScrollManager.PreSelectedButton.transform.position.y)
        {
            destinationValue = content.transform.localPosition.y - scrollValue;
            changeScrollValue = true;
        }

    }
    //�@��ɃX�N���[��
    public void ScrollUp(Transform button)
    {
        if (changeScrollValue)
        {
            content.transform.localPosition = new Vector3(content.transform.localPosition.x, destinationValue, content.transform.localPosition.z);
            changeScrollValue = false;
        }

        if (ScrollManager.PreSelectedButton != null
            && button.position.y < ScrollManager.PreSelectedButton.transform.position.y)
        {
            destinationValue = content.transform.localPosition.y + scrollValue;
            changeScrollValue = true;
        }
    }

    public void Reset()
    {
        PreSelectedButton = null;
        transform.position = defaultScrollValue;
    }
}
