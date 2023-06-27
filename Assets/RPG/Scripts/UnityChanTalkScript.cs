using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanTalkScript : MonoBehaviour
{
    //�@��b�\�ȑ���
    private GameObject conversationPartner;
    //�@��b�\�A�C�R��
    [SerializeField]
    private GameObject talkIcon = null;


    // TalkUI�Q�[���I�u�W�F�N�g
    [SerializeField]
    private GameObject talkUI = null;
    //�@���b�Z�[�WUI
    private Text messageText = null;
    //�@�\�����郁�b�Z�[�W
    private string allMessage = null;
    //�@�g�p���镪��������
    [SerializeField]
    private string splitString = "<>";
    //�@�����������b�Z�[�W
    private string[] splitMessage;
    //�@�����������b�Z�[�W�̉��Ԗڂ�
    private int messageNum;
    //�@�e�L�X�g�X�s�[�h
    [SerializeField]
    private float textSpeed = 0.05f;
    //�@�o�ߎ���
    private float elapsedTime = 0f;
    //�@�����Ă��镶���ԍ�
    private int nowTextNum = 0;
    //�@�}�E�X�N���b�N�𑣂��A�C�R��
    [SerializeField]
    private Image clickIcon = null;
    //�@�N���b�N�A�C�R���̓_�ŕb��
    [SerializeField]
    private float clickFlashTime = 0.2f;
    //�@1�񕪂̃��b�Z�[�W��\���������ǂ���
    private bool isOneMessage = false;
    //�@���b�Z�[�W�����ׂĕ\���������ǂ���
    private bool isEndMessage = false;

    void Start()
    {
        clickIcon.enabled = false;
        messageText = talkUI.GetComponentInChildren<Text>();
    }

    void Update()
    {
        //�@���b�Z�[�W���I����Ă��邩�A���b�Z�[�W���Ȃ��ꍇ�͂���ȍ~�������Ȃ�
        if (isEndMessage || allMessage == null)
        {
            return;
        }

        //�@1��ɕ\�����郁�b�Z�[�W��\�����Ă��Ȃ�	
        if (!isOneMessage)
        {
            //�@�e�L�X�g�\�����Ԃ��o�߂����烁�b�Z�[�W��ǉ�
            if (elapsedTime >= textSpeed)
            {
                messageText.text += splitMessage[messageNum][nowTextNum];

                nowTextNum++;
                elapsedTime = 0f;

                //�@���b�Z�[�W��S���\���A�܂��͍s�����ő吔�\�����ꂽ
                if (nowTextNum >= splitMessage[messageNum].Length)
                {
                    isOneMessage = true;
                }
            }
            elapsedTime += Time.deltaTime;

            //�@���b�Z�[�W�\�����Ƀ}�E�X�̍��{�^������������ꊇ�\��
            if (Input.GetButtonDown("Jump"))
            {
                //�@�����܂łɕ\�����Ă���e�L�X�g�Ɏc��̃��b�Z�[�W�𑫂�
                messageText.text += splitMessage[messageNum].Substring(nowTextNum);
                isOneMessage = true;
            }
            //�@1��ɕ\�����郁�b�Z�[�W��\������
        }
        else
        {

            elapsedTime += Time.deltaTime;

            //�@�N���b�N�A�C�R����_�ł��鎞�Ԃ𒴂������A���]������
            if (elapsedTime >= clickFlashTime)
            {
                clickIcon.enabled = !clickIcon.enabled;
                elapsedTime = 0f;
            }

            //�@�}�E�X�N���b�N���ꂽ�玟�̕����\������
            if (Input.GetButtonDown("Jump"))
            {
                nowTextNum = 0;
                messageNum++;
                messageText.text = "";
                clickIcon.enabled = false;
                elapsedTime = 0f;
                isOneMessage = false;

                //�@���b�Z�[�W���S���\������Ă�����Q�[���I�u�W�F�N�g���̂̍폜
                if (messageNum >= splitMessage.Length)
                {
                    EndTalking();
                }
            }
        }
    }

    private void LateUpdate()
    {
        //�@��b���肪����ꍇ��TalkIcon�̈ʒu����b����̓���ɕ\��
        if (conversationPartner != null)
        {
            talkIcon.transform.Find("Panel").position = Camera.main.GetComponent<Camera>().WorldToScreenPoint(conversationPartner.transform.position + Vector3.up * 2f);
        }
    }

    //�@��b�����ݒ�
    public void SetConversationPartner(GameObject partnerObj)
    {
        talkIcon.SetActive(true);
        conversationPartner = partnerObj;
    }

    //�@��b��������Z�b�g
    public void ResetConversationPartner(GameObject parterObj)
    {
        //�@��b���肪���Ȃ��ꍇ�͉������Ȃ�
        if (conversationPartner == null)
        {
            return;
        }
        //�@��b����ƈ����Ŏ󂯎�������肪�����C���X�^���XID�����Ȃ��b������Ȃ���
        if (conversationPartner.GetInstanceID() == parterObj.GetInstanceID())
        {
            talkIcon.SetActive(false);
            conversationPartner = null;
        }
    }
    //�@��b�����Ԃ�
    public GameObject GetConversationPartner()
    {
        return conversationPartner;
    }

    //�@��b���J�n����
    public void StartTalking()
    {
        var villagerScript = conversationPartner.GetComponent<VillagerScript>();
        villagerScript.SetState(VillagerScript.State.Talk, transform);
        this.allMessage = villagerScript.GetConversation().GetConversationMessage();
        //�@����������ň��ɕ\�����郁�b�Z�[�W�𕪊�����
        splitMessage = Regex.Split(allMessage, @"\s*" + splitString + @"\s*", RegexOptions.IgnorePatternWhitespace);
        //�@����������
        nowTextNum = 0;
        messageNum = 0;
        messageText.text = "";
        talkUI.SetActive(true);
        talkIcon.SetActive(false);
        isOneMessage = false;
        isEndMessage = false;
        //�@��b�J�n���̓��͈͂�U���Z�b�g
        Input.ResetInputAxes();
    }
    //�@��b���I������
    void EndTalking()
    {
        isEndMessage = true;
        talkUI.SetActive(false);
        //�@���j�e�B�����Ƒ��l�����̏�Ԃ�ύX����
        var villagerScript = conversationPartner.GetComponent<VillagerScript>();
        villagerScript.SetState(VillagerScript.State.Wait);
        GetComponent<UnityChanScript>().SetState(UnityChanScript.State.Normal);
        Input.ResetInputAxes();
    }
}