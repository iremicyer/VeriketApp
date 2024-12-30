using System.ServiceProcess;

namespace VeriketService
{
    internal static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new BatchService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
