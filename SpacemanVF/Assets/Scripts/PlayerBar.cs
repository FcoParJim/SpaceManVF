using UnityEngine;
using UnityEngine.UI;


public enum BarType //Declaramos un enumerado para tener en lista tanto la barra de man� como la barra de vida.
{
    healthBar,
    manaBar
}


public class PlayerBar : MonoBehaviour
{
    private Slider slider; //Variable para el Slider
    public BarType type; //Variable para identificar el tipo de Barra

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>(); //Invocamos la variable slider, recuperando el componente Slider con GetComponent
        switch (type)//Utilizamos el condicional switch para que elija una barra u otra en funci�n de lo que el Player recolecte
        {
            case BarType.healthBar://En el caso de la barra tipo healthBar,
                slider.maxValue = PlayerControllerVF.MAX_HEALTH; //el valor M�ximo en el Slider de Unity, es igual a la variable MAX_HEALTH de VS del playercontroller.
                break;
            case BarType.manaBar:
                slider.maxValue = PlayerControllerVF.MAX_MANA; //Mismo c�digo que Health.
                break;
        }
    }

    // Update is called once per frame
    void Update()

    /* Aqu� en el Update, colocamos este c�digo para que la barra se vaya actualizando constantemente,
    seg�n lo que el jugador vaya ganando de vida o perdi�ndola. */
    {
        switch (type)//Utilizamos el condicional switch para que elija una barra u otra en funci�n de lo que el Player recolecte.
        {
            /*Aqu� tenemos que llamar a la clase GameObject para que nos busque el Player de Unity y traernos el componente de la clase PlayerController, 
             * no podemos hacer como en el slider.maxValue = PlayerControllerVF.MAX.HEALTH, porque aqu� no son variables constantes (const), si no que son 
             * cambiantes en todo momento. La barra de vida ir� variendo seg�n un enemigo nos ataque, o tomemos una poci�n y recuperemos vida o Man�.
             * Por eso, despu�s de traernos el componente de la clase PlayerControllerVF, el PlayerController va a traerse los valores de vida para mostrarlos en la barra del juego.*/
            case BarType.healthBar://En el caso de la barra tipo healthBar,
                slider.value = GameObject.Find("Player").
                    GetComponent<PlayerControllerVF>().GetHealth();
                break;
            case BarType.manaBar:
                slider.value = GameObject.Find("Player"). //Mismo c�digo que Health.
                    GetComponent<PlayerControllerVF>().GetMana();
                break;
        }
    }
}
