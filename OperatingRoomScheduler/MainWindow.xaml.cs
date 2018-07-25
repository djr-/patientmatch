using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.ProjectOxford;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace OperatingRoomScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFaceServiceClient _faceServiceClient = new FaceServiceClient("YOUR_KEY_HERE");    //TODO: Update this line with your cognitive services key.

        public MainWindow()
        {
            InitializeComponent();
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Send the image to the face api detect.
            string imageFilePath = @"C:\PatientMatch\me_again.jpg";
            Guid guidForMeJpg = Guid.Parse("0eb7c2a9-2d69-4a50-9b0b-41695bdac620");
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                Face[] faces = await _faceServiceClient.DetectAsync(imageFileStream);
                VerifyResult verifyResult = await _faceServiceClient.VerifyAsync(faces.First().FaceId, guidForMeJpg);
                //Console.WriteLine(verifyResult.IsIdentical);
                //Console.WriteLine(verifyResult.Confidence);
                //Console.WriteLine(faces.FirstOrDefault()?.FaceId);
                // TODO: Instead of manually saving this, we should let the orchestration service submit patient images as needed.
                // This ID expires after 24h.
                // Temporary GUID for hackathon: {0eb7c2a9-2d69-4a50-9b0b-41695bdac620}
            }
        }
    }
}
