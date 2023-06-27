using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SceneMovementData", menuName = "CreateSceneMovementData")]
public class SceneMovementData : ScriptableObject
{

    public enum SceneType
    {
        StartGame,
        FirstVillage,
        FirstVillageToWorldMap,
        WorldMapToBattle,
        BattleToWorldMap,
    }

    [SerializeField]
    private SceneType sceneType;

    //�@���[���h�}�b�v���퓬�V�[���ֈڍs�������̃��[���h�}�b�v�̈ʒu���
    private Vector3 worldMapPos;
    //�@���[���h�}�b�v���퓬�V�[���ֈڍs�������̃��[���h�}�b�v�̈ʒu���
    private Quaternion worldMapRot;

    public void OnEnable()
    {
        sceneType = SceneType.StartGame;
    }

    public void SetSceneType(SceneType scene)
    {
        sceneType = scene;
    }

    public SceneType GetSceneType()
    {
        return sceneType;
    }

    public void SetWorldMapPos(Vector3 pos)
    {
        worldMapPos = pos;
    }

    public Vector3 GetWorldMapPos()
    {
        return worldMapPos;
    }

    public void SetWorldMapRot(Quaternion rot)
    {
        worldMapRot = rot;
    }

    public Quaternion GetWorldMapRot()
    {
        return worldMapRot;
    }
}