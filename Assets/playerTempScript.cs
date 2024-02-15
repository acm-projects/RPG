using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTempScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb; 
    private Vector2 movement; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); 
    }
}
