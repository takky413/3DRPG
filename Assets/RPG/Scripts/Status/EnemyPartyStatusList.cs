using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyPartyStatusList", menuName = "CreateEnemyPartyStatusList")]
public class EnemyPartyStatusList : ScriptableObject
{
    [SerializeField]
    private List<EnemyPartyStatus> partyMembersList = null; //自分でEnemyPartyStatusをGameObjectに修正

    public List<EnemyPartyStatus> GetPartyMembersList() //自分でEnemyPartyStatusをGameObjectに修正
    {
        return partyMembersList;
    }
}