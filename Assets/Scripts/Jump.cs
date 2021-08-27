using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D ownRigidbody2D;
    private Player myself;
    //ステート管理用
    private PlayerStateController stateController;
    private string currenState;
    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<PlayerStateController>();
        myself = gameObject.GetComponent<Player>();
        ownRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HoldStart();
            JumpStart();
        }

        currenState = stateController.StateProcessor.State.getStateName();
        if (myself.Controlable)
        {
            HoverStart();
        }
        //死んだらホバー解除
        if (myself.hoverFlag && myself.damagedFlag)
        {
            myself.hoverFlag = false;
        }
        //ジャンプボタン長押し解除
        if (Input.GetKeyUp(KeyCode.Space))
        {
            HoldEnd();

        }



        if ((myself.isGrounded == true) && (Input.GetKeyDown(KeyCode.Space) == true))
        {
            //ownRigidbody2D.velocity = (new Vector2(ownRigidbody2D.velocity.x, myself.jumpForce / 43));
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            
            /*if (ownRigidbody2D.velocity.y > 0.0f)
            {
                if (ownRigidbody2D.velocity.y > -10)
                {
                    ownRigidbody2D.velocity = (new Vector2(ownRigidbody2D.velocity.x, ownRigidbody2D.velocity.y - 10.0f));
                }
                
            }*/
        }


    }
    void FixedUpdate()
    {
        if (myself.hoverFlag)
        {
            HoverYVelocityControl();
        } else
        {
            ownRigidbody2D.gravityScale = 5;
        }

    }

    //　ジャンプボタン押下時 
    public void JumpButtonDown()
    {
        HoldStart();
        JumpStart();

    }

    //　ジャンプボタン離した時
    public void JumpButtonUp()
    {
        HoldEnd();
    }


    public void JumpStart()
    {

        if (myself.isGrounded && myself.Controlable)
        {

            //ownRigidbody2D.velocity = Vector3.zero;
            ownRigidbody2D.velocity = (new Vector2(ownRigidbody2D.velocity.x, myself.jumpForce / 43));
            //ownRigidbody2D.AddForce(transform.up * myself.jumpForce / 43, ForceMode2D.Impulse);
            stateController.Jump();

            //StateProcessor.State = PlayerAlive;
        }
    }

    public void HoldStart()
    {
        myself.jumpHoldFlag = true;

    }

    public void HoldEnd()
    {
        if (ownRigidbody2D.velocity.y > 0.0f)
        {
            if (ownRigidbody2D.velocity.y > -10)
            {
                ownRigidbody2D.velocity = (new Vector2(ownRigidbody2D.velocity.x, ownRigidbody2D.velocity.y - 10.0f));
            }

        }
        myself.jumpHoldFlag = false;
    }
    public void HoverStart()
    {
        if (myself.jumpHoldFlag)
        {
            if (ownRigidbody2D.velocity.y < 0.0f && ownRigidbody2D.velocity.y > -1.5f)
            {
                myself.hoverFlag = true;
                if (myself.Controlable && !myself.isGrounded)
                {
                    stateController.Hover();
                    myself.anim.SetBool("hover", true);
                }
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
        if (currenState == "Hover")
        {
            stateController.Jump();
            myself.anim.SetBool("hover", false);
        }

    }

    public void HoverYVelocityControl()
    {
        ownRigidbody2D.gravityScale = 0.1f;
    }
}
