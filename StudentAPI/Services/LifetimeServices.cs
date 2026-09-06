namespace StudentAPI.Services
{
    // =====================================================
    // Singleton — পুরো App এ একটাই object
    // =====================================================
    public interface ISingletonService
    {
        string GetInstanceId();
        int GetRequestCount();
        void IncrementCount();
    }

    public class SingletonService : ISingletonService
    {
        // ✅ একটাই object — সব request share করে
        private readonly string _instanceId;
        private int _requestCount = 0;

        public SingletonService()
        {
            // একবারই তৈরি হয়
            _instanceId = Guid.NewGuid().ToString()[..8];
        }

        public string GetInstanceId() => _instanceId;

        public int GetRequestCount() => _requestCount;

        public void IncrementCount() => _requestCount++;
    }

    // =====================================================
    // Scoped — প্রতিটা Request এ নতুন object
    // =====================================================
    public interface IScopedService
    {
        string GetInstanceId();
    }

    public class ScopedService : IScopedService
    {
        private readonly string _instanceId;

        public ScopedService()
        {
            // প্রতিটা Request এ নতুন
            _instanceId = Guid.NewGuid().ToString()[..8];
        }

        public string GetInstanceId() => _instanceId;
    }

    // =====================================================
    // Transient — প্রতিবার চাইলে নতুন object
    // =====================================================
    public interface ITransientService
    {
        string GetInstanceId();
    }

    public class TransientService : ITransientService
    {
        private readonly string _instanceId;

        public TransientService()
        {
            // প্রতিবার inject হলে নতুন
            _instanceId = Guid.NewGuid().ToString()[..8];
        }

        public string GetInstanceId() => _instanceId;
    }
}