/*
 * Created by SharpDevelop.
 * User: Chukashev
 * Date: 04.12.2017
 * Time: 9:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace CSVBugTool
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public DetailInfo infoPage;
		public string[] splitOpt = new string[1]{ ",.;" };
		public MainForm()
		{
			InitializeComponent();
		}
		void OpenFileClick(object sender, EventArgs e)
		{
			var CSVOpenFileDialog = this.openFileDialog1;
			if (CSVOpenFileDialog == null)
			{
				return;
			}
			CSVOpenFileDialog.ShowDialog();
			using (System.IO.StreamReader FileStream = new System.IO.StreamReader(CSVOpenFileDialog.OpenFile(), System.Text.Encoding.UTF8))
			{
				var Row = 0;
				for (string InputString; !FileStream.EndOfStream;)
				{
					InputString = FileStream.ReadLine();
					var Parts = InputString.Split(splitOpt, StringSplitOptions.None);
					if (Row == 0)
					{
						for (var i = 0; i < this.dataGridView1.Columns.Count; ++i)
						{
							Parts = Parts.Except(Parts.Where(Name => Name == this.dataGridView1.Columns[i].Name)).ToArray();
						}
						foreach(var Column in Parts)
						{
							this.dataGridView1.Columns.Add(Column, Column);
						}
					}
					else
					{
						this.dataGridView1.Rows.Add(Parts);
					}
					++Row;
					this.textBox1.Text = Row.ToString();
				}
				FileStream.Close();
			}
		}
		void NeedDetailInfoClick(object sender, EventArgs e)
		{
			try
			{
				if (this.infoPage.IsDisposed)
				{
					this.infoPage = new DetailInfo(this.Handle);
				}
			}
			catch
			{
				this.infoPage = new DetailInfo(this.Handle);
			}
			if (this.dataGridView1.SelectedRows.Count > 0)
			{
				var sData = new Dictionary<string, string>();
				foreach(DataGridViewCell cell in this.dataGridView1.SelectedRows[0].Cells)
				{
					sData.Add(this.dataGridView1.Columns[cell.ColumnIndex].Name, cell.Value != null ? cell.Value.ToString() : "");
				}
				this.infoPage.SetValues(sData, this.dataGridView1.SelectedRows[0].Index);
				this.infoPage.Show();
				this.infoPage.Focus();
			}
		}
		public void UpdateRow(Dictionary<string, string> sData, int rowIndex)
		{
			foreach (DataGridViewColumn column in this.dataGridView1.Columns)
			{
				this.dataGridView1.Rows[rowIndex].Cells[column.Index].Value = sData[column.Name];
			}
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			this.saveFileDialog1.ShowDialog();
			if (this.saveFileDialog1.FileName != "")
			{
				using(System.IO.StreamWriter FileStream = new System.IO.StreamWriter(this.saveFileDialog1.OpenFile(), System.Text.Encoding.UTF8))
				{
					List<string> OutputString = new List<string>();
					OutputString.Clear();
					foreach (DataGridViewColumn column in this.dataGridView1.Columns)
					{
						OutputString.Add(column.Name + splitOpt[0]);
					}
					FileStream.WriteLine(string.Concat(OutputString).TrimEnd(splitOpt[0].ToArray()));
					foreach (DataGridViewRow row in this.dataGridView1.Rows)
					{
						OutputString.Clear();
						foreach (DataGridViewCell cell in row.Cells)
						{
							OutputString.Add(cell.Value.ToString() + splitOpt[0]);
						}
						FileStream.WriteLine(string.Concat(OutputString).TrimEnd(splitOpt[0].ToArray()));
					}
				}
			}
		}
	}
}
