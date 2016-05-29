﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Stoma2
{
	public partial class ValidatedTextBox : UserControl
	{
		private static readonly int TEXTBOX_MARGIN = 1;

		public ValidatedTextBox()
		{
			InitializeComponent();
			UpdateTextBoxPosition();
		}

		private void ValidatedTextBox_Resize(object sender, EventArgs e)
		{
			UpdateTextBoxPosition();
		}

		private int TotalWidth
		{
			get { return m_borderWidth + TEXTBOX_MARGIN; }
		}

		private void UpdateTextBoxPosition()
		{
			txtText.Location = new Point(TotalWidth, TotalWidth);
			txtText.Size = new Size(Width - TotalWidth * 2,
				Height - TotalWidth * 2);
		}

		override public string Text
		{
			get { return txtText.Text; }
			set { txtText.Text = value; }
		}

		public Font Font
		{
			get { return txtText.Font; }
			set { txtText.Font = value; }
		}
		
		private Color m_borderColor = Color.DarkGray;
		public Color BorderColor
		{
			get { return m_borderColor; }
			set
			{
				m_borderColor = value;
				Refresh();
			}
		}

		private int m_borderWidth = 1;
		public int BorderWidth
		{
			get { return m_borderWidth; }
			set
			{
				m_borderWidth = value;
				UpdateTextBoxPosition();
				Refresh();
			}
		}

		private void ValidatedTextBox_Paint(object sender, PaintEventArgs e)
		{
			float half = TotalWidth / 2;
			e.Graphics.DrawRectangle(new Pen(BorderColor, BorderWidth),
				half, half,
				txtText.Size.Width + TotalWidth, txtText.Size.Height + TotalWidth + 1);
		}

		public enum EValidationType
		{
			NoValidation,
			NotEmpty,
			Name
		}

		public EValidationType ValidationType;

		new public bool Validate()
		{
			bool result = false;

			switch (ValidationType)
			{
				case EValidationType.NoValidation:
					result = true;
					break;
				case EValidationType.NotEmpty:
					result = Text != "";
					break;
				case EValidationType.Name:
					result = Regex.IsMatch(Text, @"^[\p{L}\p{M}' \.\-]+$");
					break;
			}

			if (result)
			{
				BorderColor = Color.Green;
			}
			else
			{
				BorderColor = Color.Red;
			}

			return result;
		}

		private void txtText_TextChanged(object sender, EventArgs e)
		{
			Validate();
		}

		private void txtText_Leave(object sender, EventArgs e)
		{
			Validate();
		}
	}
}
