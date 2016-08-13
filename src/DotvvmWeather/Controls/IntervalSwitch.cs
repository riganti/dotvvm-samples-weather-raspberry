using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotvvmWeather.ViewModels;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Binding;

namespace DotvvmWeather.Controls
{
    public class IntervalSwitch : DotvvmMarkupControl
    {
        

        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        public static readonly DotvvmProperty IntervalProperty
            = DotvvmProperty.Register<int, IntervalSwitch>(c => c.Interval, 0);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DotvvmProperty TextProperty
            = DotvvmProperty.Register<string, IntervalSwitch>(c => c.Text, null);


        public IntervalSwitch() : base("span")
        {
        }


        public void SetInterval()
        {
            ((DefaultViewModel) DataContext).Interval = Interval;
        }


    }
}
