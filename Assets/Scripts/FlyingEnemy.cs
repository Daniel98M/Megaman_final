using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] GameObject player;
    Animator flyAnim;
    CircleCollider2D flyCol;
    AIPath myPath;
    private Player scriptPlayer;
    bool flyDead = false;
    // Start is called before the first frame update
    void Start()
    {
        myPath = GetComponent<AIPath>();
        flyAnim = GetComponent<Animator>();
        flyCol = GetComponent<CircleCollider2D>();
        scriptPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }
    IEnumerator Death()
    {
        flyAnim.SetBool("isDead", true);
        yield return new WaitForSeconds(0.9f);
        Destroy(this.gameObject);
    }
    void ChasePlayer()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask("Player"));
        if (col != null && scriptPlayer.dead == false && !flyDead)
        {
            myPath.isStopped = false;
        }
        else
        {
            myPath.isStopped = true;
        }
        float dirH = transform.position.x - player.transform.position.x;
        if (dirH != 0 && col != null)
        {
            if (dirH < 0)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
            else if (dirH > 0)
            {
                transform.localScale = new Vector2(1f, 1f);

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject go = other.gameObject;
        if (go.tag == "MBullet")
        {
            hp--;
        }
        if (hp == 0)
        {
            flyCol.enabled = false;
            flyDead = true;
            StartCoroutine(Death());
        }
    }
}
