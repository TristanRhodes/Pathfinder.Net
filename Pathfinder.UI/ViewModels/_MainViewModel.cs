using System;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using Pathfinder.UI.Messages;

namespace Pathfinder.UI.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
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