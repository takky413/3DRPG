using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyPartyStatusList", menuName = "CreateEnemyPartyStatusList")]
public class EnemyPartyStatusList : ScriptableObject
{
    [SerializeField]
    private List<EnemyPartyStatus> partyMembersList = null; //©•ª‚ÅEnemyPartyStatus‚ğGameObject‚ÉC³

    public List<EnemyPartyStatus> GetPartyMembersList() //©•ª‚ÅEnemyPartyStatus‚ğGameObject‚ÉC³
    {
        return partyMembersList;
    }
}