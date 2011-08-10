using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Chiroptera.Base;

namespace Chiroptera.Win
{
	public partial class HiliteForm : Form
	{
		public HiliteForm()
		{
			InitializeComponent();

			Hilite[] hilites = PythonInterface.HiliteManager.GetHilites();
			foreach (Hilite hilite in hilites)
			{
				ListViewItem item = HiliteToListViewItem(hilite);
				listView.Items.Add(item);
			}
		}

		ListViewItem HiliteToListViewItem(Hilite hilite)
		{
			ListViewItem item = new ListViewItem(new string[5]);
			item.Tag = hilite;
			UpdateListViewItem(item);
			return item;
		}

		void UpdateListViewItem(ListViewItem item)
		{
			Hilite hilite = (Hilite)item.Tag;
			item.SubItems[0].Text = hilite.Pattern;
			item.SubItems[1].Text = hilite.Style.Fg.Name;
			item.SubItems[2].Text = hilite.Style.Bg.Name;
			item.SubItems[3].Text = hilite.IgnoreCase.ToString();
			item.SubItems[4].Text = hilite.HiliteLine.ToString();
		}

		private void fgButton_Click(object sender, EventArgs e)
		{
			colorDialog.Color = fgButton.BackColor;
			if(colorDialog.ShowDialog(this) == DialogResult.OK)
				fgButton.BackColor = colorDialog.Color;
		}

		private void bgButton_Click(object sender, EventArgs e)
		{
			colorDialog.Color = bgButton.BackColor;
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
				bgButton.BackColor = colorDialog.Color;
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count == 0)
			{
				patternTextBox.Text = "";
				ignoreCaseCheckBox.Checked = false;
				hiliteLineCheckBox.Checked = false;
				fgCheckBox.Checked = true;
				fgButton.Enabled = false;
				bgCheckBox.Checked = true;
				bgButton.Enabled = false;
				fgButton.BackColor = System.Drawing.Color.Empty;
				bgButton.BackColor = System.Drawing.Color.Empty;
				return;
			}

			ListViewItem item = listView.SelectedItems[0];
			
			Hilite hilite = (Hilite)item.Tag;

			patternTextBox.Text = hilite.Pattern;
			ignoreCaseCheckBox.Checked = hilite.IgnoreCase;
			fgButton.BackColor = hilite.Style.Fg.ToSystemColor();
			bgButton.BackColor = hilite.Style.Bg.ToSystemColor();
			hiliteLineCheckBox.Checked = hilite.HiliteLine;

			if (hilite.Style.Fg.IsEmpty)
			{
				fgCheckBox.Checked = true;
				fgButton.Enabled = false;
			}
			else
			{
				fgCheckBox.Checked = false;
				fgButton.Enabled = true;
			}

			if (hilite.Style.Bg.IsEmpty)
			{
				bgCheckBox.Checked = true;
				bgButton.Enabled = false;
			}
			else
			{
				bgCheckBox.Checked = false;
				bgButton.Enabled = true;
			}
		}

		private void newButton_Click(object sender, EventArgs e)
		{
			Hilite hilite = new Hilite("", false, new TextStyle(), false);
			PythonInterface.HiliteManager.AddHilite(hilite);

			ListViewItem item = HiliteToListViewItem(hilite);
			listView.Items.Add(item);

			item.Selected = item.Focused = true;
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count == 0)
			{
				return;
			}

			ListViewItem item = listView.SelectedItems[0];

			Hilite hilite = (Hilite)item.Tag;

			PythonInterface.HiliteManager.RemoveHilite(hilite);

			int idx = item.Index;

			item.Remove();

			if (idx >= listView.Items.Count)
				idx--;

			if (idx < 0)
				idx = 0;

			if (idx < listView.Items.Count)
			{
				ListViewItem newItem = listView.Items[idx];
				newItem.Focused = newItem.Selected = true;
			}
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count == 0)
			{
				return;
			}

			ListViewItem item = listView.SelectedItems[0];

			Hilite hilite = (Hilite)item.Tag;

			try
			{
				hilite.Pattern = patternTextBox.Text;
			}
			catch (ArgumentException exc)
			{
				MessageBox.Show(this, "Error in pattern: " + exc.Message, "Error in pattern");
				return;
			}

			hilite.IgnoreCase = ignoreCaseCheckBox.Checked;
			Chiroptera.Base.Color fg, bg;

			if (fgCheckBox.Checked)
				fg = Chiroptera.Base.Color.Empty;
			else
				fg = Chiroptera.Base.Color.FromSystemColor(fgButton.BackColor);
			if (bgCheckBox.Checked)
				bg = Chiroptera.Base.Color.Empty;
			else
				bg = Chiroptera.Base.Color.FromSystemColor(bgButton.BackColor);

			hilite.Style = new TextStyle(fg, bg);

			hilite.HiliteLine = hiliteLineCheckBox.Checked;

			UpdateListViewItem(item);
			item.Selected = false;
			item.Selected = item.Focused = true; // updates the controls
		}

		private void fgCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (fgCheckBox.Checked)
				fgButton.Enabled = false;
			else
				fgButton.Enabled = true;
		}

		private void bgCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (bgCheckBox.Checked)
				bgButton.Enabled = false;
			else
				bgButton.Enabled = true;
		}
	}
}
