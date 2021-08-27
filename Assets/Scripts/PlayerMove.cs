using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D ownRigidbody2D;
    private Player myself;


    private string currenState;

    private float moveTime;//���ݎ��� ������Time.deltaTime������
    private float endTime;

    //�X�e�[�g�Ǘ��p
    private PlayerStateController stateController;

    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<PlayerStateController>();
        myself = gameObject.GetComponent<Player>();
        ownRigidbody2D = GetComponent<Rigidbody2D>();
        endTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currenState = stateController.StateProcessor.State.getStateName();
        moveTime += Time.deltaTime;//�o�ߎ��Ԃ�ϐ��Ɋi�[
        

        //���쏈��
        if (myself.Controlable)
        {

            float x = Input.GetAxisRaw("Horizontal");
            if(x != 0)
            {
                int direction = x < 0 ? -1 : 1;
                //���L�[: -1�A�E�L�[: 1
                HorizontalMove(direction);
            }
            
        }
        if (myself.walkFlag && (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)))
        {
            HorizontalMoveEnd();
        }

    }



    void FixedUpdate()
    {
        if (myself.walkFlag)
        {
            WalkVelocityControl();
        }

    }

    // ���ړ��{�^����������
    public void LButtonPushUp()
    {
        Debug.Log("���ړ��{�^����������");
        HorizontalMoveEnd();
    }

    // ���ړ��{�^��������
    public void LButtonPushDown()
    {
        HorizontalMove(-1);
    }

    // �E�ړ��{�^����������
    public void RButtonPushUp()
    {
        Debug.Log("�E�ړ��{�^����������");
        HorizontalMoveEnd();
    }

    // �E�ړ��{�^��������
    public void RButtonPushDown()
    {
        
        HorizontalMove(1);
    }








    //�����ړ�
    private void HorizontalMove(int direction)
    {
        myself.Direction = direction;

        if (myself.Controlable)
        {
            myself.anim.SetBool("walk", true);
            //�����t���O�𗧂Ă�
            if (!myself.walkFlag)
            {
                myself.walkFlag = true;
            }
            //localScale.x��-1�ɂ���Ɖ摜�����]����
            Vector2 temp = transform.localScale;
            temp.x = myself.Direction*Mathf.Abs(transform.localScale.x);
            transform.localScale = temp;
            if (currenState == "Default")
            {
                stateController.Walk();
            }
        }

    }

    public void HorizontalMoveEnd()
    {
        
        if (myself.walkFlag)
        {
            myself.walkFlag = false;
            
            
        }
        if (!myself.anim.GetCurrentAnimatorStateInfo(0).IsName("airdash"))
        {
            ownRigidbody2D.velocity = new Vector2(0, ownRigidbody2D.velocity.y);
        }
        myself.anim.SetBool("walk", false);
        if (currenState == "Walk")
        {
            stateController.Default();
        }
    }


    public void WalkVelocityControl()
    {
        
        //���͕����ֈړ�
        if (myself.Controlable)
        {
            
            
            ownRigidbody2D.velocity = new Vector2(myself.Direction * myself.speed * 0.5f, ownRigidbody2D.velocity.y);
        }

    }

}
