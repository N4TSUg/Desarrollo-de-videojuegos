using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class KunaiController : MonoBehaviour
{
    private string direccion = "Derecha";
    private float damage = 1f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    private Text enemigosMuertosText;
    private static int enemigosMuertos = 0;
    void Start()
    {
        // Initialize the Kunai object
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        enemigosMuertosText = GameObject.Find("EnemigosMuertos")?.GetComponent<Text>();
        Destroy(this.gameObject, 5f);
    }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {

            var enemigo = collision.GetComponent<ZombieController>();

            if (enemigo != null)
            {
                bool murio = enemigo.RecibirDaño(damage);
                if (murio)
                {
                    enemigosMuertos++;
                    if (enemigosMuertosText != null)
                    {
                        enemigosMuertosText.text = "ENEMIGOS MUERTOS: " + enemigosMuertos;
                    }
                }
            }
            Destroy(this.gameObject);
        }

    }

    public void SetDirection(string direction)
    {
        this.direccion = direction;
    }

    public void SetDamage(float d)
    {
        damage = d;
    }
}