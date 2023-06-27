using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyPartyStatusList", menuName = "CreateEnemyPartyStatusList")]
public class EnemyPartyStatusList : ScriptableObject
{
    [SerializeField]
    private List<EnemyPartyStatus> partyMembersList = null; //������EnemyPartyStatus��GameObject�ɏC��

    public List<EnemyPartyStatus> GetPartyMembersList() //������EnemyPartyStatus��GameObject�ɏC��
    {
        return partyMembersList;
    }
}