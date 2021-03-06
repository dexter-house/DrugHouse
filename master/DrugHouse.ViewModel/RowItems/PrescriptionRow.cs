﻿/*  DrugHouse - An Hospital management software
    Copyright (C) {2015}  {Jahan Arun, J}     */

using DrugHouse.Model.Enum;
using DrugHouse.Model.Types;
using DrugHouse.Shared.Enumerations;

namespace DrugHouse.ViewModel.RowItems
{
    public class PrescriptionRow : NotificationObject
    {
        public PrescriptionRow(Prescription p)
        {
            PrescriptionValue = p;
            DrugTypeValue = p.Drug.DrugType;
        }

        public PrescriptionRow()
        {
            PrescriptionValue = new Prescription {DbStatus = RepositoryStatus.New};
        }

        public class PropName
        {
            public const string Prescription = "Prescription";
            public const string Drug = "Drug";
            public const string DrugType = "DrugType";
            public const string Remark = "Remark";
            public const string Duration = "Duration";
            public const string Name = "Name";
            public static string Dosage = "Dosage";
            public static string TotalDrugs = "TotalDrugs";
        }

        private readonly Prescription PrescriptionValue;

        public Prescription Prescription
        {
            get { return PrescriptionValue; }
        }
        public Drug Drug
        {
            get { return Prescription.Drug; }
            set
            {
                Prescription.Drug = value ?? Drug.Empty;
                DrugType = Prescription.Drug.DrugType;
                OnPropertyChanged(PropName.Drug);
                OnPropertyChanged(PropName.Name);
                Prescription.DbStatus = RepositoryStatus.Edited;
            }
        }

        public string Name
        {
            get { return Drug.Name; }
        }

        private DrugType DrugTypeValue;
        public DrugType DrugType
        {
            get { return DrugTypeValue; }
            set
            {
                if (DrugTypeValue == value) return;
                DrugTypeValue = value;
                OnPropertyChanged(PropName.DrugType);
            }
        }

        public string Dosage
        {
            get { return Prescription.Dosage; }
            set
            {
                Prescription.Dosage = value;
                OnPropertyChanged(PropName.Dosage);
                OnPropertyChanged(PropName.TotalDrugs);

                Prescription.DbStatus = RepositoryStatus.Edited;
            }
        }

        public string Remark
        {
            get { return Prescription.Remark; }
            set
            {
                Prescription.Remark = value;
                OnPropertyChanged(PropName.Remark);
                Prescription.DbStatus = RepositoryStatus.Edited;
            }
        }

        public int Duration
        {
            get { return Prescription.Duration; }
            set
            {
                Prescription.Duration = value;
                OnPropertyChanged(PropName.Duration);
                OnPropertyChanged(PropName.TotalDrugs);
                Prescription.DbStatus = RepositoryStatus.Edited;
            }
        }
        public int TotalDrugs
        {
            get { return Prescription.TotalDrugs; }
            
        }

        public void Delete()
        {
            Prescription.DbStatus = RepositoryStatus.Deleted;
        }
    }
}
