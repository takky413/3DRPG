using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleScript : MonoBehaviour
{
    //�@�퓬���̃L�����N�^�[�̏��
    public enum BattleState
    {
        Idle,
        DirectAttack,
        MagicAttack,
        Healing,
        UseHPRecoveryItem,
        UseMPRecoveryItem,
        UseNumbnessRecoveryItem,
        UsePoisonRecoveryItem,
        IncreaseAttackPowerMagic,
        IncreaseDefencePowerMagic,
        NumbnessRecoveryMagic,
        PoisonnouRecoveryMagic,
        Damage,
        Guard,
        Dead,
    }

    private BattleManager battleManager;
    private BattleStatusScript battleStatusScript;
    [SerializeField]
    private CharacterStatus characterStatus = null;
    private Animator animator;
    private BattleState battleState;

    //�@���̃X�e�[�^�X����R�s�[

    //�@HP
    private int hp = 0;
    //�@MP
    private int mp = 0;

    //�@�⏕�̑f����
    private int auxiliaryAgility = 0;
    //�@�⏕�̗�
    private int auxiliaryPower = 0;
    //�@�⏕�̑ł��ꋭ��
    private int auxiliaryStrikingStrength = 0;
    //�@Ⴢ��Ԃ�
    private bool isNumbness;
    //�@�ŏ�Ԃ�
    private bool isPoison;

    //�@���I�������X�L��
    private Skill currentSkill;
    //�@���̃^�[�Q�b�g
    private GameObject currentTarget;
    //�@���g�p�����A�C�e��
    private Item currentItem;
    //�@�^�[�Q�b�g��CharacterBattleScript
    private CharacterBattleScript targetCharacterBattleScript;
    //�@�^�[�Q�b�g��CharacterStatus
    private CharacterStatus targetCharacterStatus;
    //�@�U���I����̃A�j���[�V�������I���������ǂ���
    private bool isDoneAnimation;
    //�@�L�����N�^�[������ł��邩�ǂ���
    private bool isDead;

    //�@�U���̓A�b�v���Ă��邩�ǂ���
    private bool isIncreasePower;
    //�@�U���̓A�b�v���Ă���|�C���g
    private int increasePowerPoint;
    //�@�U���̓A�b�v���Ă���^�[��
    private int numOfTurnsIncreasePower = 3;
    //�@�U���̓A�b�v���Ă���̃^�[��
    private int numOfTurnsSinceIncreasePower = 0;
    //�@�h��̓A�b�v���Ă��邩�ǂ���
    private bool isIncreaseStrikingStrength;
    //�@�h��̓A�b�v���Ă���|�C���g
    private int increaseStrikingStrengthPoint;
    //�@�h��̓A�b�v���Ă���^�[��
    private int numOfTurnsIncreaseStrikingStrength = 3;
    //�@�h��̓A�b�v���Ă���̃^�[��
    private int numOfTurnsSinceIncreaseStrikingStrength = 0;

    //�@���ʃ|�C���g�\���X�N���v�g
    private EffectNumericalDisplayScript effectNumericalDisplayScript;

    
    private void Start()
    {
        animator = GetComponent<Animator>();
        //�@���f�[�^����ݒ�
        hp = characterStatus.GetHp();
        mp = characterStatus.GetMp();
        isNumbness = characterStatus.IsNumbnessState();
        isPoison = characterStatus.IsPoisonState();

        //�@��Ԃ̐ݒ�
        battleState = BattleState.Idle;
        //�@�R���|�[�l���g�̎擾
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        battleStatusScript = GameObject.Find("BattleUI/StatusPanel").GetComponent<BattleStatusScript>();
        effectNumericalDisplayScript = battleManager.GetComponent<EffectNumericalDisplayScript>();

        //�@���Ɏ���ł���ꍇ�͓|��Ă����Ԃɂ���
        if (characterStatus.GetHp() <= 0)
        {
            animator.CrossFade("Dead", 0f, 0, 1f);
            isDead = true;
        }
    }

    void Update()
    {
        //�@���Ɏ���ł����牽�����Ȃ�
        if (isDead)
        {
            return;
        }

        //�@�����̃^�[���łȂ���Ή������Ȃ�
        if (battleState == BattleState.Idle)
        {
            return;
        }
        //�@�A�j���[�V�������I����Ă��Ȃ���Ή������Ȃ�
        if (!isDoneAnimation)
        {
            return;
        }


        //�^�[��������Ă��Ă���L�����N�^�[�ɃJ������������
        //if (gameObject.name.Equals("���j�e�B�����"))
        //{
        //    battleManager.ActivateCamera(0);
        //}
        //else if (gameObject.name.Equals("�c�k�z�m"))
        //{
        //    battleManager.ActivateCamera(1);
        //}
        //else
        //{
        //    battleManager.ActivateCamera(-1);
        //}
        //�����Őݒ肷��̂͗]���낵���Ȃ��H



        //�@�I�������A�j���[�V�����ɂ���ď����𕪂���
        if (battleState == BattleState.DirectAttack)
        {
            ShowEffectOnTheTarget();
            DirectAttack();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        else if (battleState == BattleState.MagicAttack)
        {
            ShowEffectOnTheTarget();
            MagicAttack();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        else if (battleState == BattleState.Healing
          || battleState == BattleState.NumbnessRecoveryMagic
          || battleState == BattleState.PoisonnouRecoveryMagic
          )
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        else if (battleState == BattleState.IncreaseAttackPowerMagic)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@���g�̍U���͂��A�b�v�����ꍇ�̓^�[�������J�E���g���Ȃ�
            if (currentTarget == this.gameObject)
            {
                CheckIncreaseStrikingStrength();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseStrikingStrength();
            }
        }
        else if (battleState == BattleState.IncreaseDefencePowerMagic)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@���g�̖h��͂��A�b�v�����ꍇ�̓^�[�������J�E���g���Ȃ�
            if (currentTarget == this.gameObject)
            {
                CheckIncreaseAttackPower();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseStrikingStrength();
            }
        }
        else if (battleState == BattleState.UseHPRecoveryItem
          || battleState == BattleState.UseMPRecoveryItem
          || battleState == BattleState.UseNumbnessRecoveryItem
          || battleState == BattleState.UsePoisonRecoveryItem
          )
        {
            UseItem();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        //�@�^�[�Q�b�g�̃��Z�b�g
        currentTarget = null;
        currentSkill = null;
        currentItem = null;
        targetCharacterBattleScript = null;
        targetCharacterStatus = null;
        battleState = BattleState.Idle;
        //�@���g�̑I�����I�������玟�̃L�����N�^�[�ɂ���
        battleManager.ChangeNextChara();
        isDoneAnimation = false;
    }

    public CharacterStatus GetCharacterStatus()
    {
        return characterStatus;
    }

    public void SetHp(int hp)
    {
        this.hp = Mathf.Max(0, Mathf.Min(characterStatus.GetMaxHp(), hp));

        if (this.hp <= 0)
        {
            Dead();
        }
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetMp(int mp)
    {
        this.mp = Mathf.Max(0, Mathf.Min(characterStatus.GetMaxMp(), mp));
    }

    public int GetMp()
    {
        return mp;
    }

    public bool IsDoneAnimation()
    {
        return isDoneAnimation;
    }

    public int GetAuxiliaryAgility()
    {
        return auxiliaryAgility;
    }

    public int GetAuxiliaryPower()
    {
        return auxiliaryPower;
    }

    public int GetAuxiliaryStrikingStrength()
    {
        return auxiliaryStrikingStrength;
    }

    //�@�␳�̑f������ݒ�
    public void SetAuxiliaryAgility(int value)
    {
        auxiliaryAgility = value;
    }

    //�@�␳�̗͂�ݒ�
    public void SetAuxiliaryPower(int value)
    {
        auxiliaryPower = value;
    }

    //�@�␳�̑ł��ꋭ����ݒ�
    public void SetAuxiliaryStrikingStrength(int value)
    {
        auxiliaryStrikingStrength = value;
    }

    public bool IsNumbness()
    {
        return isNumbness;
    }

    public bool IsPoison()
    {
        return isPoison;
    }

    public void SetNumbness(bool isNumbness)
    {
        this.isNumbness = isNumbness;
    }

    public void SetPoison(bool isPoison)
    {
        this.isPoison = isPoison;
    }

    public bool IsIncreasePower()
    {
        return isIncreasePower;
    }

    public void SetIsIncreasePower(bool isIncreasePower)
    {
        this.isIncreasePower = isIncreasePower;
    }

    public bool IsIncreaseStrikingStrength()
    {
        return isIncreaseStrikingStrength;
    }

    public void SetIsIncreaseStrikingStrength(bool isIncreaseStrikingStrength)
    {
        this.isIncreaseStrikingStrength = isIncreaseStrikingStrength;
    }

    public void SetBattleState(BattleState state)
    {
        this.battleState = state;
    }

    public void SetIsDoneAnimation()
    {
        isDoneAnimation = true;
    }

    //�@�I��������I�񂾃��[�h�����s
    public void ChooseAttackOptions(BattleState selectOption, GameObject target, Skill skill = null, Item item = null)
    {

        if (characterStatus as AllyStatus != null)
        {
            transform.Find("Marker/Image2").gameObject.SetActive(false);
        }

        //�@�X�L����^�[�Q�b�g�̏����Z�b�g
        currentTarget = target;
        currentSkill = skill;
        targetCharacterBattleScript = target.GetComponent<CharacterBattleScript>();
        targetCharacterStatus = targetCharacterBattleScript.GetCharacterStatus();

        //�@�I�������L�����N�^�[�̏�Ԃ�ݒ�
        battleState = selectOption;


        if (selectOption == BattleState.DirectAttack)
        {
            battleManager.ActivateCamera(-1);

            animator.SetTrigger("DirectAttack");
            battleManager.ShowMessage(gameObject.name + "��" + currentTarget.name + "��" + currentSkill.GetKanjiName() + "���s�����B");
        }
        else if (selectOption == BattleState.MagicAttack
          || selectOption == BattleState.Healing
          || selectOption == BattleState.IncreaseAttackPowerMagic
          || selectOption == BattleState.IncreaseDefencePowerMagic
          || selectOption == BattleState.NumbnessRecoveryMagic
          || selectOption == BattleState.PoisonnouRecoveryMagic
          )
        {

            animator.SetTrigger("MagicAttack");
            battleManager.ShowMessage(gameObject.name + "��" + currentTarget.name + "��" + currentSkill.GetKanjiName() + "���g�����B");
            //�@���@�g�p�҂�MP�����炷
            SetMp(GetMp() - ((Magic)skill).GetAmountToUseMagicPoints());
            //�@�g�p�҂������L�����N�^�[�ł����StatusPanel�̍X�V
            if (GetCharacterStatus() as AllyStatus != null)
            {
                battleStatusScript.UpdateStatus(GetCharacterStatus(), BattleStatusScript.Status.MP, GetMp());
            }
            Instantiate(((Magic)skill).GetSkillUserEffect(), transform.position, ((Magic)skill).GetSkillUserEffect().transform.rotation);
        }
        else if (selectOption == BattleState.UseHPRecoveryItem
          || selectOption == BattleState.UseMPRecoveryItem
          || selectOption == BattleState.UseNumbnessRecoveryItem
          || selectOption == BattleState.UsePoisonRecoveryItem
          )
        {
            currentItem = item;
            animator.SetTrigger("UseItem");
            battleManager.ShowMessage(gameObject.name + "��" + currentTarget.name + "��" + item.GetKanjiName() + "���g�����B");
        }
    }

    //�@�^�[�Q�b�g�G�t�F�N�g�̕\��
    public void ShowEffectOnTheTarget()
    {
        Instantiate<GameObject>(currentSkill.GetSkillReceivingSideEffect(), currentTarget.transform.position, currentSkill.GetSkillReceivingSideEffect().transform.rotation);
    }

    public void DirectAttack()
    {
        var targetAnimator = currentTarget.GetComponent<Animator>();
        targetAnimator.SetTrigger("Damage");
        var damage = 0;

        //�@�U�������Status
        if (targetCharacterStatus as AllyStatus != null)
        {

            //�����ɃJ�����̐ݒ�������B�ŏI�I�ɂ�currentTarget.name�ŏꍇ����
            //�U���̑ΏۂƂȂ�L�����N�^�[�ɃJ������������
            battleManager.ActivateCamera(0);

            var castedTargetStatus = (AllyStatus)targetCharacterBattleScript.GetCharacterStatus();
            //�@�U������̒ʏ�̖h��́{����̃L�����̕⏕�l
            var targetDefencePower = castedTargetStatus.GetStrikingStrength() + (castedTargetStatus.GetEquipArmor()?.GetAmount() ?? 0) + targetCharacterBattleScript.GetAuxiliaryStrikingStrength();
            damage = Mathf.Max(0, (characterStatus.GetPower() + auxiliaryPower) - targetDefencePower);
            battleManager.ShowMessage(currentTarget.name + "��" + damage + "�̃_���[�W���󂯂��B");
            //�@����̃X�e�[�^�X��HP���Z�b�g
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
            //�@�X�e�[�^�XUI���X�V
            battleStatusScript.UpdateStatus(targetCharacterStatus, BattleStatusScript.Status.HP, targetCharacterBattleScript.GetHp());

        }
        else if (targetCharacterStatus as EnemyStatus != null)
        {
            var castedTargetStatus = (EnemyStatus)targetCharacterBattleScript.GetCharacterStatus();
            //�@�U������̒ʏ�̖h��́{����̃L�����̕⏕�l
            var targetDefencePower = castedTargetStatus.GetStrikingStrength() + targetCharacterBattleScript.GetAuxiliaryStrikingStrength();
            damage = Mathf.Max(0, (characterStatus.GetPower() + (((AllyStatus)characterStatus).GetEquipWeapon()?.GetAmount() ?? 0) + auxiliaryPower) - targetDefencePower);
            battleManager.ShowMessage(currentTarget.name + "��" + damage + "�̃_���[�W���󂯂��B");
            //�@�G�̃X�e�[�^�X��HP���Z�b�g
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
        }
        else
        {
            Debug.LogError("���ڍU���Ń^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
        }

        Debug.Log(gameObject.name + "��" + currentTarget.name + "��" + currentSkill.GetKanjiName() + "������" + damage + "��^�����B");
        effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, currentTarget.transform, damage);
    }

    public void MagicAttack()
    {
        var targetAnimator = currentTarget.GetComponent<Animator>();
        targetAnimator.SetTrigger("Damage");
        var damage = 0;

        //�@�U�������Status
        if (targetCharacterStatus as AllyStatus != null)
        {

            var castedTargetStatus = (AllyStatus)targetCharacterBattleScript.GetCharacterStatus();
            var targetDefencePower = castedTargetStatus.GetStrikingStrength() + (castedTargetStatus.GetEquipArmor()?.GetAmount() ?? 0);
            damage = Mathf.Max(0, ((Magic)currentSkill).GetMagicPower() - targetDefencePower);
            battleManager.ShowMessage(currentTarget.name + "��" + damage + "�̃_���[�W���󂯂��B");
            ////�@����̃X�e�[�^�X��HP���Z�b�g
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
            //�@�X�e�[�^�XUI���X�V
            battleStatusScript.UpdateStatus(targetCharacterStatus, BattleStatusScript.Status.HP, targetCharacterBattleScript.GetHp());
        }
        else if (targetCharacterStatus as EnemyStatus != null)
        {
            var castedTargetStatus = (EnemyStatus)targetCharacterBattleScript.GetCharacterStatus();
            var targetDefencePower = castedTargetStatus.GetStrikingStrength();
            damage = Mathf.Max(0, ((Magic)currentSkill).GetMagicPower() - targetDefencePower);
            battleManager.ShowMessage(currentTarget.name + "��" + damage + "�̃_���[�W���󂯂��B");
            //�@����̃X�e�[�^�X��HP���Z�b�g
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
        }
        else
        {
            Debug.LogError("���@�U���Ń^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
        }

        Debug.Log(gameObject.name + "��" + currentTarget.name + "��" + currentSkill.GetKanjiName() + "������" + damage + "��^�����B");
        effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, currentTarget.transform, damage);

    }

    public void UseMagic()
    {
        //�@�A�j���[�V������Ԃ�����ĂȂ������̂�Damage�ɂ���
        currentTarget.GetComponent<Animator>().SetTrigger("Damage");

        var magicType = ((Magic)currentSkill).GetSkillType();
        if (magicType == Skill.Type.RecoveryMagic)
        {
            var recoveryPoint = ((Magic)currentSkill).GetMagicPower() + characterStatus.GetMagicPower();
            if (targetCharacterStatus as AllyStatus != null)
            {
                targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() + recoveryPoint);
                battleStatusScript.UpdateStatus(targetCharacterStatus, BattleStatusScript.Status.HP, targetCharacterBattleScript.GetHp());
            }
            else
            {
                targetCharacterBattleScript.SetHp(GetHp() + recoveryPoint);
            }
            Debug.Log(gameObject.name + "��" + ((Magic)currentSkill).GetKanjiName() + "���g����" + currentTarget.name + "��" + recoveryPoint + "�񕜂����B");
            battleManager.ShowMessage(currentTarget.name + "��" + recoveryPoint + "�񕜂����B");
            effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Healing, currentTarget.transform, recoveryPoint);
        }
        else if (magicType == Skill.Type.IncreaseAttackPowerMagic)
        {
            increasePowerPoint = ((Magic)currentSkill).GetMagicPower() + characterStatus.GetMagicPower();
            targetCharacterBattleScript.SetAuxiliaryPower(targetCharacterBattleScript.GetAuxiliaryPower() + increasePowerPoint);
            targetCharacterBattleScript.SetIsIncreasePower(true);
            Debug.Log(gameObject.name + "��" + ((Magic)currentSkill).GetKanjiName() + "���g����" + currentTarget.name + "�̗͂�" + increasePowerPoint + "���₵���B");
            battleManager.ShowMessage(currentTarget.name + "�̗͂�" + increasePowerPoint + "���₵���B");
        }
        else if (magicType == Skill.Type.IncreaseDefencePowerMagic)
        {
            increaseStrikingStrengthPoint = ((Magic)currentSkill).GetMagicPower() + characterStatus.GetMagicPower();
            targetCharacterBattleScript.SetAuxiliaryStrikingStrength(targetCharacterBattleScript.GetAuxiliaryStrikingStrength() + increaseStrikingStrengthPoint);
            targetCharacterBattleScript.SetIsIncreaseStrikingStrength(true);
            Debug.Log(gameObject.name + "��" + ((Magic)currentSkill).GetKanjiName() + "���g����" + currentTarget.name + "�̑ł��ꋭ����" + increaseStrikingStrengthPoint + "���₵���B");
            battleManager.ShowMessage(currentTarget.name + "�̑ł��ꋭ����" + increaseStrikingStrengthPoint + "���₵���B");
        }
        else if (magicType == Skill.Type.NumbnessRecoveryMagic)
        {
            targetCharacterStatus.SetNumbness(false);
            Debug.Log(gameObject.name + "��" + ((Magic)currentSkill).GetKanjiName() + "���g����" + currentTarget.name + "��Ⴢ��������");
            battleManager.ShowMessage(currentTarget.name + "��Ⴢ��������");
        }
        else if (magicType == Skill.Type.PoisonnouRecoveryMagic)
        {
            targetCharacterStatus.SetPoisonState(false);
            Debug.Log(gameObject.name + "��" + ((Magic)currentSkill).GetKanjiName() + "���g����" + currentTarget.name + "�̓ł�������");
            battleManager.ShowMessage(currentTarget.name + "�̓ł�������");
        }
    }

    public void UseItem()
    {
        currentTarget.GetComponent<Animator>().SetTrigger("Damage");

        //�@�L�����N�^�[�̃A�C�e���������炷
        ((AllyStatus)characterStatus).SetItemNum(currentItem, ((AllyStatus)characterStatus).GetItemNum(currentItem) - 1);

        if (currentItem.GetItemType() == Item.Type.HPRecovery)
        {
            //�@�񕜗�
            var recoveryPoint = currentItem.GetAmount();
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() + recoveryPoint);
            battleStatusScript.UpdateStatus(targetCharacterStatus, BattleStatusScript.Status.HP, targetCharacterBattleScript.GetHp());
            Debug.Log(gameObject.name + "��" + currentItem.GetKanjiName() + "���g����" + currentTarget.name + "��HP��" + recoveryPoint + "�񕜂����B");
            battleManager.ShowMessage(currentTarget.name + "��HP��" + recoveryPoint + "�񕜂����B");
            effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Healing, currentTarget.transform, recoveryPoint);
        }
        else if (currentItem.GetItemType() == Item.Type.MPRecovery)
        {
            //�@�񕜗�
            var recoveryPoint = currentItem.GetAmount();
            targetCharacterBattleScript.SetMp(targetCharacterBattleScript.GetMp() + recoveryPoint);
            battleStatusScript.UpdateStatus(targetCharacterStatus, BattleStatusScript.Status.MP, targetCharacterBattleScript.GetMp());
            Debug.Log(gameObject.name + "��" + currentItem.GetKanjiName() + "���g����" + currentTarget.name + "��MP��" + recoveryPoint + "�񕜂����B");
            battleManager.ShowMessage(currentTarget.name + "��MP��" + recoveryPoint + "�񕜂����B");
            effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Healing, currentTarget.transform, recoveryPoint);
        }
        else if (currentItem.GetItemType() == Item.Type.NumbnessRecovery)
        {
            targetCharacterStatus.SetNumbness(false);
            Debug.Log(gameObject.name + "��" + currentItem.GetKanjiName() + "���g����" + currentTarget.name + "��Ⴢ���������B");
            battleManager.ShowMessage(currentTarget.name + "��Ⴢ���������B");
        }
        else if (currentItem.GetItemType() == Item.Type.PoisonRecovery)
        {
            targetCharacterStatus.SetPoisonState(false);
            Debug.Log(gameObject.name + "��" + currentItem.GetKanjiName() + "���g����" + currentTarget.name + "�̓ł��������B");
            battleManager.ShowMessage(currentTarget.name + "�̓ł��������B");
        }

        //�@�A�C�e������0�ɂȂ�����ItemDictionary���炻�̃A�C�e�����폜
        if (((AllyStatus)characterStatus).GetItemNum(currentItem) == 0)
        {
            ((AllyStatus)characterStatus).GetItemDictionary().Remove(currentItem);
        }
    }

    //�@�h��
    public void Guard()
    {
        if (characterStatus as AllyStatus != null)
        {
            transform.Find("Marker/Image2").gameObject.SetActive(false);

            battleManager.ActivateCamera(-1);
        }

        //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
        CheckIncreaseAttackPower();
        CheckIncreaseStrikingStrength();
        animator.SetBool("Guard", true);
        battleManager.ShowMessage(gameObject.name + "�͖h����s�����B");
        SetAuxiliaryStrikingStrength(GetAuxiliaryStrikingStrength() + 10);
    }
    //�@�h�������
    public void UnlockGuard()
    {
        animator.SetBool("Guard", false);
        SetAuxiliaryStrikingStrength(GetAuxiliaryStrikingStrength() - 10);
    }

    //�@���񂾂Ƃ��Ɏ��s���鏈��
    public void Dead()
    {
        animator.SetTrigger("Dead");
        Debug.Log(gameObject.name + "�͓|�ꂽ�B");
        battleManager.ShowMessage(gameObject.name + "�͓|�ꂽ�B");
        battleManager.DeleteAllCharacterInBattleList(this.gameObject);
        if (GetCharacterStatus() as AllyStatus != null)
        {
            battleStatusScript.UpdateStatus(GetCharacterStatus(), BattleStatusScript.Status.HP, GetHp());
            battleManager.DeleteAllyCharacterInBattleList(this.gameObject);
        }
        else if (GetCharacterStatus() as EnemyStatus != null)
        {
            battleManager.DeleteEnemyCharacterInBattleList(this.gameObject);
        }
        isDead = true;
    }

    public void CheckIncreaseAttackPower()
    {
        //�@�����̃^�[�����������ɉ��炩�̌��ʖ��@���g���Ă���^�[�����𑝂₷
        if (IsIncreasePower())
        {
            numOfTurnsSinceIncreasePower++;
            if (numOfTurnsSinceIncreasePower >= numOfTurnsIncreasePower)
            {
                numOfTurnsSinceIncreasePower = 0;
                SetAuxiliaryPower(GetAuxiliaryPower() - increasePowerPoint);
                SetIsIncreasePower(false);
                Debug.Log(gameObject.name + "�̍U���̓A�b�v�̌��ʂ�������");
                battleManager.ShowMessage(gameObject.name + "�̍U���̓A�b�v�̌��ʂ�������");
            }
        }
    }

    public void CheckIncreaseStrikingStrength()
    {
        if (IsIncreaseStrikingStrength())
        {
            numOfTurnsSinceIncreaseStrikingStrength++;
            if (numOfTurnsSinceIncreaseStrikingStrength >= numOfTurnsIncreaseStrikingStrength)
            {
                numOfTurnsSinceIncreaseStrikingStrength = 0;
                SetAuxiliaryStrikingStrength(GetAuxiliaryStrikingStrength() - increaseStrikingStrengthPoint);
                SetIsIncreaseStrikingStrength(false);
                Debug.Log(gameObject.name + "�̖h��̓A�b�v�̌��ʂ�������");
                battleManager.ShowMessage(gameObject.name + "�̖h��̓A�b�v�̌��ʂ�������");
            }
        }
    }
}