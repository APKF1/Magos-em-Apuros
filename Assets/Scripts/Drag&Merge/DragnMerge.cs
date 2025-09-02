using UnityEngine;

public class DragAndDropSprite : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cameraPrincipal;

    private bool estaDentro = false;
    private bool estaArrastando = false;

    private void Awake()
    {
        // Se n�o for atribu�do no Inspector, pega automaticamente
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (cameraPrincipal == null)
            cameraPrincipal = Camera.main;
    }

    private void Update()
    {
        // Inicia arraste quando clicar dentro do colisor
        if (estaDentro && Input.GetKeyDown(KeyCode.Mouse0))
        {
            estaArrastando = true;
        }

        // Solta quando soltar o bot�o do mouse
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            estaArrastando = false;
        }
    }

    private void FixedUpdate()
    {
        if (estaArrastando)
        {
            // Converte posi��o do mouse para mundo
            Vector3 mousePos = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z; // mant�m o mesmo Z (importante em 2D)

            rb.MovePosition(mousePos);
        }
    }

    private void OnMouseOver()
    {
        estaDentro = true;
    }

    private void OnMouseExit()
    {
        estaDentro = false;
    }
}
