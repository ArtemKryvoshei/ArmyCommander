using Content.Features.CurrencySystem.Scripts;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    [System.Serializable]
    public class CurrencyReward
    {
        public CurrencyType currencyType;
        public int amount;
    }
    
    [CreateAssetMenu(fileName = "UnitStats", menuName = "Game/Unit Stats", order = 1)]
    public class UnitStatsData : ScriptableObject
    {
        [Header("Base Stats")]
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private float moveSpeed = 3f;
    
        [Header("Attack Stats")]
        [SerializeField] private float detectRange = 5f;
        [SerializeField] private float attackRange = 5f;
        [SerializeField] private int attackDamage = 1;
        [SerializeField] private float attackInterval = 1.2f;
        
        [Header("Loot")]
        public CurrencyReward[] currencyRewards;

        public int MaxHealth => maxHealth;
        public float MoveSpeed => moveSpeed;
        
        public float DetectionRange => detectRange;
        public float AttackRange => attackRange;
        public int AttackDamage => attackDamage;
        public float AttackInterval => attackInterval;
    }
}