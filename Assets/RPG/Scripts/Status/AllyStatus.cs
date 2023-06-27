using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "AllyStatus", menuName = "CreateAllyStatus")]
public class AllyStatus : CharacterStatus
{

    //�@�l���o���l
    [SerializeField]
    private int earnedExperience = 0;
    //�@�������Ă��镐��
    [SerializeField]
    private Item equipWeapon = null;
    //�@�������Ă���Z
    [SerializeField]
    private Item equipArmor = null;
    //�@�A�C�e���ƌ���Dictionary
    [SerializeField]
    private ItemDictionary itemDictionary = null;

    //�@���x���A�b�v�f�[�^
    [SerializeField]
    private LevelUpData levelUpData = null;

    public void SetEarnedExperience(int earnedExperience)
    {
        this.earnedExperience = earnedExperience;
    }

    public int GetEarnedExperience()
    {
        return earnedExperience;
    }

    public void SetEquipWeapon(Item weaponItem)
    {
        this.equipWeapon = weaponItem;
    }

    public Item GetEquipWeapon()
    {
        return equipWeapon;
    }

    public void SetEquipArmor(Item armorItem)
    {
        this.equipArmor = armorItem;
    }

    public Item GetEquipArmor()
    {
        return equipArmor;
    }

    public void CreateItemDictionary(ItemDictionary itemDictionary)
    {
        this.itemDictionary = itemDictionary;
    }

    public void SetItemDictionary(Item item, int num = 0)
    {
        itemDictionary.Add(item, num);
    }

    //�@�A�C�e�����o�^���ꂽ���Ԃ�ItemDictionary��Ԃ�
    public ItemDictionary GetItemDictionary()
    {
        return itemDictionary;
    }
    //�@�������̖��O�Ń\�[�g����ItemDictionary��Ԃ�
    public IOrderedEnumerable<KeyValuePair<Item, int>> GetSortItemDictionary()
    {
        return itemDictionary.OrderBy(item => item.Key.GetHiraganaName());
    }
    public int SetItemNum(Item tempItem, int num)
    {
        return itemDictionary[tempItem] = num;
    }
    //�@�A�C�e���̐���Ԃ�
    public int GetItemNum(Item item)
    {
        return itemDictionary[item];
    }

    //�@���x���A�b�v�f�[�^��Ԃ�
    public LevelUpData GetLevelUpData()
    {
        return levelUpData;
    }
}