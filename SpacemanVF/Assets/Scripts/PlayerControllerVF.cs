using UnityEngine;

public class PlayerControllerVF : MonoBehaviour
{
    /*Variables del movimiento del personaje
    //Con la clase Rigidbody2D estamos declarando el tipo de cuerpo r�gido en 2D que en este caso en Unity hemos especificado que es Dynamyc. A este RigidBody2D le he 
    hemos llamado PlayerRigidBody
    Y hemos declarado tambi�n una capa de m�scara para el suelo, la cual hemos llamado groundMask, aplicada a todo aquello que sufrir�*/
    //Para modificar una animaci�n en nuestro videojuego por c�digo, haremos lo siguiente: Declarar una clase privada Animator con el nombre "animator"
    /*Referenciamos las dos variables bool de Unity aqu� de esta manera: private const string "       " = "     "; Donde "const" es una variable constante que no va a cambiar
    // Declaramos una variable p�blica llamada "runningSpeed" la cual es igual a 2 metros por segundo (o por frame) 2f. Al ser p�blica se puede editar
    en Unity en cualquier momento. 
    a lo largo del c�digo.*/

    public float jumpForce = 6f;
    public float runningSpeed = 3f;
    private Rigidbody2D PlayerRigidbody;
    private Animator animator;
    Vector3 startPosition; //Posici�n INICIAL de nuestro personaje al arrancar el juego.
    public float JumpRaycastDistance = 1.5f;
    public bool puedeSaltar = true; //Variable booleana para que el personaje salte SOLO UNA VEZ. (Implementada en el Update y en el m�todo IsTouchingInTheGround)


    SpriteRenderer spRd;



    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";

    [SerializeField]
    private int healthPoints, manaPoints; //Variables de tipo de dato int para la vida y el mana del personaje.

    //Variables de tipo de dato int constantes, no van a cambiar.
    public const int INITIAL_HEALTH = 100, INITAL_MANA = 15,
        MAX_HEALTH = 200, MAX_MANA = 30, MIN_HEALTH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5; //Variable constante para declarar el coste de manaPoints para ejecutar el SuperJump
    public const float SUPERJUMP_FORCE = 1.5f; //Variable constante para declarar cuantos metros extras podr� saltar hacia arriba de fuerza, con SuperJump.



    public LayerMask groundMask;

    //Aqu� es cuando todos los elementos, todos todos, despiertan en el juego. A diferencia de Start que es empezar el juego pero con todos los elementos despiertos ya.
    //Un simil ser�a con un coche. Cuando introducimos la llave de arranque ser�a Awake, cuando giramos la llave para arrancar ser�a Start.
    //Invocamos el GameObject "PlayerRigidBody" y cargamos la variable instanciada RidgiBody2D a trav�s del m�todo GetComponent para que a�ada el componente al PlayerRigidBody.
    //De la misma manera hacemos con Animator, que se instancia y se a�ade al Awake a trav�s del m�todo GetComponent.
    void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position; //Cada vez que arranque el juego,
                                                 //la posici�n del personaje quedar� anclada a la misma posici�n inicial. 

