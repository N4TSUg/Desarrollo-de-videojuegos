using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public int puntosVida = 3;

    void Start()
    {
    }

    void Update()
    {
    }

    public bool RecibirDaño(float cantidad)
    {
        puntosVida -= Mathf.RoundToInt(cantidad);

        if (puntosVida <= 0)
        {
            Destroy(this.gameObject);
            return true;
        }

        return false;
    }
}
