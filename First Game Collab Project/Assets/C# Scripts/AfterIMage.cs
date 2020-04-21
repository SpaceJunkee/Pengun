using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterIMage : MonoBehaviour
{
    [SerializeField]
    private float activeTime = 0.1f;
    private float timeActive;
    private float alpha;
    private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.9f;

    private Transform player;

    private SpriteRenderer sr;
    private SpriteRenderer playerSr;

    private Color color;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerSr = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        sr.sprite = playerSr.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.localScale = player.localScale;
        timeActive = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        sr.color = color;

        if(Time.time >= (timeActive + activeTime))
        {
            AfterImagePool.Instance.AddToPool(gameObject);
        }

    }
}
