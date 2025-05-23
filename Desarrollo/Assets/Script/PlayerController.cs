using System;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    //Variables
    public GameObject kunaiPrefab;
    private string direccion = "Derecha";
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    public int kunaisDisponibles = 10;
    private bool puedeMoverseVerticalMente = false;
    private float defaultGravityScale = 1f;
    private bool puedeSaltar = true;
    private bool puedeLanzarKunai = true;
    public Text vidasText;
    private int vidas = 3;
    private bool estaCargandoKunai = false;
    private float tiempoInicioCarga = 0f;
    private float tiempoCargaMax = 5f;
    private float tiempoCarga = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Iniciando PlayerController");

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        defaultGravityScale = rb.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        SetupMoverseHorizontal();
        SetupMoverseVertical();
        SetupSalto();
        SetUpLanzarKunai();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();
            Debug.Log($"Colision con Enemigo: {zombie.puntosVida}");
            vidas--;
            vidasText.text = "VIDAS: " + vidas;
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro") {
            puedeMoverseVerticalMente = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro") {
            puedeMoverseVerticalMente = false;
            rb.gravityScale = defaultGravityScale;
        }
    }

    void SetupMoverseHorizontal() {
        rb.linearVelocityX = 0;
        animator.SetInteger("Estado", 0);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = 10;
            sr.flipX = false;
            animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -10;
            sr.flipX = true;
            animator.SetInteger("Estado", 1);
        }
    }

    void SetupMoverseVertical() {
        
        if (!puedeMoverseVerticalMente) return;
        rb.gravityScale = 0;
        rb.linearVelocityY = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocityY = 10;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocityY = -10;
        }
    }

    void SetupSalto() {
        if (!puedeSaltar) return;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.linearVelocityY = 12.5f;
        }
    }

    void SetUpLanzarKunai() {
        if (!puedeLanzarKunai || kunaisDisponibles <= 0) return;

        if(Input.GetKeyDown(KeyCode.K))
        {
            estaCargandoKunai = true;
            tiempoInicioCarga = Time.time;
        }

        if (!estaCargandoKunai) return;

        if(estaCargandoKunai)
        {
            tiempoCarga = Time.time - tiempoInicioCarga;

        }

        if (Input.GetKeyUp(KeyCode.K) && estaCargandoKunai)
        {
            estaCargandoKunai = false;

            float daño = 1f;
            Vector3 escala = new Vector3(1f, 1f, 1f); // Tamaño base

            if (tiempoCarga >= 5f)
            {
                daño = 3f;
                escala = new Vector3(4f, 4f, 3f); // Tamaño grande
            }
            else if (tiempoCarga >= 2f)
            {
                daño = 2f;
                escala = new Vector3(2f, 2f, 1f); // Tamaño medio
            }

            GameObject kunai = Instantiate(kunaiPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            kunai.GetComponent<KunaiController>().SetDirection(direccion);
            kunai.GetComponent<KunaiController>().SetDamage(daño);
            kunai.transform.localScale = escala;

            kunaisDisponibles -= 1;
            tiempoCarga = 0f;
        }
    }
}