using DataLayer.Services;

namespace BusinessLayer
{
    public abstract class BaseManager
    {
        protected readonly UnitOfWork _srv;

        protected BaseManager()
        {
            _srv = new UnitOfWork();
        }
    }
}
