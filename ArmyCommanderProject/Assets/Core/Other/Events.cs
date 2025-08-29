using Content.Features.BuildingsSystem.Scripts;
using Content.Features.CurrencySystem.Scripts;
using Content.Features.MapLoader.Scripts;
using Content.Features.UnitsSystem.Scripts;
using UnityEngine;

namespace Core.Other
{
    public struct ComponentsInitializeEnd { }

    public struct OnMapContentInit
    {
        public IGameplayMap activeMap;
    }

    public struct OnPlayerSpawned { }

    public struct OnPlayerRespawn { }
    
    //Input
    public struct ScreenTouchedEvent
    {
        public Vector2 Position;
        public ScreenTouchedEvent(Vector2 position) => Position = position;
    }
    public struct JoystickMoveEvent
    {
        public Vector2 Direction;
        public JoystickMoveEvent(Vector2 direction) => Direction = direction;
    }
    public struct JoystickReleasedEvent { }
    
    //Currency
    public struct CurrencyChangedEvent
    {
        public CurrencyType Type;
        public int NewAmount;

        public CurrencyChangedEvent(CurrencyType type, int newAmount)
        {
            Type = type;
            NewAmount = newAmount;
        }
    }

    //BuildSystem
    public struct OnChooseBuildingCall
    {
        public BuildManager buildManager;
    }

    public struct OnBuildRequest
    {
        public int buildingID;
    }
    
    //Battle system

    public struct OnNewCampTargeted
    {
        public Vector3 position;
    }

    public struct OnChargeCalled { }

    public struct OnPrepareAllyUnits { }

    public struct HealthChangedEvent
    {
        public IUnit Unit;
        public float Current;
        public float Max;

        public HealthChangedEvent(IUnit unit, float current, float max)
        {
            Unit = unit;
            Current = current;
            Max = max;
        }
    }

    public struct UnitDiedEvent
    {
        public IUnit Unit;

        public UnitDiedEvent(IUnit unit)
        {
            Unit = unit;
        }
    }
}