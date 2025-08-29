using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.BuildingsSystem.Scripts
{
    public interface IBuildManager
    {
        public BuildData[] GetAvailableBuilds();
        public void Build(int id);
        public void ChooseBuilding();
    }
}