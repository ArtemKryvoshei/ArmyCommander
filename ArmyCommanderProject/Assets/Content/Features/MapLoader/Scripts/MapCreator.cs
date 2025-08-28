using System;
using System.Linq;
using Core.Other;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Content.Features.MapLoader.Scripts
{
    //предполагаю что в будущем можнео будет подключить какую-то систему сохранения прогресса, а пока player prefs
    public class MapCreator : MonoBehaviour, IMapCreator
    {
        [SerializeField] private MapById[] _maps;
        private IPrefabFactory _factory;

        private void Awake()
        {
            _factory = ServiceLocator.Get<IPrefabFactory>();
        }

        private void Start()
        {
            //Пока просто грузим последнюю сохрпанённую мапу
            InitMap();
        }

        public async void InitMap()
        {
            int id = PlayerPrefs.GetInt(ConstantsHolder.MAP_SAVE_KEY, 0); 
            await InitMapInternal(id);
        }

        public async void InitMapById(int id)
        {
            await InitMapInternal(id);
        }
        
        private async UniTask InitMapInternal(int id)
        {
            var mapData = _maps.FirstOrDefault(m => m.Id == id);

            if (mapData.MapPrefab == null)
            {
                Debug.LogError($"[MapCreator] No map found with id {id}");
                return;
            }

            var spawnedMap = await _factory.CreateAsync(mapData.MapPrefab);
            var gameplayMap = spawnedMap.GetComponent<IGameplayMap>();

            if (gameplayMap == null)
            {
                Debug.LogError($"[MapCreator] Spawned map does not have IGameplayMap component!");
                return;
            }

            gameplayMap.LoadAndInitMap();
        }
    }
}