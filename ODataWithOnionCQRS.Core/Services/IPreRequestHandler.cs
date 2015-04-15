namespace ODataWithOnionCQRS.Core.Services
{
    public interface IPreRequestHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}
