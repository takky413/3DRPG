using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandScript : MonoBehaviour
{

    public enum CommandMode
    {
        CommandPanel,
        StatusPanelSelectCharacter,
        StatusPanel,
        ItemPanelSelectCharacter,
        ItemPanel,
        UseItemPanel,
        UseItemSelectCharacterPanel,
        UseItemPanelToItemPanel,
        UseItemPanelToUseItemPanel,
        UseItemSelectCharacterPanelToUseItemPanel,
        NoItemPassed
    }

    private CommandMode currentCommand;
    //�@���j�e�B�����R�}���h�X�N���v�g
    private UnityChanCommandScript unityChanCommandScript;

    //�@�ŏ��ɑI������Button��Transform
    private GameObject firstSelectButton;

    //�@�R�}���h�p�l��
    private GameObject commandPanel;
    //�@�X�e�[�^�X�\���p�l��
    private GameObject statusPanel;
    //�@�L�����N�^�[�I���p�l��
    private GameObject selectCharacterPanel;
    //�@�A�C�e���\���p�l��
    private GameObject itemPanel;
    //�@�A�C�e���p�l���{�^����\������ꏊ
    private GameObject content;
    //�@�A�C�e�����g���I���p�l��
    private GameObject useItemPanel;
    //�@�A�C�e�����g�������N�ɂ��邩�I������p�l��
    private GameObject useItemSelectCharacterPanel;
    //�@���\���p�l��
    private GameObject itemInformationPanel;
    //�@�A�C�e���g�p��̏��\���p�l��
    private GameObject useItemInformationPanel;

    //�@�R�}���h�p�l����CanvasGroup
    private CanvasGroup commandPanelCanvasGroup;
    //�@�L�����N�^�[�I���p�l����CanvasGroup
    private CanvasGroup selectCharacterPanelCanvasGroup;
    //�@�A�C�e���p�l����Canvas Group
    private CanvasGroup itemPanelCanvasGroup;
    //�@�A�C�e�����g���I���p�l����CanvasGroup
    private CanvasGroup useItemPanelCanvasGroup;
    //�@�A�C�e�����g���L�����N�^�[�I���p�l����CanvasGroup;
    private CanvasGroup useItemSelectCharacterPanelCanvasGroup;

    //�@�L�����N�^�[��
    private Text characterNameText;
    //�@�X�e�[�^�X�^�C�g���e�L�X�g
    private Text statusTitleText;
    //�@�X�e�[�^�X�p�����[�^�e�L�X�g1
    private Text statusParam1Text;
    //�@�X�e�[�^�X�p�����[�^�e�L�X�g2
    private Text statusParam2Text;
    //�@���\���^�C�g���e�L�X�g
    private Text informationTitleText;
    //�@���\���e�L�X�g
    private Text informationText;

    //�@�p�[�e�B�[�X�e�[�^�X
    [SerializeField]
    private PartyStatus partyStatus = null;

    //�@�L�����N�^�[�I���̃{�^���̃v���n�u
    [SerializeField]
    private GameObject characterPanelButtonPrefab = null;
    //�@�L�����N�^�[�A�C�e���̃{�^���̃v���n�u
    [SerializeField]
    private GameObject itemPanelButtonPrefab = null;
    //�@�A�C�e���g�p���̃{�^���̃v���n�u
    [SerializeField]
    private GameObject useItemPanelButtonPrefab = null;

    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    //�@�A�C�e���{�^���ꗗ
    private List<GameObject> itemPanelButtonList = new List<GameObject>();

    //�@�A�C�e���p�l���{�^���łǂ̔ԍ��̃{�^�������ɃX�N���[�����邩
    [SerializeField]
    private int scrollDownButtonNum = 12;
    //�@�A�C�e���p�l���{�^���łǂ̔ԍ��̃{�^�����牺�ɃX�N���[�����邩
    [SerializeField]
    private int scrollUpButtonNum = 14;
    //�@ScrollManager
    private ScrollManager scrollManager;


    void Awake()
    {
        //�@�R�}���h��ʂ��J�����������Ă���UnityChanCommandScript���擾
        unityChanCommandScript = GameObject.FindWithTag("Player").GetComponent<UnityChanCommandScript>();

        //�@���݂̃R�}���h��������
        currentCommand = CommandMode.CommandPanel;

        //�@�K�w��H���Ă��擾
        firstSelectButton = transform.Find("CommandPanel/StatusButton").gameObject;

        //�@�p�l���n
        commandPanel = transform.Find("CommandPanel").gameObject;
        statusPanel = transform.Find("StatusPanel").gameObject;
        selectCharacterPanel = transform.Find("SelectCharacterPanel").gameObject;
        itemPanel = transform.Find("ItemPanel").gameObject;
        content = itemPanel.transform.Find("Mask/Content").gameObject;
        useItemPanel = transform.Find("UseItemPanel").gameObject;
        useItemSelectCharacterPanel = transform.Find("UseItemSelectCharacterPanel").gameObject;
        itemInformationPanel = transform.Find("ItemInformationPanel").gameObject;
        useItemInformationPanel = transform.Find("UseItemInformationPanel").gameObject;

        //�@CanvasGroup
        commandPanelCanvasGroup = commandPanel.GetComponent<CanvasGroup>();
        selectCharacterPanelCanvasGroup = selectCharacterPanel.GetComponent<CanvasGroup>();
        itemPanelCanvasGroup = itemPanel.GetComponent<CanvasGroup>();
        useItemPanelCanvasGroup = useItemPanel.GetComponent<CanvasGroup>();
        useItemSelectCharacterPanelCanvasGroup = useItemSelectCharacterPanel.GetComponent<CanvasGroup>();

        //�@�X�e�[�^�X�p�e�L�X�g
        characterNameText = statusPanel.transform.Find("CharacterNamePanel/Text").GetComponent<Text>();
        statusTitleText = statusPanel.transform.Find("StatusParamPanel/Title").GetComponent<Text>();
        statusParam1Text = statusPanel.transform.Find("StatusParamPanel/Param1").GetComponent<Text>();
        statusParam2Text = statusPanel.transform.Find("StatusParamPanel/Param2").GetComponent<Text>();

        //�@���\���p�e�L�X�g
        informationTitleText = itemInformationPanel.transform.Find("Title").GetComponent<Text>();
        informationText = itemInformationPanel.transform.Find("Information").GetComponent<Text>();

        //�X�N���[���p
        scrollManager = content.GetComponent<ScrollManager>();
    }


    private void OnEnable()
    {
        //�@���݂̃R�}���h�̏�����
        currentCommand = CommandMode.CommandPanel;
        //�@�R�}���h���j���[�\�����ɑ��̃p�l���͔�\���ɂ���
        statusPanel.SetActive(false);
        selectCharacterPanel.SetActive(false);

        // �L�����N�^�[�I���{�^��������ΑS�č폜
        for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
        }
        //�@�A�C�e���p�l���{�^��������ΑS�č폜
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        //�@�A�C�e�����g���L�����N�^�[�I���{�^��������ΑS�č폜
        for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemPanel.transform.GetChild(i).gameObject);
        }
        //�@�A�C�e�����g������̃L�����N�^�[�I���{�^��������ΑS�č폜
        for (int i = useItemSelectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemSelectCharacterPanel.transform.GetChild(i).gameObject);
        }

        selectedGameObjectStack.Clear();
        itemPanelButtonList.Clear();

        commandPanelCanvasGroup.interactable = true;
        selectCharacterPanelCanvasGroup.interactable = false;
        EventSystem.current.SetSelectedGameObject(firstSelectButton);

        itemPanel.SetActive(false);
        useItemPanel.SetActive(false);
        useItemSelectCharacterPanel.SetActive(false);
        itemInformationPanel.SetActive(false);
        useItemInformationPanel.SetActive(false);

        itemPanelCanvasGroup.interactable = false;
        useItemPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanelCanvasGroup.interactable = false;
    }


    private void Update()
    {

        //�@�L�����Z���{�^�������������̏���
        if (Input.GetButtonDown("Cancel"))
        {
            //�@�R�}���h�I����ʎ�
            if (currentCommand == CommandMode.CommandPanel)
            {
                unityChanCommandScript.ExitCommand();
                gameObject.SetActive(false);
                //�@�X�e�[�^�X�L�����N�^�[�I���܂��̓X�e�[�^�X�\����
            }
            else if (currentCommand == CommandMode.StatusPanelSelectCharacter || currentCommand == CommandMode.StatusPanel)
            {
                selectCharacterPanelCanvasGroup.interactable = false;
                selectCharacterPanel.SetActive(false);
                statusPanel.SetActive(false);
                //�@�L�����N�^�[�I���p�l���̎q�v�f�̃{�^�����폜
                for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                }
                //�@�O�̃p�l���őI�����Ă����Q�[���I�u�W�F�N�g��I��
                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                commandPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.CommandPanel;
            }
            //�@�ǂ̃L�����N�^�[�̃A�C�e����\�����邩�̑I����
            else if (currentCommand == CommandMode.ItemPanelSelectCharacter)
            {
                selectCharacterPanelCanvasGroup.interactable = false;
                selectCharacterPanel.SetActive(false);
                itemInformationPanel.SetActive(false);

                for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                commandPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.CommandPanel;
                //�@�A�C�e���ꗗ�\����
            }
            else if (currentCommand == CommandMode.ItemPanel)
            {
                itemPanelCanvasGroup.interactable = false;
                itemPanel.SetActive(false);
                itemInformationPanel.SetActive(false);
                //�@���X�g���N���A
                itemPanelButtonList.Clear();
                //�@ItemPanel��Cancel����������content�ȉ��̃A�C�e���p�l���{�^����S�폜
                for (int i = content.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(content.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                selectCharacterPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.ItemPanelSelectCharacter;
                //�@�A�C�e����I�����A�ǂ��g������I�����Ă��鎞
            }
            else if (currentCommand == CommandMode.UseItemPanel)
            {
                useItemPanelCanvasGroup.interactable = false;
                useItemPanel.SetActive(false);
                //�@UseItemPanel��Cancel�{�^������������UseItemPanel�̎q�v�f�̃{�^���̑S�폜
                for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(useItemPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                itemPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.ItemPanel;
                //�@�A�C�e�����g�p���鑊��̃L�����N�^�[��I�����Ă��鎞
            }
            else if (currentCommand == CommandMode.UseItemSelectCharacterPanel)
            {
                useItemSelectCharacterPanelCanvasGroup.interactable = false;
                useItemSelectCharacterPanel.SetActive(false);
                //�@UseItemSelectCharacterPanel��Cancel�{�^������������A�C�e�����g�p����L�����N�^�[�{�^���̑S�폜
                for (int i = useItemSelectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(useItemSelectCharacterPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                useItemPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.UseItemPanel;
            }
        }

        //�@�A�C�e���𑕔��A�������O�����\����̏���
        if (currentCommand == CommandMode.UseItemPanelToItemPanel)
        {
            if (Input.anyKeyDown
                || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
                || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
                )
            {
                currentCommand = CommandMode.ItemPanel;
                useItemInformationPanel.SetActive(false);
                itemPanel.transform.SetAsLastSibling();
                itemPanelCanvasGroup.interactable = true;

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());

            }
            //�@�A�C�e�����g�p���鑊��̃L�����N�^�[�I������A�C�e�����ǂ����邩�Ɉڍs���鎞
        }
        else if (currentCommand == CommandMode.UseItemSelectCharacterPanelToUseItemPanel)
        {
            if (Input.anyKeyDown
                || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
                || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
                )
            {
                currentCommand = CommandMode.UseItemPanel;
                useItemInformationPanel.SetActive(false);
                useItemPanel.transform.SetAsLastSibling();
                useItemPanelCanvasGroup.interactable = true;

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
            }
            //�@�A�C�e�����̂Ă��I��������̏��
        }
        else if (currentCommand == CommandMode.UseItemPanelToUseItemPanel)
        {
            if (Input.anyKeyDown
                || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
                || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
                )
            {
                currentCommand = CommandMode.UseItemPanel;
                useItemInformationPanel.SetActive(false);
                useItemPanel.transform.SetAsLastSibling();
                useItemPanelCanvasGroup.interactable = true;
            }
            //�@�A�C�e�����g�p�A�n���A�̂Ă��I��������ɂ��̃A�C�e���̐���0�ɂȂ�����
        }
        else if (currentCommand == CommandMode.NoItemPassed)
        {
            if (Input.anyKeyDown
                || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
                || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
                )
            {
                currentCommand = CommandMode.ItemPanel;
                useItemInformationPanel.SetActive(false);
                useItemPanel.SetActive(false);
                itemPanel.transform.SetAsLastSibling();
                itemPanelCanvasGroup.interactable = true;

                //�@�A�C�e���p�l���{�^��������΍ŏ��̃A�C�e���p�l���{�^����I��
                if (content.transform.childCount != 0)
                {
                    EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
                }
                else
                {
                    //�@�A�C�e���p�l���{�^�����Ȃ���΁i�A�C�e���������Ă��Ȃ��jItemSelectPanel�ɖ߂�
                    currentCommand = CommandMode.ItemPanelSelectCharacter;
                    itemPanelCanvasGroup.interactable = false;
                    itemPanel.SetActive(false);
                    selectCharacterPanelCanvasGroup.interactable = true;
                    selectCharacterPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                }
            }
        }

        //�@�I���������ꂽ���i�}�E�X��UI�O���N���b�N�����j�͌��݂̃��[�h�ɂ���Ė������I��������
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (currentCommand == CommandMode.CommandPanel)
            {
                EventSystem.current.SetSelectedGameObject(commandPanel.transform.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.ItemPanel)
            {
                EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
                scrollManager.Reset();
            }
            else if (currentCommand == CommandMode.ItemPanelSelectCharacter)
            {
                EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.StatusPanel)
            {
                EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.StatusPanelSelectCharacter)
            {
                EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.UseItemPanel)
            {
                EventSystem.current.SetSelectedGameObject(useItemPanel.transform.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.UseItemSelectCharacterPanel)
            {
                EventSystem.current.SetSelectedGameObject(useItemSelectCharacterPanel.transform.GetChild(0).gameObject);
            }
        }
    }

    //�@�I�������R�}���h�ŏ�������
    public void SelectCommand(string command)
    {
        if (command == "Status")
        {
            currentCommand = CommandMode.StatusPanelSelectCharacter;
            //�@UI�̃I���E�I�t��I���A�C�R���̐ݒ�
            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            GameObject characterButtonIns;

            //�@�p�[�e�B�[�����o�[���̃{�^�����쐬
            foreach (var member in partyStatus.GetAllyStatus())
            {
                characterButtonIns = Instantiate<GameObject>(characterPanelButtonPrefab, selectCharacterPanel.transform);
                characterButtonIns.GetComponentInChildren<Text>().text = member.GetCharacterName();
                characterButtonIns.GetComponent<Button>().onClick.AddListener(() => ShowStatus(member));
            }
        }
        else if (command == "Item")
        {
            currentCommand = CommandMode.ItemPanelSelectCharacter;
            statusPanel.SetActive(false);
            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            GameObject characterButtonIns;

            //�@�p�[�e�B�����o�[���̃{�^�����쐬
            foreach (var member in partyStatus.GetAllyStatus())
            {
                characterButtonIns = Instantiate<GameObject>(characterPanelButtonPrefab, selectCharacterPanel.transform);
                characterButtonIns.GetComponentInChildren<Text>().text = member.GetCharacterName();
                characterButtonIns.GetComponent<Button>().onClick.AddListener(() => CreateItemPanelButton(member));
            }
            
        }
        //�@�K�w����ԍŌ�ɕ��בւ�
        selectCharacterPanel.transform.SetAsLastSibling();
        selectCharacterPanel.SetActive(true);
        selectCharacterPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);

    }


    //�@�L�����N�^�[�̃X�e�[�^�X�\��
    public void ShowStatus(AllyStatus allyStatus)
    {
        currentCommand = CommandMode.StatusPanel;
        statusPanel.SetActive(true);
        //�@�L�����N�^�[�̖��O��\��
        characterNameText.text = allyStatus.GetCharacterName();

        //�@�^�C�g���̕\��
        var text = "���x��\n";
        text += "HP\n";
        text += "MP\n";
        text += "�o���l\n";
        text += "��Ԉُ�\n";
        text += "��\n";
        text += "�f����\n";
        text += "�ł��ꋭ��\n";
        text += "���@��\n";
        text += "��������\n";
        text += "�����Z\n";
        text += "�U����\n";
        text += "�h���\n";
        statusTitleText.text = text;

        //�@HP��MP��Division�L���̕\��
        text = "\n";
        text += allyStatus.GetHp() + "\n";
        text += allyStatus.GetMp() + "\n";
        statusParam1Text.text = text;

        //�@�X�e�[�^�X�p�����[�^�̕\��
        text = allyStatus.GetLevel() + "\n";
        text += allyStatus.GetMaxHp() + "\n";
        text += allyStatus.GetMaxMp() + "\n";
        text += allyStatus.GetEarnedExperience() + "\n";
        if (!allyStatus.IsPoisonState() && !allyStatus.IsNumbnessState())
        {
            text += "����";
        }
        else
        {
            if (allyStatus.IsPoisonState())
            {
                text += "��";
                if (allyStatus.IsNumbnessState())
                {
                    text += "�AჂ�";
                }
            }
            else
            {
                if (allyStatus.IsNumbnessState())
                {
                    text += "Ⴢ�";
                }
            }
        }

        text += "\n";
        text += allyStatus.GetPower() + "\n";
        text += allyStatus.GetAgility() + "\n";
        text += allyStatus.GetStrikingStrength() + "\n";
        text += allyStatus.GetMagicPower() + "\n";
        text += allyStatus?.GetEquipWeapon()?.GetKanjiName() ?? "";
        text += "\n";
        text += allyStatus.GetEquipArmor()?.GetKanjiName() ?? "";
        text += "\n";
        text += allyStatus.GetPower() + (allyStatus.GetEquipWeapon()?.GetAmount() ?? 0) + "\n";
        text += allyStatus.GetStrikingStrength() + (allyStatus.GetEquipArmor()?.GetAmount() ?? 0) + "\n";
        statusParam2Text.text = text;
    }


    //�@�L�����N�^�[�������Ă���A�C�e���̃{�^���\��
    public void CreateItemPanelButton(AllyStatus allyStatus)
    {
        itemInformationPanel.SetActive(true);
        selectCharacterPanelCanvasGroup.interactable = false;

        //�@�A�C�e���ꗗ�̃X�N���[���l�̏�����
        scrollManager.Reset();

        //�@�A�C�e���p�l���{�^�������쐬�������ǂ���
        int itemPanelButtonNum = 0;
        GameObject itemButtonIns;

        //�@�I�������L�����N�^�[�̃A�C�e�������A�C�e���p�l���{�^�����쐬
        //�@�����Ă���A�C�e�����̃{�^���̍쐬�ƃN���b�N���̎��s���\�b�h�̐ݒ�
        foreach (var item in allyStatus.GetItemDictionary().Keys)
        {
            itemButtonIns = Instantiate<GameObject>(itemPanelButtonPrefab, content.transform);
            itemButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetKanjiName();
            itemButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItem(allyStatus, item));
            itemButtonIns.GetComponent<ItemPanelButtonScript>().SetParam(item);

            //�@�w�肵���ԍ��̃A�C�e���p�l���{�^���ɃA�C�e���X�N���[���p�X�N���v�g�����t����
            if (itemPanelButtonNum != 0
                && (itemPanelButtonNum % scrollDownButtonNum == 0
                || itemPanelButtonNum % (scrollDownButtonNum + 1) == 0)
                )
            {
                itemButtonIns.AddComponent<ScrollDownScript>();
            }
            else if (itemPanelButtonNum != 0
              && (itemPanelButtonNum % scrollUpButtonNum == 0
              || itemPanelButtonNum % (scrollUpButtonNum + 1) == 0)
              )
            {
                //�@�A�C�e���X�N���[���X�N���v�g�̎��t���Đݒ�l�̃Z�b�g
                itemButtonIns.AddComponent<ScrollUpScript>();
            }

            //�@�A�C�e������\��
            itemButtonIns.transform.Find("Num").GetComponent<Text>().text = allyStatus.GetItemNum(item).ToString();

            //�@�������Ă��镐���h��ɂ͖��O�̑O��E��\�����A����Text��ێ����Ēu��
            if (allyStatus.GetEquipWeapon() == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
            }
            else if (allyStatus.GetEquipArmor() == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
            }

            //�@�A�C�e���{�^�����X�g�ɒǉ�
            itemPanelButtonList.Add(itemButtonIns);

            //�@�A�C�e���p�l���{�^���ԍ����X�V
            itemPanelButtonNum++;

            if (itemPanelButtonNum == scrollUpButtonNum + 2)
            {
                Debug.Log(itemPanelButtonNum);
                itemPanelButtonNum = 2;
            }
        }

        //�@�A�C�e���p�l���̕\���ƍŏ��̃A�C�e���̑I��
        if (content.transform.childCount != 0)
        {
            //�@SelectCharacerPanel�ōŌ�ɂǂ̃Q�[���I�u�W�F�N�g��I�����Ă�����
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);
            currentCommand = CommandMode.ItemPanel;
            itemPanel.SetActive(true);
            itemPanel.transform.SetAsLastSibling();
            itemPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
        }
        else
        {
            informationTitleText.text = "";
            informationText.text = "�A�C�e���������Ă��܂���B";
            selectCharacterPanelCanvasGroup.interactable = true;
        }
    }


    //�@�A�C�e�����ǂ����邩�̑I��
    public void SelectItem(AllyStatus allyStatus, Item item)
    {
        //�@�A�C�e���̎�ނɉ����ďo���鍀�ڂ�ύX����
        if (item.GetItemType() == Item.Type.ArmorAll
            || item.GetItemType() == Item.Type.ArmorUnityChan
            || item.GetItemType() == Item.Type.ArmorYuji
            || item.GetItemType() == Item.Type.WeaponAll
            || item.GetItemType() == Item.Type.WeaponUnityChan
            || item.GetItemType() == Item.Type.WeaponYuji)
        {

            var itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            if (item == allyStatus.GetEquipWeapon() || item == allyStatus.GetEquipArmor())
            {
                itemMenuButtonIns.GetComponentInChildren<Text>().text = "�������O��";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => RemoveEquip(allyStatus, item));
            }
            else
            {
                itemMenuButtonIns.GetComponentInChildren<Text>().text = "��������";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => Equip(allyStatus, item));
            }

            itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "�n��";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => PassItem(allyStatus, item));

            itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "�̂Ă�";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => ThrowAwayItem(allyStatus, item));

        }
        else if (item.GetItemType() == Item.Type.NumbnessRecovery
          || item.GetItemType() == Item.Type.PoisonRecovery
          || item.GetItemType() == Item.Type.HPRecovery
          || item.GetItemType() == Item.Type.MPRecovery
          )
        {

            var itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "�g��";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItem(allyStatus, item));

            itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "�n��";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => PassItem(allyStatus, item));

            itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "�̂Ă�";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => ThrowAwayItem(allyStatus, item));

        }
        else if (item.GetItemType() == Item.Type.Valuables)
        {
            informationTitleText.text = item.GetKanjiName();
            informationText.text = item.GetInformation();
        }

        if (item.GetItemType() != Item.Type.Valuables)
        {
            useItemPanel.SetActive(true);
            itemPanelCanvasGroup.interactable = false;
            currentCommand = CommandMode.UseItemPanel;
            //�@ItemPanel�ōŌ�ɂǂ��I�����Ă������H
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            useItemPanel.transform.SetAsLastSibling();
            EventSystem.current.SetSelectedGameObject(useItemPanel.transform.GetChild(0).gameObject);
            useItemPanelCanvasGroup.interactable = true;
            Input.ResetInputAxes();

        }
    }


    //�@�A�C�e�����g�p����L�����N�^�[��I������
    public void UseItem(AllyStatus allyStatus, Item item)
    {
        useItemPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanel.SetActive(true);
        //�@UseItemPanel�łǂ���Ō�ɑI�����Ă�����
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject characterButtonIns;
        //�@�p�[�e�B�����o�[���̃{�^�����쐬
        foreach (var member in partyStatus.GetAllyStatus())
        {
            characterButtonIns = Instantiate<GameObject>(characterPanelButtonPrefab, useItemSelectCharacterPanel.transform);
            characterButtonIns.GetComponentInChildren<Text>().text = member.GetCharacterName();
            characterButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItemToCharacter(allyStatus, member, item));
        }
        //�@UseItemSelectCharacterPanel�Ɉڍs����
        currentCommand = CommandMode.UseItemSelectCharacterPanel;
        useItemSelectCharacterPanel.transform.SetAsLastSibling();
        EventSystem.current.SetSelectedGameObject(useItemSelectCharacterPanel.transform.GetChild(0).gameObject);
        useItemSelectCharacterPanelCanvasGroup.interactable = true;
        Input.ResetInputAxes();
    }


    public void UseItemToCharacter(AllyStatus fromChara, AllyStatus toChara, Item item)
    {
        useItemInformationPanel.SetActive(true);
        useItemSelectCharacterPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanel.SetActive(false);

        if (item.GetItemType() == Item.Type.HPRecovery)
        {
            if (toChara.GetHp() == toChara.GetMaxHp())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "�͌��C�ł��B";
            }
            else
            {
                toChara.SetHp(toChara.GetHp() + item.GetAmount());
                //�@�A�C�e�����g�p�����|��\��
                useItemInformationPanel.GetComponentInChildren<Text>().text = fromChara.GetCharacterName() + "��" + item.GetKanjiName() + "��" + toChara.GetCharacterName() + "�Ɏg�p���܂����B\n" +
                    toChara.GetCharacterName() + "��" + item.GetAmount() + "�񕜂��܂����B";
                //�@�����Ă���A�C�e���������炷
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }
        else if (item.GetItemType() == Item.Type.MPRecovery)
        {
            if (toChara.GetMp() == toChara.GetMaxMp())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "��MP�͍ő�ł��B";
            }
            else
            {
                toChara.SetMp(toChara.GetMp() + item.GetAmount());
                //�@�A�C�e�����g�p�����|��\��
                useItemInformationPanel.GetComponentInChildren<Text>().text = fromChara.GetCharacterName() + "��" + item.GetKanjiName() + "��" + toChara.GetCharacterName() + "�Ɏg�p���܂����B\n" +
                    toChara.GetCharacterName() + "��MP��" + item.GetAmount() + "�񕜂��܂����B";
                //�@�����Ă���A�C�e���������炷
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }
        else if (item.GetItemType() == Item.Type.PoisonRecovery)
        {
            if (!toChara.IsPoisonState())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "�͓ŏ�Ԃł͂���܂���B";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "�͓ł���񕜂��܂����B";
                toChara.SetPoisonState(false);
                //�@�����Ă���A�C�e���������炷
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }
        else if (item.GetItemType() == Item.Type.NumbnessRecovery)
        {
            if (!toChara.IsNumbnessState())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "��Ⴢ��Ԃł͂���܂���B";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "��Ⴢꂩ��񕜂��܂����B";
                toChara.SetNumbness(false);
                //�@�����Ă���A�C�e���������炷
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }

        //�@�A�C�e�����g�p������A�C�e�����g�p���鑊���UseItemSelectCharacterPanel�̎q�v�f�̃{�^����S�폜
        for (int i = useItemSelectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemSelectCharacterPanel.transform.GetChild(i).gameObject);
        }
        //�@itemPanleButtonList����Y������A�C�e����T�������X�V����
        var itemButton = itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
        itemButton.transform.Find("Num").GetComponent<Text>().text = fromChara.GetItemNum(item).ToString();

        //�@�A�C�e������0��������{�^���ƃL�����N�^�[�X�e�[�^�X����A�C�e�����폜
        if (fromChara.GetItemNum(item) == 0)
        {
            //�@�A�C�e����0�ɂȂ������C��ItemPanel�ɖ߂��ׁAUseItemPanel����UseItemSelectCharacterPanel���ł̃I�u�W�F�N�g�o�^���폜
            selectedGameObjectStack.Pop();
            selectedGameObjectStack.Pop();
            //�@itemPanelButtonList����A�C�e���p�l���{�^�����폜
            itemPanelButtonList.Remove(itemButton);
            //�@�A�C�e���p�l���{�^�����g�̍폜
            Destroy(itemButton);
            //�@�A�C�e����n�����L�����N�^�[���g��ItemDictionary���炻�̃A�C�e�����폜
            fromChara.GetItemDictionary().Remove(item);
            //�@ItemPanel�ɖ߂�ׁAUseItemPanel���ɍ�����{�^����S�폜
            for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(useItemPanel.transform.GetChild(i).gameObject);
            }
            //�@�A�C�e������0�ɂȂ����̂�CommandMode.NoItemPassed�ɕύX
            currentCommand = CommandMode.NoItemPassed;
            useItemInformationPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
        else
        {
            //�@�A�C�e�������c���Ă���ꍇ��UseItemPanel�ŃA�C�e�����ǂ����邩�̑I���ɖ߂�
            currentCommand = CommandMode.UseItemSelectCharacterPanelToUseItemPanel;
            useItemInformationPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
    }


    //�@�n��
    public void PassItem(AllyStatus allyStatus, Item item)
    {

        useItemPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanel.SetActive(true);
        //�@UseItemPanel�łǂ���Ō�ɑI�����Ă�����
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject characterButtonIns;
        //�@�p�[�e�B�����o�[���̃{�^�����쐬
        foreach (var member in partyStatus.GetAllyStatus())
        {
            if (member != allyStatus)
            {
                characterButtonIns = Instantiate<GameObject>(characterPanelButtonPrefab, useItemSelectCharacterPanel.transform);
                characterButtonIns.GetComponentInChildren<Text>().text = member.GetCharacterName();
                characterButtonIns.GetComponent<Button>().onClick.AddListener(() => PassItemToOtherCharacter(allyStatus, member, item));
            }
        }
        //�@UseItemSelectCharacterPanel�Ɉڍs����
        currentCommand = CommandMode.UseItemSelectCharacterPanel;
        useItemSelectCharacterPanel.transform.SetAsLastSibling();
        EventSystem.current.SetSelectedGameObject(useItemSelectCharacterPanel.transform.GetChild(0).gameObject);
        useItemSelectCharacterPanelCanvasGroup.interactable = true;
        Input.ResetInputAxes();

    }


    //�@�n��������w�肵�A�C�e�����̑���������
    public void PassItemToOtherCharacter(AllyStatus fromChara, AllyStatus toChara, Item item)
    {

        useItemInformationPanel.SetActive(true);
        useItemSelectCharacterPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanel.SetActive(false);

        //�@�����Ă���A�C�e���������炷
        fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
        //�@�n���ꂽ�L�����N�^�[���A�C�e���������Ă��Ȃ���΂��̃A�C�e����o�^
        if (!toChara.GetItemDictionary().ContainsKey(item))
        {
            toChara.SetItemDictionary(item, 0);
        }
        //�@�n���ꂽ�L�����N�^�[�̃A�C�e�����𑝂₷
        toChara.SetItemNum(item, toChara.GetItemNum(item) + 1);
        //�@�A�C�e����n���I�������A�C�e����n�������UseItemSelectCharacterPanel�̎q�v�f�̃{�^����S�폜
        for (int i = useItemSelectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemSelectCharacterPanel.transform.GetChild(i).gameObject);
        }
        //�@itemPanleButtonList����Y������A�C�e����T�������X�V����
        var itemButton = itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
        itemButton.transform.Find("Num").GetComponent<Text>().text = fromChara.GetItemNum(item).ToString();
        //�@�A�C�e����n�����|��\��
        useItemInformationPanel.GetComponentInChildren<Text>().text = fromChara.GetCharacterName() + "��" + item.GetKanjiName() + "��" + toChara.GetCharacterName() + "�ɓn���܂����B";

        //�@�A�C�e������0��������{�^���ƃL�����N�^�[�X�e�[�^�X����A�C�e�����폜
        if (fromChara.GetItemNum(item) == 0)
        {
            //�@�������Ă��镐���Z�������瑕�����O��
            if (fromChara.GetEquipArmor() == item)
            {
                fromChara.SetEquipArmor(null);
            }
            else if (fromChara.GetEquipWeapon() == item)
            {
                fromChara.SetEquipWeapon(null);
            }
            //�@�A�C�e����0�ɂȂ������C��ItemPanel�ɖ߂��ׁAUseItemPanel����UseItemSelectCharacterPanel���ł̃I�u�W�F�N�g�o�^���폜
            selectedGameObjectStack.Pop();
            selectedGameObjectStack.Pop();
            //�@itemPanelButtonList����A�C�e���p�l���{�^�����폜
            itemPanelButtonList.Remove(itemButton);
            //�@�A�C�e���p�l���{�^�����g�̍폜
            Destroy(itemButton);
            //�@�A�C�e����n�����L�����N�^�[���g��ItemDictionary���炻�̃A�C�e�����폜
            fromChara.GetItemDictionary().Remove(item);
            //�@ItemPanel�ɖ߂�ׁAUseItemPanel���ɍ�����{�^����S�폜
            for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(useItemPanel.transform.GetChild(i).gameObject);
            }
            //�@�A�C�e������0�ɂȂ����̂�CommandMode.NoItemPassed�ɕύX
            currentCommand = CommandMode.NoItemPassed;
            useItemInformationPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
        else
        {
            //�@�A�C�e�������c���Ă���ꍇ��UseItemPanel�ŃA�C�e�����ǂ����邩�̑I���ɖ߂�
            currentCommand = CommandMode.UseItemSelectCharacterPanelToUseItemPanel;
            useItemInformationPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
    }


    //�@�̂Ă�
    public void ThrowAwayItem(AllyStatus allyStatus, Item item)
    {
        //�@�A�C�e���������炷
        allyStatus.SetItemNum(item, allyStatus.GetItemNum(item) - 1);
        //�@�A�C�e������0�ɂȂ�����
        if (allyStatus.GetItemNum(item) == 0)
        {

            //�@�������Ă��镐����̂Ă�ꍇ�̏���
            if (item == allyStatus.GetEquipArmor())
            {
                var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
                equipArmorButton = null;
                allyStatus.SetEquipArmor(null);
            }
            else if (item == allyStatus.GetEquipWeapon())
            {
                var equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "";
                equipWeaponButton = null;
                allyStatus.SetEquipWeapon(null);
            }
        }
        //�@ItemPanel�̎q�v�f�̃A�C�e���p�l���{�^������Y������A�C�e���̃{�^����T���Đ����X�V����
        var itemButton = itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
        itemButton.transform.Find("Num").GetComponent<Text>().text = allyStatus.GetItemNum(item).ToString();
        useItemInformationPanel.GetComponentInChildren<Text>().text = item.GetKanjiName() + "���̂Ă܂����B";

        //�@�A�C�e������0��������{�^���ƃL�����N�^�[�X�e�[�^�X����A�C�e�����폜
        if (allyStatus.GetItemNum(item) == 0)
        {
            selectedGameObjectStack.Pop();
            itemPanelButtonList.Remove(itemButton);
            Destroy(itemButton);
            allyStatus.GetItemDictionary().Remove(item);

            currentCommand = CommandMode.NoItemPassed;
            useItemPanelCanvasGroup.interactable = false;
            useItemPanel.SetActive(false);
            useItemInformationPanel.transform.SetAsLastSibling();
            useItemInformationPanel.SetActive(true);
            //�@ItemPanel�ɖ߂��UseItemPanel�̎q�v�f�̃{�^����S�폜
            for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(useItemPanel.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            useItemPanelCanvasGroup.interactable = false;
            useItemInformationPanel.transform.SetAsLastSibling();
            useItemInformationPanel.SetActive(true);
            currentCommand = CommandMode.UseItemPanelToUseItemPanel;
        }

        Input.ResetInputAxes();

    }


    //�@��������
    public void Equip(AllyStatus allyStatus, Item item)
    {
        //�@�L�����N�^�[���ɑ����o���镐���Z���ǂ����𒲂ב�����؂�ւ���
        if (allyStatus.GetCharacterName() == "���j�e�B�����")
        {
            if (item.GetItemType() == Item.Type.ArmorAll
                || item.GetItemType() == Item.Type.ArmorUnityChan)
            {
                var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "E";
                //�@�������Ă���Z�������ItemPanel��Equip��E���O��
                if (allyStatus.GetEquipArmor() != null)
                {
                    equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == allyStatus.GetEquipArmor().GetKanjiName());
                    equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
                }
                allyStatus.SetEquipArmor(item);
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "�𑕔����܂����B";
            }
            else if (item.GetItemType() == Item.Type.WeaponAll
              || item.GetItemType() == Item.Type.WeaponUnityChan)
            {
                var equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "E";
                //�@�������Ă��镐�킪�����ItemPanel��Equip��E���O��
                if (allyStatus.GetEquipWeapon() != null)
                {
                    equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == allyStatus.GetEquipWeapon().GetKanjiName());
                    equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "";
                }
                allyStatus.SetEquipWeapon(item);
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "�𑕔����܂����B";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "�𑕔��o���܂���B";
            }
        }
        else if (allyStatus.GetCharacterName() == "�咹�䂤��")
        {
            if (item.GetItemType() == Item.Type.ArmorAll
                || item.GetItemType() == Item.Type.ArmorYuji)
            {
                var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "E";

                if (allyStatus.GetEquipArmor() != null)
                {
                    equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == allyStatus.GetEquipArmor().GetKanjiName());
                    equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
                }
                allyStatus.SetEquipArmor(item);
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "�𑕔����܂����B";
            }
            else if (item.GetItemType() == Item.Type.WeaponAll
              || item.GetItemType() == Item.Type.WeaponYuji)
            {
                var equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "E";

                if (allyStatus.GetEquipWeapon() != null)
                {
                    equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == allyStatus.GetEquipWeapon().GetKanjiName());
                    equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "";
                }
                allyStatus.SetEquipWeapon(item);
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "�𑕔����܂����B";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "�𑕔��o���܂���B";
            }
        }
        //�@������؂�ւ�����ItemPanel�ɖ߂�
        useItemPanelCanvasGroup.interactable = false;
        useItemPanel.SetActive(false);
        itemPanelCanvasGroup.interactable = true;
        //�@ItemPanel�ɖ߂�̂�UseItemPanel�̎q�v�f�ɍ�����{�^����S�폜
        for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemPanel.transform.GetChild(i).gameObject);
        }

        useItemInformationPanel.transform.SetAsLastSibling();
        useItemInformationPanel.SetActive(true);

        currentCommand = CommandMode.UseItemPanelToItemPanel;

        Input.ResetInputAxes();

    }


    //�@�������O��
    public void RemoveEquip(AllyStatus allyStatus, Item item)
    {
        //�@�A�C�e���̎�ނɉ����đ������O��
        if (item.GetItemType() == Item.Type.ArmorAll
            || item.GetItemType() == Item.Type.ArmorUnityChan
            || item.GetItemType() == Item.Type.ArmorYuji)
        {
            var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
            equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
            allyStatus.SetEquipArmor(null);
        }
        else if (item.GetItemType() == Item.Type.WeaponAll
          || item.GetItemType() == Item.Type.WeaponUnityChan
          || item.GetItemType() == Item.Type.WeaponYuji)
        {
            var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
            equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
            allyStatus.SetEquipWeapon(null);
        }
        //�@�������O�����|��\��
        useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "���O���܂����B";
        //�@�������O������ItemPanel�ɖ߂鏈��
        useItemPanelCanvasGroup.interactable = false;
        useItemPanel.SetActive(false);
        itemPanelCanvasGroup.interactable = true;
        //�@ItemPanel�ɖ߂�̂�UseItemPanel�̎q�v�f�̃{�^����S�폜
        for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemPanel.transform.GetChild(i).gameObject);
        }

        useItemInformationPanel.transform.SetAsLastSibling();
        useItemInformationPanel.SetActive(true);

        currentCommand = CommandMode.UseItemPanelToItemPanel;
        Input.ResetInputAxes();
    }
}