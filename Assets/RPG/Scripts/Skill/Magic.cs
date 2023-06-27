using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Magic", menuName = "CreateMagic")]
public class Magic : Skill
{
    public enum MagicAttribute
    {
        Fire,
        Water,
        Thunder,
        Other
    }

    //�@���@��
    [SerializeField]
    private int magicPower = 0;
    //�@�g��MP
    [SerializeField]
    private int amountToUseMagicPoints = 0;
    //�@���@�̑���
    [SerializeField]
    private MagicAttribute magicAttribute = MagicAttribute.Other;

    //�@���@�͂�Ԃ�
    public int GetMagicPower()
    {
        return magicPower;
    }
    //�@���@�̑�����Ԃ�
    public MagicAttribute GetMagicAttribute()
    {
        return magicAttribute;
    }

    public void SetAmountToUseMagicPoints(int point)
    {
        amountToUseMagicPoints = Mathf.Max(0, Mathf.Min(amountToUseMagicPoints, point));
    }

    public int GetAmountToUseMagicPoints()
    {
        return amountToUseMagicPoints;
    }
}