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
				List<string> Parts = new List<string>();
				for (string InputString = ""; !FileStream.EndOfStream;)
				{
					Parts.Clear();
					InputString = "";
					do
					{
						InputString = FileStream.ReadLine();
						List<string> buffer = InputString.Split(splitOpt, StringSplitOptions.None).ToList();
						if (Parts.Count > 0)
						{
							Parts[Parts.Count - 1] = Parts.Last() + "\n" + buffer.First();
							buffer.RemoveAt(0);
						}
						Parts.AddRange(buffer);
						if (Row == 0)
						{
							for (var i = 0; i < this.dataGridView1.Columns.Count; ++i)
							{
								Parts = Parts.Except(Parts.Where(Name => Name == this.dataGridView1.Columns[i].Name)).ToList();
							}
							foreach(var Column in Parts)
							{
								this.dataGridView1.Columns.Add(Column, Column);
							}
						}
						else
						{
							if (Parts.Count >= this.dataGridView1.Columns.Count)
							{
								this.dataGridView1.Rows.Add(Parts.ToArray());
							}
						}
					}
					while (Parts.Count < this.dataGridView1.Columns.Count);
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
				try
				{
					this.dataGridView1.Rows[rowIndex].Cells[column.Index].Value = sData[column.Name];
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					throw new NullReferenceException();
				}
			}
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			this.saveFileDialog1.ShowDialog();
			SaveNeed();
		}
		void SaveButtonClick(object sender, EventArgs e)
		{
			this.saveFileDialog1.FileName = this.openFileDialog1.FileName;
			SaveNeed();
		}
		void SaveNeed()
		{
			if (this.saveFileDialog1.FileName != "")
			{
				using(System.IO.StreamWriter FileStream = new System.IO.StreamWriter(this.saveFileDialog1.OpenFile(), System.Text.Encoding.UTF8))
				{
					string stringForWrite = "";
					foreach (DataGridViewColumn column in this.dataGridView1.Columns)
					{
						stringForWrite += column.Name + splitOpt[0];
					}
					FileStream.WriteLine(stringForWrite.TrimEnd(splitOpt[0].ToArray()));
					foreach (DataGridViewRow row in this.dataGridView1.Rows)
					{
						stringForWrite = "";
						foreach (DataGridViewCell cell in row.Cells)
						{
							if (cell.Value == null)
							{
								break;
							}
							stringForWrite += cell.Value.ToString() + splitOpt[0];
						}
						stringForWrite = stringForWrite.TrimEnd(splitOpt[0].ToArray());
						if (stringForWrite != "")
						{
							FileStream.WriteLine(stringForWrite);
						}
					}
					FileStream.Close();
				}
			}
		}
	}
}
