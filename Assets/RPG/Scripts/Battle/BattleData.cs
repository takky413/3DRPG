using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BattleData", menuName = "CreateBattleData")]
public class BattleData : ScriptableObject
{
    //�@�����p�[�e�B�[�f�[�^
    [SerializeField]
    private BattlePartyStatus battlePartyStatus;
    //�@�G�p�[�e�B�[�f�[�^
    private EnemyPartyStatus enemyPartyStatus;

    public void SetAllyPartyStatus(BattlePartyStatus partyStatus)
    {
        battlePartyStatus = partyStatus;
    }

    public BattlePartyStatus GetAllyPartyStatus()
    {
        return battlePartyStatus;
    }

    public void SetEnemyPartyStatus(EnemyPartyStatus enemyPartyStatus)
    {
        this.enemyPartyStatus = enemyPartyStatus;
    }

    public EnemyPartyStatus GetEnemyPartyStatus()
    {
        return enemyPartyStatus;
    }
}