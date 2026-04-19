namespace TodoApp
{
    public class DIContainer
    {
        private readonly Dictionary<Type, (Type Implementation, LifeCycle LifeCycle)> _registrations = new();
        private readonly Dictionary<Type, object> _singletons = new();

        /// <summary>
        /// Registers a service with the specified interface and implementation types, along with its lifecycle.
        /// </summary>
        /// <typeparam name="TInterface">The interface type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <param name="lifeCycle">The lifecycle of the service.</param>
        public void Register<TInterface, TImplementation>(LifeCycle lifeCycle = LifeCycle.Transient)
            where TInterface : class
            where TImplementation : class, TInterface, new()
        {
            _registrations[typeof(TInterface)] = (typeof(TImplementation), lifeCycle);
        }

        /// <summary>
        /// Resolves an instance of the specified service type. Throws an exception if the service is not registered.
        /// </summary>
        /// <typeparam name="TInterface">The interface type.</typeparam>
        /// <returns>The resolved service instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
        public TInterface GetService<TInterface>()
        {
            // Check if the service is registered
            var type = typeof(TInterface);
            if (!_registrations.TryGetValue(type, out var registration))
            {
                throw new InvalidOperationException($"Service of type {type.Name} is not registered.");
            }

            // Create an instance based on the lifecycle
            switch (registration.LifeCycle)
            {
                case LifeCycle.Transient:
                    return (TInterface)Activator.CreateInstance(registration.Implementation)!;
                case LifeCycle.Singleton:
                    if (!_singletons.TryGetValue(type, out var singleton))
                    {
                        singleton = Activator.CreateInstance(registration.Implementation)!;
                        _singletons[type] = singleton;
                    }

                    return (TInterface)singleton!;
                default:
                    throw new InvalidOperationException($"Unsupported lifecycle: {registration.LifeCycle}");
            }
        }
    }

    public enum LifeCycle
    {
        Transient,
        Singleton
    }
}