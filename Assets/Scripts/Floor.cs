using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStampable
{
    void Stamped();
}

public class Floor : MonoBehaviour , IStampable
{
    public Rigidbody2D rb;
    public  int lifespan = 300;
    public Animator anim = null;
    private bool fallFlag;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fallFlag = false;
        //Debug.Log(anim);
    }

    // Update is called once per frame
    void Update()
    {
        //最高到達点より一定以上y距離が離れるか出現から5秒以上経過で消去
        if (GManager.instance.progress - transform.position.y > 20.0f)
        {
            DestroySelf();
        }
    }
    void FixedUpdate()
    {
        if (fallFlag)
        {
            Vector2 myGravity = new Vector2(0, -9.81f * 2);
            rb.AddForce(myGravity);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Stamped()
    {
        //transform.localScale = new Vector3(1,1,0);
        // コルーチンの起動
        StartCoroutine(DelayCoroutine());
    }



    // コルーチン本体
    public IEnumerator DelayCoroutine()
    {

        yield return new WaitForSeconds(1);
        anim.SetInteger("BreakLevel", 1);
        yield return new WaitForSeconds(1);
        anim.SetInteger("BreakLevel", 2);
        yield return new WaitForSeconds(1);
        anim.SetInteger("BreakLevel", 3);
        fallFlag = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(2);
        // 3秒後に崩落
        Destroy(gameObject);

    }




}
