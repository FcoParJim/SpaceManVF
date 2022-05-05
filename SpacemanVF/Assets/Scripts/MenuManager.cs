using UnityEngine;
using System.Collections;

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

    public void QuitGame() /*M�todo que hemos creado para que el juego salga al pulsar el bot�n Quit, sea la plataforma que sea en la que juguemos. 
                            * Hemos modificado el c�digo ya que daba error al construir el ejecutable. Este fragmento de c�digo s� funciona.*/
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }



}
