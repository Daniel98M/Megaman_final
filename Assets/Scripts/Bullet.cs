using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D bulletBody;
    Animator bulletAnim;
    private Player scriptPlayer;
    // Start is called before the first frame update
    void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
        bulletAnim = GetComponent<Animator>();
        scriptPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    IEnumerator Explotion()
    {
        bulletAnim.SetBool("hit", true);
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletBody.velocity.x == 0)
        {
            if (scriptPlayer.direction == true)
            {
                bulletBody.velocity = new Vector2(speed, bulletBody.velocity.y);
                transform.localScale = new Vector2(1f, 1f);
            }
            else
            {
                bulletBody.velocity = new Vector2(speed * (-1), bulletBody.velocity.y);
                transform.localScale = new Vector2(-1f, 1f);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(Explotion());
    }
}
