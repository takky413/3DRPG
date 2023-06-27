using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToOtherScene : MonoBehaviour
{
    private LoadSceneManager sceneManager;
    //�@�ǂ̃V�[���֑J�ڂ��邩
    [SerializeField]
    private SceneMovementData.SceneType scene = SceneMovementData.SceneType.FirstVillage;
    //�@�V�[���J�ڒ����ǂ���
    [SerializeField] private bool isTransition; //�f�o�b�N[SerializeField]��ǉ�

    private void Awake()
    {
        sceneManager = FindObjectOfType<LoadSceneManager>();
    }

    private void OnTriggerEnter(Collider col)
    {
        //�@���̃V�[���֑J�ړr���łȂ���
        if (col.tag == "Player" && !isTransition)
        {
            isTransition = true;
            sceneManager.GoToNextScene(scene);
        }
    }
   
    //�@�t�F�[�h��������ɃV�[���ǂݍ���
    IEnumerator FadeAndLoadScene(SceneMovementData.SceneType scene)
    {
        
        //���̑��̏���
        
        isTransition = false;

        yield return null; //�����Œǉ�����
    }
}