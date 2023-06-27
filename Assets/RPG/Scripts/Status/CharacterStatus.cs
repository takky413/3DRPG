using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class CharacterStatus : ScriptableObject
{

    //�@�L�����N�^�[�̖��O
    [SerializeField]
    private string characterName = "";
    //�@�ŏ�Ԃ��ǂ���
    [SerializeField]
    private bool isPoisonState = false;
    //�@Ⴢ��Ԃ��ǂ���
    [SerializeField]
    private bool isNumbnessState = false;
    //�@�L�����N�^�[�̃��x��
    [SerializeField]
    private int level = 1;
    //�@�ő�HP
    [SerializeField]
    private int maxHp = 100;
    //�@HP
    [SerializeField]
    private int hp = 100;
    //�@�ő�MP
    [SerializeField]
    private int maxMp = 50;
    //�@MP
    [SerializeField]
    private int mp = 50;
    //�@�f����
    [SerializeField]
    private int agility = 5;
    //�@��
    [SerializeField]
    private int power = 10;
    //�@�ł��ꋭ��
    [SerializeField]
    private int strikingStrength = 10;
    //�@���@��
    [SerializeField]
    private int magicPower = 10;

    //�@�����Ă���X�L��
    [SerializeField] private List<Skill> skillList = null;

    public void SetCharacterName(string characterName)
    {
        this.characterName = characterName;
    }

    public string GetCharacterName()
    {
        return characterName;
    }

    public void SetPoisonState(bool poisonFlag)
    {
        isPoisonState = poisonFlag;
    }

    public bool IsPoisonState()
    {
        return isPoisonState;
    }

    public void SetNumbness(bool numbnessFlag)
    {
        isNumbnessState = numbnessFlag;
    }

    public bool IsNumbnessState()
    {
        return isNumbnessState;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetMaxHp(int hp)
    {
        this.maxHp = hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public void SetHp(int hp)
    {
        this.hp = Mathf.Max(0, Mathf.Min(GetMaxHp(), hp));
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetMaxMp(int mp)
    {
        this.maxMp = mp;
    }

    public int GetMaxMp()
    {
        return maxMp;
    }

    public void SetMp(int mp)
    {
        this.mp = Mathf.Max(0, Mathf.Min(GetMaxMp(), mp));
    }

    public int GetMp()
    {
        return mp;
    }

    public void SetAgility(int agility)
    {
        this.agility = agility;
    }

    public int GetAgility()
    {
        return agility;
    }

    public void SetPower(int power)
    {
        this.power = power;
    }

    public int GetPower()
    {
        return power;
    }

    public void SetStrikingStrength(int strikingStrength)
    {
        this.strikingStrength = strikingStrength;
    }

    public int GetStrikingStrength()
    {
        return strikingStrength;
    }

    public void SetMagicPower(int magicPower)
    {
        this.magicPower = magicPower;
    }

    public int GetMagicPower()
    {
        return magicPower;
    }

    public void SetSkillList(List<Skill> skillList)
    {
        this.skillList = skillList;
    }

    public List<Skill> GetSkillList()
    {
        return skillList;
    }
}
