using Log.Common;
using Log.Provider.Default;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Log.Service
{
    public partial class LogService : ServiceBase
    {
        private ILogServices _logServices;
        public LogService()
        {
            InitializeComponent();

            _logServices = new LogServices(new CustomFileProvider(ConfigurationManager.AppSettings["LogPath"], ConfigurationManager.AppSettings["LogName"]));
        }

        protected override void OnStart(string[] args)
        {
            var items = _logServices.GetEntries().GetAwaiter().GetResult();
        }

        protected override void OnStop()
        {            
        }
    }
}
