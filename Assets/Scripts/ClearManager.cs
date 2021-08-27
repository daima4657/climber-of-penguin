using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI�p�ɐ錾
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
{
    private Text heightText;
    private Text scoreText;
    private Text rankNameText;
    private Text nextRankText;
    private Image rankImage;


    // Start is called before the first frame update
    void Start()
    {
        heightText = GameObject.Find("ResultHeightTitle").GetComponent<Text>();
        scoreText = GameObject.Find("ResultScoreTitle").GetComponent<Text>();
        rankNameText = GameObject.Find("RankName").GetComponent<Text>();
        nextRankText = GameObject.Find("NextRunkText").GetComponent<Text>();
        rankImage = GameObject.Find("RankImage").GetComponent<Image>();
        GManager.instance.GameClearFlag = true;
        SetResultView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        //Debug.Log("�Q�[�����Z�b�g");
        GManager.instance.GameClearFlag = false;
        GManager.instance.DataResetFlag = true;

    }

    //Clear�ւ̑J�ڎ��ɃX�R�A�Ȃǂ̃f�[�^����ʂɓK�p����
    public void SetResultView()
    {
        float score = GManager.instance.progress / 5;
        heightText.text = "Height : " + score.ToString() + "m";
        scoreText.text = "Score : " + GManager.instance.TotalScore.ToString() +"pt";

        //���_�ɉ����ă����N�̕\����؂�ւ���
        if (GManager.instance.TotalScore > 6000)
        {
            rankImage.sprite = GManager.instance.rankImageEmperor;
            rankNameText.text = "RANK4:�G���y���[�y���M����";
            nextRankText.text = "";
        }
        else if (GManager.instance.TotalScore > 3000)
        {
            rankImage.sprite = GManager.instance.rankImageAdeliae;
            rankNameText.text = "RANK3:�A�f���[�y���M����";
            nextRankText.text = "Next Rank : 6000pt";
        }
        else if(GManager.instance.TotalScore > 1000)
        {
            rankImage.sprite = GManager.instance.rankImageChild;
            rankNameText.text = "RANK2:�Ђȃy���M����";
            nextRankText.text = "Next Rank : 3000pt";
        } else
        {
            rankImage.sprite = GManager.instance.rankImageEgg;
            rankNameText.text = "RANK1:���܂���";
            nextRankText.text = "Next Rank : 1000pt";
        }


    }
}
