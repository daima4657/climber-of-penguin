using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI�p�ɐ錾

public class ProgressManager : MonoBehaviour
{
    private Text heightText;
    private Text scoreText;
    private float record;
    //private int totalScore;
    private float floorPositionX;
    public GameObject goalObj;
    public GameObject floorObj;
    public GameObject floorObjMoving;
    public GameObject floorObjExit;
    public GameObject floorObjExitPath;
    private Record recordManager;
    private GameObject variableScore;

    // Start is called before the first frame update
    void Start()
    {
        heightText = GameObject.Find("Height").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        recordManager = GetComponent<Record>();
        variableScore = GameObject.Find("VariableScore");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //progress�̍X�V�|�C���g�ɓ��B�����Ƃ��̏���
    public void SetProgress(float num)
    {
        //progress�̍X�V
        GManager.instance.progress = GManager.instance.DataResetFlag ? 0 : num;

        //����̐���
        if (GManager.instance.progress < 495)
        {
            GenerateFloor(num);
        }

        //�w�i�̐���
        if (GManager.instance.progress > GetComponent<GenerateBG>().lastGeneretePositionY - 40)
        {
            GetComponent<GenerateBG>().Generate();
        }

        //EXIT�ɓ���������Score�����Z����
        if (GManager.instance.progress % 50 == 0)
        {

            if (GManager.instance.progress != 0)
            {
                //���Z����X�R�A�̌v�Z
                float additionalScore = 0;
                additionalScore = (GetComponent<Record>().Limit_time + GManager.instance.progress/100 - 0.5f - GetComponent<Record>().Current_time) * record;
                additionalScore = Mathf.Floor(additionalScore);
                GManager.instance.TotalScore += (int)additionalScore;
                scoreText.text = GManager.instance.TotalScore.ToString();
                //�X�R�A�̕ϓ��l����ʂɕ\������
                variableScore.GetComponent<VariableScore>().ScoreDisplay((int)additionalScore);
            }
            //Time Limit�̍X�V
            recordManager.SetLimit();

        }

        //�X�R�A���X�V
        record = GManager.instance.progress / 5;
        heightText.text = record.ToString() + "m";

    }

    //����̐������W�b�N
    public void GenerateFloor(float num)
    {
        
        floorPositionX = Random.Range(-7.0f, 7.0f);

        if ((GManager.instance.progress + 10) % 50 == 0)//EXIT����𐶐�
        {
            GameObject obj = Instantiate(floorObjExit, new Vector3(7.0f, GManager.instance.progress + 5.0f, 0), Quaternion.identity);
            GameObject obj2 = Instantiate(goalObj, new Vector3(9.46f, GManager.instance.progress + 6.63f, 0), Quaternion.identity);
            GameObject exitPath = Instantiate(floorObjExitPath, new Vector3(12.12f, GManager.instance.progress + 5.0f, 0), Quaternion.identity);
        }
        else if ((GManager.instance.progress + 5) % 50 == 0)//EXIT�̎��̑���
        {
            floorPositionX = Random.Range(-7.0f, 0f);//�l�݂��������Ȃ��悤��
            GameObject obj = Instantiate(floorObj, new Vector3(floorPositionX, GManager.instance.progress + 5.0f, 0), Quaternion.identity);
        }
        else if (GManager.instance.progress % 25 == 0)//�������𐶐�
        {
            GameObject obj = Instantiate(floorObjMoving, new Vector3(floorPositionX, GManager.instance.progress + 5.0f, 0), Quaternion.identity);
        }
        else//�ʏ�̏��𐶐�
        {
            //Debug.Log("�ʏ�̏��𐶐� progress��" + GManager.instance.progress);
            GameObject obj = Instantiate(floorObj, new Vector3(floorPositionX, GManager.instance.progress + 5.0f, 0), Quaternion.identity);
        }


    }

    public void ScoreReset()
    {
        Debug.Log("SCORE RESET");
        GManager.instance.TotalScore = 0;
        scoreText.text = GManager.instance.TotalScore.ToString();
    }

}
