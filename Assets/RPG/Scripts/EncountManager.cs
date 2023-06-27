using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountManager : MonoBehaviour
{
    //�@LoadSceneManager
    private LoadSceneManager sceneManager;
    [SerializeField]
    private float encountMinTime = 3f;
    //�@�G�Ƒ������郉���_������
    [SerializeField]
    private float encountMaxTime = 30f;
    //�@�o�ߎ���
    [SerializeField]
    private float elapsedTime;
    //�@�ړI�̎���
    [SerializeField]
    private float destinationTime;
    //�@���j�e�B�����
    private Transform unityChanObjct;
    //�@���j�e�B�����X�N���v�g
    private UnityChanScript unityChanScript;

    //�@�퓬�f�[�^
    [SerializeField]
    private BattleData battleData = null;
    //�@�G�p�[�e�B�[���X�g
    [SerializeField]
    private EnemyPartyStatusList enemyPartyStatusList = null;
    //�@���[���h�}�b�v�t�B�[���h
    [SerializeField]
    private Terrain worldMapField;

    private AudioSource audioSource;

    [SerializeField]
    private SceneMovementData sceneMovementData = null;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<LoadSceneManager>();
        SetDestinationTime();
        unityChanObjct = GameObject.FindWithTag("Player").transform;
        unityChanScript = unityChanObjct.GetComponent<UnityChanScript>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�@�ړ����Ă��Ȃ����͌v�����Ȃ�
        if (Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
            && Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
            )
        {
            return;
        }
        //�@���j�e�B����񂪉��炩�̍s�������Ă�����v�����Ȃ�
        if (unityChanScript.GetState() == UnityChanScript.State.Talk
            || unityChanScript.GetState() == UnityChanScript.State.Command
            )
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= destinationTime)
        {
            //�@���[���h�}�b�v��̃��j�e�B�����̈ʒu�ɉ����đ�������G�����肷��
            if (-worldMapField.terrainData.size.x / 2 <= unityChanObjct.position.x && unityChanObjct.position.x <= 0f
                && 0f <= unityChanObjct.position.z && unityChanObjct.position.z <= worldMapField.terrainData.size.z / 2
                )
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup1"));
            }
            else if (0f <= unityChanObjct.position.x && unityChanObjct.position.x <= worldMapField.terrainData.size.x / 2
              && 0 <= unityChanObjct.position.z && unityChanObjct.position.z <= worldMapField.terrainData.size.z / 2
              )
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup2"));
            }
            else if (-worldMapField.terrainData.size.x / 2 <= unityChanObjct.position.x && unityChanObjct.position.x <= 0f
              && -worldMapField.terrainData.size.z / 2 <= unityChanObjct.position.z && unityChanObjct.position.z <= 0f
              )
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup1"));
            }
            else if (0f <= unityChanObjct.position.x && unityChanObjct.position.x <= worldMapField.terrainData.size.x / 2
              && -worldMapField.terrainData.size.z / 2 <= unityChanObjct.position.z && unityChanObjct.position.z <= 0f
              )
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup2"));
            }
            else
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup1"));
            }

            Debug.Log("����");
            audioSource.Play();
            sceneMovementData.SetWorldMapPos(unityChanObjct.transform.position);
            sceneMovementData.SetWorldMapRot(unityChanObjct.transform.rotation);
            sceneManager.GoToNextScene(SceneMovementData.SceneType.WorldMapToBattle);
            elapsedTime = 0f;
            SetDestinationTime();
        }
    }

    //�@���ɓG�Ƒ������鎞��
    public void SetDestinationTime()
    {
        destinationTime = Random.Range(encountMinTime, encountMaxTime);
    }
}