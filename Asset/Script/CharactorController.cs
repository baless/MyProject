using UnityEngine;
using System.Collections;

public class CharactorController : MonoBehaviour
{
    public GameObject ItemEffect;
    public Vector3 EffectRotation;
    
    const int MinLane = -3;
    const int MaxLane = 3;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const int MaxLife = 20;
    const float StunDuration = 0.5f;

    CharacterController controller;
    Animator animator;
    AudioSource audios;
    public AudioClip hited;
    public AudioClip dead;
    public AudioClip jump;
    public AudioClip getem;

    private bool IsPause = true;

    private int hashHit = Animator.StringToHash("Base Layer.Hit");
    private int hashDead = Animator.StringToHash("Base Layer.Dead");
    private int hashWalk = Animator.StringToHash("Base Layer.Walk");
    private int hashJump = Animator.StringToHash("Base Layer.Jump");

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    public int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedJump;

    public float speedX;
    public float accelerationZ;

    public int Life()
    {
        return life;
    }

    public bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audios = GetComponent<AudioSource>();
        GameObject.Find("ToMain").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("ToReStart").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("DeadScore").GetComponent<UnityEngine.UI.Text>().enabled = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //디버그용 키설정
        if (Input.GetKeyDown("left"))
            MoveToLeft();
        if (Input.GetKeyDown("right"))
            MoveToRight();
        if (Input.GetKeyDown("space"))
            Jump();
        if (Input.GetKeyDown("escape"))
            Escape();

        if(IsStun())
        {
            //행동을 정지하여 기절상태로부터 복귀카운트를 진행한다.
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        { 
        //조금씩 가속하여 Z축으로 전진
        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

        //X축은 목표한 포지션까지 남은 비율로 속도를 계산
        float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
        moveDirection.x = ratioX * speedX;
        }

        //중력의 힘을 매 프레임에 추가
        moveDirection.y -= gravity * Time.deltaTime;

        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //이동 후 지면에 닿는 순간 Y방향의 속도는 리셋한다.
        if (controller.isGrounded)
            moveDirection.y = 0;

        if(moveDirection.z > 0.0f)
        {
            animator.Play(hashWalk);
        }
    }

    //좌측 렌으로 이동
    public void MoveToLeft()
    {
        if (IsStun()) return;
        if (targetLane > MinLane)
            targetLane--;
    }
    public void MoveToRight()
    {
        if (IsStun()) return;
        if (targetLane < MaxLane)
            targetLane++;
    }
    public void Jump()
    {
        if (IsStun()) return;
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
            animator.Play(hashJump);
            audios.PlayOneShot(jump);
            Handheld.Vibrate();
        }
    }
    public void Escape()
    {
        if (IsPause == true)
        {
            Time.timeScale = 0;
            audios.mute = true;
            IsPause = false;
            GameObject.Find("ToMain").GetComponent<UnityEngine.UI.Image>().enabled = true;
            GameObject.Find("ToReStart").GetComponent<UnityEngine.UI.Image>().enabled = true;
        }

        else
        {
            Time.timeScale = 1;
            audios.mute = false;
            IsPause = true;
            GameObject.Find("ToMain").GetComponent<UnityEngine.UI.Image>().enabled = false;
            GameObject.Find("ToReStart").GetComponent<UnityEngine.UI.Image>().enabled = false;
        }
    }
    

    //CharacterController에 콜리죤이 생성됬을 때의 처리
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return;

        if (hit.gameObject.tag == "Enemy")
        {
            //라이프를 줄이고 기절상태에 돌입
            life--;
            recoverTime = StunDuration;

            //데미지 트리거의 설정
            animator.Play(hashHit);

            Handheld.Vibrate();

            if (life > 0)
            {
                audios.PlayOneShot(hited);
            }

            //부딛힌 적 오브젝트의 삭제
            Destroy(hit.gameObject);

            //사망
            if(life <= 0)
            {
                animator.Play(hashDead);
                audios.PlayOneShot(dead);
                Handheld.Vibrate();
            }
        }

        else if (hit.gameObject.tag == "WhiteItem")
        {
            if (life <= MaxLife-1)
            {
                life++;
            }
            audios.PlayOneShot(getem);
            Destroy(hit.gameObject);
            Instantiate(ItemEffect,transform.position,Quaternion.Euler(-90,0,0));
        }

        else if (hit.gameObject.tag == "BrownItem")
        {
            if (life <= MaxLife-2)
            {
                life += 2;
            }
            else if(life <= MaxLife-1)
            {
                life++;
            }
            audios.PlayOneShot(getem);
            Destroy(hit.gameObject);
            Instantiate(ItemEffect, transform.position, Quaternion.Euler(-90,0,0));
        }

        else if (hit.gameObject.tag == "YelloItem")
        {
            life = MaxLife;
            audios.PlayOneShot(getem);
            Destroy(hit.gameObject);
            Instantiate(ItemEffect,transform.position,Quaternion.Euler(-90,0,0));
        }
    }
}