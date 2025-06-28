//

namespace CoreEngine
{
    public interface IBaseService
    { }

    public interface IServiceRegister : IBaseService
    {
        public void Register();
        public void Unregister();
    }
}
