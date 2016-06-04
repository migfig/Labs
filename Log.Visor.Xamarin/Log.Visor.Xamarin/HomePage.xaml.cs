using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Log.Visor.Xamarin
{
    public class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponentX();
        }

        private void InitializeComponentX()
        {
            this.LoadFromXaml(typeof(HomePage));
        }
    }
}
