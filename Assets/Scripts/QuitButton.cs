using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class QuitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configurações do Botão")]
    [SerializeField] private Button quitButton;
    
    [Header("Efeito de Brilho")]
    [SerializeField] private bool enableGlowEffect = true;
    [SerializeField] private float glowIntensity = 1.5f;
    [SerializeField] private float glowSpeed = 2f;
    [SerializeField] private Color glowColor = Color.white;
    
    [Header("Confirmação (Opcional)")]
    [SerializeField] private bool showConfirmation = true;
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button confirmYesButton;
    [SerializeField] private Button confirmNoButton;

    private Color originalColor;
    private Image buttonImage;
    private bool isGlowing = false;
    private Coroutine glowCoroutine;

    void Start()
    {
        // Se o botão não foi atribuído no inspector, tenta encontrar no mesmo GameObject
        if (quitButton == null)
            quitButton = GetComponent<Button>();
        
        // Pega a imagem do botão para o efeito de brilho
        buttonImage = quitButton.GetComponent<Image>();
        if (buttonImage != null)
            originalColor = buttonImage.color;
        
        // Adiciona o listener ao botão
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        
        // Configura botões de confirmação se existirem
        if (confirmYesButton != null)
            confirmYesButton.onClick.AddListener(QuitGame);
        
        if (confirmNoButton != null)
            confirmNoButton.onClick.AddListener(CancelQuit);
        
        // Esconde o painel de confirmação no início
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
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

    public void OnQuitButtonPressed()
    {
        if (showConfirmation && confirmationPanel != null)
        {
            // Mostra painel de confirmação
            confirmationPanel.SetActive(true);
            
            // Pausa o jogo (opcional)
            Time.timeScale = 0f;
        }
        else
        {
            // Sai diretamente
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        
        #if UNITY_EDITOR
            // No editor do Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // No build final
            Application.Quit();
        #endif
    }

    public void CancelQuit()
    {
        // Esconde o painel de confirmação
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
        
        // Retoma o jogo
        Time.timeScale = 1f;
    }

    void OnDestroy()
    {
        // Para o efeito de brilho
        StopGlowing();
        
        // Remove listeners para evitar erros
        if (quitButton != null)
            quitButton.onClick.RemoveListener(OnQuitButtonPressed);
        
        if (confirmYesButton != null)
            confirmYesButton.onClick.RemoveListener(QuitGame);
        
        if (confirmNoButton != null)
            confirmNoButton.onClick.RemoveListener(CancelQuit);
    }
}