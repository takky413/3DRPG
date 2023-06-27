using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationScopeScript : MonoBehaviour
{

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player"
            && col.GetComponent<UnityChanScript>().GetState() != UnityChanScript.State.Talk
            )
        {
            //�@���j�e�B����񂪋߂Â������b����Ƃ��Ď����̃Q�[���I�u�W�F�N�g��n��
            col.GetComponent<UnityChanTalkScript>().SetConversationPartner(transform.parent.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player"
            && col.GetComponent<UnityChanScript>().GetState() != UnityChanScript.State.Talk
            )
        {
            //�@���j�e�B����񂪉������������b���肩��O��
            col.GetComponent<UnityChanTalkScript>().ResetConversationPartner(transform.parent.gameObject);
        }
    }
}