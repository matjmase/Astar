using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Astar.Behaviors
{
    public class LabelTextInsertionBehavior : Behavior<Label>
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
            "Value", typeof(string),
            typeof(LabelTextInsertionBehavior),
            new PropertyMetadata("", ValuePropertyChanged)
            );

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty BeforeProperty =
            DependencyProperty.Register(
            "Before", typeof(string),
            typeof(LabelTextInsertionBehavior),
            new PropertyMetadata("", ValuePropertyChanged)
            );

        public string Before
        {
            get { return (string)GetValue(BeforeProperty); }
            set { SetValue(BeforeProperty, value); }
        }

        public static readonly DependencyProperty AfterProperty =
            DependencyProperty.Register(
            "After", typeof(string),
            typeof(LabelTextInsertionBehavior),
            new PropertyMetadata("", ValuePropertyChanged)
            );

        public string After
        {
            get { return (string)GetValue(AfterProperty); }
            set { SetValue(AfterProperty, value); }
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }


        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            base.OnDetaching();
        }

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behav = (LabelTextInsertionBehavior)d;

            if (behav?.AssociatedObject?.IsLoaded != true)
            {
                return;
            }

            behav.AssociatedObject.Content = behav.Before + behav.Value + behav.After;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Content = Before + Value + After;
        }
    }
}
