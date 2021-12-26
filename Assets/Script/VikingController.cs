using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VikingController : MonoBehaviour
{
    float movingSpeed = 20f;
    Animator animator;
    CharacterController controller;
    RoadGenerator roadGenerator;
    public Vector3 move;
    Vector3 leftRightMove;
    
    [SerializeField] Text showScore;
    [SerializeField] Text showTime;
    [SerializeField] Text gameOverScore;
    [SerializeField] Text gameOverTime;
    [SerializeField] Text tip;
    public GameObject menuButton;
    int score = 0;
    int minute=0;
    float second=0;
    // Gravity Variables
    [SerializeField] private float gravityValue= -0.04f;
    [SerializeField] private float groundedGravity=-.5f;
    private float angle = 0;
    public GameObject showGameOver;
    // Jumping Variables
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float initialJumpVelocity= 0.6f;
    bool isPlayerGrounded = true;
    bool isJumping = false;
    private bool turnMode = false;
    private bool unlockLR = false;
    private float timeScale;
    bool run;
    bool jump;
    bool die;
    bool stopingGame = false;
    [SerializeField]
    private float distance=6;
    Vector3 jumpMovement;
    private string onCollision;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //Time.fixedDeltaTime = 60;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        roadGenerator = GetComponent<RoadGenerator>();
        maxJumpHeight = 5f;
        maxJumpTime = 0.5f;
        //controller = GetComponent<CharacterController>();
        //setupJumpVariables();
        //float timeToApex = maxJumpTime / 2 /Time.deltaTime;
        //initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        //gravityValue = -initialJumpVelocity / timeToApex;
        
        move = new Vector3(0, 0, 1).normalized;
        leftRightMove = new Vector3(1, 0, 0).normalized;
        score = 0;
        minute = 0;
        second = 0;
        showScore.text = "Score : " + score.ToString();
        showTime.text = "Time : " + string.Format("{0:00}", minute) + " : " + string.Format("{0:00}", (int)second);
        unlockLR = false;
        stopingGame = true;
        showGameOver.SetActive(false);
        isPlayerGrounded = true;
        isJumping = false;
        detectGrounded();
        handleGravity();

    }

    // Update is called once per frame
    void Update()
    {
        if (stopingGame && Input.GetKeyDown(KeyCode.Space))
        {
            stopingGame = false;
            tip.enabled = false;
        }
        if (!stopingGame)
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
            if (transform.position.y <= -2f)
            {
                stopingGame = true;
                stopTheGame();
            }
            controller.Move(move * movingSpeed * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Space) && isPlayerGrounded)
            {
                jumpMovement.y = initialJumpVelocity;
                isPlayerGrounded = false;
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
            if (Input.GetKey(KeyCode.D) && !isJumping) // right
            {
                if (turnMode)
                {
                    run = true;
                    transform.Rotate(0, 90, 0, Space.Self);
                    angle += 90;
                    angle %= 360;
                    move.x = Mathf.Sin(angle * Mathf.Deg2Rad);
                    move.z = Mathf.Cos(angle * Mathf.Deg2Rad);
                    leftRightMove.x = Mathf.Cos(angle * Mathf.Deg2Rad);
                    leftRightMove.z = -Mathf.Sin(angle * Mathf.Deg2Rad);
                    turnMode = false;
                }
                else if (!unlockLR)
                {
                    controller.Move(leftRightMove * 10 * Time.deltaTime);
                }

            }
            animator.SetBool("isRun", run);
            animator.SetBool("isJump", jump);
            controller.Move(jumpMovement);
        }
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2 / Time.deltaTime;
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        gravityValue = -initialJumpVelocity / timeToApex;
        groundedGravity = -0.05f;
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
            stopingGame = true;
            stopTheGame();
        }
        if (collision.gameObject.name=="spider")
        {
            stopingGame = true;
            stopTheGame();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "spider")
        {
            stopingGame = true;
            stopTheGame();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.transform.name+" Exit");
        if (collision.gameObject.name == "spider")
        {
            stopingGame = true;
            stopTheGame();
        }
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
        menuButton.SetActive(false);
        showScore.enabled = false;
        showTime.enabled = false;
        showGameOver.SetActive(true);
        gameOverScore.text = "Score : " + score.ToString();
        gameOverTime.text = "Time : " + string.Format("{0:00}", minute) + " : " + string.Format("{0:00}", (int)second);
    }

    private void reStartTheGame()
    {
        Time.timeScale = 1;
    }
}
