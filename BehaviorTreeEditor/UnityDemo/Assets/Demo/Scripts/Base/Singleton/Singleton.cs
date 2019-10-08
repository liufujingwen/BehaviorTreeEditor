public class Singleton<T>
{
    protected Singleton()
    {
    }

    private static readonly T ms_Instance = System.Activator.CreateInstance<T>();
    public static T Instance => ms_Instance;
}
