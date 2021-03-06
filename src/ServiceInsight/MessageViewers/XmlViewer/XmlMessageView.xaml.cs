﻿namespace ServiceInsight.MessageViewers.XmlViewer
{
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit.Folding;

    public partial class XmlMessageView : IXmlMessageView
    {
        FoldingManager foldingManager;
        XmlFoldingStrategy foldingStrategy;

        public XmlMessageView()
        {
            InitializeComponent();
            foldingManager = FoldingManager.Install(document.TextArea);
            foldingStrategy = new XmlFoldingStrategy();
            SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display);
            document.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
        }

        public virtual void Display(string message)
        {
            document.Document.Text = message ?? string.Empty;
            foldingStrategy.UpdateFoldings(foldingManager, document.Document);
        }

        public virtual void Clear()
        {
            document.Document.Text = string.Empty;
            foldingStrategy.UpdateFoldings(foldingManager, document.Document);
        }
    }
}
