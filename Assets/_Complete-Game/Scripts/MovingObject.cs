using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
        //raycasthit2d entra buit i surt amb info
    {
        Vector2 start = transform.position;
        //convertim vector 3 en vector 2
        Vector2 end = start + new Vector2(xDir, yDir);
        //calcula pos final

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        //fa una linia de la pos inicial a la final 
        boxCollider.enabled = true;

        if (hit.transform == null)
        //si la linia hit no ha colisionat amb res
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;

    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
            //waits for a frame before evaluating the condition of the loop

        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
        //aquest component es generic pq si es un enemic es tractara del player i si es el player es tractara d'una paret
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;
                //no s'executa el code si la funcio move ens informa de que no ha xocat

        T hitComponent = hit.transform.GetComponent<T>();
        //ens dona la referencia del component de tipus t attached al objecte amb el que sha xocat

        if (!canMove && hitComponent != null)
            //si no es pot moure i l'objecte amb el que ha tocat no te ref null, per tant es pot interactuar amb ell (si fos una paret externa no shi pot interactuar)
            OnCantMove(hitComponent);

    }

    protected abstract void OnCantMove <T> (T component)
        where T : Component;
    
}
