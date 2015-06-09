using System;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using Pathfinder.UI.Messages;

namespace Pathfinder.UI.ViewModels
{
    public class PathfinderViewModelBase : ViewModelBase
    {
        public PathfinderViewModelBase()
        {
            if (IsInDesignMode)
            {
                // code runs in blend --> create design time data.
            }
            else
            {
                SetupCommands();
                SubscribeToEvents();
            }
        }


        protected virtual void SetupCommands()
        {
        }

        protected virtual void TearDownCommands()
        {
        }


        protected virtual void SubscribeToEvents()
        {
        }

        protected virtual void UnsubscribeFromEvents()
        {
        }


        protected void SafeExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }


        protected virtual void HandleException(Exception ex)
        {
            this.MessengerInstance.Send(new ExceptionMessage(ex));
        }


        protected void RaiseSmartPropertyChanged([CallerMemberName]string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }

        protected void RaiseAllPropertyChanged()
        {
            RaiseSmartPropertyChanged(null);
        }
    }
}