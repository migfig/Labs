using Microsoft.Owin.Hosting;
using System;

namespace Log.Service
{
    public interface IServiceable
    {
        bool Start();
        bool Stop();
    }

    public class WebApiApp: IServiceable
    {
        private readonly int _portNumber;
        private IDisposable _instance;

        public WebApiApp(int port)
        {
            _portNumber = port;
        }

        public bool Start()
        {
            _instance = WebApp.Start<Startup>(string.Format("http://localhost:{0}", _portNumber));
            return true;
        }

        public bool Stop()
        {
            _instance.Dispose();
            return true;
        }
    }
}
