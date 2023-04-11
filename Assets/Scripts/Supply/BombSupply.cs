using System;
using System.Collections.Generic;
using UnityEngine;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Supply
{
    public class BombSupply : NetworkSupply
    {
        [SerializeField] private LayerMask enemy;
        private float _range = 4f;
        private List<Collider2D> _enemiesColliders = new List<Collider2D>();

        protected override void Pick(PlayerController playerController)
        {
            Debug.Log("Kill all enemies in radius");
            var enemies = Physics2D.OverlapCircleAll(transform.position, _range, enemy);
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<NetworkEnemy.NetworkEnemy>().DeathImmediately();
                playerController.SetKill();
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _range);
        }


    }
}