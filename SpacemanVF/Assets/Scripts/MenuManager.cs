using UnityEngine;

public class MenuManager : MonoBehaviour
{
    /*Invocamos una clase p�blica Canvas para crear una variable menuCanvas 
     * dentro de nuestro Script MenuManager para que gestione 
     * todos los objetos del Canvas en Unity.*/

    //Creamos un Singleton que ser� el gobernador de este Script MenuManager
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;
    

    private void Awake()
    {
        /*Declaramos el sharedInstance como en los dem�s Script, de manera 
         * que si llegamos aqu� y no est� asignado el sharedInstance, se asigna a este Script.*/

        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public void ShowMainMenu() //M�todo para mostrar el men� en el juego
    {
        menuCanvas.enabled = true;
    }

    public void HideMainMenu()//M�todo para ocultar el men� en el juego.
    {
        menuCanvas.enabled = false;
    }

    public void ShowGameMenu()
    {
        gameCanvas.enabled = true;
    }

    public void HideGameMenu()
    {
        gameCanvas.enabled = false;
    }

    public void ShowGameOverMenu()
    {
        gameOverCanvas.enabled = true;
        
    }

    public void HideGameOverMenu()
    {
        gameOverCanvas.enabled = false;
    }

    public void ExitGame()
    {
        /*Aqu� hemos declarado unos "if" "regionales" por as� decirlo, para forzar la salida en cualquier plataforma que se est� jugando el juego.
         * La sintaxis es la siguiente: Si estamos en el EDITOR DE UNITY, para forzar la salida del juego (darle al bot�n STOP del emulador de Unity)
         * el reproductor del editor estar� en false, es decir, no correr�. Y si no, si nos encontramos en cualquier plataforma que no sea el editor de Unity,
         * utilizamos el m�todo Aplication.Quit();. Y finalizamos la sentencia "if" con un #endif.*/

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//Este c�digo est� m�s intenso que Aplication.quit porque es el que se va a ejecutar en el editor de Unity.
#else
Aplication.Quit(); //Si compilasemos para playstation, PC, o moviles el c�digo que resaltar�a ser�a este porque es el que se ejecutar�a.
#endif

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
}
