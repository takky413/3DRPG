using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanelButtonScript : MonoBehaviour, ISelectHandler
{

    private Item item;
    //�@�A�C�e���^�C�g���\���e�L�X�g
    private Text itemTitleText;
    //�@�A�C�e�����\���e�L�X�g
    private Text itemInformationText;

    private void Awake()
    {
        itemTitleText = transform.root.Find("ItemInformationPanel/Title").GetComponent<Text>();
        itemInformationText = transform.root.Find("ItemInformationPanel/Information").GetComponent<Text>();
    }

    //�@�{�^�����I�����ꂽ���Ɏ��s
    public void OnSelect(BaseEventData eventData)
    {
        ShowItemInformation();
    }

    //�@�A�C�e�����̕\��
    public void ShowItemInformation()
    {
        itemTitleText.text = item.GetKanjiName();
        itemInformationText.text = item.GetInformation();
    }
    //�@�f�[�^���Z�b�g����
    public void SetParam(Item item)
    {
        this.item = item;
    }
}