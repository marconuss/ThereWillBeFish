using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour
{

    public float speed;

    private Rigidbody2D rb2D;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent <Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();

    }

    void move()
    {
        float y = Input.GetAxis("Vertical");
        float moveVertical = y * speed;
        rb2D.velocity = new Vector2(rb2D.velocity.x, moveVertical);
    
    }
}
