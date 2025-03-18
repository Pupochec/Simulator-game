using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform CameraTransform;
    [SerializeField] private Transform HandPosition;
    [SerializeField] private GameObject ButtonDrop;


    public override void InstallBindings()
    {
        Container.Bind<Joystick>().FromInstance(joystick).AsSingle();
        Container.Bind<GameObject>().FromInstance(ButtonDrop).AsCached();
        Container.Bind<Transform>().WithId("CameraTransform").FromInstance(CameraTransform).AsCached();
        Container.Bind<Transform>().WithId("HandPosition").FromInstance(HandPosition).AsCached();
    }
}
