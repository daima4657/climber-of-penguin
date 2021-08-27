using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI用に宣言

public class VariableScore : MonoBehaviour
{
    private Text myText;
    //　フェードアウトするスピード
    private float fadeOutSpeed = 2f;
    //　移動値
    [SerializeField]
    private float moveSpeed = 0.4f;
    private bool displayFlag;


    // Start is called before the first frame update
    void Start()
    {
        displayFlag = false;
        myText = GetComponent<Text>();
        myText.color = new Color(0f, 1f, 0f, 0f);
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        //transform.position += Vector3.up * (moveSpeed * 4) * Time.deltaTime;

        if (displayFlag)
        {
            myText.color = Color.Lerp(myText.color, new Color(0f, 1f, 0f, 0f), fadeOutSpeed * Time.deltaTime);
        }
        

        if (myText.color.a <= 0.1f)
        {
            myText.color = new Color(0f, 1f, 0f, 0f);
            displayFlag = false;
            //Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreDisplay(int num)
    {
        displayFlag = true;
        myText.color = new Color(0f,1f,0f,1f);
        myText.text = "+" + num.ToString();
    }
}
