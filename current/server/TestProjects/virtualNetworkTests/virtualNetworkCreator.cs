using System;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    /// <summary>
    /// This classes sole purpose is to make a single-parameter constructor accessible to a generic.
    /// </summary>
    public static class virtualNetworkCreator
    {
        /// <summary>
        /// Instantiate the given class with the given parameter.
        /// </summary>
        /// <typeparam name="networkTypeToCreate">Type of return object</typeparam>
        /// <param name="pipeName">Named pipe name parameter</param>
        /// <returns>new networkTypeToCreate(pipeName)</returns>
        public static virtualNetworkBase makeNew<networkTypeToCreate> ( string pipeName)
            where networkTypeToCreate : virtualNetworkBase
        {
            if (typeof(networkTypeToCreate) == typeof(virtualNetwork))
                return new virtualNetwork(pipeName);
            if (typeof(networkTypeToCreate) == typeof(gpSimNetwork))
                return new gpSimNetwork(pipeName);

            throw new ArgumentException();
        }
    }
}