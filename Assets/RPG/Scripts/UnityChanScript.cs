using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanScript : MonoBehaviour
{

    public enum State
    {
        Normal,
        Talk,
        Command,
        Wait
    }

    private CharacterController characterController;
    private Animator animator;
    //�@�L�����N�^�[�̑��x
    private Vector3 velocity;
    //�@�L�����N�^�[�̕����X�s�[�h
    [SerializeField]
    private float walkSpeed = 2f;
    //�@�L�����N�^�[�̑���X�s�[�h
    [SerializeField]
    private float runSpeed = 4f;

    //�@���j�e�B�����̏��
    private State state;
    //�@���j�e�B������b�����X�N���v�g
    private UnityChanTalkScript unityChanTalkScript;


    //�@��ԕύX�Ə����ݒ�
    public void SetState(State state)
    {
        this.state = state;

        if (state == State.Talk)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            unityChanTalkScript.StartTalking();
        }
        else if (state == State.Command)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        else if (state == State.Wait)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
    }


    public State GetState()
    {
        return state;
    }


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        state = State.Wait;
        unityChanTalkScript = GetComponent<UnityChanTalkScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal)
        {
            if (characterController.isGrounded)
            {
                velocity = Vector3.zero;

                var input = new Vector3(-Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

                if (input.magnitude > 0.1f)
                {
                    transform.LookAt(transform.position + input.normalized);
                    //�@�������������J�����̌����ɍ��킹�ĕϊ�
                    var convertInputToCameraDirection = Quaternion.FromToRotation(input, new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z));
                    //�@���j�e�B�����̊p�x���J�����̕����ɍ��킹���p�x��Y�l��������]������
                    transform.rotation = Quaternion.AngleAxis(convertInputToCameraDirection.eulerAngles.y, Vector3.up);

                    animator.SetFloat("Speed", input.magnitude);
                    if (input.magnitude > 0.5f)
                    {
                        velocity += transform.forward * runSpeed;
                    }
                    else
                    {
                        velocity += transform.forward * walkSpeed;
                    }
                }
                else
                {
                    animator.SetFloat("Speed", 0f);

                }

                if (unityChanTalkScript.GetConversationPartner() != null
                    && Input.GetButtonDown("Jump")
                    )
                {
                    SetState(State.Talk);
                }
            }
        }
        else if (state == State.Talk){
        }
        else if (state == State.Command)
        {
        }
        else if (state == State.Wait)
        {
            if (unityChanTalkScript.GetConversationPartner() != null && Input.GetButtonDown("Jump"))
            {
                SetState(State.Talk);
            }
        }

        //�@�V�[���J�ڌ�Ɉړ�������ƃf�t�H���g�̈ʒu�ɃL�����N�^�[���Z�b�g����Ă��܂��̂ŉ��
        if (state != State.Wait)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            if (!Mathf.Approximately(Input.GetAxis("Horizontal"), 0f) || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f))
            {
                SetState(State.Normal);
            }
        }
    }
}