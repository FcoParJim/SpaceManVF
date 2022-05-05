using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //M�todo onTriggerEnter2D para que cuando se pose el personaje en la plataforma, �sta automaticamente arranque a moverse.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Animator animator = GetComponent<Animator>(); /*Invocamos la clase Animator de Unity aqu� en VS con el nombre animator para que VS la reconozca como una variable, 
                                                       * y a su vez nos traemos la propia clase Animator de Unity con el m�todo GetComponente para tenerla aqu� en VS.*/
        animator.enabled = true; //el animator estar� activo por defecto.
    }
}
