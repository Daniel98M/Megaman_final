using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int jumpForce;
    [SerializeField] AudioClip death_clip;
    Rigidbody2D myBody;
    Animator myAnim;
    bool isGroundedRight = true;
    bool isGroundedLeft = true;
    bool onLeftTopWall = false;
    bool onLeftBottWall = false;
    bool onRightTopWall = false;
    bool onRightBottWall = false;
    bool jump = false;
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        StartCoroutine(DeathCorrutine(dead));
    }
    IEnumerator DeathCorrutine(bool dead)
    {
        while (dead)
        {
            
            //AudioSource.PlayClipAtPoint(death_clip, transform.position);
            yield return new WaitForSeconds(1);
            Debug.Log("1s");
            yield return new WaitForSeconds(1);
            Debug.Log("2s");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale == new Vector3(0.9f, 0.9f, 0))
        {
            RaycastHit2D rayRight = Physics2D.Raycast(transform.position + new Vector3(0.72f, 0, 0), Vector2.down, 1f, LayerMask.GetMask("Ground"));
            RaycastHit2D rayLeft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 0), Vector2.down, 1f, LayerMask.GetMask("Ground"));
            isGroundedRight = (rayRight.collider != null);
            isGroundedLeft = (rayLeft.collider != null);
            Debug.DrawRay(transform.position + new Vector3(0.72f, 0, 0), Vector2.down * 1f, Color.red);
            Debug.DrawRay(transform.position + new Vector3(-0.5f, 0, 0), Vector2.down * 1f, Color.red);
        }
        else
        {
            RaycastHit2D rayRight = Physics2D.Raycast(transform.position + new Vector3(-0.72f, 0, 0), Vector2.down, 1f, LayerMask.GetMask("Ground"));
            RaycastHit2D rayLeft = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector2.down, 1f, LayerMask.GetMask("Ground"));
            isGroundedRight = (rayRight.collider != null);
            isGroundedLeft = (rayLeft.collider != null);
            Debug.DrawRay(transform.position + new Vector3(-0.72f, 0, 0), Vector2.down * 1f, Color.red);
            Debug.DrawRay(transform.position + new Vector3(0.5f, 0, 0), Vector2.down * 1f, Color.red);
        }
        Jump();
        Fire();
    }
    void Fire()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            myAnim.SetLayerWeight(1,1);
        }
        else
        {
            myAnim.SetLayerWeight(1, 0);
        }
    }
    void Jump()
    {
        //Modificado para que cuando colisione con una pared no haga la animacion de correr
        if(isGroundedRight || isGroundedLeft)
        {
            myAnim.SetBool("isJumping", false);
            myAnim.SetBool("isFalling", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jump = true;
            }
        }
        if (!isGroundedRight && !isGroundedLeft && myBody.velocity.y != 0 && jump)
        {
            myAnim.SetBool("isJumping", true);
            jump = false;
        }
        else if(!isGroundedRight && !isGroundedLeft && myBody.velocity.y != 0 && !jump)
        {
            myAnim.SetBool("isFalling", true);
        }
    }

    private void FixedUpdate()
    {
        float dirH = Input.GetAxis("Horizontal");
        if (dirH != 0)
        {
            myAnim.SetBool("isRunning", true);
            if (dirH < 0)
            {
                transform.localScale = new Vector2(-0.9f, 0.9f);
                RaycastHit2D rayLTW = Physics2D.Raycast(transform.position + new Vector3(0, 0.95f, 0), Vector2.left, 0.8f, LayerMask.GetMask("Ground"));
                RaycastHit2D rayLBW = Physics2D.Raycast(transform.position + new Vector3(0, -0.96f, 0), Vector2.left, 0.8f, LayerMask.GetMask("Ground"));
                Debug.DrawRay(transform.position + new Vector3(0, 0.92f, 0), Vector2.left * 0.8f, Color.red);
                Debug.DrawRay(transform.position + new Vector3(0, -0.96f, 0), Vector2.left * 0.8f, Color.red);
                onRightTopWall = false;
                onRightBottWall = false;
                onLeftTopWall = (rayLTW.collider != null);
                onLeftBottWall = (rayLBW.collider != null);
            }
            else if(dirH > 0)
            {
                transform.localScale = new Vector2(0.9f, 0.9f);
                RaycastHit2D rayRTW = Physics2D.Raycast(transform.position + new Vector3(0, 0.95f, 0), Vector2.right, 0.8f, LayerMask.GetMask("Ground"));
                RaycastHit2D rayRBW = Physics2D.Raycast(transform.position + new Vector3(0, -0.96f, 0), Vector2.right, 0.8f, LayerMask.GetMask("Ground"));
                Debug.DrawRay(transform.position + new Vector3(0, 0.92f, 0), Vector2.right * 0.8f, Color.red);
                Debug.DrawRay(transform.position + new Vector3(0, -0.96f, 0), Vector2.right * 0.8f, Color.red);
                onRightTopWall = (rayRTW.collider != null);
                onRightBottWall = (rayRBW.collider != null);
                onLeftTopWall = false;
                onLeftBottWall = false;

            }
        }
        else
        {
            myAnim.SetBool("isRunning", false);
        }
        if ((!onLeftTopWall && !onLeftBottWall) && (!onRightTopWall && !onRightBottWall))
        {
            myBody.velocity = new Vector2(dirH * speed, myBody.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject go = other.gameObject;
        if (go.tag == "Enemy")
        {
            myAnim.SetBool("isDead", true);
            dead = true;
        }
    }
}
