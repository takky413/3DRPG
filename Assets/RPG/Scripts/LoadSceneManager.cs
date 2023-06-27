using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager loadSceneManager;
    //�@�V�[���ړ��Ɋւ���f�[�^�t�@�C��
    [SerializeField]
    private SceneMovementData sceneMovementData = null;
    //�@�t�F�[�h�v���n�u
    [SerializeField]
    private GameObject fadePrefab = null;
    //�@�t�F�[�h�C���X�^���X
    private GameObject fadeInstance;
    //�@�t�F�[�h�̉摜
    private Image fadeImage;
    [SerializeField]
    private float fadeSpeed = 5f;

    //�@�V�[���J�ڒ����ǂ���
    private bool isTransition;

    //�@�t�F�[�h�Ɏg�p����}�e���A��
    [SerializeField]
    private Material material;


    private void Awake()
    {
        // LoadSceneManger�͏�Ɉ�����ɂ���
        if (loadSceneManager == null)
        {
            loadSceneManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //�@���̃V�[�����Ăяo��
    public void GoToNextScene(SceneMovementData.SceneType scene)
    {
        isTransition = true;
        sceneMovementData.SetSceneType(scene);
        StartCoroutine(FadeAndLoadScene(scene));
    }

    public bool IsTransition()
    {
        return isTransition;
    }

    //�@�t�F�[�h��������ɃV�[���ǂݍ���
    IEnumerator FadeAndLoadScene(SceneMovementData.SceneType scene)
    {

        if (scene != SceneMovementData.SceneType.WorldMapToBattle)
        {
            //�@�t�F�[�hUI�̃C���X�^���X��
            fadeInstance = Instantiate<GameObject>(fadePrefab);
            fadeImage = fadeInstance.GetComponentInChildren<Image>();
            //�@�t�F�[�h�A�E�g����
            yield return StartCoroutine(Fade(1f));
        }
        else
        {
            yield return StartCoroutine(FadeWorldMapToBattle(0.1f));
        }

        //�@�V�[���̓ǂݍ���
        if (scene == SceneMovementData.SceneType.FirstVillage)
        {
            yield return StartCoroutine(LoadScene("Village"));
        }
        else if (scene == SceneMovementData.SceneType.FirstVillageToWorldMap)
        {
            yield return StartCoroutine(LoadScene("WorldMap"));
        }
        else if (scene == SceneMovementData.SceneType.WorldMapToBattle)
        {
            yield return StartCoroutine(LoadScene("Battle"));
        }
        else if (scene == SceneMovementData.SceneType.BattleToWorldMap)
        {
            yield return StartCoroutine(LoadScene("WorldMap"));
        }
        else if (scene == SceneMovementData.SceneType.StartGame)
        {
            yield return StartCoroutine(LoadScene("Village"));
        }

        //�@�t�F�[�hUI�̃C���X�^���X��
        fadeInstance = Instantiate<GameObject>(fadePrefab);
        fadeImage = fadeInstance.GetComponentInChildren<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        //�@�t�F�[�h�C������
        yield return StartCoroutine(Fade(1f));

        Destroy(fadeInstance);

        isTransition = false; //�����Œǉ�//���ꂪ�Ȃ��ƈ���ʑJ�ڂ�����A���isTransiton = true�ɂȂ��Ă��܂��A���j���[��ʂ��J���Ȃ��Ȃ��Ă����B
    }

    //�@�t�F�[�h����
    IEnumerator Fade(float alpha)
    {
        var fadeImageAlpha = fadeImage.color.a;

        while (Mathf.Abs(fadeImageAlpha - alpha) > 0.01f)
        {
            fadeImageAlpha = Mathf.Lerp(fadeImageAlpha, alpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = new Color(0f, 0f, 0f, fadeImageAlpha);
            yield return null;
        }
    }

    //�@���[���h�}�b�v���o�g���}�b�v�ł̃t�F�[�h����
    IEnumerator FadeWorldMapToBattle(float destinationAmount)
    {
        var fadeAmount = material.GetFloat("_Amount");

        while (Mathf.Abs(material.GetFloat("_Amount") - destinationAmount) > 0.01f)
        {
            material.SetFloat("_Amount", Mathf.Lerp(material.GetFloat("_Amount"), destinationAmount, fadeSpeed * Time.deltaTime));
            yield return null;
        }
        material.SetFloat("_Amount", destinationAmount);
    }

    //�@���ۂɃV�[����ǂݍ��ޏ���
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            yield return null;
        }
    }


    //�@���߂���Q�[�����n�߂�
    public void StartGame()
    {
        isTransition = true;
        sceneMovementData.SetSceneType(SceneMovementData.SceneType.StartGame);
        StartCoroutine(FadeAndLoadScene(SceneMovementData.SceneType.StartGame));
    }
}