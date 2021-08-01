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

        //ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HoldStart();
            Jump();
        }

        //操作処理
        if (myself.controlable)
        {


            if (Input.GetKeyUp(KeyCode.Space))
            {
                HoldEnd();
                
            }


            HoverStart();



                //左キー: -1、右キー: 1
            float x = Input.GetAxisRaw("Horizontal");
            //対象のスケールに合わせてxの値を調整しておく

            if (!myself.touch || myself.isGrounded)
            {
                //左か右を入力したら
                if (x == 1 || boolRight)
                {
                    x = 1 * Mathf.Abs(transform.localScale.x);
                    // 右キー押下時
                    HorizontalMove(x);
                }
                else if (x == -1 || boolLeft)
                {
                    x = -1 * Mathf.Abs(transform.localScale.x);
                    HorizontalMove(x);
                }
                else
                {
                    //左も右も入力していなかったら
                    //横移動の速度を0にしてピタッと止まるようにする
                    if (myself.walkFlag)
                    {
                        ownRigidbody2D.velocity = new Vector2(0, ownRigidbody2D.velocity.y);
                        HorizontalMoveEnd();
                    }

                    //Dash→Wait
                    //anim.SetBool("Dash", false);
                }
            }
        }

        //ボタン処理

        if (!this.push)
        {
            this.boolRight = false;
            this.boolLeft = false;
        }
    }



    //　ジャンプボタン押下時 
    public void JumpButtonDown()
    {
        HoldStart();
        Jump();

    }

    //　ジャンプボタン離した時
    public void JumpButtonUp()
    {
        HoldEnd();
    }

    // 左移動ボタン離した時
    public void LButtonPushUp()
    {
        this.push = false;
    }

    // 左移動ボタン押下時
    public void LButtonPushDown()
    {
        this.boolLeft = true;
        this.push = true;
    }

    // 右移動ボタン離した時
    public void RButtonPushUp()
    {
        this.push = false;
    }

    // 右移動ボタン押下時
    public void RButtonPushDown()
    {
        this.boolRight = true;
        this.push = true;
    }

    //水平移動
    private void HorizontalMove(float x)
    {
        //歩きフラグを立てる
        if (!myself.walkFlag)
        {
            myself.walkFlag = true;
        }
        if (myself.controlable)
        {
            
            //入力方向へ移動
            ownRigidbody2D.velocity = new Vector2(x * myself.speed, ownRigidbody2D.velocity.y);
            //localScale.xを-1にすると画像が反転する
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
