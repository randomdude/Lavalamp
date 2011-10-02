using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    public class threadParams
    {
        public CSharpNetwork net;

        public threadParams(CSharpNetwork toTest)
        {
            net = toTest;
        }
    }
}