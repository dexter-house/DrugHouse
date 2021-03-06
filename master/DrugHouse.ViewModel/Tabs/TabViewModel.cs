﻿/*  DrugHouse - An Hospital management software
    Copyright (C) {2015}  {Jahan Arun, J}     */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using DrugHouse.Model;
using DrugHouse.ViewModel.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace DrugHouse.ViewModel.Tabs
{
    public abstract class TabViewModel : DrugHouseViewModelBase, ITabViewModel  
    {

        public event EventHandler OnSave;
        public event PropertyChangedEventHandler OnFocusChanged;

        #region Constructors
        protected TabViewModel(MasterViewModel vm)
        {
            MasterWindow = vm;
            IsSaveEnabled = false;
            Data = DataAccess.GetInstance();
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            IsReadyToClose = false;
        }

        #endregion

        public class EventPropName
        {
            public const string RelayCommandUpdated = "RelayCommandUpdated";
        }

        #region Properties
        protected virtual IDataAccess Data { get; private set; }
        public abstract string TabName { get; }
        public bool IsReadyToClose { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public virtual Guid Guid
        {
            get
            {
                return Helper.Helper.GetAttribute<TabAttribute>(this.GetType()).Guid;
            }
        }

        public bool IsMultipleTabsAllowed
        {
            get
            {
                return Helper.Helper.GetAttribute<TabAttribute>(this.GetType()).IsMultipleInstanceAllowed;
            }
        }

        public MasterViewModel MasterWindow
        {
            get;
            private set;
        }
        public bool IsSaveEnabled { get; protected set; }
        #endregion

        #region Public Methods
        public override void RaiseDirty()
        {
            IsSaveEnabled = true;
        }
        #endregion

        #region RelayCommand Methods


        protected virtual bool CanExecuteClose()
        {
            return true;
        }

        public virtual void Refresh()
        {
            
        }

        public void PrepareToClose()
        {
            if (CanExecuteSave())
            {
                var result = MessageService.YesNoCancel("Do you want to save changes?");
                switch (result)
                {
                    case MessageResult.Yes:
                        ExecuteSave();
                        break;
                    case MessageResult.No:
                        break;
                    case MessageResult.Cancel:
                        return;
                }
            }
            Data.Dispose();
            IsReadyToClose = true;
        }                                        

        private void ExecuteSave()
        {
            try
            {
                SaveOperations();
                Refresh();
                IsSaveEnabled = false;
                if(OnSave !=null)
                    OnSave(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageService.Error("Save Operation failed with following error:" + Environment.NewLine +
                                  Environment.NewLine + ex.Message, "Save error!");
            }
        }

        protected virtual void SaveOperations()
        {
            Data.SaveTrackedChanges();   
        }

        protected bool CanExecuteSave()
        {
            return IsSaveEnabled;
        }
        #endregion

        protected void ChangeFoucs(string prop)
        {
            var handler = OnFocusChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(prop));
        }

    }
}
