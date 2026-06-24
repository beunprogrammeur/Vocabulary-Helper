using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace VocabHelper.WPF.Behaviours
{
    public class DragDropFilesBehaviour : Behavior<UIElement>
    {
        // Dependency Property to bind your ViewModel Command
        public static readonly DependencyProperty FileDroppedCommandProperty =
            DependencyProperty.Register(
                nameof(FileDroppedCommand),
                typeof(ICommand),
                typeof(DragDropFilesBehaviour),
                new PropertyMetadata(null));

        public ICommand FileDroppedCommand
        {
            get => (ICommand)GetValue(FileDroppedCommandProperty);
            set => SetValue(FileDroppedCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            // Hook into the UI element's drop event
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            // Clean up the event handler to prevent memory leaks
            AssociatedObject.Drop -= AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if (FileDroppedCommand == null) return;

            // Check if the drop action contains files
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Execute the command if it can run, passing the clean string array
                if (files != null && FileDroppedCommand.CanExecute(files))
                {
                    FileDroppedCommand.Execute(files);
                }
            }
        }
    }
}
