namespace SimpleDIContainer
{
    public class DIContainer
    {
        private readonly Dictionary<Type, (Type Implementation, ServiceLifeTime LifeTime)> _registrations = new();
        private readonly Dictionary<Type, object> _singletonInstances = new();

        /// <summary>
        /// Registers a service with the specified interface and implementation types, along with its lifetime.
        /// </summary>
        /// <typeparam name="TInterface">The interface type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <param name="lifeTime">The lifetime of the service.</param>
        public void Register<TInterface, TImplementation>(ServiceLifeTime lifeTime = ServiceLifeTime.Transient)
            where TInterface : class
            where TImplementation : class, TInterface, new()
        {
            _registrations[typeof(TInterface)] = (typeof(TImplementation), lifeTime);
        }

        /// <summary>
        /// Resolves and returns an instance of the requested service type. If the service is registered as a singleton, it will return the same instance for subsequent calls. If the service is registered as transient, it will return a new instance each time.
        /// </summary>
        /// <typeparam name="TInterface">The interface type.</typeparam>
        /// <returns>The resolved service instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
        public TInterface GetService<TInterface>()
        {
            // Check if the service type is registered
            var type = typeof(TInterface);
            if (!_registrations.TryGetValue(type, out var registration))
            {
                throw new InvalidOperationException($"Service of type {type.Name} is not registered.");
            }

            // Handle service creation based on its lifetime
            switch (registration.LifeTime)
            {
                case ServiceLifeTime.Singleton:
                    if (!_singletonInstances.TryGetValue(type, out var singletonInstance))
                    {
                        singletonInstance = Activator.CreateInstance(registration.Implementation);
                        _singletonInstances[type] = singletonInstance ?? throw new InvalidOperationException($"Failed to create instance of type {registration.Implementation.Name}.");
                    }
                    return (TInterface)singletonInstance;

                case ServiceLifeTime.Transient:
                    var transientInstance = Activator.CreateInstance(registration.Implementation);
                    if (transientInstance is null)
                        throw new InvalidOperationException($"Failed to create instance of type {registration.Implementation.Name}.");
                    return (TInterface)transientInstance;

                default:
                    throw new InvalidOperationException($"Unsupported service lifetime: {registration.LifeTime}");
            }
        }
    }
}