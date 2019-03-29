using System;
using System.Collections.Generic;
using SadConsole;
using SadConsole.Input;
using Microsoft.Xna.Framework;
using flxkbr.unknownasofyet.text;

namespace flxkbr.unknownasofyet
{
    public class TextPanel : SadConsole.Console
    {
        public enum State { Inactive, Writing, Waiting }

        public event EventHandler<EventArgs> DialogsCompleted;

        readonly Color BorderColor = Globals.Colors.Red;
        readonly Color BackgroundColor = Globals.Colors.Black;

        public State CurrentState { 
            get => currentState;
            private set => currentState = value;
        }
        public Queue<DialogUnit> CurrentDialogs { get; private set; }

        private State currentState;
        private double charInterval;
        private DialogUnit currentDialog;
        private int iParagraph, iLine, iChar;
        private int yOffset;
        private double timeSinceLastChar;

        public TextPanel() : base(Globals.TextPanelWidth, Globals.TextPanelHeight)
        {
            this.Position = new Point(0, Globals.TextPanelY);
            this.DefaultBackground = BackgroundColor;
            Utils.CreateBorder(this, BorderColor, BackgroundColor);
            iParagraph = iLine = iChar = 0;
            CurrentState = State.Inactive;
            CurrentDialogs = new Queue<DialogUnit>();
        }

        public void WriteDialogs(List<DialogUnit> dialogs)
        {
            if (dialogs.Count == 0)
            {
                System.Console.Error.WriteLine("Passed empty list of dialogs to TextPanel");
                return;
            }
            CurrentDialogs = new Queue<DialogUnit>(dialogs);
            WriteDialog(CurrentDialogs.Dequeue());
        }

        public void WriteDialog(DialogUnit dialog)
        {
            if (CurrentState == State.Writing)
                return;
            currentDialog = dialog;
            iParagraph = iLine = iChar = 0;
            yOffset = (dialog.Source != null) ? 4 : 2;
            int chars = 0;
            timeSinceLastChar = 0d;
            foreach (var line in currentDialog.Paragraphs[iParagraph])
            {
                chars += line.Length;
            }
            charInterval = Globals.TextWritingDuration / (double)chars;
            CurrentState = State.Writing;

            // write Source if present and first char
            if (currentDialog.Source != null)
            {
                Print(3, 2, currentDialog.Source.ToUpper());
            }
            SetGlyph(2, yOffset, currentDialog.Paragraphs[0][0][0]);
        }

        private bool advanceParagraph()
        {
            ClearText();
            if (iParagraph + 1 == currentDialog.Paragraphs.Count)
            { // current dialog is over
                if (CurrentDialogs.Count == 0)
                { // no dialogs left
                    currentDialog = null;
                    iParagraph = iLine = iChar = 0;
                    CurrentState = State.Inactive;
                    return false;
                }
                else
                { // next dialog
                    WriteDialog(CurrentDialogs.Dequeue());
                }
            }
            else
            { // next paragraph
                iLine = iChar = 0;
                int chars = 0;
                timeSinceLastChar = 0;
                ++iParagraph;
                foreach (var line in currentDialog.Paragraphs[iParagraph])
                {
                    chars += line.Length;
                }
                charInterval = Globals.TextWritingDuration / (double)chars;
                if (currentDialog.Source != null)
                {
                    Print(3, 2, currentDialog.Source.ToUpper());
                }
                SetGlyph(2, yOffset, currentDialog.Paragraphs[iParagraph][0][0]);
                CurrentState = State.Writing;
            }
            return true;
        }

        private void write()
        {
            if (timeSinceLastChar >= charInterval)
            {
                timeSinceLastChar = 0d;
                var currentParagraph = currentDialog.Paragraphs[iParagraph];
                var currentLine = currentParagraph[iLine];
                if (iChar + 1 == currentLine.Length)
                { // line is done.
                    if (iLine + 1 == currentParagraph.Count)
                    { // paragraph is done
                        SetGlyph(Width - 3, Height - 2, 31);
                        CurrentState = State.Waiting;
                        return;
                    }
                    else
                    { // next line
                        ++iLine;
                        iChar = 0;
                        currentLine = currentParagraph[iLine];
                    }
                }
                else
                { // next char
                    ++iChar;
                }
                int x = 2 + iChar;
                int y = yOffset + (2*iLine);
                SetGlyph(x, y, currentLine[iChar]);
            }
        }

        public void ClearText()
        {
            for (var x = 1; x < Globals.TextPanelWidth - 1; ++x)
            {
                for (var y = 1; y < Globals.TextPanelHeight - 1; ++y)
                {
                    this.SetGlyph(x, y, 0);
                }
            }
        }

        public bool HandleInput(Keyboard info)
        {
            if (CurrentState == State.Waiting)
            {
                if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    bool moreDialog = advanceParagraph();
                    if (!moreDialog)
                    {
                        DialogsCompleted?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            return false;
        }

        public override void Update(TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);
            timeSinceLastChar += timeElapsed.TotalMilliseconds;
            switch (CurrentState)
            {
                case State.Writing:
                    write();
                    break;
            }
        }
    }
}