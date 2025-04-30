using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1; // 0=left 1=middle 2=right
    public float laneDistance = 4; // distance between two lanes

    public bool isGrounded;
    
    public float jumpForce;
    public float gravity = -20;

    public Animator animator;
    private bool isSliding = false;

    // start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        controller = GetComponent<CharacterController>();
    }

    //update is called once per frame
    void Update(){
        if(!PlayerManager.isGameStarted){
            return;
        }

        //increase speed
        if(forwardSpeed < maxSpeed){
            forwardSpeed += 0.1f*Time.deltaTime;
        }

        animator.SetBool("isGameStarted", true);

        direction.z = forwardSpeed;

        isGrounded = controller.isGrounded;
        animator.SetBool("isGrounded", isGrounded);

        if(controller.isGrounded){
            if(SwipeManager.swipeUp){
            Jump();
            }
        }
        else{
            direction.y += gravity*Time.deltaTime;
        }

         if(SwipeManager.swipeDown && !isSliding){
            StartCoroutine(Slide());
        }


        //gather the inputs the lane they should be

        if(SwipeManager.swipeRight){
            desiredLane++;
            if(desiredLane > 2) {
                desiredLane = 2;
            }
        }

        if(SwipeManager.swipeLeft){
            desiredLane--;
            if(desiredLane < 0) {
                desiredLane = 0;
            }
        }

        // calculate where we should be in the future

        Vector3 targetPosition = transform.position.z*transform.forward+transform.position.y*transform.up;

        if(desiredLane == 0){
            targetPosition += Vector3.left*laneDistance;
        }
        else if(desiredLane == 2){
            targetPosition += Vector3.right*laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 80*Time.fixedDeltaTime);
        controller.center = controller.center;
    }

    private void FixedUpdate(){
        controller.Move(direction*Time.fixedDeltaTime);

        if(!PlayerManager.isGameStarted){
            return;
        }
    }

    private void Jump(){
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit){
        if(hit.transform.tag == "Obstacle"){
            PlayerManager.gameOver = true;
        }
    }

    private IEnumerator Slide(){
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}
