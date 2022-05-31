using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TbulletControl : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D bulletBody;
    // Start is called before the first frame update
    void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletBody.velocity = new Vector2(speed * (-1), bulletBody.velocity.y);
        transform.localScale = new Vector2(1f, 1f);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
    }
}
