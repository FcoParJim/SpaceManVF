using UnityEngine;
using UnityEngine.UI; //Es la librer�a donde est�n los paquetes de assets para las UI (Interfaces de ususario)

public class GameView : MonoBehaviour
{
    public Text coinText, scoreText, maxScoreText, ScoreTotalText, CoinsTotalText;

    private PlayerControllerVF controller;

    // Use this for initialization
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerControllerVF>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) /*Si el estado de juego del GameManager es ingame, 
         estos son los "playholders" a rellenar con sus respectivas cantidades num�ricas.*/
        {
            int coins = GameManager.sharedInstance.collectedObject; /*las monedas son int porque ser�n siempre n�s enteros, y no habr� ocasi�n de que sean decimales.
                                                                     El n� de coins es igual al contador (collectedObject) que est� en la instancia compartida del GameManager.*/


            float score = controller.GetTravelledDistance(); /*el score es valor float porque pueden ser n� enteros y decimales, ya que son metros recorridos durante la partida.
                                                               Ahora el Score es igual a la distancia que ha recorrido nuestro personaje. 
                                                              * (controller.GetTravelledDistance(); Lo ponemos en el Update para que vaya actualiz�ndose
                                                              * constantemente. Para ahorrarnos el GetComponent, ya tenemos referenciado arriba al PlayerController, 
                                                              * Y as� s�lo tenemos que invocar a "Controller" en el update.*/


            float maxScore = PlayerPrefs.GetFloat("maxscore", 0); /*Nos traemos la clase Playerprefs con el m�todo GetFloat, 
                                                                  * donde el valor de maxscore por defecto ser� 0 para la primera partida.*/


            coinText.text = coins.ToString(); /*Para que aparezca el n� de coins (valor n�merico) en la variable coinText (valor string) de Unity en la pantalla de juego,
                                               * hay que convertir ese valor n�merico a string con ToString.*/

            scoreText.text = "Score: " + score.ToString("f1"); /*Aqu� el valor score(vanor n�merico)
                                                              *lo convertimos a string tambi�n precediendo
                                                              * el texto Score que aparecer� en la pantalla de juego.
                                                              * Adem�s dentro del m�todo Tostring hay un par�metro que
                                                              *permite personalizar cuantos decimales quieres que aparezcan;
                                                              *en nuestro caso queremos que sea uno(f1), si fueran 2(f2, etx) */

            maxScoreText.text = "MaxScore: " + maxScore.ToString("f1"); // Igual que Score
        }
        if (GameManager.sharedInstance.currentGameState == GameState.gameOver) /*En este caso hemos declarado las variables int coins y float score iguales que en el inGame.
                                                                                * Pero con la diferencia de que hemos declarado dos variables nuevas para la pantalla de gameOver;
                                                                                * una variable ScoreTotalText que nos mostrar� la puntuaci�n total obtenida en la partida,
                                                                                * y una condici�n Coinstotaltext que nos mostrar� el m�ximo de monedas obtenidas en la partida.
                                                                                * ATENCI�N, para que Unity no nos de error de referencia de objetos, hay que asignar los objetos de 
                                                                                * la jerarqu�a de Unity, a las casillas de este mismo Script, aunque no se utilicen como por ejemplo,
                                                                                * aqu� en el gameOver el maxScoreText , y al rev�s en el gameCanvas, con las dos variables declaradas aqu�
                                                                                * en el gameOver.*/
        {
            int coins = GameManager.sharedInstance.collectedObject;
            float score = controller.GetTravelledDistance();
            
            ScoreTotalText.text = "Score " + score.ToString("f1");
            CoinsTotalText.text = coins.ToString();
        }
    }

}
