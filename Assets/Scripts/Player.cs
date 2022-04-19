using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int jumpForce;
    [SerializeField] AudioClip death_clip;
    [SerializeField] AudioClip bullet_clip;
    [SerializeField] float fireRate = 1f;
    [SerializeField] GameObject bullet;
    Rigidbody2D myBody;
    Animator myAnim;
    bool isGroundedRight = true;
    bool isGroundedLeft = true;
    bool onLeftTopWall = false;
    bool onLeftBottWall = false;
    bool onRightTopWall = false;
    bool onRightBottWall = false;
    bool jump = false;
    public bool dead = false;
    public bool direction;
    bool reset = false;
    private float nextFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

    }
    IEnumerator Shooting()
    {
        myAnim.SetLayerWeight(1, 1);
        AudioSource.PlayClipAtPoint(bullet_clip, transform.position);
        yield return new WaitForSeconds(0.3f);
        myAnim.SetLayerWeight(1, 0);
    }
    IEnumerator DeathCorrutine()
    {
        yield return new WaitForSeconds(1f);
        AudioSource.PlayClipAtPoint(death_clip, transform.position);
        yield return new WaitForSeconds(1f);
        RestartGame();
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        Grounded();
        if (!dead)
        {
            Jump();
            Fire();
        }    
    }
    void Grounded()
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
    }
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (transform.localScale == new Vector3(0.9f, 0.9f, 0))
            {
                direction = true;
            }
            else
            {
                direction = false;
            }
            Instantiate(bullet, transform.position, transform.rotation);
            StartCoroutine(Shooting());
        }
        
    }
    void Jump()
    {
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
        if (!dead)
        {
            Move();
        }
    }
    void Move()
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
            else if (dirH > 0)
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
    void Death()
    {
        if (dead && !reset)
        {
            myBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(DeathCorrutine());
            myAnim.SetBool("isDead", true);
            reset = true;
        }
    }
    void RestartGame()
    {
        SceneManager.LoadScene("Megaman");
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject go = other.gameObject;
        if (go.tag == "Enemy")
        {
            dead = true;
        }
    }
}
