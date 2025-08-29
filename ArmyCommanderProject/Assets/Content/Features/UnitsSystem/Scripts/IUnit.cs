using Core.PoolSystem;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    public interface IUnit : IPoolableObject
    {
        public GameObject GetUnitGameObject();
        public UnitStatsData GetStats();
        public UnitTeam GetUnitTeam();
        public void SetUnitPool(GameObjectPool<UnitBase> _myPool);
    }
}