using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PatientDetector
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MediaCapture _mediaCapture;

        public MainPage()
        {
            this.InitializeComponent();

            messageElement.Text = "MainPage()";

            StartCamera();
        }

        private async void StartCamera()
        {
            try
            {
                messageElement.Text = "Initializing...";
                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync();
                messageElement.Text = "Device successfully initialized!";
                previewElement.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
                messageElement.Text = "Preview started!";
            }
            catch (Exception e)
            {
                messageElement.Text = e.Message;
            }
        }
    }
}
