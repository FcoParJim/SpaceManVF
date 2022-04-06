using UnityEngine;

public enum CollectableType //Creamos un enumerado para tener en un mismo Script los coleccionables de pocion, mana y monedas
{
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money; /*Variable de tipo CollectableType para indicar 
                                                          * que la moneda es un tipo de coleccionable. 
                                                          * En Unity estar� en el desplegable del Script Collectable
                                                          * que hemos asignado a Coin.*/

    private SpriteRenderer sprite; //Para acceder por c�digo a la renderizaci�n del Sprite
    private CircleCollider2D itemcollider;//Para acceder por c�digo al collider de la coin

    bool hasBeenCollected;

    public Collectable(bool hasBeenCollected)
    {
        this.hasBeenCollected = hasBeenCollected;
    }

    public int value = 1;// Valor que proporcionar� la recolecci�n al Player, o 1 coin, o 1 punto de vida, o de man�.

    GameObject player; //Declaramos una variable player con la clase GameObject

    private void Awake()
    /*Instanciamos en el Awake que vamos a obtener de las variables privadas,
     * la componente SpriteRenderer y asignarla con el nombre Sprite,
     * y la componente CircleCollider2D con el nombre itemCollider.*/
    {
        sprite = GetComponent<SpriteRenderer>();
        itemcollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = GameObject.Find("Player"); /*Invocamos la variable player y la instanciamos desde la clase GameObject
                                             * utilizando el m�todo Find y recuperamos el PlayerController de Unity, aqu� en el Script.*/
    }


    void Show() //M�todo para mostrar el coleccionable

    {
        sprite.enabled = true; //El sprite en True para que se vea
        itemcollider.enabled = true; //el collider en true para que est� activo
        hasBeenCollected = false; //la variable hasBeenCollected en false ya que aun no se ha recolectado.
    }

    void Hide() //M�todo para ocultar el coleccionable
    {
        sprite.enabled = false; //sprite en false para que no se vea
        itemcollider.enabled = false; //el collider en false para que no est� activo
    }

    void Collect() //M�todo para que el coleccionable se oculte cuando ya ha sido colectado
    {
        Hide(); //M�todo Hide para ocultar el colecionable
        hasBeenCollected = true; //variable en true ya que el colecionable ha sido colectado

        switch (this.type) /*Utilzamos el condicional Switch para que CollectableType cambie 
                            * entre un caso (case) y otro seg�n lo que vayamos a programar en cada caso
                            * (money, healthPotion, manaPotion)*/
        {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play(); /*Para que el sonido de la moneda suene unicamente cuando se recolecta,
                                                     * recuperamos la componente AudioSource con el m�todo Play para que se reproduzca el sonido en ese momento.
                /*Notificamos al GameManager de la recolecci�n de la moneda.
                 * Llamamos al CollectObject del GameManager de manera que, yo mismo (this)
                 * soy un objeto de tipo recolectable(CollectableType) de tipo de moneda,
                 * y voy a ayudar a incrementar el contador (CollectedObject)*/
                break;
            case CollectableType.healthPotion:
                player.GetComponent<PlayerControllerVF>().CollectHealth(this.value);/*Recuperamos el componente PlayerController
                                                                                     * y va a coleccionar puntos de vida, en este caso el mismo valor de la moneda (this)*/
                break;
            case CollectableType.manaPotion:
                player.GetComponent<PlayerControllerVF>().CollectMana(this.value);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //M�todo Trigger 2D para la moneda
    {

        if (collision.tag == "Player")/*Si el objeto con etiqueta Player colisiona
                                      * con el GameObject de este script, colecci�nalo.
                                      * En este caso, la moneda se ocultar� cuando el jugador
                                      * choque con ella.*/

        {
            Collect(); //Utilizamos el m�todo Collect para que haga lo arriba explicado. 
        }
    }
}
