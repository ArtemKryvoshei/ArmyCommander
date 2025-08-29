namespace Content.Features.UnitsSystem.Scripts
{
    public interface IPlayerUnitSpawner
    {
        UnitType SpawnType { get; }
        float SpawnTime { get; }
        void StartSpawn();
    }
}