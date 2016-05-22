using Microsoft.Owin.Hosting;

namespace Log.Service
{
    public interface IStartable
    {
        void Start();
    }

    public class WebApiApp: IStartable
    {
        private readonly int _portNumber;

        public WebApiApp(int port)
        {
            _portNumber = port; // int.Parse(port);
        }

        public void Start()
        {
            WebApp.Start<Startup>(string.Format("http://localhost:{0}", _portNumber));
        }
    }
}
