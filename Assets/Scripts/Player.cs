using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using PlayerState;

public class Player : MonoBehaviour
{

    //変更前のステート名
    //private string _beforeStateName;
    //ステート
    /*public StateProcessor StateProcessor = new StateProcessor(); //プロセッサー
    public PlayerStateDefault StateDefault = new PlayerStateDefault();
    public PlayerAlive PlayerAlive = new PlayerAlive();
    public PlayerDamaged PlayerDamaged = new PlayerDamaged();*/




    private Rigidbody2D ownRigidbody2D;
    //上方向へ加える力
    public float jumpForce = 62.0f;
    public float speed = 4f; //歩くスピード
    public LayerMask groundLayer;//Linecastで判定する
    [SerializeField] public bool isGrounded;//着地判定

    [SerializeField] public bool controlable;//操作可能判定

    private BoxCollider2D myCollision;
    public bool walkFlag;//歩き状態
    public bool damagedFlag;//やられ状態
    public bool jumpHoldFlag;//ジャンプ状態
    public bool hoverFlag;//ホバリング状態

    public string CurrenState;

    //bool push;
    //bool boolLeft;
    //bool boolRight;
    public bool touch;

    //着地した床オブジェクトを入れる箱
    public GameObject _stampTarget;

    private Animator anim = null;

    //ステート管理用
    public PlayerStateController stateController;


    // Start is called before the first frame update
    void Start()
    {
        ownRigidbody2D = GetComponent<Rigidbody2D>();
        //this.push = false;
        //this.boolLeft = false;
        //this.boolRight = false;
        stateController = GetComponent<PlayerStateController>();
        //stateController.Default();
        controlable = true;
        damagedFlag = false;
        myCollision = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrenState = stateController.StateProcessor.State.getStateName();
        Debug.Log(stateController.StateProcessor.State.getStateName());
        //Debug.Log(transform.position.y);




        //ステートの値が変更されたら実行処理を行う
        /*if (StateProcessor.State == null)
        {
            return;
        }

        if (StateProcessor.State.getStateName() != _beforeStateName)
        {
            Debug.Log(" Now State:" + StateProcessor.State.getStateName());
            _beforeStateName = StateProcessor.State.getStateName();
            StateProcessor.Execute();
        }*/

        //Linecasetで足元に地面有るか判定
        /*isGrounded = Physics2D.Linecast(
            transform.position - transform.up * 1.2f,
            transform.position - transform.up * 1.6f,
            groundLayer);*/
        //BoxCastはその名の通りは箱(平面)で当たり判定が取れる優れモノだ。レイヤーマスクも設定できるぞ
        isGrounded = Physics2D.BoxCast(transform.position - transform.up * 0.8f, new Vector2(1, 1), 0, Vector2.zero, 0.0f, groundLayer);


        //ステートのコントロール
        if (damagedFlag)
        {
            stateController.Dead();
        }
        else
        {
            if (isGrounded)
            {
                stateController.Default();
            }
            if (!isGrounded)
            {
                if (CurrenState != "Hover")
                {

                    stateController.Jump();
                }
                //
            }
            if (!hoverFlag)
            {
                if (CurrenState == "Hover")
                {
                    stateController.Jump();
                }
            }
        }


        //アニメーションのコントロール
        if (CurrenState == "Hover")
        {
            anim.SetBool("hover", true);
        }
        else
        {
            anim.SetBool("hover", false);
        }

        //Debug.Log(transform.up);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float maxDistance = 10;


        //足場を踏んだ時の処理
        RaycastHit2D hit = Physics2D.BoxCast(transform.position - transform.up * 0.8f, new Vector2(1, 1), 0, Vector2.zero,0.0f, groundLayer);
        //Linecastを可視化
        //Debug.Log(isGrounded);
        //Debug.Log(hit.collider);
        //Debug.DrawLine(transform.position - transform.up * 1.2f, transform.position - transform.up * 1.6f, Color.red);
        if (hit.collider && isGrounded)


        {
            var stamptarget = hit.collider.gameObject.GetComponent<IStampable>();
            if(stamptarget != null)
            {

                //stateController.Default();
                hit.collider.gameObject.GetComponent<IStampable>().Stamped();
            }

            var movingFloor = hit.collider.gameObject.GetComponent<FloorSlideable>();
            if(movingFloor != null)
            {
                if (jumpHoldFlag)
                {
                }
                if (!jumpHoldFlag)//ジャンプボタン押下時は除く
                {
                    //動く足場の上に乗ってるときは速度を足場と一致させる
                    Vector2 addVelocity = Vector2.zero;
                    addVelocity = hit.collider.gameObject.GetComponent<FloorSlideable>().GetVelocity();
                    //各種座標軸の速度を求める
                    //ownRigidbody2D.velocity = new Vector2(1 * speed, ownRigidbody2D.velocity.y);
                    ownRigidbody2D.velocity = addVelocity;
                }

            }
        }

        //落下した場合の処理
        if(transform.position.y < GManager.instance.progress  - 20 || transform.position.y < -10)
        {
            
            GManager.instance.GameReset();
            //GManager.instance.SetProgress(0);

        }

        //最新の標高をGameManagerに記録する
        if(transform.position.y > GManager.instance.progress && transform.position.y - GManager.instance.progress < 5)
        {
            Debug.Log("プレイヤーのy位置は" + transform.position.y);
            GManager.instance.SetProgress(GManager.instance.progress + 5);
        }

        

        //やられ
        if (damagedFlag)
        {
            //stateController.Dead();
            ownRigidbody2D.velocity = new Vector2(0, ownRigidbody2D.velocity.y);
            myCollision.enabled = false;
            // x軸を軸にして毎秒2度、回転させるQuaternionを作成（変数をrotとする）
            Quaternion rot = Quaternion.AngleAxis(1, Vector3.forward);
            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        }

    }




    // 接地判定（壁の側面でジャンプさせないようにするため、OnTriggerに変更）
    private void OnTriggerEnter2D(Collider2D collision)
    // private void OnCollisionEnter2D ( Collision2D collision )
    {
        
        if (collision.CompareTag("Damageable"))
        {
            controlable = false;
            if (!damagedFlag)
            {
                //ふっとび
                ownRigidbody2D.AddForce(Vector2.up * jumpForce / 2);
            }
            damagedFlag = true;
        }
        touch = true;
        ownRigidbody2D.velocity = new Vector2(0, ownRigidbody2D.velocity.y);
        //isGrounded = true;

        if (collision.CompareTag("Goal"))
        {
            GManager.instance.TransitionToClearScene();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    // private void OnCollisionExit2D ( Collision2D collision )
    {
        touch = false;
        //isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        //設地判定用のGizmoを可視化する
        Gizmos.color = Color.red; //色指定
        Gizmos.DrawCube((transform.position - transform.up * 0.8f), new Vector2(1, 1)); //中心点とサイズ
    }


}
