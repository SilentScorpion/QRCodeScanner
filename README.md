# QRCodeScanner
Created a QR Code reader for Android phones using the Open Source Zxing C# plugin 

Create QR Codes online:
http://www.qr-code-generator.com/

Nowadays, we find it easier to scan QR codes directly instead of typing them out. This functionality can be easily achieved in Unity3D using the ZXing C# plugin available here..

https://github.com/zxing

``C# (Unity)
 //run this piece of code every 10th Frame of the Update()
                       
                   try {
                       IBarcodeReader barcodeReader = new BarcodeReader ();
                       // decode the current frame
                       var result = barcodeReader.Decode (backCam.GetPixels32 (), backCam.width, backCam.height);
                       if (result != null) {
                           qrText = result.Text;
                           if(qrText.StartsWith ("http://xxx.xxxxxxxx.com/")){
                               transform.FindDeepChildBFS ("QRCodeText").gameObject.SetActive (true);
                               transform.FindDeepChildBFS ("QRCodeText").GetComponent <Text>().text = qrText;
                               camAvailable = false;
                               backCam.Stop ();
                           }
                       }
                   } catch (System.Exception ex) {
                       Debug.LogWarning ("Couldn't read QR Code: " + ex.Message);
                   }
   
               }
  ``
 


