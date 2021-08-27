using UnityEngine;
using System.Collections;
using PlayerState;
//using UniRx;
using UnityEngine.UI;
using System;

public class PlayerStateController : MonoBehaviour
{

    //変更前のステート名
    private string _beforeStateName;

    //ステート
    public StateProcessor StateProcessor = new StateProcessor();           //プロセッサー
    public PlayerStateDefault StateDefault = new PlayerStateDefault();
    public PlayerWalk StateWalk = new PlayerWalk();
    public PlayerDead StateDead = new PlayerDead();
    public PlayerJump StateJump = new PlayerJump();
    public PlayerHover StateHover = new PlayerHover();
    public PlayerAirDash StateAirDash = new PlayerAirDash();

    // Use this for initialization
    void Start()
    {

        //DefaultState
        StateProcessor.State = StateDefault;
        StateDefault.execDelegate = Default;
        StateWalk.execDelegate = Walk;
        StateDead.execDelegate = Dead;
        StateJump.execDelegate = Jump;
        StateHover.execDelegate = Hover;
        StateAirDash.execDelegate = AirDash;
    }

    // Update is called once per frame
    void Update()
    {

        //ステートの値が変更されたら実行処理を行う
        if (StateProcessor.State == null)
        {
            return;
        }

        if (StateProcessor.State.getStateName() != _beforeStateName)
        {
            //Debug.Log(" Now State:" + StateProcessor.State.getStateName());
            _beforeStateName = StateProcessor.State.getStateName();
            StateProcessor.Execute();
        }

    }

    public void Default()
    {
        StateProcessor.State = StateDefault;
        /*gameObject.transform.GetComponent<Text>().text = "初期状態です";
        //１秒後にStateAに状態遷移
        Observable
            .Timer(TimeSpan.FromSeconds(1))
            .Subscribe(x => StateProcessor.State = StateA);*/
    }

    public void Walk()
    {
        StateProcessor.State = StateWalk;
        /*gameObject.transform.GetComponent<Text>().text = "StateAです";
        //１秒後にStateBに状態遷移
        Observable
            .Timer(TimeSpan.FromSeconds(1))
            .Subscribe(x => StateProcessor.State = StateB);*/
    }

    public void Dead()
    {
        StateProcessor.State = StateDead;
        /*gameObject.transform.GetComponent<Text>().text = "StateBです";
        //１秒後にDefaultに状態遷移
        Observable
            .Timer(TimeSpan.FromSeconds(1))
            .Subscribe(x => StateProcessor.State = StateDefault);*/

    }
    public void Jump()
    {
        StateProcessor.State = StateJump;
    }
    public void Hover()
    {
        StateProcessor.State = StateHover;
    }

    public void AirDash()
    {
        StateProcessor.State = StateAirDash;
    }
}