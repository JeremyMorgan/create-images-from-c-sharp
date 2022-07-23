using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

internal class Program
{
    private static void Main(string[] args)
    {
        string win1 = "Test Window"; //The name of the window

        // initial height and width of image we're going to create.
        int width = 800;
        int height = 600;

        CvInvoke.NamedWindow(win1); //Create the window using the specific name
        Mat img = new Mat(height, width, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
        img.SetTo(new Bgr(0, 0, 255).MCvScalar); // set it to gray

        // draw a fancy early 2000s gradient background 
        for (int i = 0; i < height; i++)
        {            
            CvInvoke.Line(img, new System.Drawing.Point(0, i), new System.Drawing.Point(width, i), new Bgr((i*255/height), 0, 0).MCvScalar, 1);
        }

        // get memory info 
        string meminfo = runCommand("wmic computersystem get totalphysicalmemory");

        // get CPU Info 
        string cpuName = runCommand("wmic cpu get name");
        string cpuDescription = runCommand("wmic cpu get Description");
        string cpuMaxClockSpeed = runCommand("wmic cpu get MaxClockSpeed");       
        string cpuCurrentClockSpeed = runCommand("wmic cpu get CurrentClockSpeed");
        string cpuNumberofCores = runCommand("wmic cpu get NumberOfCores");
        string cpuNumberofLogicalProcessors = runCommand("wmic cpu get NumberOfLogicalProcessors");
        
        // set margins and line height
        int leftMargin = 130;
        int topMargin = 200;
        int lineHeight = 30;
        int lineNumber = 0;

        // create our rectangle
        System.Drawing.Rectangle mainBox = new System.Drawing.Rectangle(100,150,600,270);

        // start writing our image
        string headerMessage = "Jeremy's System";
        writeHeader(headerMessage, headerMessage.Length*17, 80, img);
        
        // draw our rectangle
        CvInvoke.Rectangle(img, mainBox, new Bgr(255, 255, 255).MCvScalar, -1, LineType.FourConnected);

        // write to image data
        writeBlackText("Memory = " + meminfo, leftMargin, topMargin+(lineHeight * lineNumber++), img);
        writeBlackText(cpuName, leftMargin, topMargin+(lineHeight * lineNumber++), img);
        writeBlackText(cpuDescription, leftMargin, topMargin+(lineHeight * lineNumber++), img);
        writeBlackText("Max Clock Speed: " + cpuMaxClockSpeed, leftMargin, topMargin+(lineHeight * lineNumber++), img);
        writeBlackText("Current Clock Speed: " + cpuCurrentClockSpeed, leftMargin, topMargin+(lineHeight * lineNumber++), img);
        writeBlackText("Cores: " + cpuNumberofCores, leftMargin, topMargin+(lineHeight * lineNumber++), img);
        writeBlackText("Logical Processors: " + cpuNumberofLogicalProcessors, leftMargin, topMargin+(lineHeight * lineNumber++), img);
        
        // show the image! 
        CvInvoke.Imshow(win1, img); //Show the image
        CvInvoke.WaitKey(0);  //Wait for the key pressing event
        CvInvoke.Imwrite("ourImage.png",img);

        CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed       
    }

    private static string getInfo(string information){
        return information.Split('\n')[1].Trim();
    }

    private static void writeBlackText(string textToWrite, int x, int y, Mat img )
    {
        CvInvoke.PutText(
           img,
           textToWrite,
           new System.Drawing.Point(x, y),
           FontFace.HersheyTriplex,
           .7,
           new Bgr(0, 0, 0).MCvScalar);
    }
    private static void writeHeader(string textToWrite, int x, int y, Mat img )
    {
        CvInvoke.PutText(
           img,
           textToWrite,
           new System.Drawing.Point(x, y),
           FontFace.HersheyTriplex,
           1,
           new Bgr(255, 255, 255).MCvScalar);
    }

    private static string runCommand(string strCommand)
    {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + strCommand);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();

                proc.StartInfo = procStartInfo;
                proc.Start();

                return proc.StandardOutput.ReadToEnd().Split('\n')[1].Trim();
            }
            catch (Exception objException)
            {
                return objException.Message;
            }
    }
}