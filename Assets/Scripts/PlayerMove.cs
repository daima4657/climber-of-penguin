using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D ownRigidbody2D;
    private Player myself;
    bool push;
    bool boolLeft;
    bool boolRight;

    // Start is called before the first frame update
    void Start()
    {
        ownRigidbody2D = GetComponent<Rigidbody2D>();
        myself = gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        //�W�����v����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HoldStart();
            Jump();
        }

        //���쏈��
        if (myself.controlable)
        {


            if (Input.GetKeyUp(KeyCode.Space))
            {
                HoldEnd();
                
            }


            HoverStart();



                //���L�[: -1�A�E�L�[: 1
            float x = Input.GetAxisRaw("Horizontal");
            //�Ώۂ̃X�P�[���ɍ��킹��x�̒l�𒲐����Ă���

            if (!myself.touch || myself.isGrounded)
            {
                //�����E����͂�����
                if (x == 1 || boolRight)
                {
                    x = 1 * Mathf.Abs(transform.localScale.x);
                    // �E�L�[������
                    HorizontalMove(x);
                }
                else if (x == -1 || boolLeft)
                {
                    x = -1 * Mathf.Abs(transform.localScale.x);
                    HorizontalMove(x);
                }
                else
                {
                    //�����E�����͂��Ă��Ȃ�������
                    //���ړ��̑��x��0�ɂ��ăs�^�b�Ǝ~�܂�悤�ɂ���
                    if (myself.walkFlag)
                    {
                        ownRigidbody2D.velocity = new Vector2(0, ownRigidbody2D.velocity.y);
                        HorizontalMoveEnd();
                    }

                    //Dash��Wait
                    //anim.SetBool("Dash", false);
                }
            }
        }

        //�{�^������

        if (!this.push)
        {
            this.boolRight = false;
            this.boolLeft = false;
        }
    }



    //�@�W�����v�{�^�������� 
    public void JumpButtonDown()
    {
        HoldStart();
        Jump();

    }

    //�@�W�����v�{�^����������
    public void JumpButtonUp()
    {
        HoldEnd();
    }

    // ���ړ��{�^����������
    public void LButtonPushUp()
    {
        this.push = false;
    }

    // ���ړ��{�^��������
    public void LButtonPushDown()
    {
        this.boolLeft = true;
        this.push = true;
    }

    // �E�ړ��{�^����������
    public void RButtonPushUp()
    {
        this.push = false;
    }

    // �E�ړ��{�^��������
    public void RButtonPushDown()
    {
        this.boolRight = true;
        this.push = true;
    }

    //�����ړ�
    private void HorizontalMove(float x)
    {
        //�����t���O�𗧂Ă�
        if (!myself.walkFlag)
        {
            myself.walkFlag = true;
        }
        if (myself.controlable)
        {
            
            //���͕����ֈړ�
            ownRigidbody2D.velocity = new Vector2(x * myself.speed, ownRigidbody2D.velocity.y);
            //localScale.x��-1�ɂ���Ɖ摜�����]����
            Vector2 temp = transform.localScale;
            temp.x = x;
            transform.localScale = temp;
            //Wait -> Dash
            //anim.SetBool("dash", true);
            if (myself.isGrounded)
            {
                myself.stateController.Walk();
            }
        }

    }

    private void HorizontalMoveEnd()
    {
        myself.walkFlag = false;
        if (myself.isGrounded)
        {
            //myself.stateController.Default();
        }
            
    }

    public void Jump()
    {
        if (myself.isGrounded && myself.controlable)
        {
            
            this.ownRigidbody2D.velocity = Vector3.zero;
            this.ownRigidbody2D.AddForce(transform.up * myself.jumpForce / 50, ForceMode2D.Impulse);
            myself.stateController.Jump();

            //StateProcessor.State = PlayerAlive;
        }
    }

    public void HoldStart()
    {
        myself.jumpHoldFlag = true;
    }

    public void HoldEnd()
    {
        myself.jumpHoldFlag = false;
    }
    public void HoverStart()
    {
        if (Input.GetKey(KeyCode.Space) || myself.jumpHoldFlag)
        {
            if (ownRigidbody2D.velocity.y < 0.0f && ownRigidbody2D.velocity.y > -2.0f)
            {
                myself.stateController.Hover();
                ownRigidbody2D.velocity = new Vector2(0, 0);

                myself.hoverFlag = true;
            }

        }
        else
        {
            HoverEnd();
        }
    }

    public void HoverEnd()
    {
        myself.hoverFlag = false;
        //myself.stateController.Jump();
    }
}
