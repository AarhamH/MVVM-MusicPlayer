using PalmClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PalmClient.Extensions
{
    public class ClickLoseFocusExtension
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled", typeof(bool), typeof(ClickLoseFocusExtension), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
            {
                BindsTwoWayByDefault = false,
            });

        private static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement fe))
                throw new InvalidOperationException();
            if ((bool)e.NewValue)
            {
                fe.MouseDown += OnMouseDown;
                fe.KeyUp += OnKeyUp;
            }
            else
            {
                fe.MouseDown -= OnMouseDown;
                fe.KeyUp -= OnKeyUp;
            }
        }

        public static void OnMouseDown(object? sender, MouseButtonEventArgs args)
        {
            Keyboard.ClearFocus();
        }

        public static void OnKeyUp(object? sender, KeyEventArgs args)
        {
            if (args.Key == Key.Return || args.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }
    }
}
