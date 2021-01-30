using System;
using UnityEngine;

/// <summary>
/// Used by Sci-Fi_Objects_Pack1
/// </summary>
namespace eWolf.Common.Helper
{
    public class SceneHelpers : MonoBehaviour
    {
        // Needed so we can show custom buttons in the Inspector

        public void Update()
        {
            UpdateSnapshot();
        }

        private void UpdateSnapshot()
        {
            if (Input.GetKeyDown("c"))
            {
                string screenshotFilename;
                DateTime td = System.DateTime.Now;
                screenshotFilename = "..//ScreenShots//SS - " + td.ToString("yyyy MM dd-HH-mm-ss-ffff") + ".png";
                ScreenCapture.CaptureScreenshot(screenshotFilename);
                Debug.Log("Taken Snap Shot." + td.ToString("yyyy MM dd-HH-mm-ss-ffff"));
            }
        }
    }
}
