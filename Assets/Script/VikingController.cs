using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VikingController : MonoBehaviour
{
    public float movingSpeed = 15f;
    int roadnum = 1;
    Animator animator;
    CharacterController controller;
    RoadGenerator roadGenerator;
    Vector3 move;
    Vector3 leftRightMove;
    [SerializeField]
    Text showScore;
    [SerializeField]
    Text showTime;
    int score = 0;
    int minute=0;
    float second=0;
    // Gravity Variables
    private float gravityValue = -9.8f;
    private float groundedGravity = -0.05f;
    private float angle = 0;

    // Jumping Variables
    [SerializeField]
    private float maxJumpHeight = 5f;
    [SerializeField]
    private float maxJumpTime = 1f;
    [SerializeField]
    private float initialJumpVelocity;
    private bool isPlayerGrounded = true;
    private bool isJumping = false;
    private bool turnMode = false;
    private bool unlockLR = false;
    bool run;
    bool jump;
    [SerializeField]
    private float distance=6;
    Vector3 jumpMovement;
    private string onCollision;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        roadGenerator = GetComponent<RoadGenerator>();
        //controller = GetComponent<CharacterController>();
        setupJumpVariables();
        move = new Vector3(0, 0, 1).normalized;
        leftRightMove = new Vector3(1, 0, 0).normalized;
        score = 0;
        minute = 0;
        second = 0;
        showScore.text = "Score : " + score.ToString();
        showTime.text = "Time : " + string.Format("{0:00}", minute) + " : " + string.Format("{0:00}", (int)second);
        unlockLR = false;
    }

    // Update is called once per frame
    void Update()
    {
        second += Time.deltaTime;
        if (second >= 60)
        {
            minute++;
            second = 0;
        }
        showTime.text = "Time : " + string.Format("{0:00}", minute) + " : " + string.Format("{0:00}", (int)second);
        detectGrounded();
        handleGravity();
        if (isPlayerGrounded)
        {
            run = true;
            jump = false;
        }
        if (transform.position.y <= -5f)
        {
            stopTheGame();
        }
        controller.Move(move * movingSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space)&& isPlayerGrounded)
        {
            jumpMovement.y = initialJumpVelocity;
            isJumping = true;
            jump = true;
            run = false;
        }
        if (Input.GetKey(KeyCode.A) && !isJumping) // left
        {
            if (turnMode)
            {
                run = true;
                transform.Rotate(0, -90, 0, Space.Self);
                angle -= 90;
                angle %= 360;
                move.x = Mathf.Sin(angle * Mathf.Deg2Rad);
                move.z = Mathf.Cos(angle * Mathf.Deg2Rad);
                leftRightMove.x = Mathf.Cos(angle * Mathf.Deg2Rad);
                leftRightMove.z = -Mathf.Sin(angle * Mathf.Deg2Rad);
                turnMode = false;
            }
            else if (!unlockLR)
            {
                controller.Move(leftRightMove * (-10) * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.D)&&!isJumping) // right
        {
            if (turnMode)
            {
                run = true;
                transform.Rotate(0, 90, 0, Space.Self);
                angle += 90;
                angle %= 360;
                move.x = Mathf.Sin(angle * Mathf.Deg2Rad);
                move.z = Mathf.Cos(angle * Mathf.Deg2Rad);
                leftRightMove.x= Mathf.Cos(angle * Mathf.Deg2Rad);
                leftRightMove.z= -Mathf.Sin(angle * Mathf.Deg2Rad);
                turnMode = false;
            }
            else if(!unlockLR)
            {
                controller.Move(leftRightMove * 10 * Time.deltaTime);
            }
            
        }
        animator.SetBool("isRun", run);
        animator.SetBool("isJump", jump);
        controller.Move(jumpMovement);
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2 / Time.deltaTime;

        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        gravityValue = -initialJumpVelocity / timeToApex;
    }

    void detectGrounded()
    {
        isPlayerGrounded = controller.isGrounded;
        if (isPlayerGrounded) isJumping = false;
    }
    void handleGravity()
    {
        if (isPlayerGrounded) jumpMovement.y = groundedGravity;
        else
        {
            jumpMovement.y += gravityValue;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("obstacle"))
        {
            stopTheGame();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.transform.name+" Exit");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        TriggerSet triggerSet = other.GetComponent<TriggerSet>();
        if (other.transform.name == "coin(Clone)" && !triggerSet.isCall)
        {
            triggerSet.isCall = true;
            Destroy(other.gameObject);
            score ++;
            showScore.text = "Score : " + score.ToString();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.transform.name);
        TriggerSet triggerSet =  other.GetComponent<TriggerSet>();
        if (other.transform.name == "newRoad"&&!triggerSet.isCall)
        {
            triggerSet.isCall = true;
            roadGenerator.isGenRoad = true;
        }
        if (other.transform.name == "canTurnLeft" && !triggerSet.isCall)
        {
            triggerSet.isCall = true;
            turnMode = true;
        }
        if (other.transform.name == "canTurnRight" && !triggerSet.isCall)
        {
            triggerSet.isCall = true;
            turnMode = true;
        }
        if(other.transform.name== "unlockLeftRight" && !triggerSet.isCall)
        {
            unlockLR = false;
        }
        if(other.transform.name == "lockLeftRight" && !triggerSet.isCall)
        {
            unlockLR = true;
        }
    }
    private void stopTheGame()
    {
        Time.timeScale = 0;
    }

    private void reStartTheGame()
    {
        Time.timeScale = 1;
    }
}
