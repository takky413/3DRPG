using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectNumericalDisplayScript : MonoBehaviour
{
    public enum NumberType
    {
        Damage,
        Healing
    }

    //�@�_���[�W�|�C���g�\���p�v���n�u
    [SerializeField]
    private GameObject damagePointText;
    //�@�񕜃|�C���g�\���p�v���n�u
    [SerializeField]
    private GameObject healingPointText;
    //�@�|�C���g�̕\���I�t�Z�b�g�l
    [SerializeField]
    private Vector3 offset = new Vector3(0f, 0.8f, -0.5f);

    public void InstantiatePointText(NumberType numberType, Transform target, int point)
    {
        var rot = Quaternion.LookRotation(target.position - Camera.main.transform.position);
        if (numberType == NumberType.Damage)
        {
            var pointTextIns = Instantiate<GameObject>(damagePointText, target.position + offset, rot);
            pointTextIns.GetComponent<TextMeshPro>().text = point.ToString();
            Destroy(pointTextIns, 3f);
        }
        else if (numberType == NumberType.Healing)
        {
            var pointTextIns = Instantiate<GameObject>(healingPointText, target.position + offset, rot);
            pointTextIns.GetComponent<TextMeshPro>().text = point.ToString();
            Destroy(pointTextIns, 3f);
        }
    }
}