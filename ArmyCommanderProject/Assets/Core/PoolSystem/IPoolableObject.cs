namespace Core.PoolSystem
{
    public interface IPoolableObject
    {
        void OnSpawned();
        
        void OnDespawned();
    }
}