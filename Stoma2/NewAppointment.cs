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
	public partial class NewAppointment : Form
	{
        private List<DoctorRecord> doctorRecords = new List<DoctorRecord>();
        public ClientRecord clientRecord = null;
        public AppointmentRecord EditRecord = null; 

        public NewAppointment()
		{
			InitializeComponent();           
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{              
            if (clientRecord != null)
            {
                AppointmentFields newRecord = new AppointmentFields();
                FormDataToFields(newRecord);
                newRecord.ClientId = clientRecord.ID;
                newRecord.Create();
            }
            else
            {
                FormDataToFields(EditRecord.Data);
                EditRecord.Save();
            }                   
			Close();
		}

        private void NewAppointment_Load(object sender, EventArgs e)
        {
            foreach (DoctorRecord rec in StomaDB.GetDoctors())
            {
                doctorCategory.Items.Add(rec.GetFullName());
                doctorRecords.Add(rec);
            }
            if (EditRecord != null)
            {
                this.Text = "Редактирование приема";
                btnAdd.Text = "Сохранить";
                diagnosisTextBox.Text = EditRecord.Data.Diagnosis;
                txtTooth.Text = EditRecord.Data.Tooth.ToString();
                doctorCategory.SelectedIndex = (int)EditRecord.Data.DoctorId - 1;
            }
        }

        private void FormDataToFields(AppointmentFields fields)
        {
            fields.Date = DateUtils.GetCurrentTimestamp();
            fields.Diagnosis = diagnosisTextBox.Text;
            fields.Tooth = Int64.Parse(txtTooth.Text);
            fields.DoctorId = doctorRecords[doctorCategory.SelectedIndex].ID;
        }
	}
}
