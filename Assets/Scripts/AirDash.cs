using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDash : MonoBehaviour
{
    private Rigidbody2D ownRigidbody2D;
    private Player myself;
    //ステート管理用
    private PlayerStateController stateController;
    [SerializeField] private int airDashKeyDetect;
    private float airDashUnmakingTime;
    private string currenState;
    private float moveTime;//現在時間 ここはTime.deltaTimeが入る
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
        moveTime += Time.deltaTime;//経過時間を変数に格納
        //空中ダッシュ
        if (myself.Controlable && !myself.isGrounded && myself.CanAirDash)
        {
            if((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                //左か右のキー入力を感知してダブルクリックの内の1回目を記憶しておく
                float x = Input.GetAxisRaw("Horizontal");
                //GetAxisRawの数値を整数にしておく
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
                //ダブルクリックを感知したいので、キーから指を放したタイミングについても記憶しておく
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

            //空中ダッシュ終わり
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
         airDashKeyDetect = 0   : キー入力無し
         airDashKeyDetect = 1   : 右方向へのキー入力1回
         airDashKeyDetect = -1  : 左方向へのキー入力1回
         airDashKeyDetect = 10  : 右方向へのキー入力1回 + ボタンタップ解除済
         airDashKeyDetect = -10 : 左方向へのキー入力1回 + ボタンタップ解除済
         */
        if (airDashKeyDetect == 0)
        {
            //左か右を入力したら
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

    // 左移動ボタン離した時
    public void LButtonPushUp()
    {
        int direction = 0;
        AirDashProcessor(direction);
    }

    // 左移動ボタン押下時
    public void LButtonPushDown()
    {
        int direction = -1;
        AirDashProcessor(direction);
        if (airDashKeyDetect == -10)
        {
            AirDush(-1);
        }
    }

    // 右移動ボタン離した時
    public void RButtonPushUp()
    {
        int direction = 0;
        AirDashProcessor(direction);
    }

    // 右移動ボタン押下時
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
