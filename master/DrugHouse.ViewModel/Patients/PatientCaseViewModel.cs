﻿/*  DrugHouse - An Hospital management software
    Copyright (C) {2015}  {Jahan Arun, J}     */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using DrugHouse.Model;
using DrugHouse.Model.Enum;
using DrugHouse.Model.Types;
using DrugHouse.ViewModel.RowItems;
using DrugHouse.ViewModel.Tabs;

namespace DrugHouse.ViewModel.Patients
{
    public abstract class PatientCaseViewModel : DrugHouseViewModelBase
    {
        protected readonly ICase Case;
        protected readonly IDataAccess DataAccess;


        protected PatientCaseViewModel(ICase patientCase, IDataAccess dataAccess)
        {
            Case = patientCase;
            DataAccess = dataAccess;
            if (Case.DbStatus == RepositoryStatus.New)
                Timer();

            DiagnosesItems = MasterViewModel.Globals.Diagnoses(DataAccess).Select(diagnosis => new ToggleButtonItem(diagnosis)).ToList();
            DictionaryItems = MasterViewModel.Globals.DictionaryCollection(DataAccess).Select(d => d.Name).ToList();

            foreach (var item in DiagnosesItems.Where(item => Case.PrimaryDiagnosis.Equals(item.Model)))
            {
                PrimariDiagnosisValue = item;
            }
        }

        public new class PropNameBase
        {
            public const string DoctorFee = "DoctorFee";
            public const string Diagnoses = "Diagnoses";
            public const string PrimaryDiagnosis = "PrimaryDiagnosis";
            public const string CaseDate = "CaseDate";
        }

        #region Properties

        public List<ToggleButtonItem> DiagnosesItems { get; private set; }
               
        public List<string> DictionaryItems { get; private set; }
        
        private ToggleButtonItem PrimariDiagnosisValue;
        public ToggleButtonItem PrimaryDiagnosis
        {
            get
            {
                return PrimariDiagnosisValue;
            }
            set
            {
                if (value == null)
                    return;
                PrimariDiagnosisValue = value;
                RaisePropertyChanged(PropNameBase.PrimaryDiagnosis);
                Case.PrimaryDiagnosis = (SimpleEntity)PrimariDiagnosisValue.Model;
            }
        }
   
        public DateTime CaseDate
        {
            get { return Case.Date; }
            set
            {
                Case.Date = value;
                SelectionChanged(PropNameBase.CaseDate);
            }
        }

        public decimal DoctorFee
        {
            get { return Case.DoctorFee; }
            set
            {
                Case.DoctorFee = value;
                RaisePropertyChanged(PropNameBase.DoctorFee);
            }
        }
      
        #endregion

        protected void Timer()
        {
            var timer = new Timer {Interval = 1000};
            timer.Elapsed += (sender, e) =>
            {
                CaseDate = DateTime.Now;
                if (Case.DbStatus != RepositoryStatus.New)
                    timer.Stop();
            };
            timer.Start();
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
            Case.DbStatus = RepositoryStatus.Edited;
        }
    }
}
