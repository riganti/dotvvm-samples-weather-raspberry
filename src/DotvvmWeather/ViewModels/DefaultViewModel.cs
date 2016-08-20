using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotvvmWeather.Services;
using DotVVM.Framework.ViewModel;

namespace DotvvmWeather.ViewModels
{
    public class DefaultViewModel : DotvvmViewModelBase
    {

        public WeatherData Current { get; private set; }

        public List<WeatherData> Data { get; private set; }

        public int Interval { get; set; } = 3600;


        public override Task PreRender()
        {
            // refresh data
            Data = Program.WeatherService.GetData(24, Interval / 10 - 1);
            Current = Program.WeatherService.GetLastData();

            return base.PreRender();
        }

        public void SetInterval(int interval)
        {
            Interval = interval;
        }

        public void Refresh()
        {
            // nothing to do - the refresh is done in the PreRender method
        }
    }
}
