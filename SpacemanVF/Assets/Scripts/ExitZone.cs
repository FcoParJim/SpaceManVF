using UnityEngine;

public class ExitZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //No lo vamos a necesitar.
    }

    // Update is called once per frame
    void Update()
    {
        //No lo vamos a necesitar.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*si el personaje entra dentro de la zona de colisi�n,
         * La instancia compartida(Singleton) de la clase LevelManager,
         * eliminar� un bloque (RemoveLevelBlock) y a�adir� un bloque nuevo (AddLevelBlock)
         * de modo que ir� reciclando, con uno eliminado, a�adir� uno nuevo en pantalla.*/

        if (collision.tag == "Player")
        {
            LevelManager.SharedInstance.AddLevelBlock();
            LevelManager.SharedInstance.RemoveLevelBlock();
        }
        
    }
}
