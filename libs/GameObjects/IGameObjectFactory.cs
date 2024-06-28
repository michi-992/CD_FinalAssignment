namespace libs;

public interface IGameObjectFactory
{
    // Create GameObject based on dynamic input
    public GameObject CreateGameObject(dynamic obj);
}