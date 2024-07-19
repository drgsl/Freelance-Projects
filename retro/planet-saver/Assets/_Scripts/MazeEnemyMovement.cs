﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeEnemyMovement : MonoBehaviour
{

    public float timeBetweenAttacks = .5f;
    public int attackDamage = 10;

    private GameObject player;
    NavMeshAgent nav;
    PlayerHealth playerHealth;
    private bool playerInRange;
    private float timer;

    AudioSource audiosource;
    public AudioClip attack;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange)
        {
            Attack();
        }

        nav.SetDestination(player.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerInRange = false;
        }
    }

    void Attack()
    {
        audiosource.PlayOneShot(attack);
        timer = 0f;
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

}
