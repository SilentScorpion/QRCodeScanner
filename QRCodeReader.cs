/**
 * 
 * @author Ajay Kamath
 * 
 */

    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using ZXing;
    using ZXing.QrCode;
    
    public class QRCodeReader : MonoBehaviour {
    
        /*** Private variables here ***/
        private bool camAvailable;
        private WebCamTexture backCam;
        private Texture defaultBackground;
        private bool placeDelay;
        private string qrText;
        private int i = 0;
    
        public RawImage background;
        public AspectRatioFitter fitter;
        public static bool qrDecoded;

        void OnEnable() {
            defaultBackground = background.texture;
            WebCamDevice[] devices = WebCamTexture.devices;
        
            if (devices.Length == 0) {
                        camAvailable = false;
                         Debug.Log("No cameras detected");
                         return;
                     }
        
            //If any devices are detected
            for (int i = 0; i < devices.Length; i++) {
                         if (!devices[i].isFrontFacing) {
                                 backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                             }
                     }
        
            //If no Rear Camera is found
            if (backCam == null) {
                         Debug.Log("No Rear Camera found");
                         return;
                     }
        
            backCam.Play();
                 background.texture = backCam;
                 camAvailable = true;
        
        }

    void Update() {

        //Read the QR Code from the Scanner

        if (camAvailable) {
            float ratio = (float)backCam.width / (float)backCam.height;
            fitter.aspectRatio = ratio;
            float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient = -backCam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

            //run this piece of code every 10th Frame of the Update()
            if (i % 10 == 0) {

                try {
                    IBarcodeReader barcodeReader = new BarcodeReader();
                    // decode the current frame
                    var result = barcodeReader.Decode(backCam.GetPixels32(), backCam.width, backCam.height);
                    if (result != null) {
                        qrText = result.Text;
                        if (qrText.StartsWith("http://cubedots.codeadroits.com/")) {
                            transform.FindDeepChildBFS("QRCodeText").gameObject.SetActive(true);
                            transform.FindDeepChildBFS("QRCodeText").GetComponent<Text>().text = qrText;
                            OpenProjectWindow();
                            camAvailable = false;
                            backCam.Stop();
                        }
                    }
                }
                catch (System.Exception ex) {
                    Debug.LogWarning("Couldn't read QR Code: " + ex.Message);
                }

            }
            i++;
        }

        //What happens when the user presses the hardware back button in Android phones
        if (Input.GetKeyDown(KeyCode.Escape)) {
            backCam.Stop();
            backCam = null;
            camAvailable = false;
            background.texture = null;
            gameObject.SetActive(false);
        }
    }
   
   }
