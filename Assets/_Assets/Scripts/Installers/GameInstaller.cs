using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InventoryEvent>().AsSingle();
        Container.Bind<InventoryController>().AsSingle();
        Container.Bind<GameData>().AsSingle();
    }
}
