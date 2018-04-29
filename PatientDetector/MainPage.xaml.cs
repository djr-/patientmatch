using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
using Windows.Media.MediaProperties;
using System.Threading.Tasks;
using Microsoft.ProjectOxford;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Windows.Storage;

namespace PatientDetector
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly string FACE_IMAGE_FILENAME = "face.jpg";
        private readonly MediaCapture _mediaCapture = new MediaCapture();
        private readonly IFaceServiceClient _faceServiceClient = new FaceServiceClient("YOUR_KEY_HERE");    //TODO: Update this line with your cognitive services key.
        private readonly uint _roomNumber = 123;
        private string _imageFilePath;
        private RoomOrchestrationService _orchestrationService = new RoomOrchestrationService();

        public MainPage()
        {
            InitializeComponent();

            StartCamera();

            BeginFaceDetection();
        }

        private async void StartCamera()
        {
            try
            {
                await _mediaCapture.InitializeAsync();
                previewElement.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
            }
            catch (Exception e)
            {
                messageElement.Text = e.Message;
            }
        }

        private async Task BeginPhotoCapture()
        {
            var photoFile = await KnownFolders.PicturesLibrary.CreateFileAsync(FACE_IMAGE_FILENAME, CreationCollisionOption.ReplaceExisting);
            _imageFilePath = photoFile.Path;

            await _mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), photoFile);
        }

        private async void BeginFaceDetection()
        {
            await BeginPhotoCapture();

            // TODO: Rather than sending every image we take to Cognitive Services, we should use OpenCV on the pi to detect if there is a face in the image before attempting to pass the info to cognitive services.

            try
            {
                var faces = await Task.Run(() => DetectFaces(_imageFilePath));

                messageElement.Text = $"Found {faces.Length} faces.";

                if (faces.Length > 0)
                {
                    foreach (var face in faces)
                    {
                        var currentFaceId = face.FaceId;
                        var expectedPatient = _orchestrationService.GetPatientInfo(_roomNumber);
                        var expectedFaceId = expectedPatient.PatientFaceId;
                        var verifyResult = await _faceServiceClient.VerifyAsync(currentFaceId, expectedFaceId);

                        if (verifyResult.IsIdentical)
                        {
                            messageElement.Text = $"Patient match: {expectedPatient.PatientName}";
                        }
                        else
                        {
                            messageElement.Text = $"Patient not found in image: {expectedPatient.PatientName}";
                        }
                    }
                }
            }
            catch (FaceAPIException e)
            {
                messageElement.Text = e.ErrorMessage;
            }
            catch (Exception e)
            {
                messageElement.Text = e.Message;
            }
        }

        private async Task<Face[]> DetectFaces(string imageFilePath)
        {
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                Face[] faces = await _faceServiceClient.DetectAsync(imageFileStream);

                return faces;
            }
        }
    }
}
