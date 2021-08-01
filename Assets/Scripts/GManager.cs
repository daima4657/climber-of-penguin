using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI�p�ɐ錾
using UnityEngine.SceneManagement;
public class GManager : MonoBehaviour
{

    public float progress;
    public float record;
    public static GManager instance = null;
    private Text scoreText;

    private float floorPositionX;
    public GameObject goalObj;
    public GameObject floorObj;
    public GameObject floorObjMoving;
    public GameObject floorObjExit;
    public GameObject floorObjExitPath;

    private float rockPositionX = 0;
    private float rockStayTime = 0;
    public GameObject smallRock;
    public GameObject bigRock;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        progress = 0;
        scoreText = GameObject.Find("Score").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(progress);
    }

    public void SetProgress(float num)
    {
        progress = num;
        floorPositionX = Random.Range(-7.0f, 7.0f);
        if ((progress + 10) % 50 == 0)//EXIT����𐶐�
        {
            GameObject obj = Instantiate(floorObjExit, new Vector3(7.0f, progress + 5.0f, 0), Quaternion.identity);
            GameObject obj2 = Instantiate(goalObj, new Vector3(9.46f, progress + 6.63f, 0), Quaternion.identity);
            GameObject exitPath = Instantiate(floorObjExitPath, new Vector3(12.12f, progress + 5.0f, 0), Quaternion.identity);
        }
        else if ((progress + 5) % 50 == 0)//EXIT�̎��̑���
        {
            floorPositionX = Random.Range(-7.0f, 0f);//�l�݂��������Ȃ��悤��
            GameObject obj = Instantiate(floorObj, new Vector3(floorPositionX, progress + 5.0f, 0), Quaternion.identity);
        }
        else if (progress % 25 == 0)//�������𐶐�
        {
            GameObject obj = Instantiate(floorObjMoving, new Vector3(floorPositionX, progress + 5.0f, 0), Quaternion.identity);
        } else//�ʏ�̏��𐶐�
        {
            Debug.Log("�ʏ�̏��𐶐� progress��"+ progress);
            GameObject obj = Instantiate(floorObj, new Vector3(floorPositionX, progress + 5.0f, 0), Quaternion.identity);
        }

        if(progress > this.GetComponent<GenerateBG>().lastGeneretePositionY - 40)
        {
            this.GetComponent<GenerateBG>().Generate();
        }

        //�X�R�A���X�V
        record = progress / 5;
        scoreText.text = record.ToString()+"m";
        //�X�𗎂Ƃ�
        //GenerateRock();

    }

    //�Q�[�����Z�b�g
    public void GameReset()
    {
        //this.GetComponent<RockFall>().ChangeActivation(true);
        SetProgress(0);
        this.GetComponent<GenerateBG>().Reset();
        SceneManager.LoadScene("GameScene");
    }

    //�N���A��ʂւ̑J��
    public void TransitionToClearScene()
    {
        //this.GetComponent<RockFall>().ChangeActivation(false);
        SceneManager.LoadScene("Clear");
    }

    /*public void GenerateRock()
    {
        rockPositionX = Random.Range(-7.0f, 7.0f);
        GameObject rock1 = Instantiate(smallRock, new Vector3(rockPositionX, progress + 5.0f, 0), Quaternion.identity);
        rockStayTime += Time.deltaTime;
        if(rockStayTime > 3.0f)
        {
            Debug.Log("big rock!");
            GameObject rock2 = Instantiate(bigRock, new Vector3(rockPositionX, progress + 5.0f, 0), Quaternion.identity);
        }
    }*/
}
