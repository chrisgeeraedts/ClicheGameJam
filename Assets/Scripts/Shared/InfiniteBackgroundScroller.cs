using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackgroundScroller : MonoBehaviour
{
    private BoxCollider2D bc;
    private Rigidbody2D rb;

    private float width;
    public float speed = -3f;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = bc.size.x;
        rb.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 vector = new Vector2(width*2f, 0);
        transform.position = (Vector2)transform.position+vector;
    }
}
