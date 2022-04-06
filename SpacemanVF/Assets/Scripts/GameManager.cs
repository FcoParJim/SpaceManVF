using UnityEngine;

/*Creamos un Enumerado (Enum), y �sto es un tipo de variable que 
 * contiene varios estados. P.Ej; en una br�jula, la estrella o punto cardinal
 * ser�a el Enumerado, y Norte, Sur, Este u Oeste ser�an los posibles estados del Enumerado.
 * Vamos a declarar un Enum con los 3 posibles estados del videojuego, y adem�s
 * declararemos una variable para saber en cual de los 3 estados se encuentra el videojuego.*/
public enum GameState
{
    menu,
    inGame,
    gameOver
}
public class GameManager : MonoBehaviour
{

    /*Declaramos una variable publica de tipo GameState a la que llamaremos currentGameState
 * (Estado actual del juego) el cual iniciaremos con GameState.menu, ya que
 * el jugador al comenzar el juego tendr� el men� para comenzar una partida nueva.*/

    public GameState currentGameState = GameState.menu;

    /*Creamos una variable static de la clase GameManager, cuyo nombre es sharedInstance (Instancia compartida).
     * Este ser� el nombre del singleton y ser� el unico
     *que se instancie  como GameManager propiamente dicho,
     * para que no haya otros GameManager 
     * que entren en conflicto.*/

    public static GameManager sharedInstance;

    /*Declaramos la variable "PlayerController" en esta actual clase "GameManager", 
   * y la declaramos de manera privada y con el nombre "controller", de tal modo 
   * que s�lo se puede modificar por c�digo aqu� en Visual Studio.*/
    private PlayerControllerVF controller;

    public int collectedObject = 0; //Contador de cuantos objetos llevamos recolectados.

    //Declaramos el m�todo Awake, y dentro de �l vamos a instanciar el Singleton llamado "SharedInstance"
    /*Si (if) el sharedInstance es igual, igual a null (si la instancia compartida no ha sido asignada previamente,
     * la asginas al script actual (GameManager). De modo que la primera vez que se llega al SharedInstance,
     * como no ha sido inicializado, es nulo, por lo tanto esa variable ejecuta que se le asgine el SharedInstance
     * al GameManager. El 2� que llegue al SharedInstance, ya no obtendr� el nivel nulo, porque ya est� asignada.*/
    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        /*Controller es igual a que la clase GameObject localice (Find) un objeto.
        *Como no se puede convertir implicitamente la clase GameObject en "Player"
        *(el nombre de nuestro GameObject de personaje en Unity, definido en Hierarchy, panel Jerarqu�a)
        * tenemos que obtener el componente PlayerController del GameObject para que el casting sea posible.
        * De modo que invocamos al m�todo "GetComponent" con el nombre de la clase que ya est� declarada
        * arriba del Awake, "PlayerController", por lo que ahora s� es posible el casting impl�cito.
        * Adem�s, de esta manera el Game Manager tiene localizado a nuestro PlayerController, enlazado con Unity*/


        controller = GameObject.Find("Player").
            GetComponent<PlayerControllerVF>();
        SetGameState(GameState.menu);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && currentGameState !=
             GameState.inGame)/*Con el && (Ampersand) solucionamos el BUG
                              *que inici�bamos la partida infinitamente despu�s de morir
                              *Ahora s�lo podremos iniciar la partida, si (if) pulsamos 
                              *"Submit" que est� asociado al bot�n "Enter" y a la misma vez,
                              *si se cumple la segunda condici�n booleana que es:
                              *Que el GameManager tenga un estado(currentGameState)
                              *distinto(!=) al de "Partida"*/
        {
            StartGame();
        }
       

    }

    public void StartGame() //M�todo para iniciar el juego
    {
        SetGameState(GameState.inGame);
    }

    public void GameOver() //M�todo para finalizar una partida
    {
        SetGameState(GameState.gameOver);
        
    }

    public void BackToMenu() //M�todo para devolvernos al men� principal
    {
        SetGameState(GameState.menu);
    }

    /* Declaramos un nuevo m�todo, con el par�metro GameState, el cual �ste
  * identificar� el estado actual del juego, y newGameState ser� 
  * el nuevo estado al que pasar� el juego seg�n pasen las cosas (menu, inGame, gameOver)*/
    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu) //Si el nuevo estado del juego es menu, colocar el menu
        {
            //TODO: colocar la l�gica del men�
            
            MenuManager.sharedInstance.HideGameMenu();
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.ShowMainMenu(); /*Invocamos al singleton del script MenuManager,
                                                       *para declarar aqu� en el script GameManager,
                                                       *el m�todo ShowMainmenu mientras el juego est�
                                                       *en el estado menu.*/

        }
        else if (newGameState == GameState.inGame) // si el nuevo estado es inGame, colocar el escenario de juego
        {
            LevelManager.SharedInstance.RemoveAllLevelBlocks(); //Limpizamos la escena de bloques antiguos antes de crear los nuevos.
            LevelManager.SharedInstance.GenerateInitialBlocks(); //Genera los bloques iniciales.

            controller.StartGame(); /* El PlayerControllerVF(Player en Unity) comienza una partida.
                                     * Tambi�n comienza una partida despu�s de haber limpiado la escena de bloques antiguos, 
                                     * y haber generado los iniciales.*/

            
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.HideMainMenu();/*Invocamos al singleton del script MenuManager,
                                                       * para declarar aqu� en el script GameManager,
                                                       * el m�todo HideMainmenu mientras el juego est�
                                                       * en el estado inGame.*/
            MenuManager.sharedInstance.ShowGameMenu();


        }
        else if (newGameState == GameState.gameOver) // si el nuevo estado es gameOver, preparar el juego para gameOver
        {
            //TODO: preparar el juego para el Game Over
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameMenu();
            MenuManager.sharedInstance.ShowGameOverMenu();
            

        }

        /* con esta linea de codigo, decimos lo siguiente:
         * la variable currentGameState, despu�s de todos los cambios con los "If"
         * ser� actualizada por newGameState. De manera que currentGameState es la variable
         * del GameManager y que newGameState es el par�metro le pasamos a currentGameState y modifica el GameManager.*/

        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)/*M�todo CollectObject, que va a incrementar el contador 
                                                       * que hemos declarado arriba como "CollectedObject"*/
    {
        collectedObject += collectable.value; /* //Con esta funci�n vamos a incrementar (+=) en el valor
                                               * que hayamos definido en la variable public int value. definida
                                               * en el script Collectable. (en este caso 1), el contador de objetos recolectados,
                                               * (collectedObject).*/
    }


}
