using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController1 : MonoBehaviour
{
    float movingSpeed = 20f;
    Animator animator;
    GameObject g;
    CharacterController controller;
    RoadGenerator roadGenerator;
    VikingController vikingController;
    AudioSource audioSource;
    Vector3 move;
    // Gravity Variables
    [SerializeField] private float gravityValue = -0.04f;
    [SerializeField] private float groundedGravity = -0.5f;
    [SerializeField] private float initialJumpVelocity = 1f;
    public AudioClip growl;
    private float angle = 0;
    [SerializeField] bool isPlayerGrounded = true;
    bool isJumping = false;
    bool run;
    bool jump;
    bool stopingGame;
    Vector3 jumpMovement;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        g = GameObject.Find("vikingPlayer");
        roadGenerator = g.GetComponent<RoadGenerator>();
        vikingController = g.GetComponent<VikingController>();
        move = new Vector3(0, 0, 1).normalized;
        isPlayerGrounded = true;
        isJumping = false;
        stopingGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopingGame && Input.GetKeyDown(KeyCode.Space)) stopingGame = false;
        if (!stopingGame)
        {
            detectGrounded();
            handleGravity();
            if (isPlayerGrounded)
            {
                run = true;
                jump = false;
            }
            if (transform.position.y <= -2)
            {
                transform.position = g.transform.position + vikingController.move * (-12);
            }
            controller.Move(move * movingSpeed * Time.deltaTime);
            animator.SetBool("isRun", run);
            animator.SetBool("isJump", jump);
            controller.Move(jumpMovement);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerSet triggerSet = other.GetComponent<TriggerSet>();
        if (other.gameObject.CompareTag("enemyJump") && !triggerSet.isCall)
        {
            audioSource.PlayOneShot(growl);
            triggerSet.isCall = true;
            jumpMovement.y = initialJumpVelocity;
            isPlayerGrounded = false;
            isJumping = true;
            jump = true;
            run = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        TriggerSet triggerSet = other.GetComponent<TriggerSet>();
        if (other.gameObject.CompareTag("enemyTurnRight") && !triggerSet.isCall)
        {
            audioSource.PlayOneShot(growl);
            triggerSet.isCall = true;
            run = true;
            transform.Rotate(0, 90, 0, Space.Self);
            angle += 90;
            angle %= 360;
            move.x = Mathf.Sin(angle * Mathf.Deg2Rad);
            move.z = Mathf.Cos(angle * Mathf.Deg2Rad);
        }
        if (other.gameObject.CompareTag("enemyTurnLeft") && !triggerSet.isCall)
        {
            audioSource.PlayOneShot(growl);
            triggerSet.isCall = true;
            run = true;
            transform.Rotate(0, -90, 0, Space.Self);
            angle -= 90;
            angle %= 360;
            move.x = Mathf.Sin(angle * Mathf.Deg2Rad);
            move.z = Mathf.Cos(angle * Mathf.Deg2Rad);
            
        }
        if (other.transform.name == "newRoad" && triggerSet.isCall)
        {
            triggerSet.isCall = false;
            roadGenerator.isRemove = true;
        }
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
}
