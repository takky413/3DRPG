using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleResult : MonoBehaviour
{

    //�@���ʂ�\�����Ă��烏�[���h�}�b�v�ɖ߂��悤�ɂȂ�܂ł̎���
    [SerializeField]
    private float timeToDisplay = 3f;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private Text resultText;
    [SerializeField]
    private PartyStatus partyStatus;
    //�@�퓬���ʕ\�������Ă��邩�ǂ���
    private bool isDisplayResult;
    //�@���ʂ�\�����퓬���甲���o���邩�ǂ���
    private bool isFinishResult;
    //�@�퓬�ɏ����������ǂ���
    private bool won;
    //�@���������ǂ���
    private bool ranAway;
    //�@�퓬���ʃe�L�X�g�̃X�N���[���l
    [SerializeField]
    private float scrollValue = 50f;
    //�@MusicManager
    [SerializeField]
    private MusicManager musicManager;


    void Update()
    {
        //�@���ʕ\���O�͉������Ȃ�
        if (!isDisplayResult)
        {
            return;
        }

        //�@���ʕ\����͌��ʕ\���e�L�X�g���X�N���[�����Č����悤�ɂ���
        if (Input.GetAxis("Vertical") != 0f)
        {
            resultText.transform.localPosition += new Vector3(0f, -Input.GetAxis("Vertical") * scrollValue, 0f);
        }
        //�@�퓬�𔲂��o���܂ł̑ҋ@���Ԃ��z���Ă��Ȃ�
        if (!isFinishResult)
        {
            return;
        }
        //�@Submit��Action��Fire1�{�^�����������烏�[���h�}�b�v�ɖ߂�
        //if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Action") || Input.GetButtonDown("Fire1"))
        if (Input.GetButtonDown("Submit")) //���ݒ�ŃG���[���o�邽�߁A"Submit" = �G���^�[�L�[�̐g�ɂ��Ă���
        {
            if (won || ranAway)
            {
                GameObject.Find("SceneManager").GetComponent<LoadSceneManager>().GoToNextScene(SceneMovementData.SceneType.BattleToWorldMap);
            }
            else
            {
                GameObject.Find("SceneManager").GetComponent<LoadSceneManager>().GoToNextScene(SceneMovementData.SceneType.FirstVillage);
            }
        }
    }


    //�@�������̏�������
    public void InitialProcessingOfVictoryResult(List<GameObject> allCharacterList, List<GameObject> allyCharacterInBattleList)
    {
        StartCoroutine(DisplayVictoryResult(allCharacterList, allyCharacterInBattleList));
    }


    //�@�������̌���
    public IEnumerator DisplayVictoryResult(List<GameObject> allCharacterList, List<GameObject> allyCharacterInBattleList)
    {
        yield return new WaitForSeconds(timeToDisplay);
        won = true;
        resultPanel.SetActive(true);
        //�@�퓬�Ŋl�������o���l
        var earnedExperience = 0;
        //�@�퓬�Ŋl����������
        var earnedMoney = 0;
        //�@�퓬�Ŋl�������A�C�e���Ƃ��̌�
        Dictionary<Item, int> getItemDictionary = new Dictionary<Item, int>();
        //�@Float�̃����_���l
        float randomFloat;
        //�@�A�C�e���擾�m��
        float probability;
        //�@�L�����N�^�[�X�e�[�^�X
        CharacterStatus characterStatus;
        //�@�G�̃A�C�e���f�B�N�V���i���[
        ItemDictionary enemyItemDictionary;

        foreach (var character in allCharacterList)
        {
            characterStatus = character.GetComponent<CharacterBattleScript>().GetCharacterStatus();
            if (characterStatus as EnemyStatus != null)
            {
                earnedExperience += ((EnemyStatus)characterStatus).GetGettingExperience();
                earnedMoney += ((EnemyStatus)characterStatus).GetGettingMoney();
                enemyItemDictionary = ((EnemyStatus)characterStatus).GetDropItemDictionary();
                //�@�G�������Ă���A�C�e���̎�ނ̐������J��Ԃ�
                foreach (var item in enemyItemDictionary.Keys)
                {
                    //�@0�`100�̊Ԃ̃����_���l���擾
                    randomFloat = Random.Range(0f, 100f);
                    //�@�A�C�e���̎擾�m�����擾
                    probability = enemyItemDictionary[item];
                    //�@�����_���l���A�C�e���擾�m���ȉ��̒l�ł���΃A�C�e���擾
                    if (randomFloat <= probability)
                    {
                        if (getItemDictionary.ContainsKey(item))
                        {
                            getItemDictionary[item]++;
                        }
                        else
                        {
                            getItemDictionary.Add(item, 1);
                        }
                    }
                }
            }
        }
        resultText.text = earnedExperience + "�̌o���l���l�������B\n";
        resultText.text += earnedMoney + "�̂������l�������B\n";

        //�@�p�[�e�B�[�X�e�[�^�X�ɂ����𔽉f����
        partyStatus.SetMoney(partyStatus.GetMoney() + earnedMoney);

        //�@int�̃����_���l
        int randomInt;
        AllyStatus allyStatus;

        //�@�擾�����A�C�e���𖡕��p�[�e�B�[�ɕ��z����
        foreach (var item in getItemDictionary.Keys)
        {
            //�@�p�[�e�B�[�����o�[�̒N�ɃA�C�e����n��������
            randomInt = Random.Range(0, allyCharacterInBattleList.Count);
            allyStatus = (AllyStatus)allyCharacterInBattleList[randomInt].GetComponent<CharacterBattleScript>().GetCharacterStatus();
            //�@�L�����N�^�[�����ɃA�C�e���������Ă��鎞
            if (allyStatus.GetItemDictionary().ContainsKey(item))
            {
                allyStatus.SetItemNum(item, allyStatus.GetItemNum(item) + getItemDictionary[item]);
            }
            else
            {
                allyStatus.SetItemDictionary(item, getItemDictionary[item]);
            }
            resultText.text += allyStatus.GetCharacterName() + "��" + item.GetKanjiName() + "��" + getItemDictionary[item] + "��ɓ��ꂽ�B\n";
            resultText.text += "\n";
        }

        //�@�オ�������x��
        var levelUpCount = 0;
        //�@�オ����HP
        var raisedHp = 0;
        //�@�オ����MP
        var raisedMp = 0;
        //�@�オ�����f����
        var raisedAgility = 0;
        //�@�オ������
        var raisedPower = 0;
        //�@�オ�����ł��ꋭ��
        var raisedStrikingStrength = 0;
        //�@�オ�������@��
        var raisedMagicPower = 0;
        //�@LevelUpData
        LevelUpData levelUpData;

        //�@���x���A�b�v���̌v�Z
        foreach (var characterObj in allyCharacterInBattleList)
        {
            var character = (AllyStatus)characterObj.GetComponent<CharacterBattleScript>().GetCharacterStatus();
            //�@�ϐ���������
            levelUpCount = 0;
            raisedHp = 0;
            raisedMp = 0;
            raisedAgility = 0;
            raisedPower = 0;
            raisedStrikingStrength = 0;
            raisedMagicPower = 0;
            levelUpData = character.GetLevelUpData();

            //�@�L�����N�^�[�Ɍo���l�𔽉f
            character.SetEarnedExperience(character.GetEarnedExperience() + earnedExperience);

            //�@���̃L�����N�^�[�̌o���l�ŉ����x���A�b�v�������ǂ���
            for (int i = 1; i < levelUpData.GetLevelUpDictionary().Count; i++)
            {
                //�@���x���A�b�v�ɕK�v�Ȍo���l�𖞂����Ă�����
                if (character.GetEarnedExperience() >= levelUpData.GetRequiredExperience(character.GetLevel() + i))
                {
                    levelUpCount++;
                }
                else
                {
                    break;
                }
            }
            //�@���x���𔽉f
            character.SetLevel(character.GetLevel() + levelUpCount);

            //�@���x���A�b�v���̃X�e�[�^�X�A�b�v���v�Z�����f����
            for (int i = 0; i < levelUpCount; i++)
            {
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseMaxHP())
                {
                    raisedHp += Random.Range(levelUpData.GetMinHPRisingLimit(), levelUpData.GetMaxHPRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseMaxMP())
                {
                    raisedMp += Random.Range(levelUpData.GetMinMPRisingLimit(), levelUpData.GetMaxMPRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseAgility())
                {
                    raisedAgility += Random.Range(levelUpData.GetMinAgilityRisingLimit(), levelUpData.GetMaxAgilityRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreasePower())
                {
                    raisedPower += Random.Range(levelUpData.GetMinPowerRisingLimit(), levelUpData.GetMaxPowerRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseStrikingStrength())
                {
                    raisedStrikingStrength += Random.Range(levelUpData.GetMinStrikingStrengthRisingLimit(), levelUpData.GetMaxStrikingStrengthRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseMagicPower())
                {
                    raisedMagicPower += Random.Range(levelUpData.GetMinMagicPowerRisingLimit(), levelUpData.GetMaxMagicPowerRisingLimit());
                }
            }
            if (levelUpCount > 0)
            {
                resultText.text += character.GetCharacterName() + "��" + levelUpCount + "���x���オ����Lv" + character.GetLevel() + "�ɂȂ����B\n";
                if (raisedHp > 0)
                {
                    resultText.text += "�ő�HP��" + raisedHp + "�オ�����B\n";
                    character.SetMaxHp(character.GetMaxHp() + raisedHp);
                }
                if (raisedMp > 0)
                {
                    resultText.text += "�ő�MP��" + raisedMp + "�オ�����B\n";
                    character.SetMaxMp(character.GetMaxMp() + raisedMp);
                }
                if (raisedAgility > 0)
                {
                    resultText.text += "�f������" + raisedAgility + "�オ�����B\n";
                    character.SetAgility(character.GetAgility() + raisedAgility);
                }
                if (raisedPower > 0)
                {
                    resultText.text += "�͂�" + raisedPower + "�オ�����B\n";
                    character.SetPower(character.GetPower() + raisedPower);
                }
                if (raisedStrikingStrength > 0)
                {
                    resultText.text += "�ł��ꋭ����" + raisedStrikingStrength + "�オ�����B\n";
                    character.SetStrikingStrength(character.GetStrikingStrength() + raisedStrikingStrength);
                }
                if (raisedMagicPower > 0)
                {
                    resultText.text += "���@�͂�" + raisedMagicPower + "�オ�����B\n";
                    character.SetMagicPower(character.GetMagicPower() + raisedMagicPower);
                }
                resultText.text += "\n";
            }
        }
        //�@���ʂ��v�Z���I�����
        isDisplayResult = true;

        //�@�퓬�I����BGM�ɕύX����
        musicManager.ChangeBGM();

        //�@���ʌ�ɐ��b�ҋ@
        yield return new WaitForSeconds(timeToDisplay);
        //�@�퓬���甲���o��
        resultPanel.transform.Find("FinishText").gameObject.SetActive(true);
        isFinishResult = true;
    }


    //�@�s�펞�̏�������
    public void InitialProcessingOfDefeatResult()
    {
        StartCoroutine(DisplayDefeatResult());
    }

    //�@�s�펞�̕\��
    public IEnumerator DisplayDefeatResult()
    {
        yield return new WaitForSeconds(timeToDisplay);
        resultPanel.SetActive(true);
        resultText.text = "���j�e�B�����B�͑S�ł����B";
        isDisplayResult = true;
        yield return new WaitForSeconds(timeToDisplay);
        var finishText = resultPanel.transform.Find("FinishText");
        finishText.GetComponent<Text>().text = "�ŏ��̊X��";
        finishText.gameObject.SetActive(true);

        //�@�������S�ł����̂Ń��j�e�B������HP���������񕜂��Ă���
        var unityChanStatus = partyStatus.GetAllyStatus().Find(character => character.GetCharacterName() == "���j�e�B�����");
        if (unityChanStatus != null)
        {
            unityChanStatus.SetHp(1);
        }

        isFinishResult = true;
    }


    //�@���������̏�������
    public void InitialProcessingOfRanAwayResult()
    {
        StartCoroutine(DisplayRanAwayResult());
    }

    //�@���������̕\��
    public IEnumerator DisplayRanAwayResult()
    {
        yield return new WaitForSeconds(timeToDisplay);
        ranAway = true;
        resultPanel.SetActive(true);
        resultText.text = "���j�e�B�����B�͓����o�����B";
        isDisplayResult = true;
        yield return new WaitForSeconds(timeToDisplay);
        var finishText = resultPanel.transform.Find("FinishText");
        finishText.GetComponent<Text>().text = "���[���h�}�b�v��";
        finishText.gameObject.SetActive(true);
        isFinishResult = true;
    }

}