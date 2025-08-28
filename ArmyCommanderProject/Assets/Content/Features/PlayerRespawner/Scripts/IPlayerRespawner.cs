using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.PlayerRespawner.Scripts
{
    public interface IPlayerRespawner
    {
        Transform PlayerTransform { get; }
        void RespawnPlayer();
    }
}