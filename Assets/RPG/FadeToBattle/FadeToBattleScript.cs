using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBattleScript : MonoBehaviour
{

    [SerializeField]
    private Material material;
    //�@�t�F�[�hAmount�̓��B�l
    private float destinationAmount;

    private void Start()
    {
        //�@�X�^�[�g���ɏ�����
        material.SetFloat("_Amount", 0f);
    }
    //�@�J�����Ɏ��t����ƌĂ΂��
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}