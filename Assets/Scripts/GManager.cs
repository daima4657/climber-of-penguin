using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI用に宣言
using UnityEngine.SceneManagement;
public class GManager : MonoBehaviour
{

    public float progress;
    public float record;
    public static GManager instance = null;
    public GameObject smallRock;
    public GameObject bigRock;
    private Record recordManager;
    private RockFall rockFall;
    private GameObject scoreGui;
    public ProgressManager progressManager;

    public Sprite rankImageEgg;
    public Sprite rankImageChild;
    public Sprite rankImageAdeliae;
    public Sprite rankImageEmperor;

    private bool dataResetFlag;
    public bool DataResetFlag
    {
        get => dataResetFlag;
        set => dataResetFlag = value;
    }

    private bool gameResetFlag;
    public bool GameResetFlag
    {
        get => gameResetFlag;
        set => gameResetFlag = value;
    }


    [SerializeField] private bool gameClearFlag;
    public bool GameClearFlag
    {
        get => gameClearFlag;
        set => gameClearFlag = value;
    }

    //最新のスコア
    [SerializeField] private int totalScore;
    public int TotalScore
    {
        get => totalScore;
        set => totalScore = value;
    }


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
        TotalScore = 0;
        recordManager = GetComponent<Record>();
        rockFall = GetComponent<RockFall>();
        progressManager = GetComponent<ProgressManager>();
        scoreGui = GameObject.Find("Score GUI");


    }

    // Update is called once per frame
    void Update()
    {
        //ゲームリセット前に諸々のデータを初期化する。
        if (DataResetFlag)
        {
            DataReset();
            GameResetFlag = true;
        }
    }

    void LateUpdate()
    {
        //シーンの再読み込み
        if (GameResetFlag)
        {
            GameReset();
        }
    }


    //スコア等リセット
    public void DataReset()
    {

        //Debug.Log("deat reset");
        //this.GetComponent<RockFall>().ChangeActivation(true);
        progressManager.SetProgress(0);
        GetComponent<GenerateBG>().Reset();
        GetComponent<ProgressManager>().ScoreReset();
        GetComponent<Record>().Reset();
    }
    //ゲームリセット
    public void GameReset()
    {
        if (!scoreGui.activeSelf)
        {
            scoreGui.SetActive(true);
        }
        rockFall.RestartGenerateRockCoroutine();
        DataResetFlag = false;
        GameResetFlag = false;

        SceneManager.LoadScene("GameScene");
    }

    //クリア画面への遷移
    public void TransitionToClearScene()
    {
        //this.GetComponent<RockFall>().ChangeActivation(false);
        scoreGui.SetActive(false);
        SceneManager.LoadScene("Clear");
    }




}
