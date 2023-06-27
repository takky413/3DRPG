using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "CreateSkill")]
public class Skill : ScriptableObject
{
    public enum Type
    {
        DirectAttack,
        Guard,
        GetAway,
        Item,
        MagicAttack,
        RecoveryMagic,
        PoisonnouRecoveryMagic,
        NumbnessRecoveryMagic,
        IncreaseAttackPowerMagic,
        IncreaseDefencePowerMagic
    }

    [SerializeField]
    private Type skillType = Type.DirectAttack;
    [SerializeField]
    private string kanjiName = "";
    [SerializeField]
    private string hiraganaName = "";
    [SerializeField]
    private string information = "";
    //�@�g�p�҂̃G�t�F�N�g
    [SerializeField]
    private GameObject skillUserEffect = null;
    //�@���@���󂯂鑤�̃G�t�F�N�g
    [SerializeField]
    private GameObject skillReceivingSideEffect = null;

    //�@�X�L���̎�ނ�Ԃ�
    public Type GetSkillType()
    {
        return skillType;
    }
    //�@�X�L���̖��O��Ԃ�
    public string GetKanjiName()
    {
        return kanjiName;
    }
    //�@�X�L���̕������̖��O��Ԃ�
    public string GetHiraganaName()
    {
        return hiraganaName;
    }
    //�@�X�L������Ԃ�
    public string GetInformation()
    {
        return information;
    }
    //�@�g�p�҂̃G�t�F�N�g��Ԃ�
    public GameObject GetSkillUserEffect()
    {
        return skillUserEffect;
    }
    //�@���@���󂯂鑤�̃G�t�F�N�g��Ԃ�
    public GameObject GetSkillReceivingSideEffect()
    {
        return skillReceivingSideEffect;
    }
}
