using System;
namespace Daedalus.TextView
{
    public delegate void LinkClickDelegate(string link);

    public interface ITextView
    {
        event LinkClickDelegate LinkClicked;
        Chiroptera.Win.ParagraphContainer ParagraphContainer { get; set; }
        void ScrollDown(int paragraphs);
        void ScrollPageDown();
        void ScrollPageUp();
        void ScrollUp(int paragraphs);

        int Lines { get; }
        int Columns { get; }
    }
}
