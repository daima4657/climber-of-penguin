using UnityEngine;
using System.Collections;

//Playerオブジェクトのステート

namespace PlayerState
{
    //ステートの実行を管理するクラス
    public class StateProcessor
    {
        //ステート本体
        private PlayerState _State;
        public PlayerState State
        {
            set { _State = value; }
            get { return _State; }
        }

        //実行ブリッジ
        public void Execute()
        {
            State.Execute();
        }
    }

    //ステートのクラス
    public abstract class PlayerState
    {
        //デリゲート
        public delegate void executeState();
        public executeState execDelegate;

        //実行処理
        public virtual void Execute()
        {
            if (execDelegate != null)
            {
                execDelegate();
            }
        }
        public abstract string getStateName();
    }

    //以下状態クラス

    //DefaultPosition
    public class PlayerStateDefault : PlayerState
    {
        public override string getStateName()
        {
            return "Default";
        }
    }

    //Walk
    public class PlayerWalk : PlayerState
    {
        public override string getStateName()
        {
            return "Walk";
        }
    }

    public class PlayerDead : PlayerState
    {
        public override string getStateName()
        {
            return "Dead";
        }

        public override void Execute()
        {
            //Debug.Log("特別な処理がある場合は子が処理してもよい");
            if (execDelegate != null)
            {
                execDelegate();
            }
        }
    }

    //Jump
    public class PlayerJump : PlayerState
    {
        public override string getStateName()
        {
            return "Jump";
        }
    }

    //Hover
    public class PlayerHover : PlayerState
    {
        public override string getStateName()
        {
            return "Hover";
        }
    }

    //AirDash
    public class PlayerAirDash : PlayerState
    {
        public override string getStateName()
        {
            return "AirDash";
        }
    }
}

