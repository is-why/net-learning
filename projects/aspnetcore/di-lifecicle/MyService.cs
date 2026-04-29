namespace DiLifecicle
{
    public interface ITransient
    {
        Guid GetGuid();
    }

    public interface IScoped
    {
        Guid GetGuid();
    }

    public interface ISingleton
    {
        Guid GetGuid();
    }

    public class MyService : ITransient, IScoped, ISingleton
    {
        private readonly Guid _guid;

        public MyService()
        {
            _guid = Guid.NewGuid();
        }

        public Guid GetGuid()
        {
            return _guid;
        }
    }
}