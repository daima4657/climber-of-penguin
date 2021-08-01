using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSlideable : Floor
{
    public float speed = 12f; //移動速度
    private bool directionX;

    private Vector2 oldPos = Vector2.zero;//少し前の速度(現在の速度を割り出す用)
    private Vector2 myVelocity = Vector2.zero;//現在の速度
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        directionX = true;
        oldPos = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        /*一定間隔で左右に往復する動き*/
        if (transform.position.x > 7.0f)
        {
            directionX = false;
            
        }
        else if(transform.position.x < -7.0f)
        {
            directionX = true;
        }
        float x = directionX ? 1.0f : -1.0f;
        HorizontalMove(x);
    }

    //現在の速度を変数に代入しておく
    private void FixedUpdate()
    {
        myVelocity = (rb.position - oldPos) / Time.deltaTime;
        oldPos = rb.position;
    }

    //水平移動
    private void HorizontalMove(float x)
    {
        //入力方向へ移動
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        //Wait -> Dash
        //anim.SetBool("dash", true);

    }

    //現在の速度を返す
    public Vector2 GetVelocity()
    {
        return myVelocity;
    }

}
