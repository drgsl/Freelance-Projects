using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Boss : MonoBehaviour, IHealth
{
    public EnemyData data;

    // IHealth
    public int Health { get; set; }
    public int MaxHealth { get; set; }

    private bool Invincible = false;


    protected static bool EnemiesPaused = false;

    public static void PauseAllEnemies()
    {
        EnemiesPaused = true;
    }

    public static void unPauseAllEnemies()
    {
        EnemiesPaused = false;
    }

    protected void Start()
    {
        // IHealth
        MaxHealth = data.MaxHealth;
        Health = MaxHealth;
    }

    private void OnTriggerStay(Collider other)
    {
        if(Invincible)
        {
            return;
        }

        if (other.gameObject.CompareTag("DamageEnemy"))
        {
            TakeDamage(Player.Instance.CurrentWeapon.data.BaseDamage);

            StartCoroutine(Invincibility(data.InvincibilityPeriod));
        }
    }

    private IEnumerator Invincibility(float seconds)
    {
        Invincible = true;
        yield return new WaitForSeconds(seconds);
        Invincible = false;
    }

    // IHealth
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    public void Heal(int heal)
    {
        Health += heal;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }
    public void Die()
    {
        XP_PointData randXP_data = data.XP_Drops[Random.Range(0, data.XP_Drops.Count)];
        XP_PointsManager.DropXP(randXP_data, transform.position);
        
        // EnemyManager.RestartEnemy(this);
        Destroy(gameObject);
    }
}
