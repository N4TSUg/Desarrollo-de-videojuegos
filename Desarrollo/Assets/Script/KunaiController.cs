using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KunaiController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    private string direccion = "Derecha";

    
    void Start()
    {
        //Initializate the kunai object
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
         // Update the Kunai object
        if (direccion == "Derecha")
        {
            rb.linearVelocityX = 15;
            sr.flipY = false;
            
        }
        else if (direccion == "Izquierda")
        {
            rb.linearVelocityX = -15;
            sr.flipY = true;
        }
    }

       void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        
    }

     public void SetDirection(string direction)
    {
        this.direccion = direction;
    }

}
