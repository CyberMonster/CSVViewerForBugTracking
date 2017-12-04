/*
 * Created by SharpDevelop.
 * User: Chukashev
 * Date: 04.12.2017
 * Time: 10:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CSVBugTool
{
	/// <summary>
	/// Description of DetailInfo.
	/// </summary>
	public partial class DetailInfo : Form
	{
		public int rowIndex;
		public IntPtr parentHandle;
		public DetailInfo(IntPtr Parent)
		{
			InitializeComponent();
			this.parentHandle = Parent;
		}
		
		public void SetValues(Dictionary<string, string> row, int index)
		{
			try
			{
				this.rowIndex = index;
				this.FirstPartOfCode.Text = row["ErrorCode"].Split('|')[0];
				this.Code.Text = row["ErrorCode"].Split('|')[1];
				this.BitrixTaskCode.Text = row["BitrixTaskCode"];
				this.TName.Text = row["Name"];
				this.Summary.Text = row["Summary"];
				this.Path.Text = row["Path"];
				this.SystemInfo.Text = row["SystemInfo"];
				this.DateOfDiscovery.Text = row["DateOfDiscovery"];
				this.Severity.Text = row["Severity"];
				this.Priority.Text = row["Priority"];
				this.Status.Text = row["Status"];
				this.Author.Text = row["Author"];
				this.AssignedTo.Text = row["AssignedTo"];
				this.StepsToReproduce.Text = row["StepsToReproduce"];
				this.Result.Text = row["Result"];
				this.ExpectedResult.Text = row["ExpectedResult"];
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		
		public void SendRow()
		{
			Dictionary<string, string> sData = new Dictionary<string, string>();
			try
			{
				sData.Add("ErrorCode", string.Concat(this.FirstPartOfCode.Text, '|', this.Code.Text));
				sData.Add("BitrixTaskCode", this.BitrixTaskCode.Text);
				sData.Add("Name", this.TName.Text);
				sData.Add("Summary", this.Summary.Text);
				sData.Add("Path", this.Path.Text);
				sData.Add("SystemInfo", this.SystemInfo.Text);
				sData.Add("DateOfDiscovery", this.DateOfDiscovery.Text);
				sData.Add("Severity", this.Severity.Text);
				sData.Add("Priority", this.Priority.Text);
				sData.Add("Status", this.Status.Text);
				sData.Add("Author", this.Author.Text);
				sData.Add("AssignedTo", this.AssignedTo.Text);
				sData.Add("StepsToReproduce", this.StepsToReproduce.Text);
				sData.Add("Result", this.Result.Text);
				sData.Add("ExpectedResult", this.ExpectedResult.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			SendRow(sData);
		}
		public void SendRow(Dictionary<string, string> sData)
		{
			(FromHandle(this.parentHandle) as MainForm).UpdateRow(sData, this.rowIndex);
		}
		void DetailInfoFormClosing(object sender, FormClosingEventArgs e)
		{
			SendRow();
		}
	}
}
