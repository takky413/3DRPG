using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdleCommandScript : MonoBehaviour
{
    //�@�p�[�e�B�[�X�e�[�^�X
    [SerializeField]
    private PartyStatus partyStatus = null;
    //�@�ǂꂾ����������Ă��Ȃ���Ε\�����邩
    [SerializeField]
    private float idleTime = 5f;
    //�@�ǂꂾ����������Ă��Ȃ���
    private float elapsedTime;
    //�@���݃L�����N�^�[�X�e�[�^�X�p�l�����J���Ă��邩�ǂ���
    private bool isOpenCharacterStatusPanel;
    //�@�L�����N�^�[���̃X�e�[�^�X�p�l���v���n�u
    [SerializeField]
    private GameObject characterPanelPrefab = null;
    //�@�L�����N�^�[�X�e�[�^�X�p�l��
    private GameObject characterStatusPanel;
    //�@SceneManager
    private LoadSceneManager sceneManager = null;
    //�@UnityChanScript
    [SerializeField]
    private UnityChanScript unityChanScript = null;

    private void Awake()
    {
        characterStatusPanel = transform.Find("CharacterStatusPanel").gameObject;
    }
    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<LoadSceneManager>();
    }


    // Update is called once per frame
    void Update()
    {
        //�@�V�[���J�ړr���ƃ��j�e�B�����̏�Ԃɂ���Ă͕\�����Ȃ�
        if (sceneManager.IsTransition()
            || unityChanScript.GetState() == UnityChanScript.State.Talk
            || unityChanScript.GetState() == UnityChanScript.State.Command
            )
        {
            elapsedTime = 0f;
            characterStatusPanel.SetActive(false);
            return;
        }

        //�@���炩�̃L�[�������ꂽ��
        if (Input.anyKeyDown
        || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
        || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
        )
        {

            elapsedTime = 0f;
            //�@�L�����N�^�[�X�e�[�^�X�p�l���̎q�v�f������΍폜����
            for (int i = characterStatusPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(characterStatusPanel.transform.GetChild(i).gameObject);
            }
            characterStatusPanel.SetActive(false);
            isOpenCharacterStatusPanel = false;
            //�@�L�����N�^�[�X�e�[�^�X�p�l�����J���Ă��Ȃ�������
        }
        else if (!isOpenCharacterStatusPanel)
        {
            elapsedTime += Time.deltaTime;
            //�@�o�ߎ��Ԃ���莞�Ԃ��z������L�����N�^�[�X�e�[�^�X�p�l����\��
            if (elapsedTime >= idleTime)
            {
                GameObject characterPanel;
                //�@�p�[�e�B�[�����o�[���̃X�e�[�^�X���쐬
                foreach (var member in partyStatus.GetAllyStatus())
                {
                    characterPanel = Instantiate<GameObject>(characterPanelPrefab, characterStatusPanel.transform);
                    characterPanel.transform.Find("CharacterName").GetComponent<Text>().text = member.GetCharacterName();
                    characterPanel.transform.Find("HPSlider").GetComponent<Slider>().value = (float)member.GetHp() / member.GetMaxHp();
                    characterPanel.transform.Find("HPSlider/HPText").GetComponent<Text>().text = member.GetHp().ToString();
                    characterPanel.transform.Find("MPSlider").GetComponent<Slider>().value = (float)member.GetMp() / member.GetMaxMp();
                    characterPanel.transform.Find("MPSlider/MPText").GetComponent<Text>().text = member.GetMp().ToString();
                }
                characterStatusPanel.SetActive(true);
                elapsedTime = 0f;
                isOpenCharacterStatusPanel = true;
            }
        }
    }
}

