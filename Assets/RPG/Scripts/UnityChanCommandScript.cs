using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnityChanCommandScript : MonoBehaviour
{
    private LoadSceneManager sceneManager;
    //�@�R�}���h�pUI
    [SerializeField]
    private GameObject commandUI = null;
    private UnityChanScript unityChanScript;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<LoadSceneManager>();
        unityChanScript = GetComponent<UnityChanScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.IsTransition()
            || unityChanScript.GetState() == UnityChanScript.State.Talk
            )
        {
            return;
        }
        //�@�R�}���hUI�̕\���E��\���̐؂�ւ�
        if (Input.GetButtonDown("Menu"))
        {
            //�@�R�}���h
            if (!commandUI.activeSelf)
            {
                //�@���j�e�B�������R�}���h��Ԃɂ���
                unityChanScript.SetState(UnityChanScript.State.Command);
            }
            else
            {
                ExitCommand();
            }
            //�@�R�}���hUI�̃I���E�I�t
            commandUI.SetActive(!commandUI.activeSelf);
        }
    }
    //�@CommandScript����Ăяo���R�}���h��ʂ̏I��
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        unityChanScript.SetState(UnityChanScript.State.Normal);
    }
}

