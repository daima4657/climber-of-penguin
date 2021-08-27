using UnityEngine;
using System.Collections;

//Player�I�u�W�F�N�g�̃X�e�[�g

namespace PlayerState
{
    //�X�e�[�g�̎��s���Ǘ�����N���X
    public class StateProcessor
    {
        //�X�e�[�g�{��
        private PlayerState _State;
        public PlayerState State
        {
            set { _State = value; }
            get { return _State; }
        }

        //���s�u���b�W
        public void Execute()
        {
            State.Execute();
        }
    }

    //�X�e�[�g�̃N���X
    public abstract class PlayerState
    {
        //�f���Q�[�g
        public delegate void executeState();
        public executeState execDelegate;

        //���s����
        public virtual void Execute()
        {
            if (execDelegate != null)
            {
                execDelegate();
            }
        }
        public abstract string getStateName();
    }

    //�ȉ���ԃN���X

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
            //Debug.Log("���ʂȏ���������ꍇ�͎q���������Ă��悢");
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

