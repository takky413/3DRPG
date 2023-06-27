using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public enum CommandMode
    {
        SelectCommand,
        SelectDirectAttacker,
        SelectMagic,
        SelectMagicAttackTarget,
        SelectUseMagicOnAlliesTarget,
        SelectItem,
        SelectRecoveryItemTarget
    }

    //�@���ݐ퓬�ɎQ�����Ă���S�L�����N�^�[
    private List<GameObject> allCharacterInBattleList = new List<GameObject>();
    //�@���ݐ퓬�ɎQ�����Ă��閡���L�����N�^�[
    private List<GameObject> allyCharacterInBattleList = new List<GameObject>();
    //�@���ݐ퓬�ɎQ�����Ă���G�L�����N�^�[
    private List<GameObject> enemyCharacterInBattleList = new List<GameObject>();
    //�@���݂̍U���̏���
    private int currentAttackOrder;
    //�@���ݍU�������悤�Ƃ��Ă���l���I��
    private bool isChoosing;
    //�@�퓬���J�n���Ă��邩�ǂ���
    private bool isStartBattle;
    //�@�퓬�V�[���̍ŏ��̍U�����n�܂�܂ł̑ҋ@����
    [SerializeField]
    private float firstWaitingTime = 3f;
    //�@�퓬�V�[���̃L�����ڍs���̊Ԃ̎���
    [SerializeField]
    private float timeToNextCharacter = 1f;
    //�@�҂�����
    private float waitTime;
    //�@�퓬�V�[���̍ŏ��̍U�����n�܂�܂ł̌o�ߎ���
    private float elapsedTime;
    //�@�퓬���I���������ǂ���
    private bool battleIsOver;
    //�@���݂̃R�}���h
    private CommandMode currentCommand;

    //�@�퓬�f�[�^
    [SerializeField] private BattleData battleData = null;
    //�@�L�����N�^�[�̃x�[�X�ʒu
    [SerializeField] private Transform battleBasePosition;
    //�@���ݐ퓬�ɎQ�����Ă���L�����N�^�[
    private List<GameObject> allCharacterList = new List<GameObject>();

    //�@�����p�[�e�B�[�̃R�}���h�p�l��
    [SerializeField]
    private Transform commandPanel = null;
    //�@�퓬�p�L�����N�^�[�I���{�^���v���n�u
    [SerializeField]
    private GameObject battleCharacterButton = null;
    //�@SelectCharacterPanel
    [SerializeField]
    private Transform selectCharacterPanel = null;
    //�@���@��A�C�e���I���p�l��
    [SerializeField]
    private Transform magicOrItemPanel = null;
    //�@���@��A�C�e���I���p�l����Content
    private Transform magicOrItemPanelContent = null;
    //�@BattleItemPanelButton�v���n�u
    [SerializeField]
    private GameObject battleItemPanelButton = null;
    //�@BattleMagicPanelButton�v���n�u
    [SerializeField]
    private GameObject battleMagicPanelButton = null;
    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    //�@MagicOrItemPanel�łǂ̔ԍ��̃{�^�������ɃX�N���[�����邩
    [SerializeField]
    private int scrollDownButtonNum = 8;
    //�@MagicOrItemPanel�łǂ̔ԍ��̃{�^�����牺�ɃX�N���[�����邩
    [SerializeField]
    private int scrollUpButtonNum = 10;

    //�@ScrollManager
    private ScrollManager scrollManager;

    //�@���b�Z�[�W�p�l���v���n�u
    [SerializeField]
    private GameObject messagePanel;
    //�@BattleUI
    [SerializeField]
    private Transform battleUI;
    //�@���b�Z�[�W�p�l���C���X�^���X
    private GameObject messagePanelIns;

    //�@���ʕ\�������X�N���v�g
    [SerializeField]
    private BattleResult battleResult;

    //�J����
    [SerializeField]
    private GameObject subCamera;
    [SerializeField]
    private GameObject AllyCamera0;
    [SerializeField]
    private GameObject AllyCamera1;


    // Start is called before the first frame update
    void Start()
    {


        //�@�L�����N�^�[�C���X�^���X�̐e
        Transform charactersParent = new GameObject("Characters").transform;
        //�@�L�����N�^�[��z�u����Transform
        Transform characterTransform;
        //�@�������O�̓G�������ꍇ�̏����Ɏg�����X�g
        List<string> enemyNameList = new List<string>();

        GameObject ins;
        CharacterBattleScript characterBattleScript;
        string characterName;

        magicOrItemPanelContent = magicOrItemPanel.Find("Mask/Content");
        scrollManager = magicOrItemPanelContent.GetComponent<ScrollManager>();

        //�@�����p�[�e�B�[�̃v���n�u���C���X�^���X��
        for (int i = 0; i < battleData.GetAllyPartyStatus().GetAllyGameObject().Count; i++)
        {
            characterTransform = battleBasePosition.Find("AllyPos" + i).transform;
            ins = Instantiate<GameObject>(battleData.GetAllyPartyStatus().GetAllyGameObject()[i], characterTransform.position, characterTransform.rotation, charactersParent);
            characterBattleScript = ins.GetComponent<CharacterBattleScript>();
            ins.name = characterBattleScript.GetCharacterStatus().GetCharacterName();
            if (characterBattleScript.GetCharacterStatus().GetHp() > 0)
            {
                allyCharacterInBattleList.Add(ins);
                allCharacterList.Add(ins);
            }
        }
        if (battleData.GetEnemyPartyStatus() == null)
        {
            Debug.LogError("�G�p�[�e�B�[�f�[�^���ݒ肳��Ă��܂���B");
        }
        //�@�G�p�[�e�B�[�̃v���n�u���C���X�^���X��
        for (int i = 0; i < battleData.GetEnemyPartyStatus().GetEnemyGameObjectList().Count; i++)
        {
            characterTransform = battleBasePosition.Find("EnemyPos" + i).transform;
            ins = Instantiate<GameObject>(battleData.GetEnemyPartyStatus().GetEnemyGameObjectList()[i], characterTransform.position, characterTransform.rotation, charactersParent);
            //�@���ɓ����G�����݂����當����t������
            characterName = ins.GetComponent<CharacterBattleScript>().GetCharacterStatus().GetCharacterName();
            if (!enemyNameList.Contains(characterName))
            {
                ins.name = characterName + 'A';
            }
            else
            {
                ins.name = characterName + (char)('A' + enemyNameList.Count(enemyName => enemyName == characterName));
            }
            enemyNameList.Add(characterName);
            enemyCharacterInBattleList.Add(ins);
            allCharacterList.Add(ins);
        }
        //�@�L�����N�^�[���X�g���L�����N�^�[�̑f�����̍������ɕ��בւ�
        allCharacterList = allCharacterList.OrderByDescending(character => character.GetComponent<CharacterBattleScript>().GetCharacterStatus().GetAgility()).ToList<GameObject>();
        //�@���݂̐퓬
        allCharacterInBattleList = allCharacterList.ToList<GameObject>();
        //�@�m�F�̈ו��בւ������X�g��\��
        foreach (var character in allCharacterInBattleList)
        {
            Debug.Log(character.GetComponent<CharacterBattleScript>().GetCharacterStatus().GetCharacterName() + " : " + character.GetComponent<CharacterBattleScript>().GetCharacterStatus().GetAgility());
        }
        //�@�퓬�O�̑҂����Ԃ�ݒ�
        waitTime = firstWaitingTime;
        //�@�����_���l�̃V�[�h�̐ݒ�
        Random.InitState((int)Time.time);


        ShowMessage("�퓬�J�n");
    }


    // Update is called once per frame
    void Update()
    {

        //�@�퓬���I�����Ă����炱��ȍ~�������Ȃ�
        if (battleIsOver)
        {
            return;
        }

            //�@�I���������ꂽ���i�}�E�X��UI�O���N���b�N�����j�͌��݂̃��[�h�ɂ���Ė������I��������
            if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (currentCommand == CommandMode.SelectCommand)
            {
                EventSystem.current.SetSelectedGameObject(commandPanel.GetChild(1).gameObject);
            }
            else if (currentCommand == CommandMode.SelectDirectAttacker)
            {
                EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.SelectMagic)
            {
                scrollManager.Reset();
                EventSystem.current.SetSelectedGameObject(magicOrItemPanelContent.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.SelectMagicAttackTarget)
            {
                EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.SelectUseMagicOnAlliesTarget)
            {
                EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.SelectItem)
            {
                scrollManager.Reset();
                EventSystem.current.SetSelectedGameObject(magicOrItemPanelContent.GetChild(0).gameObject);
            }
            else if (currentCommand == CommandMode.SelectRecoveryItemTarget)
            {
                EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
            }
        }

        //�@�퓬�J�n
        if (isStartBattle)
        {
            //�@���݂̃L�����N�^�[�̍U�����I����Ă���
            if (!isChoosing)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime < waitTime)
                {
                    return;
                }

                //���b�Z�[�W�p�l�����\������Ă���Ԃ͉������Ȃ�
                if (messagePanelIns != null)
                {
                    return;
                }

                elapsedTime = 0f;
                isChoosing = true;

                //�@�L�����N�^�[�̍U���̑I���Ɉڂ�
                MakeAttackChoise(allCharacterInBattleList[currentAttackOrder]); //���ꂪ����ƁA�C���f�b�N�X���I�[�o�[���Ă���Ƃ����G���[���o��
                //�@���̃L�����N�^�[�̃^�[���ɂ���
                currentAttackOrder++;
                //�@�S���U�����I�������ŏ�����
                if (currentAttackOrder >= allCharacterInBattleList.Count)
                {
                    currentAttackOrder = 0;
                }
            }
            else
            {
                //�@�L�����Z���{�^�������������̏���
                if (Input.GetButtonDown("Cancel"))
                {
                    if (currentCommand == CommandMode.SelectDirectAttacker)
                    {
                        // �L�����N�^�[�I���{�^��������ΑS�č폜
                        for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                        }

                        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = false;
                        selectCharacterPanel.gameObject.SetActive(false);
                        commandPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        currentCommand = CommandMode.SelectCommand;
                    }
                    else if (currentCommand == CommandMode.SelectMagic)
                    {
                        // magicOrItemPanel�Ƀ{�^��������ΑS�č폜
                        for (int i = magicOrItemPanelContent.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(magicOrItemPanelContent.transform.GetChild(i).gameObject);
                        }
                        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
                        magicOrItemPanel.gameObject.SetActive(false);
                        commandPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        currentCommand = CommandMode.SelectCommand;
                    }
                    else if (currentCommand == CommandMode.SelectMagicAttackTarget)
                    {
                        // selectCharacterPanel�Ƀ{�^��������ΑS�č폜
                        for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                        }
                        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = false;
                        selectCharacterPanel.gameObject.SetActive(false);
                        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        currentCommand = CommandMode.SelectMagic;
                    }
                    else if (currentCommand == CommandMode.SelectUseMagicOnAlliesTarget)
                    {
                        // selectCharacterPanel�Ƀ{�^��������ΑS�č폜
                        for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                        }
                        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = false;
                        selectCharacterPanel.gameObject.SetActive(false);
                        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        currentCommand = CommandMode.SelectMagic;
                    }
                    else if (currentCommand == CommandMode.SelectItem)
                    {
                        // magicOrItemPanel�Ƀ{�^��������ΑS�č폜
                        for (int i = magicOrItemPanelContent.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(magicOrItemPanelContent.transform.GetChild(i).gameObject);
                        }
                        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
                        magicOrItemPanel.gameObject.SetActive(false);
                        commandPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        currentCommand = CommandMode.SelectCommand;
                    }
                    else if (currentCommand == CommandMode.SelectRecoveryItemTarget)
                    {
                        // selectCharacterPanel�Ƀ{�^��������ΑS�č폜
                        for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                        }
                        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = false;
                        selectCharacterPanel.gameObject.SetActive(false);
                        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        currentCommand = CommandMode.SelectItem;
                    }
                }
            }
        }
        else
        {
            Debug.Log("�o�ߎ��ԁF " + elapsedTime);
            //�@�퓬�O�̑ҋ@
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
            {
                //�@2��ڈȍ~�̓L�����Ԃ̎��Ԃ�ݒ�
                waitTime = timeToNextCharacter;
                //�@�ŏ��̃L�����N�^�[�̑҂����Ԃ�0�ɂ���ׂɂ��炩���ߏ������N���A�����Ă���
                elapsedTime = timeToNextCharacter;
                isStartBattle = true;
            }
        }
    }

    //�@�L�����N�^�[�̍U���̑I������
    public void MakeAttackChoise(GameObject character)
    {
        CharacterStatus characterStatus = character.GetComponent<CharacterBattleScript>().GetCharacterStatus();
        //�@EnemyStatus�ɃL���X�g�o����ꍇ�͓G�̍U������
        if (characterStatus as EnemyStatus != null)
        {
            Debug.Log(character.gameObject.name + "�̍U��");
            EnemyAttack(character);
        }
        else
        {
            Debug.Log(characterStatus.GetCharacterName() + "�̍U��");
            AllyAttack(character);
        }
    }

    //�@�����̍U������
    public void AllyAttack(GameObject character)
    {

        currentCommand = CommandMode.SelectCommand;

        character.transform.Find("Marker/Image2").gameObject.SetActive(true);

        // �L�����N�^�[�I���{�^��������ΑS�č폜
        for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
        }

        // ���@��A�C�e���p�l���̎q�v�f��Content�Ƀ{�^��������ΑS�č폜
        for (int i = magicOrItemPanelContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(magicOrItemPanelContent.transform.GetChild(i).gameObject);
        }

        commandPanel.GetComponent<CanvasGroup>().interactable = true;
        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = false;
        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = false;

        //�@�L�����N�^�[���K�[�h��Ԃł���΃K�[�h������
        if (character.GetComponent<Animator>().GetBool("Guard"))
        {
            character.GetComponent<CharacterBattleScript>().UnlockGuard();
        }

        //�@�L�����N�^�[�̖��O��\��
        commandPanel.Find("CharacterName/Text").GetComponent<Text>().text = character.gameObject.name;

        var characterSkill = character.GetComponent<CharacterBattleScript>().GetCharacterStatus().GetSkillList();
        //�@�����Ă���X�L���ɉ����ăR�}���h�{�^���̕\��
        if (characterSkill.Exists(skill => skill.GetSkillType() == Skill.Type.DirectAttack))
        {
            var directAttackButtonObj = commandPanel.Find("DirectAttack").gameObject;
            var directAttackButton = directAttackButtonObj.GetComponent<Button>();
            directAttackButton.onClick.RemoveAllListeners();
            directAttackButtonObj.GetComponent<Button>().onClick.AddListener(() => SelectDirectAttacker(character));
            directAttackButtonObj.SetActive(true);
        }
        else
        {
            commandPanel.Find("DirectAttack").gameObject.SetActive(false);
        }
        if (characterSkill.Exists(skill => skill.GetSkillType() == Skill.Type.Guard))
        {
            var guardButtonObj = commandPanel.Find("Guard").gameObject;
            var guardButton = guardButtonObj.GetComponent<Button>();
            guardButton.onClick.RemoveAllListeners();
            guardButton.onClick.AddListener(() => Guard(character));
            guardButtonObj.SetActive(true);
        }
        else
        {
            commandPanel.Find("Guard").gameObject.SetActive(false);
        }
        if (characterSkill.Exists(skill => skill.GetSkillType() == Skill.Type.Item))
        {
            var itemButtonObj = commandPanel.Find("Item").gameObject;
            var itemButton = itemButtonObj.GetComponent<Button>();
            itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(() => SelectItem(character));
            commandPanel.Find("Item").gameObject.SetActive(true);
        }
        else
        {
            commandPanel.Find("Item").gameObject.SetActive(false);
        }
        if (characterSkill.Exists(skill => skill.GetSkillType() == Skill.Type.MagicAttack)
            || characterSkill.Find(skill => skill.GetSkillType() == Skill.Type.IncreaseAttackPowerMagic)
            || characterSkill.Find(skill => skill.GetSkillType() == Skill.Type.IncreaseDefencePowerMagic)
            || characterSkill.Find(skill => skill.GetSkillType() == Skill.Type.NumbnessRecoveryMagic)
            || characterSkill.Find(skill => skill.GetSkillType() == Skill.Type.PoisonnouRecoveryMagic)
            || characterSkill.Find(skill => skill.GetSkillType() == Skill.Type.RecoveryMagic))
        {

            var magicButtonObj = commandPanel.Find("Magic").gameObject;
            var magicButton = magicButtonObj.GetComponent<Button>();
            magicButton.onClick.RemoveAllListeners();
            magicButton.onClick.AddListener(() => SelectMagic(character));

            magicButtonObj.SetActive(true);
        }
        else
        {
            commandPanel.Find("Magic").gameObject.SetActive(false);
        }
        if (characterSkill.Exists(skill => skill.GetSkillType() == Skill.Type.GetAway))
        {
            var getAwayButtonObj = commandPanel.Find("GetAway").gameObject;
            var getAwayButton = getAwayButtonObj.GetComponent<Button>();
            getAwayButton.onClick.RemoveAllListeners();
            getAwayButton.onClick.AddListener(() => GetAway(character));
            getAwayButtonObj.SetActive(true);
        }
        else
        {
            commandPanel.Find("GetAway").gameObject.SetActive(false);
        }

        EventSystem.current.SetSelectedGameObject(commandPanel.transform.GetChild(1).gameObject);
        commandPanel.gameObject.SetActive(true);
    }

    //�@�G�̍U������
    public void EnemyAttack(GameObject character)
    {
        CharacterBattleScript characterBattleScript = character.GetComponent<CharacterBattleScript>();
        CharacterStatus characterStatus = characterBattleScript.GetCharacterStatus();


        if (characterStatus.GetSkillList().Count <= 0)
        {
            return;
        }
        //�@�G���K�[�h��Ԃł���΃K�[�h������
        if (character.GetComponent<Animator>().GetBool("Guard"))
        {
            character.GetComponent<CharacterBattleScript>().UnlockGuard();
        }

        //�@�G�̍s���A���S���Y��
        int randomValue = (int)(Random.value * characterStatus.GetSkillList().Count);
        var nowSkill = characterStatus.GetSkillList()[randomValue];

        //�@�e�X�g�p�i����̃X�L���Ŋm�F�j
        //nowSkill = characterStatus.GetSkillList()[0];

        if (nowSkill.GetSkillType() == Skill.Type.DirectAttack)
        {
            var targetNum = (int)(Random.value * allyCharacterInBattleList.Count);

            //�����ɃJ�����̐ݒ�������B
            //subCamera.SetActive(true);
            //ActivateCamera();



            //

            //�@�U�������CharacterBattleScript
            characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, allyCharacterInBattleList[targetNum], nowSkill);
            Debug.Log(character.name + "��" + nowSkill.GetKanjiName() + "���s����");
        }
        else if (nowSkill.GetSkillType() == Skill.Type.MagicAttack)
        {
            var targetNum = (int)(Random.value * allyCharacterInBattleList.Count);
            if (characterBattleScript.GetMp() >= ((Magic)nowSkill).GetAmountToUseMagicPoints())
            {
                //�@�U�������CharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.MagicAttack, allyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "��" + nowSkill.GetKanjiName() + "���s����");
            }
            else
            {
                Debug.Log("MP������Ȃ��I");
                ShowMessage("MP������Ȃ�!");
                //�@MP������Ȃ��ꍇ�͒��ڍU�����s��
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, allyCharacterInBattleList[targetNum], characterStatus.GetSkillList().Find(skill => skill.GetSkillType() == Skill.Type.DirectAttack));
                Debug.Log(character.name + "�͍U�����s����");
            }
        }
        else if (nowSkill.GetSkillType() == Skill.Type.RecoveryMagic)
        {
            if (characterBattleScript.GetMp() >= ((Magic)nowSkill).GetAmountToUseMagicPoints())
            {
                var targetNum = (int)(Random.value * enemyCharacterInBattleList.Count);
                //�@�񕜑����CharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.Healing, enemyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "��" + nowSkill.GetKanjiName() + "���s����");
            }
            else
            {
                Debug.Log("MP������Ȃ��I");
                ShowMessage("MP������Ȃ�!");
                var targetNum = (int)(Random.value * allyCharacterInBattleList.Count);
                //�@MP������Ȃ��ꍇ�͒��ڍU�����s��
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, allyCharacterInBattleList[targetNum], characterStatus.GetSkillList().Find(skill => skill.GetSkillType() == Skill.Type.DirectAttack));
                Debug.Log(character.name + "�͍U�����s����");
            }
        }
        else if (nowSkill.GetSkillType() == Skill.Type.Guard)
        {
            characterBattleScript.Guard();
            // Guard�A�j����bool�Ȃ̂ŃA�j���[�V�����J�ڂ������炷���Ɏ��̃L�����N�^�[�Ɉڍs������
            ChangeNextChara();
            Debug.Log(character.name + "��" + nowSkill.GetKanjiName() + "���s����");
        }
    }

    //�@���̃L�����N�^�[�Ɉڍs
    public void ChangeNextChara()
    {
        isChoosing = false;
    }

    public void DeleteAllCharacterInBattleList(GameObject deleteObj)
    {
        var deleteObjNum = allCharacterInBattleList.IndexOf(deleteObj);
        allCharacterInBattleList.Remove(deleteObj);
        if (deleteObjNum < currentAttackOrder)
        {
            currentAttackOrder--;
        }
        //�@�S���U�����I�������ŏ�����
        if (currentAttackOrder >= allCharacterInBattleList.Count)
        {
            currentAttackOrder = 0;
        }
    }

    public void DeleteAllyCharacterInBattleList(GameObject deleteObj)
    {
        allyCharacterInBattleList.Remove(deleteObj);
        if (allyCharacterInBattleList.Count == 0)
        {
            //Debug.Log("�������S��");
            ShowMessage("�������S��");
            battleIsOver = true;
            CharacterBattleScript characterBattleScript;
            foreach (var character in allCharacterList)
            {
                //�@�����L�����N�^�[�̐퓬�ő�������HP��MP��ʏ�̃X�e�[�^�X�ɔ��f������
                characterBattleScript = character.GetComponent<CharacterBattleScript>();
                if (characterBattleScript.GetCharacterStatus() as AllyStatus != null)
                {
                    characterBattleScript.GetCharacterStatus().SetHp(characterBattleScript.GetHp());
                    characterBattleScript.GetCharacterStatus().SetMp(characterBattleScript.GetMp());
                    characterBattleScript.GetCharacterStatus().SetNumbness(characterBattleScript.IsNumbness());
                    characterBattleScript.GetCharacterStatus().SetPoisonState(characterBattleScript.IsPoison());
                }
            }
            //�@�s�펞�̌��ʕ\��
            battleResult.InitialProcessingOfDefeatResult();
        }
    }

    public void DeleteEnemyCharacterInBattleList(GameObject deleteObj)
    {
        enemyCharacterInBattleList.Remove(deleteObj);
        if (enemyCharacterInBattleList.Count == 0)
        {
            //Debug.Log("�G���S��");
            ShowMessage("�G���S��");
            battleIsOver = true;
            CharacterBattleScript characterBattleScript;
            foreach (var character in allCharacterList)
            {
                //�@�����L�����N�^�[�̐퓬�ő�������HP��MP��ʏ�̃X�e�[�^�X�ɔ��f������
                characterBattleScript = character.GetComponent<CharacterBattleScript>();
                if (characterBattleScript.GetCharacterStatus() as AllyStatus != null)
                {
                    characterBattleScript.GetCharacterStatus().SetHp(characterBattleScript.GetHp());
                    characterBattleScript.GetCharacterStatus().SetMp(characterBattleScript.GetMp());
                    characterBattleScript.GetCharacterStatus().SetNumbness(characterBattleScript.IsNumbness());
                    characterBattleScript.GetCharacterStatus().SetPoisonState(characterBattleScript.IsPoison());
                }
            }
            //�@�������̌��ʕ\��
            battleResult.InitialProcessingOfVictoryResult(allCharacterList, allyCharacterInBattleList);
        }
    }


    //�@�L�����N�^�[�I��
    public void SelectDirectAttacker(GameObject attackCharacter)
    {
        currentCommand = CommandMode.SelectDirectAttacker;
        commandPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject battleCharacterButtonIns;

        foreach (var enemy in enemyCharacterInBattleList)
        {
            battleCharacterButtonIns = Instantiate<GameObject>(battleCharacterButton, selectCharacterPanel);
            battleCharacterButtonIns.transform.Find("Text").GetComponent<Text>().text = enemy.gameObject.name;
            battleCharacterButtonIns.GetComponent<Button>().onClick.AddListener(() => DirectAttack(attackCharacter, enemy));
        }

        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = true;
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
        selectCharacterPanel.gameObject.SetActive(true);
    }


    //�@���ڍU��
    public void DirectAttack(GameObject attackCharacter, GameObject attackTarget)
    {
        //�@�U������L������DirectAttack�X�L�����擾����
        var characterSkill = attackCharacter.GetComponent<CharacterBattleScript>().GetCharacterStatus().GetSkillList();
        Skill directAtatck = characterSkill.Find(skill => skill.GetSkillType() == Skill.Type.DirectAttack);
        attackCharacter.GetComponent<CharacterBattleScript>().ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, attackTarget, directAtatck);
        commandPanel.gameObject.SetActive(false);
        selectCharacterPanel.gameObject.SetActive(false);
    }

    //�@�h��
    public void Guard(GameObject guardCharacter)
    {
        guardCharacter.GetComponent<CharacterBattleScript>().Guard();
        commandPanel.gameObject.SetActive(false);
        ChangeNextChara();
    }


    //�@�g�p���閂�@�̑I��
    public void SelectMagic(GameObject character)
    {
        currentCommand = CommandMode.SelectMagic;
        commandPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject battleMagicPanelButtonIns;
        var skillList = character.GetComponent<CharacterBattleScript>().GetCharacterStatus().GetSkillList();

        //�@MagicOrItemPanel�̃X�N���[���l�̏�����
        scrollManager.Reset();
        int battleMagicPanelButtonNum = 0;

        foreach (var skill in skillList)
        {
            if (skill.GetSkillType() == Skill.Type.MagicAttack
                || skill.GetSkillType() == Skill.Type.RecoveryMagic
                || skill.GetSkillType() == Skill.Type.IncreaseAttackPowerMagic
                || skill.GetSkillType() == Skill.Type.IncreaseDefencePowerMagic
                || skill.GetSkillType() == Skill.Type.NumbnessRecoveryMagic
                || skill.GetSkillType() == Skill.Type.PoisonnouRecoveryMagic
                )
            {
                battleMagicPanelButtonIns = Instantiate<GameObject>(battleMagicPanelButton, magicOrItemPanelContent);
                battleMagicPanelButtonIns.transform.Find("MagicName").GetComponent<Text>().text = skill.GetKanjiName();
                battleMagicPanelButtonIns.transform.Find("AmountToUseMagicPoints").GetComponent<Text>().text = ((Magic)skill).GetAmountToUseMagicPoints().ToString();

                //�@�w�肵���ԍ��̃A�C�e���p�l���{�^���ɃA�C�e���X�N���[���p�X�N���v�g�����t����
                if (battleMagicPanelButtonNum != 0
                    && (battleMagicPanelButtonNum % scrollDownButtonNum == 0
                    || battleMagicPanelButtonNum % (scrollDownButtonNum + 1) == 0)
                    )
                {
                    //�@�A�C�e���X�N���[���X�N���v�g�̎��t���Đݒ�l�̃Z�b�g
                    battleMagicPanelButtonIns.AddComponent<ScrollDownScript>();
                }
                else if (battleMagicPanelButtonNum != 0
                  && (battleMagicPanelButtonNum % scrollUpButtonNum == 0
                  || battleMagicPanelButtonNum % (scrollUpButtonNum + 1) == 0)
                  )
                {
                    battleMagicPanelButtonIns.AddComponent<ScrollUpScript>();
                }

                //�@MP������Ȃ����̓{�^���������Ă������������@�̖��O���Â�����
                if (character.GetComponent<CharacterBattleScript>().GetMp() < ((Magic)skill).GetAmountToUseMagicPoints())
                {
                    battleMagicPanelButtonIns.transform.Find("MagicName").GetComponent<Text>().color = new Color(0.4f, 0.4f, 0.4f);
                }
                else
                {
                    battleMagicPanelButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectUseMagicTarget(character, skill));
                }
                //�@�{�^���ԍ��𑫂�
                battleMagicPanelButtonNum++;

                if (battleMagicPanelButtonNum == scrollUpButtonNum + 2)
                {
                    battleMagicPanelButtonNum = 2;
                }
            }
        }

        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
        EventSystem.current.SetSelectedGameObject(magicOrItemPanelContent.GetChild(0).gameObject);
        magicOrItemPanel.gameObject.SetActive(true);

    }


    //�@���@���g������̑I��
    public void SelectUseMagicTarget(GameObject user, Skill skill)
    {
        currentCommand = CommandMode.SelectUseMagicOnAlliesTarget;
        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject battleCharacterButtonIns;

        if (skill.GetSkillType() == Skill.Type.MagicAttack)
        {
            foreach (var enemy in enemyCharacterInBattleList)
            {
                battleCharacterButtonIns = Instantiate<GameObject>(battleCharacterButton, selectCharacterPanel);
                battleCharacterButtonIns.transform.Find("Text").GetComponent<Text>().text = enemy.gameObject.name;
                battleCharacterButtonIns.GetComponent<Button>().onClick.AddListener(() => UseMagic(user, enemy, skill));
            }
        }
        else
        {
            foreach (var allyCharacter in allyCharacterInBattleList)
            {
                battleCharacterButtonIns = Instantiate<GameObject>(battleCharacterButton, selectCharacterPanel);
                battleCharacterButtonIns.transform.Find("Text").GetComponent<Text>().text = allyCharacter.gameObject.name;
                battleCharacterButtonIns.GetComponent<Button>().onClick.AddListener(() => UseMagic(user, allyCharacter, skill));
            }
        }

        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = true;
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
        selectCharacterPanel.gameObject.SetActive(true);
    }



    //�@���@���g��
    public void UseMagic(GameObject user, GameObject targetCharacter, Skill skill)
    {
        CharacterBattleScript.BattleState battleState = CharacterBattleScript.BattleState.Idle;
        //�@���@���g�������CharacterBattleScript���擾���Ă���
        var targetCharacterBattleScript = targetCharacter.GetComponent<CharacterBattleScript>();

        //�@�g�����@�̎�ނ̐ݒ�ƑΏۂɎg���K�v���Ȃ��ꍇ�̏���
        if (skill.GetSkillType() == Skill.Type.MagicAttack)
        {
            battleState = CharacterBattleScript.BattleState.MagicAttack;
        }
        else if (skill.GetSkillType() == Skill.Type.RecoveryMagic)
        {
            if (targetCharacterBattleScript.GetHp() == targetCharacterBattleScript.GetCharacterStatus().GetMaxHp())
            {
                Debug.Log(targetCharacter.name + "�͑S���ł��B");
                ShowMessage(targetCharacter.name + "�͑S���ł��B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.Healing;
        }
        else if (skill.GetSkillType() == Skill.Type.IncreaseAttackPowerMagic)
        {
            if (targetCharacterBattleScript.IsIncreasePower())
            {
                Debug.Log("���ɍU���͂��グ�Ă��܂��B");
                ShowMessage("���ɍU���͂��グ�Ă��܂��B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.IncreaseAttackPowerMagic;
        }
        else if (skill.GetSkillType() == Skill.Type.IncreaseDefencePowerMagic)
        {
            if (targetCharacterBattleScript.IsIncreaseStrikingStrength())
            {
                Debug.Log("���ɖh��͂��グ�Ă��܂��B");
                ShowMessage("���ɖh��͂��グ�Ă��܂��B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.IncreaseDefencePowerMagic;
        }
        else if (skill.GetSkillType() == Skill.Type.NumbnessRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsNumbness())
            {
                Debug.Log(targetCharacter.name + "��Ⴢ��Ԃł͂���܂���B");
                ShowMessage(targetCharacter.name + "��Ⴢ��Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.NumbnessRecoveryMagic;
        }
        else if (skill.GetSkillType() == Skill.Type.PoisonnouRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsPoison())
            {
                Debug.Log(targetCharacter.name + "�͓ŏ�Ԃł͂���܂���B");
                ShowMessage(targetCharacter.name + "�͓ŏ�Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.PoisonnouRecoveryMagic;
        }
        user.GetComponent<CharacterBattleScript>().ChooseAttackOptions(battleState, targetCharacter, skill);
        commandPanel.gameObject.SetActive(false);
        magicOrItemPanel.gameObject.SetActive(false);
        selectCharacterPanel.gameObject.SetActive(false);
    }



    //�@�g�p����A�C�e���̑I��
    public void SelectItem(GameObject character)
    {

        var itemDictionary = ((AllyStatus)character.GetComponent<CharacterBattleScript>().GetCharacterStatus()).GetItemDictionary();

        //�@MagicOrItemPanel�̃X�N���[���l�̏�����
        scrollManager.Reset();
        var battleItemPanelButtonNum = 0;

        GameObject battleItemPanelButtonIns;

        foreach (var item in itemDictionary.Keys)
        {
            if (item.GetItemType() == Item.Type.HPRecovery
                || item.GetItemType() == Item.Type.MPRecovery
                || item.GetItemType() == Item.Type.NumbnessRecovery
                || item.GetItemType() == Item.Type.PoisonRecovery
                )
            {
                battleItemPanelButtonIns = Instantiate<GameObject>(battleItemPanelButton, magicOrItemPanelContent);
                battleItemPanelButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetKanjiName();
                battleItemPanelButtonIns.transform.Find("Num").GetComponent<Text>().text = ((AllyStatus)character.GetComponent<CharacterBattleScript>().GetCharacterStatus()).GetItemNum(item).ToString();
                battleItemPanelButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItemTarget(character, item));

                //�@�w�肵���ԍ��̃A�C�e���p�l���{�^���ɃA�C�e���X�N���[���p�X�N���v�g�����t����
                if (battleItemPanelButtonNum != 0
                    && (battleItemPanelButtonNum % scrollDownButtonNum == 0
                    || battleItemPanelButtonNum % (scrollDownButtonNum + 1) == 0)
                    )
                {
                    //�@�A�C�e���X�N���[���X�N���v�g�̎��t���Đݒ�l�̃Z�b�g
                    battleItemPanelButtonIns.AddComponent<ScrollDownScript>();
                }
                else if (battleItemPanelButtonNum != 0
                  && (battleItemPanelButtonNum % scrollUpButtonNum == 0
                  || battleItemPanelButtonNum % (scrollUpButtonNum + 1) == 0)
                  )
                {
                    battleItemPanelButtonIns.AddComponent<ScrollUpScript>();
                }
                //�@�{�^���ԍ��𑫂�
                battleItemPanelButtonNum++;

                if (battleItemPanelButtonNum == scrollUpButtonNum + 2)
                {
                    battleItemPanelButtonNum = 2;
                }
            }
        }

        if (magicOrItemPanelContent.childCount > 0)
        {
            currentCommand = CommandMode.SelectItem;
            commandPanel.GetComponent<CanvasGroup>().interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            magicOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
            EventSystem.current.SetSelectedGameObject(magicOrItemPanelContent.GetChild(0).gameObject);
            magicOrItemPanel.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("�g����A�C�e��������܂���B");
            ShowMessage("�g����A�C�e��������܂���B");
        }
    }


    //�@�A�C�e�����g�p���鑊���I��
    public void SelectItemTarget(GameObject user, Item item)
    {
        currentCommand = CommandMode.SelectRecoveryItemTarget;
        magicOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject battleCharacterButtonIns;

        foreach (var allyCharacter in allyCharacterInBattleList)
        {
            battleCharacterButtonIns = Instantiate<GameObject>(battleCharacterButton, selectCharacterPanel);
            battleCharacterButtonIns.transform.Find("Text").GetComponent<Text>().text = allyCharacter.gameObject.name;
            battleCharacterButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItem(user, allyCharacter, item));
        }

        selectCharacterPanel.GetComponent<CanvasGroup>().interactable = true;
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
        selectCharacterPanel.gameObject.SetActive(true);
    }


    //�@�A�C�e���g�p
    public void UseItem(GameObject user, GameObject targetCharacter, Item item)
    {
        var userCharacterBattleScript = user.GetComponent<CharacterBattleScript>();
        var targetCharacterBattleScript = targetCharacter.GetComponent<CharacterBattleScript>();
        var skill = userCharacterBattleScript.GetCharacterStatus().GetSkillList().Find(skills => skills.GetSkillType() == Skill.Type.Item);

        CharacterBattleScript.BattleState battleState = CharacterBattleScript.BattleState.Idle;

        if (item.GetItemType() == Item.Type.HPRecovery)
        {
            if (targetCharacterBattleScript.GetHp() == targetCharacterBattleScript.GetCharacterStatus().GetMaxHp())
            {
                Debug.Log(targetCharacter.name + "�͑S���ł��B");
                ShowMessage(targetCharacter.name + "�͑S���ł��B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseHPRecoveryItem;
        }
        else if (item.GetItemType() == Item.Type.MPRecovery)
        {
            if (targetCharacterBattleScript.GetMp() == targetCharacterBattleScript.GetCharacterStatus().GetMaxMp())
            {
                Debug.Log(targetCharacter.name + "��MP�񕜂�����K�v������܂���B");
                ShowMessage(targetCharacter.name + "��MP�񕜂�����K�v������܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseMPRecoveryItem;
        }
        else if (item.GetItemType() == Item.Type.NumbnessRecovery)
        {
            if (!targetCharacterBattleScript.IsNumbness())
            {
                Debug.Log(targetCharacter.name + "��Ⴢ��Ԃł͂���܂���B");
                ShowMessage(targetCharacter.name + "��Ⴢ��Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseNumbnessRecoveryItem;
        }
        else if (item.GetItemType() == Item.Type.PoisonRecovery)
        {
            if (!targetCharacterBattleScript.IsPoison())
            {
                Debug.Log(targetCharacter.name + "�͓ŏ�Ԃł͂���܂���B");
                ShowMessage(targetCharacter.name + "�͓ŏ�Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UsePoisonRecoveryItem;
        }
        userCharacterBattleScript.ChooseAttackOptions(battleState, targetCharacter, skill, item);
        commandPanel.gameObject.SetActive(false);
        magicOrItemPanel.gameObject.SetActive(false);
        selectCharacterPanel.gameObject.SetActive(false);
    }


    //�@������
    public void GetAway(GameObject character)
    {

        character.transform.Find("Marker/Image2").gameObject.SetActive(false);

        var randomValue = Random.value;
        if (0f <= randomValue && randomValue <= 0.8f)
        {
            Debug.Log("������̂ɐ��������B");
            ShowMessage("������̂ɐ��������B");
            battleIsOver = true;
            commandPanel.gameObject.SetActive(false);
            //�@�퓬�I��
            battleResult.InitialProcessingOfRanAwayResult();
        }
        else
        {
            Debug.Log("������̂Ɏ��s�����B");
            ShowMessage("������̂Ɏ��s�����B");
            commandPanel.gameObject.SetActive(false);
            ChangeNextChara();
        }
    }


    //�@���b�Z�[�W�\��
    public void ShowMessage(string message)
    {
        if (messagePanelIns != null)
        {
            Destroy(messagePanelIns);
        }
        messagePanelIns = Instantiate<GameObject>(messagePanel, battleUI);
        messagePanelIns.transform.Find("Text").GetComponent<Text>().text = message;
    }


    //�J�����̐ݒ�
    public void ActivateCamera(int cameraID)
    {
        //subCamera.SetActive(true);
        if (cameraID == 0)
        {
            subCamera.SetActive(false);
            AllyCamera0.SetActive(true);
            AllyCamera1.SetActive(false);
        }
        else if (cameraID == 1)
        {
            subCamera.SetActive(false);
            AllyCamera0.SetActive(false);
            AllyCamera1.SetActive(true);
        }
        else if (cameraID == -1)
        {
            subCamera.SetActive(true);
            AllyCamera0.SetActive(false);
            AllyCamera1.SetActive(false);
        }
    }

    public void InActivateCamera(int cameraID)
    {
        //subCamera.SetActive(false);
        if (cameraID == 0)
        {
            AllyCamera0.SetActive(false);
        }
        else if (cameraID == 1)
        {
            AllyCamera1.SetActive(false);
        }
    }
}