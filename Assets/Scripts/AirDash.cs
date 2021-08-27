using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDash : MonoBehaviour
{
    private Rigidbody2D ownRigidbody2D;
    private Player myself;
    //�X�e�[�g�Ǘ��p
    private PlayerStateController stateController;
    [SerializeField] private int airDashKeyDetect;
    private float airDashUnmakingTime;
    private string currenState;
    private float moveTime;//���ݎ��� ������Time.deltaTime������
    private float endTime;

    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<PlayerStateController>();
        ownRigidbody2D = GetComponent<Rigidbody2D>();
        myself = gameObject.GetComponent<Player>();
        airDashKeyDetect = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currenState = stateController.StateProcessor.State.getStateName();
        moveTime += Time.deltaTime;//�o�ߎ��Ԃ�ϐ��Ɋi�[
        //�󒆃_�b�V��
        if (myself.Controlable && !myself.isGrounded && myself.CanAirDash)
        {
            if((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                //�����E�̃L�[���͂����m���ă_�u���N���b�N�̓���1��ڂ��L�����Ă���
                float x = Input.GetAxisRaw("Horizontal");
                //GetAxisRaw�̐��l�𐮐��ɂ��Ă���
                int direction = 0;
                if (x > 0)
                {
                    direction = 1;
                }
                if (x < 0)
                {
                    direction = -1;
                }
                AirDashProcessor(direction);
            }
            if ((Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)))
            {
                //�_�u���N���b�N�����m�������̂ŁA�L�[����w��������^�C�~���O�ɂ��Ă��L�����Ă���
                int direction = 0;
                AirDashProcessor(direction);
            }


        }

        if (airDashKeyDetect != 0)
        {
            AirDashKeyDetectReset();
        }

    }
    void FixedUpdate()
    {
        if(myself.Controlable && !myself.isGrounded)
        {
            if (airDashKeyDetect == 10)
            {
                float x = Input.GetAxisRaw("Horizontal");
                if (x == 1)
                {
                    AirDush(1);
                }

            }
            if (airDashKeyDetect == -10)
            {
                float x = Input.GetAxisRaw("Horizontal");
                if (x == -1)
                {
                    AirDush(-1);
                }

            }
        }
        if (myself.airDashFlag)
        {
            if (endTime == 0)
            {
                endTime = moveTime + 0.35f;
            }
            if (ownRigidbody2D.velocity.y < 0 || ownRigidbody2D.velocity.y > 1)
            {
                ownRigidbody2D.velocity = new Vector2(ownRigidbody2D.velocity.x * 0.995f, 10f);
            }

            if (moveTime > endTime - 0.25f)
            {
                if (ownRigidbody2D.velocity.y < 0 || ownRigidbody2D.velocity.y > 5)
                {
                    ownRigidbody2D.velocity = new Vector2(ownRigidbody2D.velocity.x * 0.995f, 5f);
                }
                else
                {
                    ownRigidbody2D.velocity = new Vector2(ownRigidbody2D.velocity.x * 0.995f, 5f);
                }
            }

            //�󒆃_�b�V���I���
            if (moveTime > endTime)
            {
                AirDushEnd();
            }

        }
    }

    public void AirDush(float x)
    {
        if(myself.Controlable && !myself.isGrounded && myself.CanAirDash)
        {
            myself.Controlable = false;
            myself.CanAirDash = false;
            myself.hoverFlag = false;
            stateController.AirDash();
            airDashKeyDetect = 0;
            airDashUnmakingTime = 0;
            myself.airDashFlag = true;
            ownRigidbody2D.velocity = new Vector2(x * myself.speed * 1.2f, ownRigidbody2D.velocity.y);
            myself.anim.SetBool("airdash", true);
        }

    }
    public void AirDushEnd()
    {
        ownRigidbody2D.velocity = new Vector2(0, 0);
        stateController.Jump();
        myself.airDashFlag = false;
        myself.Controlable = true;
        endTime = 0;
        myself.anim.SetBool("airdash", false);
    }

    private void AirDashProcessor(int inputKeyDirection)
    {
        /*
         airDashKeyDetect = 0   : �L�[���͖���
         airDashKeyDetect = 1   : �E�����ւ̃L�[����1��
         airDashKeyDetect = -1  : �������ւ̃L�[����1��
         airDashKeyDetect = 10  : �E�����ւ̃L�[����1�� + �{�^���^�b�v������
         airDashKeyDetect = -10 : �������ւ̃L�[����1�� + �{�^���^�b�v������
         */
        if (airDashKeyDetect == 0)
        {
            //�����E����͂�����
            if (inputKeyDirection == 1)
            {
                airDashKeyDetect = 1;
            }
            else if (inputKeyDirection == -1)
            {
                airDashKeyDetect = -1;
            }
        }
        if (airDashKeyDetect == 1)
        {
            if (inputKeyDirection != 1)
            {
                airDashKeyDetect = 10;
            }
        }

        if (airDashKeyDetect == -1)
        {
            if (inputKeyDirection != -1)
            {
                airDashKeyDetect = -10;
            }
        }


    }

    private void AirDashKeyDetectReset()
    {
        
        airDashUnmakingTime += Time.deltaTime;
        if (airDashUnmakingTime > 0.2f)
        {
            airDashKeyDetect = 0;
            airDashUnmakingTime = 0;
        }
    }

    // ���ړ��{�^����������
    public void LButtonPushUp()
    {
        int direction = 0;
        AirDashProcessor(direction);
    }

    // ���ړ��{�^��������
    public void LButtonPushDown()
    {
        int direction = -1;
        AirDashProcessor(direction);
        if (airDashKeyDetect == -10)
        {
            AirDush(-1);
        }
    }

    // �E�ړ��{�^����������
    public void RButtonPushUp()
    {
        int direction = 0;
        AirDashProcessor(direction);
    }

    // �E�ړ��{�^��������
    public void RButtonPushDown()
    {
        int direction = 1;
        AirDashProcessor(direction);
        if (airDashKeyDetect == 10)
        {
            AirDush(1);
        }
    }

}
