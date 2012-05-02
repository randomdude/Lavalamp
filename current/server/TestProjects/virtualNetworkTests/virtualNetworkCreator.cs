using System;
using System.Windows.Forms;
using Castle.DynamicProxy.Generators;
using Moq;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    using System.ComponentModel;

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
            if (typeof(networkTypeToCreate) == typeof(CSharpNetwork))
                return new CSharpNetwork(pipeName);
            if (typeof(networkTypeToCreate) == typeof(simulatedPICNetwork))
                return new simulatedPICNetwork(pipeName, new Form1());

            throw new ArgumentException();
        }

        /// <summary>
        /// Instantiate the given class with the given parameter.
        /// </summary>
        /// <typeparam name="networkTypeToCreate">Type of return object</typeparam>
        /// <param name="pipeName">Named pipe name parameter</param>
        /// <param name="noGuiContext">if the virtual network has no gui</param>
        /// <returns>new networkTypeToCreate(pipeName)</returns>
        public static virtualNetworkBase makeNew<networkTypeToCreate> ( string pipeName,bool noGuiContext)
            where networkTypeToCreate : virtualNetworkBase
        {
            if (typeof(networkTypeToCreate) == typeof(CSharpNetwork))
                return new CSharpNetwork(pipeName);
            if (typeof(networkTypeToCreate) == typeof(simulatedPICNetwork))
            {
                if (noGuiContext)
                {
                    AttributesToAvoidReplicating.Add(typeof (System.Security.Permissions.UIPermissionAttribute));

                    Mock<ISynchronizeInvoke> eventHandleMock = new Mock<ISynchronizeInvoke>();
                    eventHandleMock.Setup(s => s.InvokeRequired).Returns(true);
                    eventHandleMock.Setup(s => s.Invoke(It.IsAny<Delegate>(), It.IsAny<object[]>())).Returns(new object());
                    eventHandleMock.Setup(s => s.BeginInvoke(It.IsAny<Delegate>(), It.IsAny<object[]>())).Returns(new Mock<IAsyncResult>(MockBehavior.Loose).Object);
                    eventHandleMock.Setup(s => s.EndInvoke(It.IsAny<IAsyncResult>()));
                    return new simulatedPICNetwork(pipeName, eventHandleMock.Object);
                }
                return new simulatedPICNetwork(pipeName , new Form());
            }
             throw new ArgumentException();
        }
    }
}