using DofusCrafter.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DofusCrafter.UI.Controls
{
    public class Disclaimer : Control
    {
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(
                "Type",
                typeof(Disclaimers),
                typeof(Disclaimer),
                new PropertyMetadata(Disclaimers.Information));

        public Disclaimers Type
        {
            get { return (Disclaimers)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
           DependencyProperty.Register(
               "Content", typeof(string), 
               typeof(Disclaimer), 
               new PropertyMetadata(""));

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
    }
}
