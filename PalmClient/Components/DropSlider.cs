using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace PalmClient.Components
{
    public class DropSlider : Slider
    {
        private Binding? SupressedBinding { get; set; }

        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            var expression = BindingOperations.GetBindingExpression(this, ValueProperty);
            if (expression != null)
            {
                SupressedBinding = expression.ParentBinding;
                var value = Value;
                BindingOperations.ClearBinding(this, ValueProperty);
                SetValue(ValueProperty, value);
            }
        }

        protected override void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            if (SupressedBinding != null)
            {
                var value = Value;
                BindingOperations.SetBinding(this, ValueProperty, SupressedBinding);
                SetCurrentValue(ValueProperty, value);
                SupressedBinding = null;
            }
            base.OnThumbDragCompleted(e);
        }
    }
}
