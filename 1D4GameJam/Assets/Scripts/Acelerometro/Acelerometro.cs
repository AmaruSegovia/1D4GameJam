using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JuegoRuletaRusa : MonoBehaviour
{
    // Referencias a la UI
    public Button botonGirar;
    public Button botonDisparar;
    public Text textoResultado;
    public Text textoGirando;
    public Text textoDebugCamaras; // Para mostrar la cámara actual en el modo desarrollador
    public Text textoJugadorActual; // Nuevo texto para mostrar el jugador actual
    public Transform tamborTransform;

    // Sonidos
    [SerializeField] AudioSource sonidoGiro;
    [SerializeField] AudioSource sonidoDisparo;
    [SerializeField] AudioClip clipBala;
    [SerializeField] AudioClip[] clipClick;

    // Lista de nombres de jugadores
    private List<string> nombresJugadores = new List<string> { "Carpincho", "Gato", "Pinguino", "Zorro" };
    private int indiceJugadorActual; // Índice del jugador actual

    // Variables del juego
    private int puntuacion = 0; // Puntos acumulados por el jugador
    private bool estaGirando = false; // Indica si el tambor está girando
    private bool puedeDisparar = false;
    private int posicionActual = 0; // Cámara actual que se va a disparar
    private bool[] camaras = new bool[6]; // Representa las cámaras del tambor (true = bala)
    private float probabilidadBala = 0.2f; // Probabilidad inicial de que haya una bala (20%)

    //Referencias a los sprites de los cristales
    [SerializeField] private GameObject cristalBlanco;
    [SerializeField] private GameObject cristalVerde;
    [SerializeField] private GameObject cristalRojo;

    //Referencia al animator
    private Animator mangoAnimator;

    void Start()
    {
        // Configurar los botones y el texto al inicio
        botonGirar.onClick.AddListener(GirarTambor);
        botonDisparar.onClick.AddListener(Disparar);
        textoResultado.text = "";
        textoGirando.text = "";
        textoDebugCamaras.text = "";

        // Seleccionar un nombre aleatorio para iniciar
        indiceJugadorActual = Random.Range(0, nombresJugadores.Count);
        textoJugadorActual.text = $"Turno de: {nombresJugadores[indiceJugadorActual]}";

        mangoAnimator = GetComponent<Animator>();

        // Generar el estado inicial del tambor
        GenerarTambor();
    }

    void Update()
    {
        // Detectar si se agita el dispositivo para girar el tambor
        if (Input.acceleration.sqrMagnitude > 2.5f && !estaGirando)
        {
            GirarTambor();
        }
    }

    // Método para generar el tambor con balas basado en la probabilidad actual
    private void GenerarTambor()
    {
        for (int i = 0; i < camaras.Length; i++)
        {
            camaras[i] = Random.value < probabilidadBala;
        }
        MostrarEstadoCamaras();
    }

    public void GirarTambor()
    {
        if (!estaGirando)
        {
            sonidoGiro.Play();
            StartCoroutine(GirarTamborRutina());
        }
    }

    private IEnumerator GirarTamborRutina()
    {

        estaGirando = true;
        mangoAnimator.SetBool("EstaGirando", true);
        textoGirando.text = "Girando...";

        cristalBlanco.SetActive(true);
        cristalVerde.SetActive(false);
        cristalRojo.SetActive(false);

        float tiempoGiro = Random.Range(2f, 3f);

        for (float t = 0; t < tiempoGiro; t += Time.deltaTime)
        {
            tamborTransform.Rotate(Vector3.forward, 360 * Time.deltaTime);
            yield return null;
        }

        posicionActual = Random.Range(0, camaras.Length);

        estaGirando = false;
        mangoAnimator.SetBool("EstaGirando", false);

        puedeDisparar = true;

        textoGirando.text = "";

        MostrarEstadoCamaras();
    }

    public void Disparar()
    {
        if (puedeDisparar)
        {
            if (camaras[posicionActual])
            {
                textoResultado.text = $"¡Bala! {nombresJugadores[indiceJugadorActual]} ha perdido.";
                Handheld.Vibrate();
                sonidoDisparo.PlayOneShot(clipBala);

                cristalRojo.SetActive(true);
                cristalVerde.SetActive(false);
                cristalBlanco.SetActive(false);

                puedeDisparar = false;
            }
            else
            {
                puntuacion++;
                textoResultado.text = $"Click... ¡Salvaste, {nombresJugadores[indiceJugadorActual]}! Puntos: {puntuacion}";
                sonidoDisparo.PlayOneShot(clipClick[Random.Range(0, clipClick.Length)]);

                cristalVerde.SetActive(true);
                cristalRojo.SetActive(false);
                cristalBlanco.SetActive(false);

                // Pasar al siguiente jugador
                PasarAlSiguienteJugador();

                // Aumentar la probabilidad de bala y generar el nuevo tambor
                AumentarProbabilidad();
                GenerarTambor();
                puedeDisparar = false;
            }
        }
    }

    private void PasarAlSiguienteJugador()
    {
        // Incrementar el índice del jugador actual
        indiceJugadorActual++;

        // Si llegamos al final de la lista, volvemos al inicio
        if (indiceJugadorActual >= nombresJugadores.Count)
        {
            indiceJugadorActual = 0;
        }

        // Actualizar el nombre del jugador en la UI
        textoJugadorActual.text = $"Turno de: {nombresJugadores[indiceJugadorActual]}";
    }

    private void AumentarProbabilidad()
    {
        probabilidadBala = Mathf.Clamp(probabilidadBala + 0.05f, 0.2f, 0.9f);
    }

    private void MostrarEstadoCamaras()
    {
        string estadoCamaras = "Cámaras: ";
        for (int i = 0; i < camaras.Length; i++)
        {
            estadoCamaras += camaras[i] ? "[B]" : "[ ]";
        }

        textoDebugCamaras.text = $"{estadoCamaras}\nCámara Actual: {posicionActual + 1}\nProbabilidad: {probabilidadBala}";
    }
}
