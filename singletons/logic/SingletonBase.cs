public class SingletonBase<T> where T : class, new()
{
    private static T instance = null;
    //private static readonly object padlock = new object(); //not needed since this will always be single threaded..

    protected SingletonBase()
    {
        // Prevent direct instantiation
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //lock (padlock)
                //{
                    if (instance == null)
                    {
                        instance = new T();
                    }
                //}
            }
            return instance;
        }
    }
}