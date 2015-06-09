using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Pathfinder.UI.Messages;
namespace Pathfinder.UI.ViewModels
{
    public class TimerViewModel : PathfinderViewModelBase
    {
        private int _ticksPerFrame;
        private bool _playing;
        
        public TimerViewModel()
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            // Defaults
            _playing = false;
            _ticksPerFrame = 5;
        }


        public ICommand PauseCommand { get; private set; }

        public ICommand PlayCommand { get; private set; }

        public ICommand NextCommand { get; private set; }


        public bool Playing
        {
            get { return _playing; }
            set
            {
                if (_playing == value)
                    return;

                _playing = value;
                RaiseSmartPropertyChanged();
            }
        }

        public int TicksPerFrame
        {
            get { return _ticksPerFrame; }

            set
            {
                if (_ticksPerFrame == value)
                    return;

                _ticksPerFrame = value;
                RaiseSmartPropertyChanged();
            }
        }


        private void CompositionTarget_Rendering(object sender, System.EventArgs e)
        {
            if (_playing)
                Tick();
        }


        protected override void SetupCommands()
        {
            PlayCommand = new RelayCommand(PlayExecute, PlayCanExecute);
            PauseCommand = new RelayCommand(PauseExecute, PauseCanExecute);
            NextCommand = new RelayCommand(NextExecute, NextCanExecute);
        }


        private bool PlayCanExecute()
        {
            return !Playing;
        }

        private void PlayExecute()
        {
            Playing = true;
        }


        private bool NextCanExecute()
        {
            return !_playing;
        }

        private void NextExecute()
        {
            Tick();
        }


        private bool PauseCanExecute()
        {
            return Playing;
        }

        private void PauseExecute()
        {
            Playing = false;
        }


        private void Tick()
        {
            this.MessengerInstance.Send(new TickMessage(TicksPerFrame));
        }
    }
}