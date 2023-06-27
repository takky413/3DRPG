using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Conversation", menuName = "CreateConversation")]
public class Conversation : ScriptableObject
{
    //�@��b���e
    [SerializeField]
    [Multiline(100)]
    private string message = null;

    //�@��b���e��Ԃ�
    public string GetConversationMessage()
    {
        return message;
    }
}