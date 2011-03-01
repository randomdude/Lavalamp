using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    public class threadParams
    {
        public virtualNetwork net;

        public threadParams(virtualNetwork toTest)
        {
            net = toTest;
        }
    }
}