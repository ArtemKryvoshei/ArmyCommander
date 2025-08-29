using UnityEngine;

namespace Content.Features.BuildingsSystem.Scripts
{
    [CreateAssetMenu(fileName = "BuildData", menuName = "Game/Build Data", order = 0)]
    public class BuildData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite uiIcon;
        [SerializeField] private string uiName;

        public int Id => id;
        public GameObject Prefab => prefab;
        public Sprite UIIcon => uiIcon;
        public string UIName => uiName;
    }
}