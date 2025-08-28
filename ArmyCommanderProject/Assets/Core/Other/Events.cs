using Content.Features.CurrencySystem.Scripts;
using Content.Features.MapLoader.Scripts;
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

}