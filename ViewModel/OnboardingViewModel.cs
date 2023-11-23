using FSM.View.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSM.ViewModel
{
    public record OnBoardingModel(string ImageSource, string Title, string Description);
    public partial class OnboardingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<OnBoardingModel> OnboardingSteps { get; set; } = new();
        public OnboardingViewModel()
        {
            OnboardingSteps.Add(new OnBoardingModel("image1.jpg", "Hi, welcome to Field Service Management.!", "A service ticket management system"));
            OnboardingSteps.Add(new OnBoardingModel("image2.jpg", "Hi, welcome to Field Service Management.!", "A service ticket management system"));
            OnboardingSteps.Add(new OnBoardingModel("image3.jpg", "Hi, welcome to Field Service Management.!", "A service ticket management system"));
        }
        private bool isLastStep;
        public bool IsLastStep
        {
            get => isLastStep;
            set
            {
                if (isLastStep != value)
                {
                    isLastStep = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLastStep)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotLastStep)));

                }
            }
        }
        public bool IsNotLastStep => !IsLastStep;

        [RelayCommand]
        async Task GoToLoginPage()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(LoginPage), true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to launch maps: {ex.Message}");
                await Shell.Current.DisplayAlert("Error, Navigation error!", ex.Message, "OK");
            }
            
        }
    }
}
