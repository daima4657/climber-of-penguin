using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSlideable : Floor
{
    public float speed = 12f; //�ړ����x
    private bool directionX;

    private Vector2 oldPos = Vector2.zero;//�����O�̑��x(���݂̑��x������o���p)
    private Vector2 myVelocity = Vector2.zero;//���݂̑��x
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
        
        
        /*���Ԋu�ō��E�ɉ������铮��*/
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

    //���݂̑��x��ϐ��ɑ�����Ă���
    private void FixedUpdate()
    {
        myVelocity = (rb.position - oldPos) / Time.deltaTime;
        oldPos = rb.position;
    }

    //�����ړ�
    private void HorizontalMove(float x)
    {
        //���͕����ֈړ�
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        //Wait -> Dash
        //anim.SetBool("dash", true);

    }

    //���݂̑��x��Ԃ�
    public Vector2 GetVelocity()
    {
        return myVelocity;
    }

}
