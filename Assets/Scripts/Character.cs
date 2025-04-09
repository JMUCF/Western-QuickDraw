using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite shootSprite;
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private SpriteRenderer sr;

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
