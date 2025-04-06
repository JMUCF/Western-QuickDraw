using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite shootSprite;
    public Sprite deadSprite;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
    }

    public void Shoot()
    {
        sr.sprite = shootSprite;
    }

    public void Die()
    {
        sr.sprite = deadSprite;
    }

    public void Reset()
    {
        sr.sprite = idleSprite;
    }
}
