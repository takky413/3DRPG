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
            //　ユニティちゃんが近づいたら会話相手として自分のゲームオブジェクトを渡す
            col.GetComponent<UnityChanTalkScript>().SetConversationPartner(transform.parent.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player"
            && col.GetComponent<UnityChanScript>().GetState() != UnityChanScript.State.Talk
            )
        {
            //　ユニティちゃんが遠ざかったら会話相手から外す
            col.GetComponent<UnityChanTalkScript>().ResetConversationPartner(transform.parent.gameObject);
        }
    }
}