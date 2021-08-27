using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D ownRigidbody2D;
    private Player myself;


    private string currenState;

    private float moveTime;//現在時間 ここはTime.deltaTimeが入る
    private float endTime;

    //ステート管理用
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
        moveTime += Time.deltaTime;//経過時間を変数に格納
        

        //操作処理
        if (myself.Controlable)
        {

            float x = Input.GetAxisRaw("Horizontal");
            if(x != 0)
            {
                int direction = x < 0 ? -1 : 1;
                //左キー: -1、右キー: 1
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

    // 左移動ボタン離した時
    public void LButtonPushUp()
    {
        Debug.Log("左移動ボタン離した時");
        HorizontalMoveEnd();
    }

    // 左移動ボタン押下時
    public void LButtonPushDown()
    {
        HorizontalMove(-1);
    }

    // 右移動ボタン離した時
    public void RButtonPushUp()
    {
        Debug.Log("右移動ボタン離した時");
        HorizontalMoveEnd();
    }

    // 右移動ボタン押下時
    public void RButtonPushDown()
    {
        
        HorizontalMove(1);
    }








    //水平移動
    private void HorizontalMove(int direction)
    {
        myself.Direction = direction;

        if (myself.Controlable)
        {
            myself.anim.SetBool("walk", true);
            //歩きフラグを立てる
            if (!myself.walkFlag)
            {
                myself.walkFlag = true;
            }
            //localScale.xを-1にすると画像が反転する
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
        
        //入力方向へ移動
        if (myself.Controlable)
        {
            
            
            ownRigidbody2D.velocity = new Vector2(myself.Direction * myself.speed * 0.5f, ownRigidbody2D.velocity.y);
        }

    }

}
