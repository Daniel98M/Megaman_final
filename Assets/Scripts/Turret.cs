using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] float fireRate = 1f;
    [SerializeField] float range = 5f;
    [SerializeField] float hp;
    [SerializeField] GameObject bullet;
    private Player scriptPlayer;
    BoxCollider2D turrCol;
    Rigidbody2D turrBody;
    Animator turrAnim;
    bool onAim = false;
    private float nextFire = 0f;
    bool turrDead = false;
    // Start is called before the first frame update
    void Start()
    {
        turrCol = GetComponent<BoxCollider2D>();
        turrAnim = GetComponent<Animator>();
        turrBody = GetComponent<Rigidbody2D>();
        scriptPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Detection();
        Fire();
    }
    IEnumerator Death()
    {
        turrAnim.SetBool("isDead", true);
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
    }
    void Detection()
    {
        RaycastHit2D ray;
        transform.localScale = new Vector3(1, 1, 0);
        ray = Physics2D.Raycast(transform.position, Vector2.left, range, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, Vector2.left * range, Color.red);
        onAim = (ray.collider != null);
    }
    void Fire()
    {
        if (onAim && Time.time > nextFire && !turrDead && scriptPlayer.dead == false)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bullet, transform.position, transform.rotation);
        }
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
            turrCol.enabled = false;
            turrDead = true;
            turrBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(Death());
        }
        if (go.tag == "Ground")
        {
            turrBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
