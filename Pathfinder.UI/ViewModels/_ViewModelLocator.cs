/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Pathfinder.UI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Pathfinder.UI.Services;

namespace Pathfinder.UI.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                //SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                // Create run time view services and models
                //SimpleIoc.Default.Register<IDataService, DataService>();
            }

            // Services
            SimpleIoc.Default.Register<IFileService, DefaultFileService>(); 

            // View Models
            SimpleIoc.Default.Register<TimerViewModel>();
            SimpleIoc.Default.Register<ToolBarViewModel>();
            SimpleIoc.Default.Register<PathfinderViewModel>();
        }

        public PathfinderViewModel Pathfinder
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PathfinderViewModel>();
            }
        }

        public ToolBarViewModel ToolBar
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ToolBarViewModel>();
            }
        }

        public TimerViewModel Timer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TimerViewModel>();
            }
        }

        public MapHostViewModel MapHost
        {
            get
            {
                // TODO: Fix!
                var pathfinder = ServiceLocator.Current.GetInstance<PathfinderViewModel>();
                return pathfinder.MapHost;
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}