#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

#endregion

namespace Chiroptera.Win
{
	public class HistoryTextBox : TextBox
	{
        // One P/Invoke down ;)
        private static void MessageBeep()
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

		public delegate void TextEnteredDelegate(string str);
		public event TextEnteredDelegate textEntered = null;

		private List<string> m_stringList;
		private int m_historyPos;
		private int m_maxSize = 100;
		private string m_searchString;
		private bool m_changingText = false;

		public event KeyEventHandler rawKeyUp = null;
		public event KeyEventHandler rawKeyDown = null;
		private bool m_skipCharEvent; // XXX TODO use skipkeypress?

		public HistoryTextBox()
		{
			m_stringList = new List<string>(m_maxSize);
			m_stringList.Add("asd");
			m_historyPos = m_stringList.Count - 1;
		}

		public string[] History
		{
			get 
			{
				string[] arr = new string[m_stringList.Count - 1];
				m_stringList.CopyTo(0, arr, 0, m_stringList.Count - 1);
				return arr; 
			}
			set { 
				m_stringList = new List<string>(value);
				m_stringList.Add("");
				m_historyPos = m_stringList.Count - 1;
			}
		}

		public void EnterPressed()
		{
			textEntered(base.Text);

			if (base.Text.Length == 0)
				return;

			if (base.PasswordChar != (char)0)
			{
				m_historyPos = m_stringList.Count - 1;
				base.Clear();
				return;
			}

			if (m_stringList.Count < 2 || m_stringList[m_stringList.Count - 2] != base.Text)
			{
				m_stringList[m_stringList.Count - 1] = base.Text;
				m_stringList.Add("");

				if (m_stringList.Count > m_maxSize)
				{
					m_stringList.RemoveAt(0);
				}
			}

			m_historyPos = m_stringList.Count - 1;
			base.Clear();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			OnRawKeyDown(e);
			if (e.Handled)
			{
				m_skipCharEvent = true;
			}
			else
			{
				m_skipCharEvent = false;
			}

			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			m_skipCharEvent = false;

			OnRawKeyUp(e);

			base.OnKeyUp(e);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (m_skipCharEvent)
			{
				m_skipCharEvent = false;
				e.Handled = true;
			}
			else if (e.KeyChar == (char)13)
			{
				EnterPressed();
				e.Handled = true;
			}
            else if (!this.Focused) // Because the textbox isn't going to do it for us.
            {
                this.Focus();
                this.Text += e.KeyChar;
                this.SelectionStart = this.Text.Length;
            }
			base.OnKeyPress(e);
		}

		protected void OnRawKeyDown(KeyEventArgs e)
		{
			if (rawKeyDown != null)
			{
				rawKeyDown(this, e);

				if (e.Handled == true)
				{
					return;
				}
			}

			if (e.KeyCode == Keys.Up)
			{
				if (m_historyPos == 0)
				{
				   	MessageBeep();
				}
				else
				{
					if (e.Shift)
					{
						if (m_historyPos == m_stringList.Count - 1)
						{
							m_stringList[m_stringList.Count - 1] = base.Text;
						}

						if (m_searchString == null)
						{
							m_searchString = base.Text;
						}

						int foundIdx = -1;

						for (int i = m_historyPos - 1; i >= 0; i--)
						{
							if (m_stringList[i].StartsWith(m_searchString))
							{
								foundIdx = i;
								break;
							}
						}

						if (foundIdx != -1)
						{
							m_changingText = true;
							base.Text = m_stringList[foundIdx];
							m_changingText = false;

							SelectionStart = base.Text.Length;
							SelectionLength = 0;
							this.ScrollToCaret();

							m_historyPos = foundIdx;
						}
						else
						{
							MessageBeep();
						}
					}
					else
					{
						if (m_historyPos == m_stringList.Count - 1)
						{
							m_stringList[m_stringList.Count - 1] = base.Text;
						}

						m_historyPos--;
						base.Text = m_stringList[m_historyPos];

						SelectionStart = base.Text.Length;
						SelectionLength = 0;
						this.ScrollToCaret();
					}
				}

				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Down)
			{
				if (m_historyPos == m_stringList.Count - 1)
				{
				    MessageBeep();
				}
				else
				{
					if (e.Shift)
					{
						int foundIdx = -1;

						for (int i = m_historyPos + 1; i < m_stringList.Count - 1; i++)
						{
							if (m_stringList[i].StartsWith(m_searchString))
							{
								foundIdx = i;
								break;
							}
						}

						if (foundIdx != -1)
						{
							m_changingText = true;
							base.Text = m_stringList[foundIdx];
							m_changingText = false;

							SelectionStart = base.Text.Length;
							SelectionLength = 0;
							this.ScrollToCaret();

							m_historyPos = foundIdx;
						}
						else
						{
							MessageBeep();
						}
					}
					else
					{
						m_historyPos++;

						base.Text = m_stringList[m_historyPos];

						SelectionStart = base.Text.Length;
						SelectionLength = 0;
						this.ScrollToCaret();
					}
				}

				e.Handled = true;
			}
		}

		protected void OnRawKeyUp(KeyEventArgs e)
		{
			if (rawKeyUp != null)
				rawKeyUp(this, e);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			if (m_changingText == false)
			{
				m_searchString = null;
			}

			base.OnTextChanged(e);
		}

		// it's spelled k-l-u-d-g-e
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        [System.Diagnostics.DebuggerStepThrough]
		protected override void WndProc(ref Message m)
		{
			const int WM_PASTE = 0x0302;
            try {
                switch (m.Msg)
                {
                    case WM_PASTE:
                        {
                            IDataObject data = Clipboard.GetDataObject();
                            if (data.GetDataPresent(DataFormats.UnicodeText) || data.GetDataPresent(DataFormats.Text))
                            {
                                string str = "";

                                if (data.GetDataPresent(DataFormats.UnicodeText))
                                {
                                    str = data.GetData(DataFormats.UnicodeText).ToString();
                                }
                                else if (data.GetDataPresent(DataFormats.Text))
                                {
                                    str = data.GetData(DataFormats.Text).ToString();
                                }

                                if (str.Length > 0)
                                {
                                    str = str.Replace("\r\n", "\n");
                                    str = str.Replace("\r", "\n");

                                    string[] arr = str.Split('\n');

                                    for (int i = 0; i < arr.Length; i++)
                                    {
                                        this.SelectedText = arr[i];
                                        if (i < arr.Length - 1)
                                        {
                                            EnterPressed();
                                        }
                                    }
                                }
                            }
                            return;
                        }
                }
            }
            catch { }
			base.WndProc(ref m);
        }

        #region Control Snooping
        public void ListenOther(Control other)
        {
            other.KeyPress += new KeyPressEventHandler(other_KeyPress);
            other.KeyUp += new KeyEventHandler(other_KeyUp);
            other.KeyDown += new KeyEventHandler(other_KeyDown);
        }

        void other_KeyDown(object sender, KeyEventArgs e) { OnKeyDown(e); }
        void other_KeyUp(object sender, KeyEventArgs e) { OnKeyUp(e); }
        void other_KeyPress(object sender, KeyPressEventArgs e) { OnKeyPress(e); }
        #endregion
    }
}

