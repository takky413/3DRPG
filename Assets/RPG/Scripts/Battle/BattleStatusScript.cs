using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStatusScript : MonoBehaviour
{
    public enum Status
    {
        HP,
        MP,
    }
    //�@�p�[�e�B�[�X�e�[�^�X
    [SerializeField]
    private PartyStatus partyStatus = null;
    //�@�L�����N�^�[�X�e�[�^�X�\��Transform���X�g
    private Dictionary<CharacterStatus, Transform> characterStatusDictionary = new Dictionary<CharacterStatus, Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //�@�ŏ��̑S�f�[�^�\��
        DisplayStatus();
    }

    //�@�X�e�[�^�X�f�[�^�̕\��
    public void DisplayStatus()
    {
        CharacterStatus member;
        Transform characterPanelTransform;
        for (int i = 0; i < 4; i++)
        {
            characterPanelTransform = transform.Find("CharacterPanel" + i);
            if (i < partyStatus.GetAllyStatus().Count)
            {
                member = partyStatus.GetAllyStatus()[i];
                characterPanelTransform.Find("CharacterName").GetComponent<Text>().text = member.GetCharacterName();
                characterPanelTransform.Find("HPSlider").GetComponent<Slider>().value = (float)member.GetHp() / member.GetMaxHp();
                characterPanelTransform.Find("HPSlider/HPText").GetComponent<Text>().text = member.GetHp().ToString();
                characterPanelTransform.Find("MPSlider").GetComponent<Slider>().value = (float)member.GetMp() / member.GetMaxMp();
                characterPanelTransform.Find("MPSlider/MPText").GetComponent<Text>().text = member.GetMp().ToString();
                characterStatusDictionary.Add(member, characterPanelTransform);
            }
            else
            {
                characterPanelTransform.gameObject.SetActive(false);
            }
        }
    }

    //�@�f�[�^�̍X�V
    public void UpdateStatus(CharacterStatus characterStatus, Status status, int destinationValue)
    {
        if (status == Status.HP)
        {
            characterStatusDictionary[characterStatus].Find("HPSlider").GetComponent<Slider>().value = (float)destinationValue / characterStatus.GetMaxHp();
            characterStatusDictionary[characterStatus].Find("HPSlider/HPText").GetComponent<Text>().text = destinationValue.ToString();
        }
        else if (status == Status.MP)
        {
            characterStatusDictionary[characterStatus].Find("MPSlider").GetComponent<Slider>().value = (float)destinationValue / characterStatus.GetMaxMp();
            characterStatusDictionary[characterStatus].Find("MPSlider/MPText").GetComponent<Text>().text = destinationValue.ToString();
        }
    }
}