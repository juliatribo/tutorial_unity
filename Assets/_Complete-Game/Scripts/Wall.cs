using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public Sprite dmgSprite;
    public int hp = 4;
    //hit points
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void DamageWall (int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        //canvia l'sprite pel que esta pegant
        hp -= loss;
        //substract loss from the walls's current hit points total
        if (hp <= 0)
            gameObject.SetActive(false);
        //borrem paret quan hit points = 0
       
    }
}