using UnityEngine;
using System.Collections;
using PlayerState;
//using UniRx;
using UnityEngine.UI;
using System;

public class PlayerStateController : MonoBehaviour
{

    //�ύX�O�̃X�e�[�g��
    private string _beforeStateName;

    //�X�e�[�g
    public StateProcessor StateProcessor = new StateProcessor();           //�v���Z�b�T�[
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

        //�X�e�[�g�̒l���ύX���ꂽ����s�������s��
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
        /*gameObject.transform.GetComponent<Text>().text = "������Ԃł�";
        //�P�b���StateA�ɏ�ԑJ��
        Observable
            .Timer(TimeSpan.FromSeconds(1))
            .Subscribe(x => StateProcessor.State = StateA);*/
    }

    public void Walk()
    {
        StateProcessor.State = StateWalk;
        /*gameObject.transform.GetComponent<Text>().text = "StateA�ł�";
        //�P�b���StateB�ɏ�ԑJ��
        Observable
            .Timer(TimeSpan.FromSeconds(1))
            .Subscribe(x => StateProcessor.State = StateB);*/
    }

    public void Dead()
    {
        StateProcessor.State = StateDead;
        /*gameObject.transform.GetComponent<Text>().text = "StateB�ł�";
        //�P�b���Default�ɏ�ԑJ��
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