using UnityEngine;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Supply
{
    public class BombSupply : NetworkSupply
    {
        [SerializeField] private LayerMask enemy;
        protected override void Pick(PlayerController playerController)
        {
            Debug.Log("Kill all enemies in radius");
            var enemies = Physics2D.OverlapCircleAll(transform.position, 10, enemy);
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<NetworkEnemy.NetworkEnemy>().DeathImmediately();
                playerController.SetKill();
            }
        }
    }
}