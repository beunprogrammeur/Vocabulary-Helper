using ICSharpCode.AvalonEdit;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;

namespace VocabHelper.WPF.Behaviours
{
    public static class AvalonEditLogBehaviour
    {
        private const int MaxCharacters = 60000;

        public static readonly DependencyProperty LogItemsSourceProperty =
            DependencyProperty.RegisterAttached(
                "LogItemsSource",
                typeof(IEnumerable),
                typeof(AvalonEditLogBehaviour),
                new PropertyMetadata(null, OnLogItemsSourceChanged));

        public static void SetLogItemsSource(DependencyObject element, IEnumerable value) => element.SetValue(LogItemsSourceProperty, value);
        public static IEnumerable GetLogItemsSource(DependencyObject element) => (IEnumerable)element.GetValue(LogItemsSourceProperty);

        private static void OnLogItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextEditor editor) return;

            if (e.OldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= (s, args) => { };
            }

            if (e.NewValue is INotifyCollectionChanged newCollection)
            {
                if (e.NewValue is IEnumerable currentItems && editor.Document != null)
                {
                    editor.Document.Text = string.Empty;
                    foreach (var item in currentItems)
                    {
                        if (item is string line)
                        {
                            editor.Document.Insert(editor.Document.TextLength, line + Environment.NewLine);
                        }
                    }
                    editor.ScrollToEnd();
                }

                newCollection.CollectionChanged += (sender, args) =>
                {
                    var doc = editor.Document;
                    if (doc == null) return;

                    if (args.Action == NotifyCollectionChangedAction.Add && args.NewItems != null)
                    {
                        foreach (var item in args.NewItems)
                        {
                            if (item is string newLine)
                            {
                                doc.Insert(doc.TextLength, newLine + Environment.NewLine);
                            }
                        }

                        if (doc.TextLength > MaxCharacters)
                        {
                            int overschot = doc.TextLength - MaxCharacters;

                            var firstLine = doc.GetLineByOffset(overschot);
                            doc.Remove(0, firstLine.EndOffset);
                        }

                        editor.ScrollToEnd();
                    }
                    else if (args.Action == NotifyCollectionChangedAction.Reset)
                    {
                        doc.Text = string.Empty;
                    }
                };
            }
        }
    }
}
