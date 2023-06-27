using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "LevelUpData", menuName = "CreateLevelUpData")]
public class LevelUpData : ScriptableObject
{

    //�@���x���A�b�v�ɕK�v�ȃg�[�^���o���l
    [SerializeField]
    private LevelUpDictionary requiredExperience = null;
    //�@MaxHp���オ�闦
    [SerializeField]
    private float probabilityToIncreaseMaxHP = 100f;
    //�@MaxMP���オ�闦
    [SerializeField]
    private float probabliityToIncreaseMaxMP = 100f;
    [SerializeField]
    //�@�f�������オ�闦
    private float probabilityToIncreaseAgility = 100f;
    [SerializeField]
    //�@�͂��オ�闦
    private float probabilityToIncreasePower = 100f;
    [SerializeField]
    //�@�ł��ꋭ�����オ�闦
    private float probabilityToIncreaseStrikingStrength = 100f;
    [SerializeField]
    //�@���@�͂��オ�闦
    private float probabilityToIncreaseMagicPower = 100f;

    //�@MaxHP���オ�������̍Œ�l
    [SerializeField]
    private int minHPRisingLimit = 1;
    //�@MaxMP���オ�������̍Œ�l
    [SerializeField]
    private int minMPRisingLimit = 1;
    //�@�f�������オ�������̍Œ�l
    [SerializeField]
    private int minAgilityRisingLimit = 1;
    //�@�͂��オ�������̍Œ�l
    [SerializeField]
    private int minPowerRisingLimit = 1;
    //�@�ł��ꋭ�����オ�������̍Œ�l
    [SerializeField]
    private int minStrikingStrengthRisingLimit = 1;
    //�@���@�͂��オ�������̍Œ�l
    [SerializeField]
    private int minMagicPowerRisingLimit = 1;


    //�@MaxHP���オ�������̍ō��l
    [SerializeField]
    private int maxHPRisingLimit = 50;
    //�@MaxMP���オ�������̍ō��l
    [SerializeField]
    private int maxMPRisingLimit = 50;
    //�@�f�������オ�������̍ō��l
    [SerializeField]
    private int maxAgilityRisingLimit = 2;
    //�@�͂��オ�������̍ō��l
    [SerializeField]
    private int maxPowerRisingLimit = 2;
    //�@�ł��ꋭ�����オ�������̍ō��l
    [SerializeField]
    private int maxStrikingStrengthRisingLimit = 2;
    //�@���@�͂��オ�������̍ō��l
    [SerializeField]
    private int maxMagicPowerRisingLimit = 2;

    //�@���̃��x���ɕK�v�Ȍo���l
    public int GetRequiredExperience(int level)
    {
        return requiredExperience.Keys.Contains(level) ? requiredExperience[level] : int.MaxValue;
    }

    public LevelUpDictionary GetLevelUpDictionary()
    {
        return requiredExperience;
    }

    public float GetProbabilityToIncreaseMaxHP()
    {
        return probabilityToIncreaseMaxHP;
    }
    public float GetProbabilityToIncreaseMaxMP()
    {
        return probabliityToIncreaseMaxMP;
    }
    public float GetProbabilityToIncreaseAgility()
    {
        return probabilityToIncreaseAgility;
    }
    public float GetProbabilityToIncreasePower()
    {
        return probabilityToIncreasePower;
    }
    public float GetProbabilityToIncreaseStrikingStrength()
    {
        return probabilityToIncreaseStrikingStrength;
    }
    public float GetProbabilityToIncreaseMagicPower()
    {
        return probabilityToIncreaseMagicPower;
    }

    public int GetMinHPRisingLimit()
    {
        return minHPRisingLimit;
    }
    public int GetMinMPRisingLimit()
    {
        return minMPRisingLimit;
    }
    public int GetMinAgilityRisingLimit()
    {
        return minAgilityRisingLimit;
    }
    public int GetMinPowerRisingLimit()
    {
        return minPowerRisingLimit;
    }
    public int GetMinStrikingStrengthRisingLimit()
    {
        return minStrikingStrengthRisingLimit;
    }
    public int GetMinMagicPowerRisingLimit()
    {
        return minMagicPowerRisingLimit;
    }


    public int GetMaxHPRisingLimit()
    {
        return maxHPRisingLimit;
    }
    public int GetMaxMPRisingLimit()
    {
        return maxMPRisingLimit;
    }
    public int GetMaxAgilityRisingLimit()
    {
        return maxAgilityRisingLimit;
    }
    public int GetMaxPowerRisingLimit()
    {
        return maxPowerRisingLimit;
    }
    public int GetMaxStrikingStrengthRisingLimit()
    {
        return maxStrikingStrengthRisingLimit;
    }
    public int GetMaxMagicPowerRisingLimit()
    {
        return maxMagicPowerRisingLimit;
    }
}