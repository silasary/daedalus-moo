using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Daedalus.TextView;
using Chiroptera.Win;

namespace Daedalus
{
    public partial class RtfTextView : RichTextBox, ITextView
    {
        public RtfTextView()
        {
            InitializeComponent();
            this.DataBindings.Add(new System.Windows.Forms.Binding("Font", Settings.Default, "ConsoleFont", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = Settings.Default.ConsoleFont;

        }

        private ParagraphContainer Pcontainer = new ParagraphContainer();
        #region ITextView Members

        public new event LinkClickDelegate LinkClicked;

        public ParagraphContainer ParagraphContainer 
        {
            get
            {
                return Pcontainer;
            }
            set
            {
                Pcontainer.paragraphAddedEvent -= new ParagraphContainer.ParagraphAddedDelegate(Pcontainer_paragraphAddedEvent);
                Pcontainer = value;
                Pcontainer.paragraphAddedEvent += new Chiroptera.Win.ParagraphContainer.ParagraphAddedDelegate(Pcontainer_paragraphAddedEvent);
                ParagraphContainer.SetColumns(80);
            }
        }

        public void ScrollDown(int paragraphs)
        {
            this.SelectionStart = this.TextLength;
        }

        public void ScrollPageDown()
        {
            throw new NotImplementedException();
        }

        public void ScrollPageUp()
        {
            throw new NotImplementedException();
        }

        public void ScrollUp(int paragraphs)
        {
            throw new NotImplementedException();
        }

        public new int Lines
        {
            get { return 0; }
        }

        public int Columns
        {
            get { return 80; }
        }

        #endregion

        void Pcontainer_paragraphAddedEvent(bool historyFull)
        {
           // if (this.Rtf == null)
                this.Text += Pcontainer[Pcontainer.Count - 1].m_text + '\n';
                base.SelectionStart = base.Text.Length;
                base.ScrollToCaret();
           // this.Rtf.Insert(this.Rtf.Length - 2, Pcontainer[Pcontainer.Count - 1].m_text + @"\par");
        }

    }
}
