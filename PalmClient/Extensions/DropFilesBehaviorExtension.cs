using PalmClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PalmClient.Extensions
{
    public class DropFilesBehaviorExtension
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled", typeof(bool), typeof(DropFilesBehaviorExtension), new FrameworkPropertyMetadata(default(bool), OnPropChanged)
            {
                BindsTwoWayByDefault = false,
            });

        public static readonly DependencyProperty InterfaceDataContextProperty = DependencyProperty.RegisterAttached(
            "InterfaceDataContext", typeof(object), typeof(DropFilesBehaviorExtension), new FrameworkPropertyMetadata(default(object?), null)
            {
                BindsTwoWayByDefault = false,
            });

        public static readonly DependencyProperty DropParamProperty = DependencyProperty.RegisterAttached(
            "DropParam", typeof(object), typeof(DropFilesBehaviorExtension), new FrameworkPropertyMetadata(default(object?), null)
            {
                BindsTwoWayByDefault = false,
            });

        private static void OnPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement fe))
                throw new InvalidOperationException();
            if ((bool)e.NewValue)
            {
                fe.AllowDrop = true;
                fe.Drop += OnDrop;
                fe.PreviewDragOver += OnPreviewDragOver;
            }
            else
            {
                fe.AllowDrop = false;
                fe.Drop -= OnDrop;
                fe.PreviewDragOver -= OnPreviewDragOver;
            }
        }

        private static void OnPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private static async void OnDrop(object sender, DragEventArgs e)
        {
            var element = sender as FrameworkElement;
            object? dataContext = null;

            if ((dataContext = GetInterfaceDataContext(element!)) == null)
            {
                dataContext = ((FrameworkElement)sender).DataContext;
            }
            
            if (!(dataContext is IFilesDropAsync filesDropped))
            {
                if (dataContext != null)
                    Trace.TraceError($"Binding error, '{dataContext.GetType().Name}' doesn't implement '{nameof(IFilesDropAsync)}'.");
                return;
            }

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
            {
                await filesDropped.OnFilesDroppedAsync(files, GetDropParam(element!));
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

        public static void SetDropParam(DependencyObject element, object? value)
        {
            element.SetValue(DropParamProperty, value);
        }

        public static object? GetDropParam(DependencyObject element)
        {
            return (object?)element.GetValue(DropParamProperty);
        }

        public static void SetInterfaceDataContext(DependencyObject element, object? value)
        {
            element.SetValue(InterfaceDataContextProperty, value);
        }

        public static object? GetInterfaceDataContext(DependencyObject element)
        {
            return (object?)element.GetValue(InterfaceDataContextProperty);
        }
    }
}
