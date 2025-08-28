using UnityEngine;

namespace Content.Features.MapLoader.Scripts
{
    public interface IGameplayMap
    {
        //Инициализируем конктент карты, врагов, союзников, может загружаем их с сохрангения
        public void LoadAndInitMap();

        public Transform GetPlayerStartPoint();
        public Transform GetStartCurrencySpawnPoint();
    }
}