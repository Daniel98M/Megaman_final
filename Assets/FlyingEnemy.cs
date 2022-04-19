using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    AIPath myPath;
    // Start is called before the first frame update
    void Start()
    {
        myPath = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }
    void ChasePlayer()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask("Player"));
        if (col != null)
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
                transform.localScale = new Vector2(-0.9f, 0.9f);
            }
            else if (dirH > 0)
            {
                transform.localScale = new Vector2(0.9f, 0.9f);

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
