using ICSharpCode.AvalonEdit;
using System.Windows;

namespace VocabHelper.WPF.Behaviours
{    
    internal class AvalonEditBehaviour
    {
        public static readonly DependencyProperty BoundTextProperty =
            DependencyProperty.RegisterAttached(
                "BoundText",
                typeof(string),
                typeof(AvalonEditBehaviour),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundTextChanged));

        public static string GetBoundText(DependencyObject d) => (string)d.GetValue(BoundTextProperty);
        public static void SetBoundText(DependencyObject d, string value) => d.SetValue(BoundTextProperty, value);

        private static void OnBoundTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextEditor editor && editor.Document != null)
            {
                string newValue = (string)e.NewValue ?? string.Empty;
                if (editor.Text != newValue)
                {
                    editor.Text = newValue;
                }

                // Koppel het TextChanged-event los en opnieuw vast om oneindige lussen te voorkomen
                editor.TextChanged -= Editor_TextChanged;
                editor.TextChanged += Editor_TextChanged;
            }
        }

        private static void Editor_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextEditor editor)
            {
                SetBoundText(editor, editor.Text);
            }
        }
    }
}