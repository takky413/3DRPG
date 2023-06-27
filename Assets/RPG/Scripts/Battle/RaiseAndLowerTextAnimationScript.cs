using UnityEngine;
using System.Collections;
using TMPro;

public class RaiseAndLowerTextAnimationScript : MonoBehaviour
{

    //�@�ړI��Y�̒l
    [SerializeField]
    private float destinationYValue = 0.5f;
    //�@�����̈ړ��X�s�[�h
    [SerializeField]
    private float charSpeed = 5f;

    private TMP_Text m_TextComponent;
    private bool hasTextChanged;

    private VertexAnim[] vertexAnims;
    private TMP_TextInfo textInfo;
    private CharacterAnim[] characterAnims;
    private TMP_MeshInfo[] cachedMeshInfo;
    private TMP_CharacterInfo charInfo;

    //�@���_�p�\����
    private struct VertexAnim
    {
        public float yValue;
    }
    //�@�������̍\����
    private struct CharacterAnim
    {
        public bool isAnimationStart;
        public bool isAnimationEnd;
        public bool isFlap;
    }

    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void Start()
    {
        //�@�Ȃ��ƃG���[�ɂȂ�
        m_TextComponent.ForceMeshUpdate();

        textInfo = m_TextComponent.textInfo;

        hasTextChanged = true;

        //�@�����Ɏg���Ă��钸�_�p�f�[�^�̍쐬
        vertexAnims = new VertexAnim[textInfo.characterCount * 4];
        //�@�������̃f�[�^�쐬
        characterAnims = new CharacterAnim[textInfo.characterCount];

        //�@�ŏ��̕����̃A�j���[�V�������X�^�[�g������
        characterAnims[0].isAnimationStart = true;

        // Cache the vertex data of the text object as the Jitter FX is applied to the original position of the characters.
        cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

        // Get new copy of vertex data if the text has changed.
        if (hasTextChanged)
        {
            // Update the copy of the vertex data for the text object.
            cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

            hasTextChanged = false;
        }

    }

    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == m_TextComponent)
            hasTextChanged = true;
    }

    private void Update()
    {

        m_TextComponent.ForceMeshUpdate();

        Matrix4x4 matrix;

        int characterCount = textInfo.characterCount;

        // If No Characters then just yield and wait for some text to be added
        if (characterCount == 0)
        {
            return;
        }

        //�@�g�p����ϐ��錾
        Vector3[] charOffset = new Vector3[characterCount];
        int materialIndex;
        int vertexIndex;
        Vector3[] sourceVertices;
        Vector2 charMidBasline;
        Vector3 offset;
        Vector3[] destinationVertices;


        for (int i = 0; i < characterCount; i++)
        {
            //�@���̕����̃A�j���[�V�������X�^�[�g���ĂȂ���Ύ��̕���
            if (!characterAnims[i].isAnimationStart)
            {
                continue;
            }
            //�@���̕����̃A�j���[�V�������I����Ă���Ύ��̕���
            if (characterAnims[i].isAnimationEnd)
            {
                continue;
            }

            charInfo = textInfo.characterInfo[i];

            // �����������Ȃ���΁i�󔒕����j���̕������A�j���[�V����������
            if (!charInfo.isVisible)
            {
                characterAnims[i].isAnimationStart = false;
                characterAnims[i].isAnimationEnd = true;
                //�@�Ō�̕����܂Ői��ł��Ȃ���Ύ��̕������X�^�[�g������
                if (i < characterCount - 1)
                {
                    characterAnims[i + 1].isAnimationStart = true;
                }
                else
                {
                    //�@�Ō�̕������I����Ă���΃Q�[���I�u�W�F�N�g��1�b��ɍ폜
                    Destroy(this.gameObject, 1f);
                }

                continue;
            }

            // Get the index of the material used by the current character.
            materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the index of the first vertex used by this text element.
            vertexIndex = textInfo.characterInfo[i].vertexIndex;

            // Get the cached vertices of the mesh used by this text element (character or sprite).
            sourceVertices = cachedMeshInfo[materialIndex].vertices;

            // Determine the center point of each character at the baseline.
            //charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, charInfo.baseLine);
            // Determine the center point of each character.
            charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

            // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
            // This is needed so the matrix TRS is applied at the origin for each character.
            offset = charMidBasline;

            destinationVertices = textInfo.meshInfo[materialIndex].vertices;

            destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
            destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
            destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
            destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

            //�@��������܂ōs���Ă��Ȃ����͏�Ɉړ�
            if (!characterAnims[i].isFlap)
            {
                charOffset[i] = new Vector3(0f, Mathf.MoveTowards(vertexAnims[i].yValue, destinationYValue, charSpeed * Time.deltaTime), 0f);
                vertexAnims[i].yValue = charOffset[i].y;
                Debug.Log(vertexAnims[i].yValue);
                //�@�ړI�n���z�����甽�]
                if (charOffset[i].y >= destinationYValue)
                {
                    characterAnims[i].isFlap = true;
                }
            }
            else
            {
                charOffset[i] = new Vector3(0f, Mathf.MoveTowards(vertexAnims[i].yValue, 0f, charSpeed * Time.deltaTime), 0f);
                vertexAnims[i].yValue = charOffset[i].y;
                //�@���������̈ʒu�ɖ߂����玟�̕������X�^�[�g
                if (vertexAnims[i].yValue <= 0f)
                {
                    characterAnims[i].isAnimationStart = false;
                    characterAnims[i].isAnimationEnd = true;
                    //�@�Ō�̕����łȂ���Ύ��̕������X�^�[�g
                    if (i < characterCount - 1)
                    {
                        characterAnims[i + 1].isAnimationStart = true;
                    }
                    else
                    {
                        //�@�Ō�̕����ł���΃Q�[���I�u�W�F�N�g��1�b��ɍ폜
                        Destroy(this.gameObject, 1f);
                    }
                }
            }
            //�@�ʒu�A��]�A�X�P�[������s����쐬
            matrix = Matrix4x4.TRS(charOffset[i], Quaternion.identity, Vector3.one);
            //�@�����ŗ^�������_��matrix�ϊ��s��ɏ]���ĕϊ�
            destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
            destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
            destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
            destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

            destinationVertices[vertexIndex + 0] += offset;
            destinationVertices[vertexIndex + 1] += offset;
            destinationVertices[vertexIndex + 2] += offset;
            destinationVertices[vertexIndex + 3] += offset;

        }

        // Push changes into meshes
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
