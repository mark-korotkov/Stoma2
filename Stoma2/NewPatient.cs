﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stoma2
{
	public partial class NewPatient : Form
	{
        private bool m_baseModified = false;
        public ClientRecord RecordForEditing = null;

        public NewPatient()
		{
			InitializeComponent();
		}

		private void FirstAppointment_Load(object sender, EventArgs e)
		{
			// Setting up Expander control
			Expander expander = new Expander();
			expander.Size = pnlExtra.Size;
			expander.Location = pnlExtra.Location;
			expander.BorderStyle = BorderStyle.FixedSingle;

			ExpanderHelper.CreateLabelHeader(expander, "Дополнительная информация",
				SystemColors.ActiveBorder, Properties.Resources.Collapse, Properties.Resources.Expand);

			pnlExtra.Location = new Point(0, 0);
			expander.Content = pnlExtra;
			Controls.Remove(pnlExtra);

			expander.Collapse();

			this.Controls.Add(expander);
			//

			// Filling date combo boxes
			for (int i = DateTime.Now.Year; i >= 1920; i--)
			{
				cmbYear.Items.Add(i.ToString());
			}

			for (int i = 1; i <= 31; i++)
			{
				cmbDay.Items.Add(i.ToString());
			}
			//

            if (RecordForEditing != null)
            {
                Text = "Редактировать пациента";
                btnApply.Text = "Сохранить";
                FieldsToFormData(RecordForEditing.Data);
            }
        }

		private void btnApply_Click(object sender, EventArgs e)
		{
            if (RecordForEditing == null)
            {
                ClientFields newRecord = new ClientFields();
                FormDataToFields(newRecord);
                newRecord.Create();
            }
            else
            {
                FormDataToFields(RecordForEditing.Data);
                RecordForEditing.Save();
            }

            m_baseModified = true;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

        public bool BaseModified
        {
            get { return m_baseModified; }
        }

        private void FormDataToFields(ClientFields fields)
        {
            fields.NameFirst = nameFirstBox.Text;
            fields.NameLast = nameLastBox.Text;
            fields.NamePatronymic = patronymicBox.Text;

            try
            {
                fields.Birthday = (new DateTime(
                    Int32.Parse(cmbYear.Text),
                    cmbMonth.SelectedIndex + 1,
                    Int32.Parse(cmbDay.Text)
                )).ToString("yyyy-MM-dd HH:mm");
            }
            catch (Exception)
            {
                fields.Birthday = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm");
            }

            fields.AddressSubject = addressSubjectBox.Text;
            fields.AddressCity = addressCityBox.Text;
            fields.AddressStreet = addressStreetBox.Text;
            fields.AddressBuilding = addressBuildingBox.Text;
            fields.AddressApartment = addressApartmentBox.Text;
            fields.Workplace = workplaceBox.Text;
            fields.Position = positionBox.Text;
            fields.Phone = phoneBox.Text;
            fields.Notes = notesBox.Text;
        }

        private void FieldsToFormData(ClientFields fields)
        {
            nameFirstBox.Text = fields.NameFirst;
            nameLastBox.Text = fields.NameLast;
            patronymicBox.Text = fields.NamePatronymic;

            if (fields.Birthday != DateTime.MinValue.ToString("yyyy-MM-dd HH:mm"))
            {
                cmbYear.Text = DateTime.Parse(fields.Birthday).Year.ToString();
                cmbMonth.SelectedIndex = DateTime.Parse(fields.Birthday).Month - 1;
                cmbDay.Text = DateTime.Parse(fields.Birthday).Day.ToString();
            }

            addressSubjectBox.Text = fields.AddressSubject;
            addressCityBox.Text = fields.AddressCity;
            addressStreetBox.Text = fields.AddressStreet;
            addressBuildingBox.Text = fields.AddressBuilding;
            addressApartmentBox.Text = fields.AddressApartment;
            workplaceBox.Text = fields.Workplace;
            positionBox.Text = fields.Position;
            phoneBox.Text = fields.Phone;
            notesBox.Text = fields.Notes;
        }
    }
}
