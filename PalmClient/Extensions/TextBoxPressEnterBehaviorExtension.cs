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
    public class TextBoxPressEnterBehaviorExtension
    {
        public static readonly DependencyProperty EnterCommandProperty = DependencyProperty.RegisterAttached(
            "EnterCommand", typeof(ICommand), typeof(TextBoxPressEnterBehaviorExtension), new FrameworkPropertyMetadata(default(ICommand), OnPropChanged)
            {
                BindsTwoWayByDefault = false,
            });

        private static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement fe))
                throw new InvalidOperationException();

            fe.KeyUp += OnKeyUp;
        }

        public static void OnKeyUp(object? sender, KeyEventArgs args)
        {
            if (args.Key == Key.Return || args.Key == Key.Enter)
            {
                GetEnterCommand(sender as DependencyObject)?.Execute((sender as TextBox)?.Text);
                Keyboard.ClearFocus();
            }
        }

        public static void SetEnterCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(EnterCommandProperty, value);
        }

        public static ICommand? GetEnterCommand(DependencyObject? element)
        {
            return (ICommand?)element?.GetValue(EnterCommandProperty);
        }
    }
}
