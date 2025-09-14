using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class Play : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configurações do Botão")]
    [SerializeField] private Button playButton;
    
    [Header("Efeito de Brilho")]
    [SerializeField] private bool enableGlowEffect = true;
    [SerializeField] private float glowIntensity = 1.5f;
    [SerializeField] private float glowSpeed = 2f;
    [SerializeField] private Color glowColor = Color.white;
    
    [Header("Configurações de Cena")]
    [SerializeField] private string gameSceneName = "Level 1";
    [SerializeField] private bool useSceneIndex = false;
    [SerializeField] private int sceneIndex = 1;
    
    [Header("Som (Opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    private Color originalColor;
    private Image buttonImage;
    private bool isGlowing = false;
    private Coroutine glowCoroutine;

    void Start()
    {
        // Se o botão não foi atribuído no inspector, tenta encontrar no mesmo GameObject
        if (playButton == null)
            playButton = GetComponent<Button>();
        
        // Pega a imagem do botão para o efeito de brilho
        buttonImage = playButton.GetComponent<Image>();
        if (buttonImage != null)
            originalColor = buttonImage.color;
        
        // Adiciona o listener ao botão
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayButtonPressed);
    }

    // Eventos do mouse para o efeito de brilho
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (enableGlowEffect)
            StartGlowing();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (enableGlowEffect)
            StopGlowing();
    }

    void StartGlowing()
    {
        if (glowCoroutine != null)
            StopCoroutine(glowCoroutine);
        
        isGlowing = true;
        glowCoroutine = StartCoroutine(GlowEffect());
    }

    void StopGlowing()
    {
        isGlowing = false;
        
        if (glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
            glowCoroutine = null;
        }
        
        // Volta à cor original
        if (buttonImage != null)
            buttonImage.color = originalColor;
    }

    IEnumerator GlowEffect()
    {
        while (isGlowing)
        {
            // Calcula o brilho usando seno para um efeito suave
            float glow = Mathf.Sin(Time.unscaledTime * glowSpeed) * 0.5f + 0.5f;
            
            // Interpola entre a cor original e a cor de brilho
            Color currentColor = Color.Lerp(originalColor, glowColor, glow * glowIntensity);
            
            if (buttonImage != null)
                buttonImage.color = currentColor;
            
            yield return null;
        }
    }

    public void OnPlayButtonPressed()
    {
        Debug.Log("Iniciando o jogo...");
        
        // Toca som se configurado
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
        
        // Para o efeito de brilho
        StopGlowing();
        
        // Carrega a cena
        StartGame();
    }

    void StartGame()
    {
        if (useSceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    void OnDestroy()
    {
        // Para o efeito de brilho
        StopGlowing();
        
        // Remove listeners para evitar erros
        if (playButton != null)
            playButton.onClick.RemoveListener(OnPlayButtonPressed);
    }
}
