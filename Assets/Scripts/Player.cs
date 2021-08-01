using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using PlayerState;

public class Player : MonoBehaviour
{

    //�ύX�O�̃X�e�[�g��
    //private string _beforeStateName;
    //�X�e�[�g
    /*public StateProcessor StateProcessor = new StateProcessor(); //�v���Z�b�T�[
    public PlayerStateDefault StateDefault = new PlayerStateDefault();
    public PlayerAlive PlayerAlive = new PlayerAlive();
    public PlayerDamaged PlayerDamaged = new PlayerDamaged();*/




    private Rigidbody2D ownRigidbody2D;
    //������։������
    public float jumpForce = 62.0f;
    public float speed = 4f; //�����X�s�[�h
    public LayerMask groundLayer;//Linecast�Ŕ��肷��
    [SerializeField] public bool isGrounded;//���n����

    [SerializeField] public bool controlable;//����\����

    private BoxCollider2D myCollision;
    public bool walkFlag;//�������
    public bool damagedFlag;//������
    public bool jumpHoldFlag;//�W�����v���
    public bool hoverFlag;//�z�o�����O���

    public string CurrenState;

    //bool push;
    //bool boolLeft;
    //bool boolRight;
    public bool touch;

    //���n�������I�u�W�F�N�g�����锠
    public GameObject _stampTarget;

    private Animator anim = null;

    //�X�e�[�g�Ǘ��p
    public PlayerStateController stateController;


    // Start is called before the first frame update
    void Start()
    {
        ownRigidbody2D = GetComponent<Rigidbody2D>();
        //this.push = false;
        //this.boolLeft = false;
        //this.boolRight = false;
        stateController = GetComponent<PlayerStateController>();
        //stateController.Default();
        controlable = true;
        damagedFlag = false;
        myCollision = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrenState = stateController.StateProcessor.State.getStateName();
        Debug.Log(stateController.StateProcessor.State.getStateName());
        //Debug.Log(transform.position.y);




        //�X�e�[�g�̒l���ύX���ꂽ����s�������s��
        /*if (StateProcessor.State == null)
        {
            return;
        }

        if (StateProcessor.State.getStateName() != _beforeStateName)
        {
            Debug.Log(" Now State:" + StateProcessor.State.getStateName());
            _beforeStateName = StateProcessor.State.getStateName();
            StateProcessor.Execute();
        }*/

        //Linecaset�ő����ɒn�ʗL�邩����
        /*isGrounded = Physics2D.Linecast(
            transform.position - transform.up * 1.2f,
            transform.position - transform.up * 1.6f,
            groundLayer);*/
        //BoxCast�͂��̖��̒ʂ�͔�(����)�œ����蔻�肪����D�ꃂ�m���B���C���[�}�X�N���ݒ�ł��邼
        isGrounded = Physics2D.BoxCast(transform.position - transform.up * 0.8f, new Vector2(1, 1), 0, Vector2.zero, 0.0f, groundLayer);


        //�X�e�[�g�̃R���g���[��
        if (damagedFlag)
        {
            stateController.Dead();
        }
        else
        {
            if (isGrounded)
            {
                stateController.Default();
            }
            if (!isGrounded)
            {
                if (CurrenState != "Hover")
                {

                    stateController.Jump();
                }
                //
            }
            if (!hoverFlag)
            {
                if (CurrenState == "Hover")
                {
                    stateController.Jump();
                }
            }
        }


        //�A�j���[�V�����̃R���g���[��
        if (CurrenState == "Hover")
        {
            anim.SetBool("hover", true);
        }
        else
        {
            anim.SetBool("hover", false);
        }

        //Debug.Log(transform.up);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float maxDistance = 10;


        //����𓥂񂾎��̏���
        RaycastHit2D hit = Physics2D.BoxCast(transform.position - transform.up * 0.8f, new Vector2(1, 1), 0, Vector2.zero,0.0f, groundLayer);
        //Linecast������
        //Debug.Log(isGrounded);
        //Debug.Log(hit.collider);
        //Debug.DrawLine(transform.position - transform.up * 1.2f, transform.position - transform.up * 1.6f, Color.red);
        if (hit.collider && isGrounded)


        {
            var stamptarget = hit.collider.gameObject.GetComponent<IStampable>();
            if(stamptarget != null)
            {

                //stateController.Default();
                hit.collider.gameObject.GetComponent<IStampable>().Stamped();
            }

            var movingFloor = hit.collider.gameObject.GetComponent<FloorSlideable>();
            if(movingFloor != null)
            {
                if (jumpHoldFlag)
                {
                }
                if (!jumpHoldFlag)//�W�����v�{�^���������͏���
                {
                    //��������̏�ɏ���Ă�Ƃ��͑��x�𑫏�ƈ�v������
                    Vector2 addVelocity = Vector2.zero;
                    addVelocity = hit.collider.gameObject.GetComponent<FloorSlideable>().GetVelocity();
                    //�e����W���̑��x�����߂�
                    //ownRigidbody2D.velocity = new Vector2(1 * speed, ownRigidbody2D.velocity.y);
                    ownRigidbody2D.velocity = addVelocity;
                }

            }
        }

        //���������ꍇ�̏���
        if(transform.position.y < GManager.instance.progress  - 20 || transform.position.y < -10)
        {
            
            GManager.instance.GameReset();
            //GManager.instance.SetProgress(0);

        }

        //�ŐV�̕W����GameManager�ɋL�^����
        if(transform.position.y > GManager.instance.progress && transform.position.y - GManager.instance.progress < 5)
        {
            Debug.Log("�v���C���[��y�ʒu��" + transform.position.y);
            GManager.instance.SetProgress(GManager.instance.progress + 5);
        }

        

        //����
        if (damagedFlag)
        {
            //stateController.Dead();
            ownRigidbody2D.velocity = new Vector2(0, ownRigidbody2D.velocity.y);
            myCollision.enabled = false;
            // x�������ɂ��Ė��b2�x�A��]������Quaternion���쐬�i�ϐ���rot�Ƃ���j
            Quaternion rot = Quaternion.AngleAxis(1, Vector3.forward);
            // ���݂̎��M�̉�]�̏����擾����B
            Quaternion q = this.transform.rotation;
            // �������āA���g�ɐݒ�
            this.transform.rotation = q * rot;
        }

    }




    // �ڒn����i�ǂ̑��ʂŃW�����v�����Ȃ��悤�ɂ��邽�߁AOnTrigger�ɕύX�j
    private void OnTriggerEnter2D(Collider2D collision)
    // private void OnCollisionEnter2D ( Collision2D collision )
    {
        
        if (collision.CompareTag("Damageable"))
        {
            controlable = false;
            if (!damagedFlag)
            {
                //�ӂ��Ƃ�
                ownRigidbody2D.AddForce(Vector2.up * jumpForce / 2);
            }
            damagedFlag = true;
        }
        touch = true;
        ownRigidbody2D.velocity = new Vector2(0, ownRigidbody2D.velocity.y);
        //isGrounded = true;

        if (collision.CompareTag("Goal"))
        {
            GManager.instance.TransitionToClearScene();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    // private void OnCollisionExit2D ( Collision2D collision )
    {
        touch = false;
        //isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        //�ݒn����p��Gizmo����������
        Gizmos.color = Color.red; //�F�w��
        Gizmos.DrawCube((transform.position - transform.up * 0.8f), new Vector2(1, 1)); //���S�_�ƃT�C�Y
    }


}