        spRd = GetComponent<SpriteRenderer>(); //Invocamos a la variable Sprd (sprite rendered) tray�ndonos el componente SpriteRenderer de nuestro personaje para que lo podamos utilizar al querer rotarlo.

    }

    /*Declaramos un m�todo derivado de Start, "StartGame" en el cual vamos a la posici�n
 * que queremos que nuestro personaje vuelva de manera inicial, una vez muera y se reinicie 
 * la partida.
 * Adem�s vamos a colocarlo a gravedad 0, de manera que cuando el personaje muere,
 * no siga cayendo a la velocidad tan grande que suele caer antes de colocarse
 * en la posici�n inicial que hemos predefinido.*/

    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);


        healthPoints = INITIAL_HEALTH;
        manaPoints = INITAL_MANA;


        // Unsafe way to get the method name

        Invoke("RestartPosition", 0.1f); /*Declaramos el m�todo Invoke(m�todo para retrasar un m�todo)
                                          * para que retrase el m�todo RestarPosition
                                          * donde contiene la posici�n a la que se reinicia nuestro personaje cuando muere,
                                          * y as� evitamos que reviva con la animaci�n de muerte, retrasando el reaparecer en su posici�n inicial, 
                                          * para que antes de reaparecer termine la animaci�n de morir.*/



    }

    private void RestartPosition()
    {
        this.transform.position = startPosition;
        this.PlayerRigidbody.velocity = Vector2.zero;

        /*Para que la c�mara no haga barrido al reiniciar la posici�n del personaje 
         * y sea instant�nea; instanciamos al GameObject que se llama "Main Camera" en Unity,
         * como mainCamera aqu� en el Script de VS.
         * Y nos traemos hacia mainCamera, el componente CameraFollow y el m�todo ResetCameraPosition.*/
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
        
    }

    // Update is called once per frame
    // Update is called once per frame (es decir, 60 veces por segundo, ya que nuestro juego correr� a 60 fps)
    // Este m�todo es para indicar tanto el Salto como SuperSalto, nosotros hemos arraigado dentro, tambi�n la programaci�n 
    //del salto normal, para que no salte infinitamente. 

    void Update()
    {
        if (Input.GetButtonDown("Jump") && puedeSaltar) /*Hemos mejorado el c�digo implementando la variable puedeSaltar. De manera que, 
                                                         * hemos indicado en la declaracion de la sentencia IF el presionar el bot�n "Jump" del InputManager M�S,
                                                         * la declaraci�n de la variable puedeSaltar en true, para que as� dentro de las llaves ({}) podamos definir
                                                         * la misma variable en False, de manera que saltar� solo una vez (puedeSaltar = false), y a su vez
                                                         * Jump(False) significa que no saltar� con SuperSalto, si no que saltar� normal.*/
        {
            Jump(false);
            puedeSaltar = false;
        }
        if (Input.GetButtonDown("SuperJump") && puedeSaltar)/* Si presionamos el bot�n del InputManager con el nombre SuperJump, que en este caso es el bot�n derecho del rat�n, la variable booleana SuperJump,
                                                             * declarada en el m�todo Jump de abajo, ser� verdadera y saltar� con SuperJump. Si es falsa, saltar� normal como siempre.
                                                             * Y adem�s tambi�n hemos corregido el SuperJump Infinito.*/
        {
            Jump(true);
            puedeSaltar = false;
        }
        



        /*Aqu� utilizamos el m�todo (isTouchingTheGround) que calcula la distancia del personaje hasta el suelo, cuando salta, para en este caso utilizar el resultado para configurar
          una variable del Animator.*/

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

        /*Vamos a dibujar un Gizmo propio, en concreto una recta que atraviese nuestro personaje para que detecte la distancia del 1.35f hasta d�nde llega.
        Para dibujar un Gizmo se suele utilizar el m�todo Update, as� lo tenemos localizado antes lanzar el juego a Producci�n.
        Invocamos la clase Debug (Algo as� como el Modo Prueba, para poder visualizar y probar cosas), llamamos al m�todo DrawRay (Dibujar rayo, para que
        nos dibuje una peque�a raya o rayo atravesando el personaje, desde el centro del personaje (this.transform.position)
        hacia abajo (vector2.Down) y se multiplica por la distancia al suelo desde el personaje; y el rayo ser� de color rojo.*/

        Debug.DrawRay(this.transform.position, Vector2.down * JumpRaycastDistance, Color.red);

        /*Si queremos variar la masa de nuestro personaje deberemos tambi�n variar el Jump Force del mismo, ya que contra mayor sea la masa de nuestro personaje,
        menor ser� aceleraci�n de su salto. F =m.a ; m = f/a;
        Si checkeamos la "Auto mass" del Inspector del Player en Unity, el motor gr�fico asignar� una masa relativa a su RididBody, pero no variar� autom�ticamente
        el Jump Force, por lo tanto al ser la auto mass de 2.39, registraremos un Jump Force de 20 y as� el personaje saltar� correctamente.*/

    }

    /*El FixedUpdate es como el Update pero a un ratio fijo. En vez de actualizarse cada
     * frame por segundo, se actualiza cada un intervalo fijo para que no sufra de lag el juego.*/

    private void FixedUpdate()
    {

        /*Si la velocidad de mi player es menor a runningSpeed, en caso de no llegar,
  * se la daremos nosotros. De manera que PlayerRigidBody.velocity es igual a
  * un nuevo vector.2 en el cual "runningSpeed" ser� el par�metro en el eje de la x
  * y en el eje de las Y, ser� PlayerRigidBody.velocity.y (para cuando 
  * el personaje salte y est� cayendo con algo de velocidad, la conserve a la hora de caer
  * y no haya un salto o lagueo entre un movimiento y otro.*/


        /*Invocamos un if y accederemos a la instancia compartida 
         * del GameManager, y de ah� al currentGameState. "S�lo
         * debo ejecutar el if que tengo debajo (if (playerRigidBody.velocity.x.etc))
         * si el currentGameState es el de la partida (inGame).
         * Colocamos un else que dir�a lo siguiente (Y si el currentGameState no est�
         * en el estado de partida (inGame), y est� en (Menu) o (GameOver),
         * el personaje deja de moverse y se para -->
         * --> PlayerRigidbody.velocity = new Vector2(0, PlayerRigidbody.velocity.y);*/

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (PlayerRigidbody.velocity.x < runningSpeed)
            {
                PlayerRigidbody.velocity = new Vector2(runningSpeed, //x
                    PlayerRigidbody.velocity.y); //y
            }
        }
        else //Si no estamos dentro de la partida
        {
            PlayerRigidbody.velocity = new Vector2(0, PlayerRigidbody.velocity.y); //El personaje caer� con velocidad 0 en el eje X(no camina)

        }

        //3. Movimiento horizontal

        /* Declaramos una variable float llamada MovimientoH que es igual al input (entrada) con el m�todo GetAxisRaw (M�todo
        que nos devuleve(-1) si pulsamos la tecla con valor negativo(lefth o A), es decir, se mover� a la izquierda porque
        as� es como Unity lo tiene configurado por defecto. 
       *En cambio si nos devuelve un valor positivo(1), ser� porque pulsamos la tecla con valor positivo(right o D),
       *es decir, se mover� a la derecha.

        * Por �ltimo, nos puede devolver 0 cuando no pulsemos ninguna tecla, y eso ser� porque no se mueve ni a derecha ni a izquierda.*/
        /*Entonces el PlayerRigidBody.velocity es igual a un nuevo vector2 donde nuestra variable movimientoH declarada,
         * se multiplicar� por la velocidad del personaje en el eje X, y adem�s nos devolver� la velocidad del PlayerRigidbody en el eje Y.*/

        float movimientoH = Input.GetAxisRaw("Horizontal");
        PlayerRigidbody.velocity = new Vector2(movimientoH * 3, PlayerRigidbody.velocity.y);

        //4. Sentido horizontal (para girar la posici�n del jugador)

        /*Si al pulsar la tecla que estemos pulsando en ese momento, nos devuleve un valor de retorno mayor que 0,
         * el flip del SpriteRendered (el movimiento del personaje movi�ndose hacia la derecha en el eje de las X) ser� FALSO, 
         * ya que, el flip no trabajar� en ese momento, por lo tanto no cambiar� la direcci�n del personaje al caminar,
         * siendo la opci�n de tecla positiva, caminar hacia la derecha.
         * En cambio, si movimientoH es menor que 0, el flip trabajar� en ese momento y voltear� automaticamente, 
         * la direcci�n del personaje al caminar, por lo tanto el valor del flip ser� true.*/

        if (movimientoH > 0)
        {
            spRd.flipX = false;
        }
        else if (movimientoH < 0)
        {
            spRd.flipX = true;
        }
    }

    /* Dentro del m�todo Jump(), �qui�n es el encargado de responder a acciones f�sicas?, PlayerRigidBody. Por lo tanto invocamos a PlayerRigidBody, y le aplicamos una fuerza; 
   * invocando el m�todo AddForce, la cual tendr� que ir hacia arriba, por lo tanto se lo indicamos con Vector2.up, que se multiplica por la fuerza del salto (6F); y un segundo parametro
   * que es el modo en el que se aplica la fuerza (ForceMode2D.Impulse), que ser�a aplic�ndola de una sola vez con un impulso, no con una fuerza constante.*/
    //Cuando invocamos el m�todo Jump no deber�a aplicar la fuerza vertical a menos que el personaje est� tocando el suelo. 
    //Hemos a�adido un IF con el m�todo IsTouchingTheGround() ->Raycast<- (Estoy tocandpo el suelo) para que: Si el personaje est� tocando el suelo, aplica la fuerza vertical declarada en Jump. 
    /*Hemos modificado el m�todo Jump, para poder incluir el Super salto (bool SuperJump). De manera que, si es SuperJump, decrementamos (-=) los manaPoints a la variable SUPERJUMP_COST,
     * ya que ha habido un coste de manaPoints para ejecutar SuperJump. Y esto ser� posible si se ejecuta SuperJump y adem�s (&&) los manaPoints son iguales o superiores al SUPERJUMP_COST,
     * si tenemos m�s puntos de man� de lo que cuesta ejecutar SuperJump. En caso de no tener suficientes puntos de man�, no se ejcutar�.*/
    void Jump(bool SuperJump)
    {
        float jumpForceFactor = jumpForce; /*Declaramos una variable JumpForceFactor que ser� el multiplicador de la fuerza del SuperJump. Por defecto ser� igual a JumpForce (la fuerza estandar de salto).
                                            * Pero si la introducimos dentro del if, y el if se ejecuta correctamente, el JumpForceFactor va a multiplicarse por SUPERJUMP_FORCE
                                            * y as� se ejecutar� bien la fuerza del SuperJump.*/

        if (SuperJump && manaPoints >= SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
        GetComponent<AudioSource>().Play(); //A�adimos aqu� en el m�todo del salto, ya sea para salto estandar o super salto, el sonido para que se reproduzca justo cuando el personaje salta.
        PlayerRigidbody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
    }


    //Nos indica si el personaje est� tocando o no el suelo.
    /* Vamos a declarar un m�todo para calcular la distancia de choque contra el suelo (groundMask) que nos devolver� un valor booliano respondiendo
     * a la pregunta �est� tocando el suelo?. 
     * Dentro de IsTouchingTheGround ponemos un if, e invocamos al motor de Physisc2D.Raycast (Raycast es el m�todo para indicar que vamos a trazar un rayo invisible
     en Unity), desde la posici�n actual (this.transformn.position) hacia abajo (vector2.down) hacia una distancia m�xima de 20 cms (o.2f) para que choque contra la capa groundMask
    la capa del suelo que hemos declarado)*/
    /*ATENCION BUG : Hemos observado que con la distancia de 0.2f el personaje no salta, por lo tanto hemos calculado 1.35f metros ya que la posici�n declarada en
    (this.transform.position) es desde el centro del personaje, a la altura del bolsillo, y no es suficiente 20 centimetros.*/
    //Si se cumple toda la sentencia IF, es que estamos tocando el suelo.
    //TODO (TODO significa ToDo en ingles, para hacer m�s tarde)
    //A la pregunta IsTouchingTheGround, si la respuesta es "S�" nos devolver� un valor true. Si la respuesta es "No", nos devolver� un valor false. 
    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, JumpRaycastDistance, groundMask)) /*He implementado la variable booleana puedeSaltar en este m�todo ��Est� tocando el suelo?��
                                                                                                        * para que cuando sea true (est� tocando el suelo), puedeSaltar sea true y salte,
                                                                                                        * y cuando sea false (no est� tocando el suelo, est� en el aire), puedeSaltar sea false, 
                                                                                                        * y no salte infinitamente.*/
        {
            puedeSaltar = true;
            return true;
        }
        else
        {
            puedeSaltar = false;
            return false;
        }
    }

    

    /*M�todo p�blico para que desde la "Kill Zone" se pueda invocar.*/
    /*Ahora invocamos dentro del m�todo Die(); la animaci�n de muerte
     *mediante las variables booleanas que declaramos. (STATE_ALIVE);
     *que en este caso, ser� falsa ya que el personaje no estar� vivo.
     *Adem�s tendremos que comunicar al GameManager como buen Gobernador
     *de nuestro juego, que el estado del juego ser� GameOver.*/
    public void Die()
    {
        float travelledDistance = GetTravelledDistance(); /*Variable de tipo de dato float para declarar aqu� en el metodo Die, 
                                                           * la distancia que recorre el personaje*/
        float PreviousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0); /* Variable de tipo de dato float para declarar la distancia previa m�xima.
                                                                       * Invocando a la clase PlayerPrefs, guardar� esa distancia previa m�xima,
                                                                       * y la devolver� como datos float a nuestro maxscore.*
                                                                       * Incorporamos un valor por defecto, para la primera vez que se ejecute, de manera
                                                                       * que la primera puntuaci�n ser� 0.*/

        if (travelledDistance > PreviousMaxDistance) /*Si la distancia recorrida es mayor que la distancia previa m�xima guardada en la clase PlayerPrefs,*/

        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);/*Coloca los valores float (SetFloat) guardados en la clase PlayerPrefs, comp�ralos con los actuales,
                                                                 * y mu�stralos ACTUALIZADOS en el maxscore.*
                                                                 * En definitiva, compara la puntuaci�n m�xima entre una partida y otra y actualiza la m�xima distancia.*/

        }

            this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
        
    }

    public void CollectHealth(int points) //M�todo para recolectar puntos de vida
    {
        this.healthPoints += points; /*Incrementamos los healthPoints actuales (puntos de vida) 
                                      * a tantos puntos que  devuelva el m�todo CollectHealth*/
        /*Para que el personaje no recolecte puntos de vida y tenga vida infinita, pondremos un corte.
         * de manera que si healthPoints (vida) son mayores o igual al MAX_HEALTH, estos puntos de vida,
         * son iguales a MAX_HEALTH.*/

        if (this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }

        if (this.healthPoints <= 0) //Si los puntos de vida son menos o igual a 0, mata al personaje.
        {
            Die();
        }

    }
    public void CollectMana(int points)//M�todo para recolectar puntos de man� y misma explicaci�n que Health.
    {
        this.manaPoints += points;

        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance() //M�todo para obtener la distancia que recorre nuestro personaje
    {
        return this.transform.position.x - startPosition.x; /*Devolver el dato resultante 
                                                             * de la diferencia entre el lugar en el que me hayo en el eje de las x
                                                             * y el punto de origen en el mismo eje de las x.*/
    }
}
