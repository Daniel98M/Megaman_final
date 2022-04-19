using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TbulletControl : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D bulletBody;
    private Player scriptPlayer;
    // Start is called before the first frame update
    void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
        scriptPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletBody.velocity = new Vector2(speed * (-1), bulletBody.velocity.y);
        transform.localScale = new Vector2(1f, 1f);
        if (scriptPlayer.dead == true)
        {
            bulletBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
    }
}
