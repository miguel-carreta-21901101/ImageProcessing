using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CG_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        public static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            // store in the image
                            dataPtr[0] = (byte)(255 - dataPtr[0]);
                            dataPtr[1] = (byte)(255 - dataPtr[1]);
                            dataPtr[2] = (byte)(255 - dataPtr[2]);

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion topY left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int step = m.widthStep;



                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            //blue = dataPtr[0];
                            //green = dataPtr[1];
                            //red = dataPtr[2];

                            // convert to gray
                            //gray = (byte)Math.Round(((int)blue + green + red) / 3.0);

                            // store in the image
                            //dataPtr[0] = gray;
                            //dataPtr[1] = gray;
                            //dataPtr[2] = gray;

                            gray = (byte)Math.Round(((dataPtr + nChan + step * y)[0] + (dataPtr + nChan + step * y)[1] + (dataPtr + nChan + step * y)[2]) / 3.0);

                            (dataPtr + nChan + step * y)[0] = gray;
                            (dataPtr + nChan + step * y)[1] = gray;
                            (dataPtr + nChan + step * y)[2] = gray;



                            // advance the pointer to the next pixel
                            //dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        //dataPtr += padding;
                    }
                }
            }
        }

        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion topY left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento bytes (padding)
                int x, y;


                int Step = m.widthStep;



                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            // store in the image
                            dataPtr[0] = dataPtr[2];
                            dataPtr[1] = dataPtr[2];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion topY left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int Step = m.widthStep;

                double value = dataPtr[0] * contrast + bright;

                if (value <= 255 && value >= 0)
                {

                    if (nChan == 3) // image in RGB
                    {
                        for (y = 0; y < height; y++)
                        {
                            for (x = 0; x < width; x++)
                            {


                                dataPtr[0] = (byte)Math.Round(dataPtr[0] * contrast + bright);
                                dataPtr[1] = (byte)Math.Round(dataPtr[1] * contrast + bright);
                                dataPtr[2] = (byte)Math.Round(dataPtr[2] * contrast + bright);

                                // advance the pointer to the next pixel
                                dataPtr += nChan;
                            }

                            //at the end of the line advance the pointer by the aligment bytes (padding)
                            dataPtr += padding;
                        }
                    }
                }
            }
        }

        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dx, int dy)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image


                //  byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                //int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {
                    int yO, xO = 0;

                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            xO = x - dx;
                            yO = y - dy;


                            if (xO >= 0 && xO < width && yO >= 0 && yO < height)
                            {
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[2];
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[0] = 0;
                                (dataPtr + nChan * x + step * y)[1] = 0;
                                (dataPtr + nChan * x + step * y)[2] = 0;
                            }

                        }

                    }
                }
            }
        }

        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image


                // byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                //int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {
                    int yO, xO = 0;

                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xO = (int)Math.Round((x - width / 2.0) * Math.Cos(angle) - (height / 2.0 - y) * Math.Sin(angle) + width / 2.0);

                            yO = (int)Math.Round(height / 2.0 - (x - width / 2.0) * Math.Sin(angle) - (height / 2.0 - y) * Math.Cos(angle));



                            if (xO >= 0 && xO < width && yO >= 0 && yO < height)
                            {

                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[2];
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[0] = 0;
                                (dataPtr + nChan * x + step * y)[1] = 0;
                                (dataPtr + nChan * x + step * y)[2] = 0;
                            }

                        }

                    }
                }
            }
        }

        public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image


                //  byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                //  int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {
                    int yO, xO = 0;

                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            yO = (int)((y - height / 2.0) / scaleFactor + height);

                            xO = (int)((x - width / 2.0) / scaleFactor + width);

                            if (xO >= 0 && xO < width && yO >= 0 && yO < height)
                            {
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[2];
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[0] = 0;
                                (dataPtr + nChan * x + step * y)[1] = 0;
                                (dataPtr + nChan * x + step * y)[2] = 0;
                            }

                        }

                    }
                }
            }
        }

        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor,
            int centerX, int centerY)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image


                // byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                //  int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {
                    int yO, xO = 0;

                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            yO = (int)Math.Round((y - height / 2) / scaleFactor + centerY);

                            xO = (int)Math.Round((x - width / 2) / scaleFactor + centerX);

                            if (xO >= 0 && xO < width && yO >= 0 && yO < height)
                            {
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * xO + stepCopy * yO)[2];
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[0] = 0;
                                (dataPtr + nChan * x + step * y)[1] = 0;
                                (dataPtr + nChan * x + step * y)[2] = 0;
                            }

                        }

                    }
                }
            }
        }


        public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image

                byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                //  int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {

                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round((
                                                      ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 1))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 0))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 0))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 0))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y + 1))[0]) +
                                                      ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0]))

                                                   / 9.0);



                            (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round((

                                    ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 1))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 0))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 0))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 0))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y + 1))[1]) +
                                    ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1]))

                                    / 9.0); ;


                            (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round((

                                    ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2]) +
                                    ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 1))[2]) +
                                    ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2]) +
                                    ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2]) +
                                    ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * y)[2]) +
                                    ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2]) +
                                    ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2]) +
                                    ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y + 1))[2]) +
                                    ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2]))

                                    / 9.0);


                            //  (dataPtr + nChan * x + step * y)[0] = blue;
                            // (dataPtr + nChan * x + step * y)[1] = green;
                            // (dataPtr + nChan * x + step * y)[2] = red;
                        }
                    }

                    // LINHAS HORIZONTAIS

                    // CIMA  

                    for (x = 1; x < width - 1; x++)
                    {
                        (dataPtr + nChan * x + step * 0)[0] = (byte)Math.Round((

                                                                                   2 * (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[0] +
                                                                                   2 * (dataPtrCopy + nChanCopy * x + stepCopy * 0)[0] +
                                                                                   2 * (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[0] +
                                                                                   ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (0 + 1))[0]) +
                                                                                   ((dataPtrCopy + nChanCopy * x + stepCopy * (0 + 1))[0]) +
                                                                                   ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (0 + 1))[0]))

                                                                               / 9.0);

                        (dataPtr + nChan * x + step * 0)[1] = (byte)Math.Round((

                                                                                   2 * (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[1] +
                                                                                   2 * (dataPtrCopy + nChanCopy * x + stepCopy * 0)[1] +
                                                                                   2 * (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[1] +
                                                                                   ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (0 + 1))[1]) +
                                                                                   ((dataPtrCopy + nChanCopy * x + stepCopy * (0 + 1))[1]) +
                                                                                   ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (0 + 1))[1]))

                                                                               / 9.0);

                        (dataPtr + nChan * x + step * 0)[2] = (byte)Math.Round((

                                                                                   2 * (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[2] +
                                                                                   2 * (dataPtrCopy + nChanCopy * x + stepCopy * 0)[2] +
                                                                                   2 * (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[2] +
                                                                                   ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 1)[2]) +
                                                                                   ((dataPtrCopy + nChanCopy * x + stepCopy * 1)[2]) +
                                                                                   ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 1)[2]))

                                                                               / 9.0);

                        // (dataPtr + nChan * x + step * 0)[0] = blue;
                        // (dataPtr + nChan * x + step * 0)[1] = green;
                        // (dataPtr + nChan * x + step * 0)[2] = red;


                        //------------------------------------------

                        //BAIXO

                        (dataPtr + nChan * x + step * (height - 1))[0] = (byte)Math.Round((

                                    2 * (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[0] +
                                    2 * (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0] +
                                    2 * (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0] +
                                    ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[0]) +
                                    ((dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[0]) +
                                    ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[0]))

                                    / 9.0);


                        (dataPtr + nChan * x + step * (height - 1))[1] = (byte)Math.Round((

                                 2 * (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[1] +
                                 2 * (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[1] +
                                 2 * (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[1] +
                                 ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[1]) +
                                 ((dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[1]) +
                                 ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[1]))

                             / 9.0);

                        (dataPtr + nChan * x + step * (height - 1))[2] = (byte)Math.Round((

                                2 * (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[2] +
                                2 * (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[2] +
                                2 * (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[2] +
                                ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[2]) +
                                ((dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[2]) +
                                ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[2]))

                            / 9.0);

                    }


                    //LINHAS VERTICAIS


                    // ESQUERDA

                    for (y = 1; y < height - 1; y++)
                    {
                        (dataPtr + nChan * 0 + step * y)[0] = (byte)Math.Round((

                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[0] +
                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[0] +
                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[0] +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[0]) +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * y)[0]) +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[0]))

                                                                               / 9.0);

                        (dataPtr + nChan * 0 + step * y)[1] = (byte)Math.Round((

                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[1] +
                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[1] +
                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[1] +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[1]) +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * y)[1]) +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[1]))

                                                                               / 9.0);

                        (dataPtr + nChan * 0 + step * y)[2] = (byte)Math.Round((

                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[2] +
                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[2] +
                                                                                   2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[2] +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[2]) +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * y)[2]) +
                                                                                   ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[2]))

                                                                               / 9.0);




                        // DIREITA

                        //--------------


                        (dataPtr + nChan * (width - 1) + step * y)[0] = (byte)Math.Round((

                                   2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[0] +
                                   2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0] +
                                   2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0] +

                                   (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[0] +
                                   ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[0]) +
                                   ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[0]))

                                   / 9.0);


                        (dataPtr + nChan * (width - 1) + step * y)[1] = (byte)Math.Round((

                                2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[1] +
                                2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[1] +
                                2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[1] +

                                  (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[1] +
                                  ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[1]) +
                                  ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[1]))

                             / 9.0);

                        (dataPtr + nChan * (width - 1) + step * y)[2] = (byte)Math.Round((

                                2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[2] +
                                2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[2] +
                                2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[2] +

                                (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[2] +
                                ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[2]) +
                                ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[2]))

                                / 9.0);


                    }


                    //CANTOS

                    // ponto 0 : 0


                    (dataPtr + nChan * 0 + step * 0)[0] = (byte)Math.Round((
                        4 * (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[0] +
                        2 * (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[0] +
                        2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[0] +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[0]) / 9.0);


                    (dataPtr + nChan * 0 + step * 0)[1] = (byte)Math.Round((
                        4 * (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[1] +
                        2 * (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[1] +
                        2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[1] +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[1]) / 9.0);


                    (dataPtr + nChan * 0 + step * 0)[2] = (byte)Math.Round((
                        4 * (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[2] +
                        2 * (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[2] +
                        2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[2] +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[2]) / 9.0);


                    // ponto width -1 : 0


                    (dataPtr + nChan * (width - 1) + step * 0)[0] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[0] +
                        2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[0] +
                        2 * (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[0] +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[0]) / 9.0);


                    (dataPtr + nChan * (width - 1) + step * 0)[1] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[1] +
                        2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[1] +
                        2 * (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[1] +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[1]) / 9.0);


                    (dataPtr + nChan * (width - 1) + step * 0)[2] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[2] +
                        2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[2] +
                        2 * (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[2] +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[2]) / 9.0);


                    // ponto 0 : height - 1


                    (dataPtr + nChan * 0 + step * (height - 1))[0] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[0] +
                        2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[0] +
                        2 * (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[0] +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[0]) / 9.0);

                    (dataPtr + nChan * 0 + step * (height - 1))[1] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[1] +
                        2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[1] +
                        2 * (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[1] +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[1]) / 9.0);

                    (dataPtr + nChan * 0 + step * (height - 1))[2] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[2] +
                        2 * (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[2] +
                        2 * (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[2] +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[2]) / 9.0);




                    //ponto width -1  : height -1


                    (dataPtr + nChan * (width - 1) + step * (height - 1))[0] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[0] +
                        2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[0] +
                        2 * (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[0] +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[0]) / 9.0);

                    (dataPtr + nChan * (width - 1) + step * (height - 1))[1] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[1] +
                        2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[1] +
                        2 * (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[1] +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[1]) / 9.0);

                    (dataPtr + nChan * (width - 1) + step * (height - 1))[2] = (byte)Math.Round((

                        4 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[2] +
                        2 * (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[2] +
                        2 * (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[2] +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[2]) / 9.0);


                }
            }
        }



        public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] matrix,
            float matrixWeight)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image


                // byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                //  int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // CORE
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            int blueAux = (int)Math.Round((

                                                      ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0]) * matrix[0, 0] +
                                                      ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 1))[0]) * matrix[0, 1] +
                                                      ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0]) * matrix[0, 2] +
                                                      ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 0))[0]) * matrix[1, 0] +
                                                      ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 0))[0]) * matrix[1, 1] +
                                                      ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 0))[0]) * matrix[1, 2] +
                                                      ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0]) * matrix[2, 0] +
                                                      ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y + 1))[0]) * matrix[2, 1] +
                                                      ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0]) * matrix[2, 2])

                                                   / matrixWeight);

                            if (blueAux < 0)
                            {
                                (dataPtr + nChan * x + step * y)[0] = 0;
                            }
                            else if (blueAux > 255)
                            {
                                (dataPtr + nChan * x + step * y)[0] = 255;
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[0] = (byte)blueAux;
                            }



                            int greenAux = (int)Math.Round((
                                                         ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1]) * matrix[0, 0] +
                                                         ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 1))[1]) * matrix[0, 1] +
                                                         ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1]) * matrix[0, 2] +
                                                         ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 0))[1]) * matrix[1, 0] +
                                                         ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 0))[1]) * matrix[1, 1] +
                                                         ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 0))[1]) * matrix[1, 2] +
                                                         ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1]) * matrix[2, 0] +
                                                         ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y + 1))[1]) * matrix[2, 1] +
                                                         ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1]) * matrix[2, 2])

                                / matrixWeight);

                            if (greenAux < 0)
                            {
                                (dataPtr + nChan * x + step * y)[1] = 0;
                            }
                            else if (greenAux > 255)
                            {
                                (dataPtr + nChan * x + step * y)[1] = 255;
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[1] = (byte)greenAux;
                            }



                            int redAux = (int)Math.Round((

                                                          ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2]) * matrix[0, 0] +
                                                          ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 1))[2]) * matrix[0, 1] +
                                                          ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2]) * matrix[0, 2] +
                                                          ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 0))[2]) * matrix[1, 0] +
                                                          ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y - 0))[2]) * matrix[1, 1] +
                                                          ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 0))[2]) * matrix[1, 2] +
                                                          ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2]) * matrix[2, 0] +
                                                          ((dataPtrCopy + nChanCopy * (x - 0) + stepCopy * (y + 1))[2]) * matrix[2, 1] +
                                                          ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2]) * matrix[2, 2])
                                / matrixWeight);


                            if (redAux < 0)
                            {
                                (dataPtr + nChan * x + step * y)[2] = 0;
                            }
                            else if (redAux > 255)
                            {
                                (dataPtr + nChan * x + step * y)[2] = 255;
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[2] = (byte)redAux;
                            }


                        }
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // BORDERS
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                    // LINHAS HORIZONTAIS
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    // UP  

                    for (x = 1; x < width - 1; x++)
                    {
                        int blueAux = (int)Math.Round((

                                                       (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[0] * (matrix[0, 0] + matrix[1, 0]) +
                                                       (dataPtrCopy + nChanCopy * x + stepCopy * 0)[0] * (matrix[0, 1] + matrix[1, 1]) +
                                                       (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[0] * (matrix[0, 2] + matrix[1, 2]) +

                                                       ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 1)[0]) * matrix[2, 0] +
                                                       ((dataPtrCopy + nChanCopy * x + stepCopy * 1)[0]) * matrix[2, 1] +
                                                       ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 1)[0]) * matrix[2, 2])

                                                   / matrixWeight);

                        if (blueAux < 0)
                        {
                            (dataPtr + nChan * x + step * 0)[0] = 0;
                        }
                        else if (blueAux > 255)
                        {
                            (dataPtr + nChan * x + step * 0)[0] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * 0)[0] = (byte)blueAux;
                        }


                        int greenAux = (int)Math.Round((
                                                        (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[1] * (matrix[0, 0] + matrix[1, 0]) +
                                                       (dataPtrCopy + nChanCopy * x + stepCopy * 0)[1] * (matrix[0, 1] + matrix[1, 1]) +
                                                       (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[1] * (matrix[0, 2] + matrix[1, 2]) +

                                                       ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 1)[1]) * matrix[2, 0] +
                                                       ((dataPtrCopy + nChanCopy * x + stepCopy * 1)[1]) * matrix[2, 1] +
                                                       ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 1)[1]) * matrix[2, 2])

                                                   / matrixWeight);

                        if (greenAux < 0)
                        {
                            (dataPtr + nChan * x + step * 0)[1] = 0;
                        }
                        else if (greenAux > 255)
                        {
                            (dataPtr + nChan * x + step * 0)[1] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * 0)[1] = (byte)greenAux;
                        }

                        int redAux = (int)Math.Round((

                                                     (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[2] * (matrix[0, 0] + matrix[1, 0]) +
                                                       (dataPtrCopy + nChanCopy * x + stepCopy * 0)[2] * (matrix[0, 1] + matrix[1, 1]) +
                                                       (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[2] * (matrix[0, 2] + matrix[1, 2]) +

                                                       ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 1)[2]) * matrix[2, 0] +
                                                       ((dataPtrCopy + nChanCopy * x + stepCopy * 1)[2]) * matrix[2, 1] +
                                                       ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 1)[2]) * matrix[2, 2])

                                                   / matrixWeight);

                        if (redAux < 0)
                        {
                            (dataPtr + nChan * x + step * 0)[2] = 0;
                        }
                        else if (redAux > 255)
                        {
                            (dataPtr + nChan * x + step * 0)[2] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * 0)[2] = (byte)redAux;
                        }



                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        //DOWN

                        blueAux = (int)Math.Round((
                                                (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[0] * (matrix[2, 0] + matrix[1, 0]) +
                                                // (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[0] * matrix[2, 0] +
                                                (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0] * (matrix[2, 1] + matrix[1, 1]) +
                                                //(dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0] * matrix[2, 1] +
                                                (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0] * (matrix[2, 2] + matrix[2, 1]) +
                                                // (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0] * matrix[2, 2] +
                                                ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[0]) * matrix[0, 0] +
                                                ((dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[0]) * matrix[0, 1] +
                                                ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[0]) * matrix[0, 2])

                                   / matrixWeight);

                        if (blueAux < 0)
                        {
                            (dataPtr + nChan * x + step * (height - 1))[0] = 0;
                        }

                        else if (blueAux > 255)
                        {
                            (dataPtr + nChan * x + step * (height - 1))[0] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * (height - 1))[0] = (byte)blueAux;
                        }


                        greenAux = (int)Math.Round((

                                                    (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[1] * (matrix[2, 0] + matrix[1, 0]) +
                                                // (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[0] * matrix[2, 0] +
                                                (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[1] * (matrix[2, 1] + matrix[1, 1]) +
                                                //(dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0] * matrix[2, 1] +
                                                (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[1] * (matrix[2, 2] + matrix[2, 1]) +
                                                // (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0] * matrix[2, 2] +
                                                ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[1]) * matrix[0, 0] +
                                                ((dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[1]) * matrix[0, 1] +
                                                ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[1]) * matrix[0, 2])

                                               / matrixWeight);

                        if (greenAux < 0)
                        {
                            (dataPtr + nChan * x + step * (height - 1))[1] = 0;
                        }

                        else if (greenAux > 255)
                        {
                            (dataPtr + nChan * x + step * (height - 1))[1] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * (height - 1))[1] = (byte)greenAux;
                        }




                        redAux = (int)Math.Round((

                                                 (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[2] * (matrix[2, 0] + matrix[1, 0]) +
                                                // (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[0] * matrix[2, 0] +
                                                (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[2] * (matrix[2, 1] + matrix[1, 1]) +
                                                //(dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0] * matrix[2, 1] +
                                                (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[2] * (matrix[2, 2] + matrix[2, 1]) +
                                                // (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0] * matrix[2, 2] +
                                                ((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[2]) * matrix[0, 0] +
                                                ((dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[2]) * matrix[0, 1] +
                                                ((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[2]) * matrix[0, 2])

                                                / matrixWeight);

                        if (redAux < 0)
                        {
                            (dataPtr + nChan * x + step * (height - 1))[2] = 0;
                        }

                        else if (redAux > 255)
                        {
                            (dataPtr + nChan * x + step * (height - 1))[2] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * (height - 1))[2] = (byte)redAux;
                        }

                    }


                    //LINHAS VERTICAIS
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    // ESQUERDA

                    for (y = 1; y < height - 1; y++)
                    {
                        int blueAux = (int)Math.Round(((dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[0] * (matrix[0, 0] + matrix[0, 1]) +
                                                    (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[0] * (matrix[1, 0] + matrix[1, 1]) +
                                                    (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[0] * (matrix[2, 0] + matrix[2, 1]) +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[0]) * matrix[0, 2] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * y)[0]) * matrix[1, 2] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[0] * matrix[2, 2]))

                                                                               / matrixWeight);

                        if (blueAux < 0)
                        {
                            (dataPtr + nChan * 0 + step * y)[0] = 0;
                        }
                        else if (blueAux > 255)
                        {
                            (dataPtr + nChan * 0 + step * y)[0] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * 0 + step * y)[0] = (byte)blueAux;
                        }



                        int greenAux = (int)Math.Round((
                                                       (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[1] * (matrix[0, 0] + matrix[0, 1]) +
                                                    // (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[0] * matrix[0, 0] +
                                                    (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[1] * (matrix[1, 0] + matrix[1, 1]) +
                                                    // (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[0] * matrix[1, 0] +
                                                    (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[1] * (matrix[2, 0] + matrix[2, 1]) +
                                                    // (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[0] * matrix[2, 0] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[1]) * matrix[0, 2] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * y)[1]) * matrix[1, 2] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[1] * matrix[2, 2]))

                                                   / matrixWeight);

                        if (greenAux < 0)
                        {
                            (dataPtr + nChan * 0 + step * y)[1] = 0;
                        }
                        else if (greenAux > 255)
                        {
                            (dataPtr + nChan * 0 + step * y)[1] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * 0 + step * y)[1] = (byte)greenAux;
                        }


                        int redAux = (int)Math.Round(
                                                      ((dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[2] * (matrix[0, 0] + matrix[0, 1]) +
                                                    // (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[0] * matrix[0, 0] +
                                                    (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[2] * (matrix[1, 0] + matrix[1, 1]) +
                                                    // (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[0] * matrix[1, 0] +
                                                    (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[2] * (matrix[2, 0] + matrix[2, 1]) +
                                                    // (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[0] * matrix[2, 0] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[2]) * matrix[0, 2] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * y)[2]) * matrix[1, 2] +
                                                    ((dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[2] * matrix[2, 2]))

                                                   / matrixWeight);

                        if (redAux < 0)
                        {
                            (dataPtr + nChan * 0 + step * y)[2] = 0;
                        }
                        else if (redAux > 255)
                        {
                            (dataPtr + nChan * 0 + step * y)[2] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * 0 + step * y)[2] = (byte)redAux;
                        }




                        // DIREITA

                        //--------------


                        blueAux = (int)Math.Round((
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[0] * (matrix[0, 1] + matrix[0, 2]) +
                               //   (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[0] * matrix[0, 1] +
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0] * (matrix[1, 1] + matrix[1, 2]) +
                               //   (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0] * matrix[1, 1] +
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0] * (matrix[2, 1] + matrix[2, 2]) +
                               //     (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0] * matrix[2, 1] +
                               (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[0] * matrix[0, 0] +
                               ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[0]) * matrix[1, 0] +
                               (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[0] * matrix[2, 0])

                                  / matrixWeight);

                        if (blueAux < 0)
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[0] = 0;
                        }
                        else if (blueAux > 255)
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[0] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (byte)blueAux;
                        }


                        greenAux = (int)Math.Round((
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[1] * (matrix[0, 1] + matrix[0, 2]) +
                               //   (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[0] * matrix[0, 1] +
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[1] * (matrix[1, 1] + matrix[1, 2]) +
                               //   (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0] * matrix[1, 1] +
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[1] * (matrix[2, 1] + matrix[2, 2]) +
                               //     (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0] * matrix[2, 1] +
                               (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[1] * matrix[0, 0] +
                               ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[1]) * matrix[1, 0] +
                               (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[1] * matrix[2, 0])

                                               / matrixWeight);

                        if (greenAux < 0)
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[1] = 0;
                        }
                        else if (greenAux > 255)
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[1] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (byte)greenAux;
                        }

                        redAux = (int)Math.Round((

                       (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[2] * (matrix[0, 1] + matrix[0, 2]) +
                               //   (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[0] * matrix[0, 1] +
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[2] * (matrix[1, 1] + matrix[1, 2]) +
                               //   (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0] * matrix[1, 1] +
                               (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[2] * (matrix[2, 1] + matrix[2, 2]) +
                               //     (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0] * matrix[2, 1] +
                               (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[2] * matrix[0, 0] +
                               ((dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[2]) * matrix[1, 0] +
                               (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[2] * matrix[2, 0])

                                               / matrixWeight);

                        if (redAux < 0)
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[2] = 0;
                        }
                        else if (redAux > 255)
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[2] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (byte)redAux;
                        }


                    }


                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //CANTOS
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // UP LEFT
                    {
                        int blueAux = (int)Math.Round((
                            (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[0] * (matrix[0, 0] + matrix[0, 1] + matrix[1, 0] + matrix[1, 1]) +
                            (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[0] * (matrix[0, 2] + matrix[1, 2]) +
                            (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[0] * (matrix[2, 0] + matrix[2, 1]) +
                            (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[0] * matrix[2, 2]) / matrixWeight);

                        if (blueAux < 0)
                        {
                            (dataPtr + nChan * 0 + step * 0)[0] = 0;
                        }

                        else if (blueAux > 255)
                        {
                            (dataPtr + nChan * 0 + step * 0)[0] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * 0 + step * 0)[0] = (byte)blueAux;
                        }

                        int greenAux = (int)Math.Round((
                            (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[1] * (matrix[0, 0] + matrix[0, 1] + matrix[1, 0] + matrix[1, 1]) +
                            (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[1] * (matrix[0, 2] + matrix[1, 2]) +
                            (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[1] * (matrix[2, 0] + matrix[2, 1]) +
                            (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[1] * matrix[2, 2]) / matrixWeight);

                        if (greenAux < 0)
                        {
                            (dataPtr + nChan * 0 + step * 0)[1] = 0;
                        }

                        else if (greenAux > 255)
                        {
                            (dataPtr + nChan * 0 + step * 0)[1] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * 0 + step * 0)[1] = (byte)greenAux;
                        }


                        int redAux = (int)Math.Round((
                            (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[2] * (matrix[0, 0] + matrix[0, 1] + matrix[1, 0] + matrix[1, 1]) +
                            (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[2] * (matrix[0, 2] + matrix[1, 2]) +
                            (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[2] * (matrix[2, 0] + matrix[2, 1]) +
                            (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[2] * matrix[2, 2]) / matrixWeight);

                        if (redAux < 0)
                        {
                            (dataPtr + nChan * 0 + step * 0)[2] = 0;
                        }

                        else if (redAux > 255)
                        {
                            (dataPtr + nChan * 0 + step * 0)[2] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * 0 + step * 0)[2] = (byte)redAux;
                        }
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // UP RIGHT

                    int blue = (int)Math.Round((

                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[0] * (matrix[0, 2] + matrix[0, 1] + matrix[1, 2] + matrix[1, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[0] * (matrix[2, 2] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[0] * (matrix[0, 0] + matrix[1, 0]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[0] * matrix[2, 0]) / matrixWeight);



                    if (blue < 0)
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[0] = 0;
                    }

                    else if (blue > 255)
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[0] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[0] = (byte)blue;
                    }




                    int green = (int)Math.Round((

                          (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[1] * (matrix[0, 2] + matrix[0, 1] + matrix[1, 2] + matrix[1, 1]) +
                          (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[1] * (matrix[2, 2] + matrix[2, 1]) +
                          (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[1] * (matrix[0, 0] + matrix[1, 0]) +
                          (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[1] * matrix[2, 0]) / matrixWeight);



                    if (green < 0)
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[1] = 0;
                    }

                    else if (green > 255)
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[1] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[1] = (byte)green;
                    }


                    int red = (int)Math.Round((

                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[2] * (matrix[0, 2] + matrix[0, 1] + matrix[1, 2] + matrix[1, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[2] * (matrix[2, 2] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[2] * (matrix[0, 0] + matrix[1, 0]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[2] * matrix[2, 0]) / matrixWeight);



                    if (red < 0)
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[2] = 0;
                    }

                    else if (red > 255)
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[2] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * (width - 1) + step * 0)[2] = (byte)red;
                    }


                    ///////////////////////////////////////////////////////////////////////////////////////////////
                    /// BOTTOM LEFT
                    /// 
                    blue = (byte)Math.Round((

                        (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[0] * (matrix[1, 0] + matrix[1, 1] + matrix[2, 0] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[0] * (matrix[0, 0] + matrix[0, 1]) +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[0] * (matrix[1, 2] + matrix[2, 2]) +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[0] * matrix[0, 2]) / matrixWeight);


                    if (blue < 0)
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[0] = 0;
                    }
                    else if (blue > 255)
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[0] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[0] = (byte)blue;
                    }

                    red = (byte)Math.Round((

                        (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[1] * (matrix[1, 0] + matrix[1, 1] + matrix[2, 0] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[1] * (matrix[0, 0] + matrix[0, 1]) +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[1] * (matrix[1, 2] + matrix[2, 2]) +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[1] * matrix[0, 2]) / matrixWeight);


                    if (red < 0)
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[1] = 0;
                    }
                    else if (red > 255)
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[1] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[1] = (byte)red;
                    }


                    green = (byte)Math.Round((

                        (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[2] * (matrix[1, 0] + matrix[1, 1] + matrix[2, 0] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[2] * (matrix[0, 0] + matrix[0, 1]) +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[2] * (matrix[1, 2] + matrix[2, 2]) +
                        (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[2] * matrix[0, 2]) / matrixWeight);


                    if (green < 0)
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[2] = 0;
                    }
                    else if (green > 255)
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[2] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * 0 + step * (height - 1))[2] = (byte)green;
                    }


                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // BOTTOM RIGHT

                    blue = (int)Math.Round((

                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[0] * (matrix[1, 1] + matrix[1, 2] + matrix[2, 2] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[0] * (matrix[0, 2] + matrix[0, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[0] * (matrix[1, 0] + matrix[2, 0]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[0] * (matrix[0, 0])) / matrixWeight);

                    if (blue < 0)
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[0] = 0;
                    }
                    else if (blue > 255)
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[0] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[0] = (byte)blue;
                    }

                    green = (int)Math.Round((

                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[1] * (matrix[1, 1] + matrix[1, 2] + matrix[2, 2] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[1] * (matrix[0, 2] + matrix[0, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[1] * (matrix[1, 0] + matrix[2, 0]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[1] * (matrix[0, 0])) / matrixWeight);

                    if (green < 0)
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[1] = 0;
                    }
                    else if (green > 255)
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[1] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[1] = (byte)green;
                    }

                    red = (int)Math.Round((

                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[2] * (matrix[1, 1] + matrix[1, 2] + matrix[2, 2] + matrix[2, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[2] * (matrix[0, 2] + matrix[0, 1]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[2] * (matrix[1, 0] + matrix[2, 0]) +
                        (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[2] * (matrix[0, 0])) / matrixWeight);

                    if (red < 0)
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[2] = 0;
                    }
                    else if (red > 255)
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[2] = 255;
                    }
                    else
                    {
                        (dataPtr + nChan * (width - 1) + step * (height - 1))[2] = (byte)red;
                    }

                }
            }
        }





        public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                int x, y;
                int pixel, right, bottom;
                int blue, green, red;
                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {

                    for (y = 0; y < height - 1; y++)
                    {
                        for (x = 0; x < width - 1; x++)
                        {
                            //BLUE
                            pixel = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            right = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            bottom = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];

                            blue = Math.Abs(pixel - right) + Math.Abs(pixel - bottom);

                            blue = (blue < 0) ? 0 : blue;
                            blue = (blue > 255) ? 255 : blue;

                            (dataPtr + nChan * x + step * y)[0] = (byte)blue;


                            //GREEN
                            pixel = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            right = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            bottom = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];

                            green = Math.Abs(pixel - right) + Math.Abs(pixel - bottom);

                            green = (green < 0) ? 0 : green;
                            green = (green > 255) ? 255 : green;

                            (dataPtr + nChan * x + step * y)[1] = (byte)green;


                            //RED
                            pixel = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            right = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            bottom = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];

                            red = Math.Abs(pixel - right) + Math.Abs(pixel - bottom);

                            red = (red < 0) ? 0 : red;
                            red = (red > 255) ? 255 : red;

                            (dataPtr + nChan * x + step * y)[2] = (byte)red;

                        }
                    }

                    // HORIZONTAL

                    //DOWN
                    for (x = 0; x < width - 1; x++)
                    {
                        //BLUE
                        pixel = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0];
                        right = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0];

                        blue = Math.Abs(pixel - right);

                        blue = (blue < 0) ? 0 : blue;
                        blue = (blue > 255) ? 255 : blue;

                        (dataPtr + nChan * x + step * (height - 1))[0] = (byte)blue;


                        //GREEN
                        pixel = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[1];
                        right = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[1];

                        green = Math.Abs(pixel - right);

                        green = (green < 0) ? 0 : green;
                        green = (green > 255) ? 255 : green;

                        (dataPtr + nChan * x + step * (height - 1))[1] = (byte)green;


                        //RED
                        pixel = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[2];
                        right = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[2];

                        red = Math.Abs(pixel - right);

                        red = (red < 0) ? 0 : red;
                        red = (red > 255) ? 255 : red;

                        (dataPtr + nChan * x + step * (height - 1))[2] = (byte)red;
                    }


                    for (y = 0; y < height - 1; y++)
                    {
                        //BLUE
                        pixel = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0];
                        bottom = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0];

                        blue = Math.Abs(pixel - bottom);

                        blue = (blue < 0) ? 0 : blue;
                        blue = (blue > 255) ? 255 : blue;

                        (dataPtr + nChan * (width - 1) + step * y)[0] = (byte)blue;


                        //GREEN
                        pixel = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[1];
                        bottom = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[1];

                        green = Math.Abs(pixel - bottom);

                        green = (green < 0) ? 0 : green;
                        green = (green > 255) ? 255 : green;

                        (dataPtr + nChan * (width - 1) + step * y)[1] = (byte)green;


                        //RED
                        pixel = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[2];
                        bottom = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[2];

                        red = Math.Abs(pixel - bottom);

                        red = (red < 0) ? 0 : red;
                        red = (red > 255) ? 255 : red;

                        (dataPtr + nChan * (width - 1) + step * y)[2] = (byte)red;
                    }



                    (dataPtr + nChan * (width - 1) + step * (height - 1))[0] = (byte)0;
                    (dataPtr + nChan * (width - 1) + step * (height - 1))[1] = (byte)0;
                    (dataPtr + nChan * (width - 1) + step * (height - 1))[2] = (byte)0;
                }
            }
        }



        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image


                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                int x, y;
                int a, b, c, d, e, f, g, h, i;
                int sX, sY;
                int blue, green, red;

                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {

                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            //BLUE
                            a = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0];
                            b = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            c = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0];
                            d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            e = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            g = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0];
                            h = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            i = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0];

                            sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                            sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                            blue = sX + sY;

                            blue = (blue > 255) ? 255 : blue;
                            blue = (blue < 0) ? 0 : blue;

                            (dataPtr + nChan * x + step * y)[0] = (byte)blue;


                            //GREEN
                            a = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1];
                            b = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            c = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1];
                            d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            e = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            g = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1];
                            h = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            i = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1];

                            sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                            sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                            green = sX + sY;

                            green = (green > 255) ? 255 : green;
                            green = (green < 0) ? 0 : green;

                            (dataPtr + nChan * x + step * y)[1] = (byte)green;


                            //RED
                            a = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2];
                            b = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            c = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2];
                            d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            e = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            g = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2];
                            h = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            i = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2];

                            sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                            sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                            red = sX + sY;

                            red = (red > 255) ? 255 : red;
                            red = (red < 0) ? 0 : red;

                            (dataPtr + nChan * x + step * y)[2] = (byte)red;

                        }
                    }

                    // LINHAS HORIZONTAIS

                    // CIMA  

                    for (x = 1; x < width - 1; x++)
                    {
                        //BLUE
                        a = d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[0];
                        b = e = (dataPtrCopy + nChanCopy * x + stepCopy * 0)[0];
                        c = f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[0];
                        g = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 1)[0];
                        h = (dataPtrCopy + nChanCopy * x + stepCopy * 1)[0];
                        i = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 1)[0];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        blue = sX + sY;

                        blue = (blue > 255) ? 255 : blue;
                        blue = (blue < 0) ? 0 : blue;

                        (dataPtr + nChan * x + step * 0)[0] = (byte)blue;


                        //GREEN
                        a = d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[1];
                        b = e = (dataPtrCopy + nChanCopy * x + stepCopy * 0)[1];
                        c = f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[1];
                        g = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 1)[1];
                        h = (dataPtrCopy + nChanCopy * x + stepCopy * 1)[1];
                        i = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 1)[1];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        green = sX + sY;

                        green = (green > 255) ? 255 : green;
                        green = (green < 0) ? 0 : green;

                        (dataPtr + nChan * x + step * 0)[1] = (byte)green;


                        //RED
                        a = d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 0)[2];
                        b = e = (dataPtrCopy + nChanCopy * x + stepCopy * 0)[2];
                        c = f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 0)[2];
                        g = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * 1)[2];
                        h = (dataPtrCopy + nChanCopy * x + stepCopy * 1)[2];
                        i = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * 1)[2];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        red = sX + sY;

                        red = (red > 255) ? 255 : red;
                        red = (red < 0) ? 0 : red;

                        (dataPtr + nChan * x + step * 0)[2] = (byte)red;



                        //------------------------------------------

                        //BAIXO

                        //BLUE
                        g = d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[0];
                        h = e = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0];
                        i = f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0];
                        a = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[0];
                        b = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[0];
                        c = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[0];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        blue = sX + sY;

                        blue = (blue > 255) ? 255 : blue;
                        blue = (blue < 0) ? 0 : blue;

                        (dataPtr + nChan * x + step * (height - 1))[0] = (byte)blue;


                        //GREEN
                        g = d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[1];
                        h = e = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[1];
                        i = f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[1];
                        a = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[1];
                        b = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[1];
                        c = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[1];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        green = sX + sY;

                        green = (green > 255) ? 255 : green;
                        green = (green < 0) ? 0 : green;

                        (dataPtr + nChan * x + step * (height - 1))[1] = (byte)green;


                        //GREEN
                        g = d = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 1))[2];
                        h = e = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[2];
                        i = f = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[2];
                        a = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (height - 2))[2];
                        b = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 2))[2];
                        c = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 2))[2];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        red = sX + sY;

                        red = (red > 255) ? 255 : red;
                        red = (red < 0) ? 0 : red;

                        (dataPtr + nChan * x + step * (height - 1))[2] = (byte)red;
                    }

                    // VERTICAIS

                    for (y = 1; y < height - 1; y++)
                    {
                        //LEFT

                        //BLUE
                        a = b = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[0];
                        d = e = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[0];
                        g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[0];
                        c = (dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[0];
                        f = (dataPtrCopy + nChanCopy * 1 + stepCopy * y)[0];
                        i = (dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[0];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        blue = sX + sY;

                        blue = (blue > 255) ? 255 : blue;
                        blue = (blue < 0) ? 0 : blue;

                        (dataPtr + nChan * 0 + step * y)[0] = (byte)blue;


                        //GREEN
                        a = b = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[1];
                        d = e = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[1];
                        g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[1];
                        c = (dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[1];
                        f = (dataPtrCopy + nChanCopy * 1 + stepCopy * y)[1];
                        i = (dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[1];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        green = sX + sY;

                        green = (green > 255) ? 255 : green;
                        green = (green < 0) ? 0 : green;

                        (dataPtr + nChan * 0 + step * y)[1] = (byte)green;


                        //RED
                        a = b = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y - 1))[2];
                        d = e = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y))[2];
                        g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * (y + 1))[2];
                        c = (dataPtrCopy + nChanCopy * 1 + stepCopy * (y - 1))[2];
                        f = (dataPtrCopy + nChanCopy * 1 + stepCopy * y)[2];
                        i = (dataPtrCopy + nChanCopy * 1 + stepCopy * (y + 1))[2];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        red = sX + sY;

                        red = (red > 255) ? 255 : red;
                        red = (red < 0) ? 0 : red;

                        (dataPtr + nChan * 0 + step * y)[2] = (byte)red;


                        // DIREITA

                        //--------------
                        //BLUE
                        b = c = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[0];
                        e = f = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0];
                        h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0];
                        a = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[0];
                        d = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[0];
                        g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[0];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        blue = sX + sY;

                        blue = (blue > 255) ? 255 : blue;
                        blue = (blue < 0) ? 0 : blue;

                        (dataPtr + nChan * (width - 1) + step * y)[0] = (byte)blue;


                        //GREEN
                        b = c = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[1];
                        e = f = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[1];
                        h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[1];
                        a = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[1];
                        d = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[1];
                        g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[1];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        green = sX + sY;

                        green = (green > 255) ? 255 : green;
                        green = (green < 0) ? 0 : green;

                        (dataPtr + nChan * (width - 1) + step * y)[1] = (byte)green;


                        //RED
                        b = c = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y - 1))[2];
                        e = f = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[2];
                        h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[2];
                        a = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y - 1))[2];
                        d = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * y)[2];
                        g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (y + 1))[2];

                        sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                        sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                        red = sX + sY;

                        red = (red > 255) ? 255 : red;
                        red = (red < 0) ? 0 : red;

                        (dataPtr + nChan * (width - 1) + step * y)[2] = (byte)red;
                    }



                    //CANTOS

                    // UP LEFT 

                    //BLUE
                    a = b = d = e = (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[0];
                    c = f = (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[0];
                    g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[0];
                    i = (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[0];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    blue = sX + sY;

                    blue = (blue > 255) ? 255 : blue;
                    blue = (blue < 0) ? 0 : blue;

                    (dataPtr + nChan * 0 + step * 0)[0] = (byte)blue;


                    //GREEN
                    a = b = d = e = (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[1];
                    c = f = (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[1];
                    g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[1];
                    i = (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[1];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    green = sX + sY;

                    green = (green > 255) ? 255 : green;
                    green = (green < 0) ? 0 : green;

                    (dataPtr + nChan * 0 + step * 0)[1] = (byte)green;


                    //RED
                    a = b = d = e = (dataPtrCopy + nChanCopy * 0 + stepCopy * 0)[2];
                    c = f = (dataPtrCopy + nChanCopy * 1 + stepCopy * 0)[2];
                    g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * 1)[2];
                    i = (dataPtrCopy + nChanCopy * 1 + stepCopy * 1)[2];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    red = sX + sY;

                    red = (red > 255) ? 255 : red;
                    red = (red < 0) ? 0 : red;

                    (dataPtr + nChan * 0 + step * 0)[2] = (byte)red;



                    //UP RIGHT

                    //BLUE
                    b = c = e = f = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[0];
                    h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[0];
                    a = d = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[0];
                    g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[0];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    blue = sX + sY;

                    blue = (blue > 255) ? 255 : blue;
                    blue = (blue < 0) ? 0 : blue;

                    (dataPtr + nChan * (width - 1) + step * 0)[0] = (byte)blue;


                    //GREEN
                    b = c = e = f = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[1];
                    h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[1];
                    a = d = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[1];
                    g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[1];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    green = sX + sY;

                    green = (green > 255) ? 255 : green;
                    green = (green < 0) ? 0 : green;

                    (dataPtr + nChan * (width - 1) + step * 0)[1] = (byte)green;


                    //RED
                    b = c = e = f = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 0)[2];
                    h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * 1)[2];
                    a = d = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 0)[2];
                    g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * 1)[2];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    red = sX + sY;

                    red = (red > 255) ? 255 : red;
                    red = (red < 0) ? 0 : red;

                    (dataPtr + nChan * (width - 1) + step * 0)[2] = (byte)red;



                    //BOTTOM LEFT

                    //BLUE
                    d = e = g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[0];
                    a = b = (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[0];
                    f = i = (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[0];
                    c = (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[0];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    blue = sX + sY;

                    blue = (blue > 255) ? 255 : blue;
                    blue = (blue < 0) ? 0 : blue;

                    (dataPtr + nChan * 0 + step * (height - 1))[0] = (byte)blue;


                    //GREEN
                    d = e = g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[1];
                    a = b = (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[1];
                    f = i = (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[1];
                    c = (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[1];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    green = sX + sY;

                    green = (green > 255) ? 255 : green;
                    green = (green < 0) ? 0 : green;

                    (dataPtr + nChan * 0 + step * (height - 1))[1] = (byte)green;


                    //RED
                    d = e = g = h = (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 1))[2];
                    a = b = (dataPtrCopy + nChanCopy * 0 + stepCopy * (height - 2))[2];
                    f = i = (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 1))[2];
                    c = (dataPtrCopy + nChanCopy * 1 + stepCopy * (height - 2))[2];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    red = sX + sY;

                    red = (red > 255) ? 255 : red;
                    red = (red < 0) ? 0 : red;

                    (dataPtr + nChan * 0 + step * (height - 1))[2] = (byte)red;



                    //BOTTOM RIGHT

                    //BLUE
                    e = f = h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[0];
                    b = c = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[0];
                    d = g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[0];
                    a = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[0];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    blue = sX + sY;

                    blue = (blue > 255) ? 255 : blue;
                    blue = (blue < 0) ? 0 : blue;

                    (dataPtr + nChan * (width - 1) + step * (height - 1))[0] = (byte)blue;


                    //GREEN
                    e = f = h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[1];
                    b = c = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[1];
                    d = g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[1];
                    a = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[1];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    green = sX + sY;

                    green = (green > 255) ? 255 : green;
                    green = (green < 0) ? 0 : green;

                    (dataPtr + nChan * (width - 1) + step * (height - 1))[1] = (byte)green;


                    //RED
                    e = f = h = i = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 1))[2];
                    b = c = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (height - 2))[2];
                    d = g = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 1))[2];
                    a = (dataPtrCopy + nChanCopy * (width - 2) + stepCopy * (height - 2))[2];

                    sX = Math.Abs((a + 2 * d + g) - (c + 2 * f + i));
                    sY = Math.Abs((g + 2 * h + i) - (a + 2 * b + c));

                    red = sX + sY;

                    red = (red > 255) ? 255 : red;
                    red = (red < 0) ? 0 : red;

                    (dataPtr + nChan * (width - 1) + step * (height - 1))[2] = (byte)red;
                }
            }




        }


        public static void Median(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {

                // obter apontador do inicio da imagem
                MIplImage m = img.MIplImage; // imagem de destino
                MIplImage mCopy = imgCopy.MIplImage; // imagem original

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the original image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the destiny image

                int y, x, i;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int step = m.widthStep;
                int nChanCopy = mCopy.nChannels;
                int stepCopy = mCopy.widthStep;

                // Core
                for (y = 1; y < height - 1; y++)
                {
                    for (x = 1; x < width - 1; x++)
                    {
                        double[] distanciasCore =
                        {
                           
                            // 0
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 1
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 2 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 3 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 6 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 7 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 8 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2))
                        };

                        int minCore = Array.IndexOf(distanciasCore, distanciasCore.Min());

                        switch (minCore)
                        {
                            case 0:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2];
                                break;

                            case 1:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                                break;

                            case 2:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2];
                                break;

                            case 3:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                                break;

                            case 4:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                                break;

                            case 5:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                                break;

                            case 6:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2];
                                break;

                            case 7:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                                break;

                            case 8:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2];
                                break;

                            default:
                                break;
                        }
                    }
                }

                // HORIZONTAL
                for (x = 0; x < width - 1; x++)
                {

                    // UP
                    double[] distanciasH =
                       {
                           
                            // 0
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 1
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x ) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 2 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                               // AQUIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII 
                            // 3 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x ) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 6 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 7 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 8 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2))
                        };



                    int min = Array.IndexOf(distanciasH, distanciasH.Min());

                    switch (min)
                    {
                        case 0:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 1:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 2:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        case 3:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 4:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 5:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        case 6:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2];
                            break;

                        case 7:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            break;

                        case 8:
                            (dataPtr + nChan * x + step * 0)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * 0)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * 0)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2];
                            break;

                        default:
                            break;

                    }

                    //BOTTOM

                    double[] distanciasB =
                        {
                           
                            // 0
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 1
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 2 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 3 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 6 
                            0,
                                
                            // 7 
                            0,
                           
                            // 8 
                            0
                        };

                    distanciasB[6] = distanciasB[3];
                    distanciasB[7] = distanciasB[4];
                    distanciasB[8] = distanciasB[5];


                    min = Array.IndexOf(distanciasB, distanciasB.Min());

                    switch (min)
                    {
                        case 0:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 1:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 2:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        case 3:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 4:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 5:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        case 6:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 7:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 8:
                            (dataPtr + nChan * x + step * (height - 1))[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * (height - 1))[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * (height - 1))[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        default:
                            break;

                    }

                }

                //VERTICAL 
                for (y = 0; y < height - 1; y++)
                {
                    //LEFT

                    double[] distanciasL =
                       {
                           
                            // 0
                            0,

                            // 1
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 2 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 3 
                            0,
                            
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 6 
                            0,
                                
                            // 7 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 8 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2))
                        };

                    distanciasL[0] = distanciasL[1];
                    distanciasL[3] = distanciasL[4];
                    distanciasL[6] = distanciasL[7];

                    int min = Array.IndexOf(distanciasL, distanciasL.Min());

                    switch (min)
                    {
                        case 0:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 1:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 2:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2];
                            break;

                        case 3:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 4:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 5:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        case 6:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            break;

                        case 7:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            break;

                        case 8:
                            (dataPtr + nChan * 0 + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * 0 + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * 0 + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2];
                            break;

                        default:
                            break;
                    }

                    //RIGHT
                    double[] distanciasR =
                        {
                           
                            // 0
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 1
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 2 
                            0,
                                
                            // 3 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            0,
                           
                            // 6 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 7 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 8 
                            0
                        };

                    distanciasR[2] = distanciasR[1];
                    distanciasR[5] = distanciasR[4];
                    distanciasR[8] = distanciasR[7];

                    min = Array.IndexOf(distanciasR, distanciasR.Min());

                    switch (min)
                    {
                        case 0:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2];
                            break;

                        case 1:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 2:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 3:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 4:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 5:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 6:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2];
                            break;

                        case 7:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            break;

                        case 8:
                            (dataPtr + nChan * (width - 1) + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * (width - 1) + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * (width - 1) + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            break;

                        default:
                            break;
                    }


                }


                // TOP LEFT
                {
                    double[] distanciasTL =
                            {
                           
                            // 0
                            0,

                            // 1
                            0,

                            // 2 
                            0,
                                
                            // 3 
                            0,
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 6 
                            0,
                                
                            // 7 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 8 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2))
                        };
                    distanciasTL[0] = distanciasTL[4];
                    distanciasTL[1] = distanciasTL[4];
                    distanciasTL[2] = distanciasTL[5];
                    distanciasTL[3] = distanciasTL[4];
                    distanciasTL[6] = distanciasTL[7];
                }


                // TOP RIGHT
                {
                    double[] distanciasTR =
                            {
                           
                            // 0
                            0,

                            // 1
                            0,

                            // 2 
                            0,
                                
                            // 3 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            0,
                           
                            // 6 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 7 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 8 
                            0
                        };
                    distanciasTR[0] = distanciasTR[3];
                    distanciasTR[1] = distanciasTR[4];
                    distanciasTR[2] = distanciasTR[4];
                    distanciasTR[5] = distanciasTR[4];
                    distanciasTR[8] = distanciasTR[7];

                    int min1 = Array.IndexOf(distanciasTR, distanciasTR.Min());

                    switch (min1)
                    {
                        case 0:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 1:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 2:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 3:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 4:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 5:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 6:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2];
                            break;

                        case 7:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            break;

                        case 8:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];
                            break;

                        default:
                            break;
                    }
                }


                // BOTTOM LEFT
                {
                    double[] distanciasBL =
                                {
                           
                            // 0
                            0,

                            // 1
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 2 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                
                            // 3
                            0,
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                           
                            // 6 
                            0,

                            //7
                            0,

                            //8
                            0
                        };

                    distanciasBL[0] = distanciasBL[1];
                    distanciasBL[3] = distanciasBL[4];
                    distanciasBL[6] = distanciasBL[4];
                    distanciasBL[7] = distanciasBL[4];
                    distanciasBL[8] = distanciasBL[5];



                    int minBL = Array.IndexOf(distanciasBL, distanciasBL.Min());

                    switch (minBL)
                    {
                        case 0:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 1:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 2:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2];
                            break;

                        case 3:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 4:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 5:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        case 6:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 7:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 8:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            break;

                        default:
                            break;
                    }
                }


                // BOTTOM RIGHT
                {
                    double[] distanciasBR =
                            {
                           
                            // 0
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2))+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 1
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),

                            // 2 
                            0,
                                
                            // 3 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                                 
                            // 4 
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y - 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y + 1))[2], 2) )+
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x) + stepCopy * (y + 1))[2], 2)) +
                            Math.Sqrt(Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[0] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[1] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1], 2) + Math.Pow((dataPtrCopy + nChanCopy * (x) + stepCopy * (y))[2] - (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2], 2)),
                              
                            // 5 
                            0,
                           
                            // 6 
                            0,

                            //7
                            0,

                            //8
                            0
                        };


                    distanciasBR[2] = distanciasBR[1];
                    distanciasBR[5] = distanciasBR[4];
                    distanciasBR[6] = distanciasBR[3];
                    distanciasBR[7] = distanciasBR[4];
                    distanciasBR[8] = distanciasBR[4];




                    int minBR = Array.IndexOf(distanciasBR, distanciasBR.Min());

                    switch (minBR)
                    {
                        case 0:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * (y - 1))[2];
                            break;

                        case 1:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 2:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * (y - 1))[2];
                            break;

                        case 3:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 4:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 5:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 6:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * (x - 1) + stepCopy * y)[2];
                            break;

                        case 7:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        case 8:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            break;

                        default:
                            break;
                    }
                }
            }
        }


        public static int[] Histogram_Gray(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion topY left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, green, red, gray;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;


                int step = m.widthStep;

                int[] array = new int[256];
                int pos;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            pos = (int)Math.Round((
                                (dataPtr + nChan * x + step * y)[0] +
                                (dataPtr + nChan * x + step * y)[1] +
                                (dataPtr + nChan * x + step * y)[2]) / 3.0);

                            array[pos]++;
                        }
                    }
                }
                return array;
            }
        }


        // ConvertToBW
        public static void ConvertToBW(Image<Bgr, byte> img, int threshold)
        {
            unsafe
            {
                int[] histograma = new int[256];
                int width = img.Width;
                int height = img.Height;
                MIplImage mUndo = img.MIplImage;

                byte* dataPtrRead = (byte*)mUndo.imageData.ToPointer(); // Pointer to the original image
                byte* dataPtrWrite = (byte*)mUndo.imageData.ToPointer(); // Pointer to the destiny image

                int nChan = mUndo.nChannels;
                int widthStep = mUndo.widthStep;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int sum = (int)Math.Round(((dataPtrRead + nChan * x + widthStep * y)[0]
                            + (dataPtrRead + nChan * x + widthStep * y)[1]
                            + (dataPtrRead + nChan * x + widthStep * y)[2]) / 3.0);  // blue + green + red

                        if (sum > threshold)
                        {
                            (dataPtrWrite + nChan * x + widthStep * y)[0] = 255;
                            (dataPtrWrite + nChan * x + widthStep * y)[1] = 255;
                            (dataPtrWrite + nChan * x + widthStep * y)[2] = 255;
                        }
                        else
                        {
                            (dataPtrWrite + nChan * x + widthStep * y)[0] = 0;
                            (dataPtrWrite + nChan * x + widthStep * y)[1] = 0;
                            (dataPtrWrite + nChan * x + widthStep * y)[2] = 0;
                        }
                    }
                }
            }
        }

        // ConvertToBW_OtsuE
        public static void ConvertToBW_Otsu(Image<Bgr, byte> img)
        {
            unsafe
            {
                int t, i;
                double q1, q2, u1, u2;
                int width = img.Width;
                int height = img.Height;
                MIplImage mUndo = img.MIplImage;
                byte* dataPtrWrite = (byte*)mUndo.imageData.ToPointer();
                double[] variancias = new double[256];
                int[] histograma = Histogram_Gray(img);

                for (t = 0; t < 256; t++)
                {
                    q1 = 0;
                    q2 = 0;
                    u1 = 0;
                    u2 = 0;
                    for (i = 0; i <= t; i++)
                    {
                        q1 += histograma[i] / (double)(width * height);
                        u1 += i * histograma[i] / (double)(width * height);
                    }

                    u1 /= q1;

                    for (i = t + 1; i < 256; i++)
                    {
                        q2 += histograma[i] / (double)(width * height);
                        u2 += i * histograma[i] / (double)(width * height);
                    }

                    u2 /= q2;

                    variancias[t] = q1 * q2 * Math.Pow(u1 - u2, 2);
                }
                int max_pos = Array.IndexOf(variancias, variancias.Max());

                ConvertToBW(img, max_pos);
            }
        }

        /// <summary>
        /// Traffic Signs Detection
        /// </summary>
        /// <param name="img">Input image</param>
        /// <param name="imgCopy">Image Copy</param>
        /// <param name="limitSign">List of speed limit value and positions (speed limit value, Left-x,Top-y,Right-x,Bottom-y) of all detected limit signs</param>
        /// <param name="warningSign">List of value (-1) and positions (-1, Left-x,Top-y,Right-x,Bottom-y) of all detected warning signs</param>
        /// <param name="prohibitionSign">List of value (-1) and positions (-1, Left-x,Top-y,Right-x,Bottom-y) of all detected prohibition signs</param>
        /// <param name="level">Image Level</param>
        /// <returns>image with traffic signs detected</returns>
        public static Image<Bgr, byte> Signs(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, out List<string[]> limitSign, out List<string[]> warningSign, out List<string[]> prohibitionSign, int level)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                int width = img.Width;
                int height = img.Height;

                List<Image<Bgr, byte>> digitsListFromDataBase = getDigitsFromDataBase();
                List<Image<Bgr, byte>> velocityDigitsList = new List<Image<Bgr, byte>>();

                int[,] etiquetas = new int[height, width];

                Dictionary<int, int> countEtiquetasDic = new Dictionary<int, int>();
                List<int[,]> extremePointsXYEtiquetasVelocity = new List<int[,]>();
                limitSign = new List<string[]>();
                warningSign = new List<string[]>();
                prohibitionSign = new List<string[]>();


                //IMAGE HSV 


                //Transforma o vermelho em branco e tudo o resto em Preto 
                imgCopy = redToBlackAndWhite(imgCopy);

                etiquetas = counterEtiquetas(imgCopy);

                etiquetas = connectedComponents(etiquetas, width, height);

                SaveEtiquetasAsCSV(etiquetas, width, height);

                countEtiquetasDic = generateEtiquetasInDictionary(etiquetas, width, height);

                countEtiquetasDic = eliminateNoiseByPercentage(countEtiquetasDic);


                List<int[,]> extremePointsXYEtiquetas = discoverExtremeXYEtiquetas(countEtiquetasDic, etiquetas, width, height);

             //   drawRectanglesOnSigns(imgCopy, extremePointsXYEtiquetas);




                // IMAGE SIGNAL

                //Imagem dos sinais de velocidade
                Image<Bgr, byte> imgVelocitySigns = setImageVelocitySigns(img, extremePointsXYEtiquetas);


                int[,] etiquetasVelocity = counterEtiquetas(imgVelocitySigns);

                etiquetasVelocity = connectedComponents(etiquetasVelocity, imgVelocitySigns.Width, imgVelocitySigns.Height);

              //  SaveEtiquetasVelocidadesAsCSV(etiquetasVelocity, extremePointsXYEtiquetas);


                Dictionary<int, int> countEtiquetasVelocityDic = generateEtiquetasInDictionary(etiquetasVelocity, imgVelocitySigns.Width, imgVelocitySigns.Height);

                countEtiquetasVelocityDic = eliminateNoiseVelocityByPercentage(countEtiquetasVelocityDic);


                bool temWarningSign = false;
                bool temProibicaoELimite = false;
                int counterHelper = 0;

                foreach (var item in extremePointsXYEtiquetas)
                {
                    if (isTriangulo(item))
                    {
                        temWarningSign = true;
                    }
                }


                foreach (var item in extremePointsXYEtiquetas)
                {
                    // SE for triangulo guarda
                    if (isTriangulo(item))
                    {
                        string[] warningVector = new string[5];
                        warningVector[0] = "-1";
                        warningVector[1] = item[1, 1].ToString();    // left X
                        warningVector[2] = item[0, 0].ToString();    // topY Y
                        warningVector[3] = item[3, 1].ToString();    // right X
                        warningVector[4] = item[2, 0].ToString();    // bottom Y

                        warningSign.Add(warningVector);
                    }
                    else
                    {
                        List<int> numerosValidos = new List<int>();

                        extremePointsXYEtiquetasVelocity = discoverExtremeXYEtiquetas(countEtiquetasVelocityDic, etiquetasVelocity, imgVelocitySigns.Width, imgVelocitySigns.Height);
                      
                        drawRectanglesOnSigns(imgVelocitySigns, extremePointsXYEtiquetasVelocity);



                        foreach (var extremePoints in extremePointsXYEtiquetasVelocity)
                        {
                            int topY = extremePoints[0, 0];
                            int leftX = extremePoints[1, 1];
                            int bottomY = extremePoints[2, 0];
                            int rightX = extremePoints[3, 1];

                            int differenceBottomTop = bottomY - topY;
                            int differenceRightLeft = rightX - leftX;

                            velocityDigitsList.Add(imgVelocitySigns.Copy(new Rectangle(leftX, topY, differenceRightLeft, differenceBottomTop)));

                            //   imgVelocitySigns.Draw(new Rectangle(leftX, topY, differenceRightLeft, differenceBottomTop), new Bgr(0, 0, 235), 3);

                        }


                        bool isVelocityDigit = false;


                        numerosValidos = validDigitsToLimitSigns(velocityDigitsList, digitsListFromDataBase);

                        String velocity = "";
                        if (numerosValidos.Count() == 2)
                        {
                            numerosValidos.Sort();
                            numerosValidos.Reverse();

                            foreach (var i in numerosValidos)
                            {
                                velocity += i.ToString();
                                if (i == 0)
                                {
                                    isVelocityDigit = true;
                                }
                            }
                        }



                        else if (numerosValidos.Count() == 3)
                        {

                            numerosValidos.Sort();
                            numerosValidos.Reverse();

                            velocity += "1";

                            foreach (var i in numerosValidos)
                            {
                                if (i == 1)
                                {
                                    isVelocityDigit = true;
                                    continue;
                                }

                                velocity += i.ToString();
                            }
                        }


                        bool temProibicaoPorCimaDoLimite = false;

                        if (extremePointsXYEtiquetas.Count() == 2 && !temWarningSign)
                        {
                            temProibicaoPorCimaDoLimite = true;
                        }

                        if (isVelocityDigit)
                            
                        {
                            // Os sinais de proibicao vêm SEMPRE por cima dos de limite
                            if (temProibicaoPorCimaDoLimite)
                            {
                                foreach (var extremePoints in extremePointsXYEtiquetas)
                                {
                                    if (counterHelper == 0)
                                    {
                                        counterHelper++;
                                        temProibicaoELimite = true;
                                        continue;
                                    }

                                    string[] velocityVector = new string[5];
                                    velocityVector[0] = velocity;
                                    velocityVector[1] = extremePoints[1, 1].ToString();    // left X
                                    velocityVector[2] = extremePoints[0, 0].ToString();    // topY Y
                                    velocityVector[3] = extremePoints[3, 1].ToString();    // right X
                                    velocityVector[4] = extremePoints[2, 0].ToString();    // bottom Y

                                    limitSign.Add(velocityVector);
                                    break;
                                }
                            }
                            else
                            {
                                string[] velocityVector = new string[5];
                                velocityVector[0] = velocity;
                                velocityVector[1] = item[1, 1].ToString();    // left X
                                velocityVector[2] = item[0, 0].ToString();    // topY Y
                                velocityVector[3] = item[3, 1].ToString();    // right X
                                velocityVector[4] = item[2, 0].ToString();    // bottom Y

                                limitSign.Add(velocityVector);
                            }
                        }
                        else
                        {
                            if (temProibicaoELimite)
                            {
                                foreach (var extremePoints in extremePointsXYEtiquetas)
                                {
                                    string[] prohibitionVector = new string[5];
                                    prohibitionVector[0] = "-1";
                                    prohibitionVector[1] = extremePoints[1, 1].ToString();    // left X
                                    prohibitionVector[2] = extremePoints[0, 0].ToString();    // topY Y
                                    prohibitionVector[3] = extremePoints[3, 1].ToString();    // right X
                                    prohibitionVector[4] = extremePoints[2, 0].ToString();    // bottom Y

                                    prohibitionSign.Add(prohibitionVector);
                                    break;
                                }
                            }
                            else
                            {
                                string[] prohibitionVector = new string[5];
                                prohibitionVector[0] = "-1";
                                prohibitionVector[1] = item[1, 1].ToString();    // left X
                                prohibitionVector[2] = item[0, 0].ToString();    // topY Y
                                prohibitionVector[3] = item[3, 1].ToString();    // right X
                                prohibitionVector[4] = item[2, 0].ToString();    // bottom Y

                                prohibitionSign.Add(prohibitionVector);
                            }
                        }
                    }
                }

                return imgVelocitySigns;
            }
        }


        public static List<int> validDigitsToLimitSigns(List<Image<Bgr, byte>> velocityDigitsList, List<Image<Bgr, byte>> digitsList)
        {
            List<int> numerosValidos = new List<int>();
            int count;
            int[] pixeisSemelhantesEmDigitos = new int[10];

            foreach (var extremePoints in velocityDigitsList)
            {
                unsafe
                {
                    Bitmap bitMapImg = new Bitmap(extremePoints.Bitmap, new Size(160, 250));
                    bitMapImg.SetResolution(300, 300);

                    Image<Bgr, byte> imagem = new Image<Bgr, byte>(bitMapImg);

                    MIplImage digito = imagem.MIplImage;
                    byte* dataPrtDigito = (byte*)digito.imageData.ToPointer();
                    int stepDigito = digito.widthStep;
                    int nChanDigito = digito.nChannels;

                    for (int imagemBaseDados = 0; imagemBaseDados < digitsList.Count(); imagemBaseDados++)
                    {
                        count = 0;

                        MIplImage digitoBaseDados = digitsList[imagemBaseDados].MIplImage;
                        byte* dataPrtDigitoBaseDados = (byte*)digitoBaseDados.imageData.ToPointer();
                        int stepDigitoBaseDados = digitoBaseDados.widthStep;
                        int nChanDigitoBaseDados = digitoBaseDados.nChannels;


                        for (int y = 0; y < 250; y++)
                        {
                            for (int x = 0; x < 160; x++)
                            {
                                if ((dataPrtDigito + nChanDigito * x + stepDigito * y)[0] == (dataPrtDigitoBaseDados + nChanDigitoBaseDados * x + stepDigitoBaseDados * y)[0]
                                     && (dataPrtDigito + nChanDigito * x + stepDigito * y)[1] == (dataPrtDigitoBaseDados + nChanDigitoBaseDados * x + stepDigitoBaseDados * y)[1]
                                     && (dataPrtDigito + nChanDigito * x + stepDigito * y)[2] == (dataPrtDigitoBaseDados + nChanDigitoBaseDados * x + stepDigitoBaseDados * y)[2])
                                {
                                    count++;
                                }
                            }
                        }


                        Console.WriteLine(imagemBaseDados + " " + count);

                        pixeisSemelhantesEmDigitos[imagemBaseDados] = count;

                    }

                    
                        Console.WriteLine("_______________");
                    

                    int max = pixeisSemelhantesEmDigitos.Max();
                    for (int i = 0; i < pixeisSemelhantesEmDigitos.Count(); i++)
                    {

                        if (pixeisSemelhantesEmDigitos[i] == max)
                        {
                            // abaixo de 18000 é ruido
                            if (max < 18000)
                            {
                                continue;
                            }
                            numerosValidos.Add(i);
                            break;

                        }
                    }

                    resetPixeisSemelhantes(pixeisSemelhantesEmDigitos);
                }

            }
            return numerosValidos;
        }
        public static void resetPixeisSemelhantes(int[] pixeisSemelhantesEmDigitos)
        {
            pixeisSemelhantesEmDigitos[0] = 0;
            pixeisSemelhantesEmDigitos[1] = 0;
            pixeisSemelhantesEmDigitos[2] = 0;
            pixeisSemelhantesEmDigitos[3] = 0;
            pixeisSemelhantesEmDigitos[4] = 0;
            pixeisSemelhantesEmDigitos[5] = 0;
            pixeisSemelhantesEmDigitos[6] = 0;
            pixeisSemelhantesEmDigitos[7] = 0;
            pixeisSemelhantesEmDigitos[8] = 0;
            pixeisSemelhantesEmDigitos[9] = 0;
        }

        public static void drawRectanglesOnSigns(Image<Bgr, byte> img, List<int[,]> extremePoints)
        {

            foreach (var item in extremePoints)
            {
                int topY = item[0, 0];
                int leftX = item[1, 1];
                int bottomY = item[2, 0];
                int rightX = item[3, 1];

                int differenceBottomTop = bottomY - topY;
                int differenceRightLeft = rightX - leftX;

                img.Draw(new Rectangle(leftX, topY, differenceRightLeft, differenceBottomTop), new Bgr(0, 0, 235), 3);

            }
        }
        public static bool isTriangulo(int[,] extremoEtiquetas)
        {
            int pontoYMenor = extremoEtiquetas[0, 0];
            int pontoXMenor = extremoEtiquetas[1, 0];
            int pontoYMaior = extremoEtiquetas[2, 0];
            int pontoXMaior = extremoEtiquetas[3, 0];



            int pontoMedio = (pontoYMaior + pontoYMenor) / 2;

            int pontoDiferenca = pontoYMaior - pontoYMenor;




            // Tringulo piramide                                    tringulo invertido   (cedencia de passagem)
            if (pontoXMenor > pontoMedio + (0.2 * pontoDiferenca) || pontoXMenor < pontoMedio - (0.2 * pontoDiferenca))
            {

                return true;
            }

            return false;
        }

        public static Image<Bgr, byte> invertColorsDigitsFromDataBase(Image<Bgr, byte> img)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int step = m.widthStep;
                int x, y;

                for (y = 0; y < height - 1; y++)
                {
                    for (x = 0; x < width - 1; x++)
                    {
                        if ((dataPtr + nChan * x + step * y)[0] < 100 && (dataPtr + nChan * x + step * y)[1] < 100 && (dataPtr + nChan * x + step * y)[2] < 100)
                        {
                            (dataPtr + nChan * x + step * y)[0] = 255;
                            (dataPtr + nChan * x + step * y)[1] = 255;
                            (dataPtr + nChan * x + step * y)[2] = 255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * y)[0] = 0;
                            (dataPtr + nChan * x + step * y)[1] = 0;
                            (dataPtr + nChan * x + step * y)[2] = 0;
                        }
                    }
                }
                return img;
            }
        }

        public static List<Image<Bgr, byte>> getDigitsFromDataBase()
        {
            unsafe
            {
                List<Image<Bgr, byte>> digits = new List<Image<Bgr, byte>>();

                int imagemDigitos;

                for (imagemDigitos = 0; imagemDigitos <= 9; imagemDigitos++)
                {
                    Image<Bgr, byte> img = new Image<Bgr, byte>(@"C:\Users\belgi\OneDrive\Documents\Faculdade\2ANO\2SEMESTRE\ComptGrafica\digitos\" + imagemDigitos + ".png");
                    Bitmap bitMapImg = new Bitmap(img.Bitmap, new Size(160, 250));
                    bitMapImg.SetResolution(300, 300);

                    Image<Bgr, byte> digito = new Image<Bgr, byte>(bitMapImg);

                    digito = invertColorsDigitsFromDataBase(digito);
                    digits.Add(digito);
                }
                return digits;
            }
        }


        public static Image<Bgr, byte> setImageVelocitySigns(Image<Bgr, byte> img, List<int[,]> extremePointsXYEtiquetas)
        {
            unsafe
            {
                foreach (var item in extremePointsXYEtiquetas)
                {
                    if (!isTriangulo(item))
                    {
                        int topY = item[0, 0];
                        int leftX = item[1, 1];
                        int bottomY = item[2, 0];
                        int rightX = item[3, 1];



                        MIplImage m = img.MIplImage;
                        byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                        int nChan = m.nChannels;
                        int step = m.widthStep;

                        for (int y = topY + 25; y < bottomY - 25; y++)
                        {
                            for (int x = leftX + 20; x < rightX - 20; x++)
                            {

                                if ((dataPtr + nChan * x + step * y)[0] < 85 && (dataPtr + nChan * x + step * y)[1] < 85 && (dataPtr + nChan * x + step * y)[2] < 85)
                                {
                                    (dataPtr + nChan * x + step * y)[0] = 255;
                                    (dataPtr + nChan * x + step * y)[1] = 255;
                                    (dataPtr + nChan * x + step * y)[2] = 255;
                                }
                                else
                                {
                                    (dataPtr + nChan * x + step * y)[0] = 0;
                                    (dataPtr + nChan * x + step * y)[1] = 0;
                                    (dataPtr + nChan * x + step * y)[2] = 0;
                                }

                            }

                        }
                    }
                }

            }
            return img;
        }


        public static List<int[,]> discoverExtremeXYEtiquetas(Dictionary<int, int> etiquetasDic, int[,] etiquetas, int width, int height)
        {
            List<int[,]> pontosExtremosXY = new List<int[,]>();

            foreach (var item in etiquetasDic)
            {
                pontosExtremosXY.Add(locateExtremePoints(etiquetas, item.Key, width, height));

            }
            return pontosExtremosXY;
        }

        public static Dictionary<int, int> eliminateNoiseVelocityByPercentage(Dictionary<int, int> dictionaryEtiquetas)
        {

            int maiorNumeroDeEtiquetasContado = 0;
            int keyMaisContada = 0;


            foreach (var item in dictionaryEtiquetas.OrderByDescending(key => key.Value))
            {
                if (item.Value >= maiorNumeroDeEtiquetasContado)
                {
                    keyMaisContada = item.Key;
                    maiorNumeroDeEtiquetasContado = item.Value;
                }
            }


            foreach (var item in dictionaryEtiquetas.OrderByDescending(key => key.Value))
            {
                double percentagemPeloMaior = (double)item.Value / maiorNumeroDeEtiquetasContado;

                if (percentagemPeloMaior < 0.45)
                {
                    if (dictionaryEtiquetas.TryGetValue(item.Key, out int value))
                    {
                        dictionaryEtiquetas.Remove(item.Key);

                    }
                }
            }

            return dictionaryEtiquetas;
        }

        public static Dictionary<int, int> eliminateNoiseByPercentage(Dictionary<int, int> dictionaryEtiquetas)
        {

            int maiorNumeroDeEtiquetasContado = 0;
            int keyMaisContada = 0;
            double percentagem = 0.3;

            foreach (var item in dictionaryEtiquetas.OrderByDescending(key => key.Value))
            {
                if (item.Value >= maiorNumeroDeEtiquetasContado)
                {
                    keyMaisContada = item.Key;
                    maiorNumeroDeEtiquetasContado = item.Value;
                }
            }


            foreach (var item in dictionaryEtiquetas.OrderByDescending(key => key.Value))
            {
                double percentagemPeloMaior = (double)item.Value / maiorNumeroDeEtiquetasContado;

                if (maiorNumeroDeEtiquetasContado > 25000)
                {
                    percentagem = 0.1;
                }

                if (percentagemPeloMaior < percentagem)
                {
                    if (dictionaryEtiquetas.TryGetValue(item.Key, out int value))
                    {
                        dictionaryEtiquetas.Remove(item.Key);

                    }
                }


            }

            return dictionaryEtiquetas;
        }

        public static int[,] locateExtremePoints(int[,] etiquetas, int etiqueta, int width, int height)
        {
            int[,] extremoDasEtiquetas = new int[4, 2];

            int xMenor = width;
            int xMaior = -1;
            int yMenor = height;
            int yMaior = -1;

            int pontoYMenor = 0, pontoYMaior = 0;

            /* 
             ----------------------------------------------------------------------      y Menor

            -------------------------------------------------------------    x Menor                     x Maior

            -----------------------------------------------------------------------       y Maior

             */

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (etiquetas[y, x] == etiqueta)
                    {
                        if (y < yMenor)
                        {
                            yMenor = y;

                            extremoDasEtiquetas[0, 0] = y;
                            extremoDasEtiquetas[0, 1] = x;
                        
                            pontoYMenor = y;
                        }

                        if (x < xMenor)
                        {
                            xMenor = x;
                            extremoDasEtiquetas[1, 0] = y;
                            extremoDasEtiquetas[1, 1] = x;
                        
                        }

                        if (y > yMaior)
                        {
                            yMaior = y;
                            extremoDasEtiquetas[2, 0] = y;
                            extremoDasEtiquetas[2, 1] = x;
                           
                            pontoYMaior = y;
                        }

                        if (x > xMaior)
                        {
                            xMaior = x;
                            extremoDasEtiquetas[3, 0] = y;
                            extremoDasEtiquetas[3, 1] = x;
                       
                        }
                    }
                }
            }


            return extremoDasEtiquetas;
        }

        public static Dictionary<int, int> generateEtiquetasInDictionary(int[,] etiquetas, int width, int height)
        {
            //  List<int> valorEtiqueta = new List<int>();
            Dictionary<int, int> dic = new Dictionary<int, int>();
            // int i = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (etiquetas[y, x] != 0)
                    {
                        if (!dic.ContainsKey(etiquetas[y, x]))
                        {
                            dic.Add(etiquetas[y, x], 1);
                        }
                        else
                        {
                            if (dic.TryGetValue(etiquetas[y, x], out int valor))
                            {
                                dic.Remove(etiquetas[y, x]);
                                dic.Add(etiquetas[y, x], valor + 1);
                            }

                        }
                    }

                }
            }

            return dic;
        }

        public static void SaveEtiquetasAsCSV(int[,] arrayToSave, int width, int height)
        {

            using (StreamWriter file = new StreamWriter("etiquetas.csv"))
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        file.Write(arrayToSave[y, x] + ";");
                    }
                    file.Write(Environment.NewLine);
                }

            }
        }

        public static void SaveEtiquetasVelocidadesAsCSV(int[,] etiquetas, List<int[,]> extremePointsXYEtiquetas)
        {
            foreach (var item in extremePointsXYEtiquetas)
            {
                int topY = item[0, 0];
                int leftX = item[1, 1];
                int bottomY = item[2, 0];
                int rightX = item[3, 1];

                int differenceBottomTop = bottomY - topY;
                int differenceRightLeft = rightX - leftX;



                using (StreamWriter file = new StreamWriter("etiquetas.csv"))
                {
                    for (int y = topY; y < bottomY; y++)
                    {
                        for (int x = leftX; x < rightX; x++)

                        {
                            file.Write(etiquetas[y, x] + ";");
                        }
                        file.Write(Environment.NewLine);
                    }

                }
            }
        }

        public static int[,] connectedComponents(int[,] etiquetas, int width, int height)
        {
            int x, y;
            bool changed = true;

            while (changed)
            {
                changed = false;
                for (y = 2; y < height - 2; y++)
                {
                    for (x = 2; x < width - 2; x++)
                    {
                        if (etiquetas[y, x] != 0)
                        {
                            int min;

                            List<int> valorEtiqueta = new List<int>();

                            if (etiquetas[y - 1, x - 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y - 1, x - 1]);
                            }


                            if (etiquetas[y - 1, x] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y - 1, x]);
                            }

                            if (etiquetas[y - 1, x + 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y - 1, x + 1]);
                            }


                            if (etiquetas[y, x - 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y, x - 1]);
                            }

                            if (etiquetas[y, x] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y, x]);
                            }

                            if (etiquetas[y, x + 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y, x + 1]);
                            }

                            if (etiquetas[y + 1, x - 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y + 1, x - 1]);
                            }

                            if (etiquetas[y + 1, x] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y + 1, x]);
                            }

                            if (etiquetas[y + 1, x + 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y + 1, x + 1]);
                            }


                            min = valorEtiqueta.Min();


                            if (etiquetas[y, x] != min)
                            {
                                changed = true;
                                etiquetas[y, x] = min;
                            }


                        }
                    }
                }

                if (!changed) break;

                changed = false;

                for (y = height - 2; y > 2; y--)
                {
                    for (x = width - 2; x > 2; x--)
                    {
                        if (etiquetas[y, x] != 0)
                        {
                            int min;

                            List<int> valorEtiqueta = new List<int>();

                            if (etiquetas[y - 1, x - 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y - 1, x - 1]);
                            }


                            if (etiquetas[y - 1, x] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y - 1, x]);
                            }

                            if (etiquetas[y - 1, x + 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y - 1, x + 1]);
                            }


                            if (etiquetas[y, x - 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y, x - 1]);
                            }

                            if (etiquetas[y, x] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y, x]);
                            }

                            if (etiquetas[y, x + 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y, x + 1]);
                            }

                            if (etiquetas[y + 1, x - 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y + 1, x - 1]);
                            }

                            if (etiquetas[y + 1, x] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y + 1, x]);
                            }

                            if (etiquetas[y + 1, x + 1] != 0)
                            {
                                valorEtiqueta.Add(etiquetas[y + 1, x + 1]);
                            }


                            min = valorEtiqueta.Min();

                            if (etiquetas[y, x] != min)
                            {
                                changed = true;
                                etiquetas[y, x] = min;
                            }

                        }
                    }
                }
            }

            return etiquetas;
        }

        public static int[,] counterEtiquetas(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int step = m.widthStep;

                int[,] etiquetas = new int[height, width];
                int count = 1;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Se encontrar branco , conta como etiqueta
                        if ((dataPtr + nChan * x + step * y)[0] == 255)
                        {
                            etiquetas[y, x] = count;
                            count++;
                        }

                    }
                }
                return etiquetas;
            }
        }

        public static Image<Bgr, byte> redToBlackAndWhite(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int step = m.widthStep;
                int x, y;
                double[] hsv = new double[3];


                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        hsv = RgbToHsv((dataPtr + nChan * x + step * y)[2], (dataPtr + nChan * x + step * y)[1], (dataPtr + nChan * x + step * y)[0]);

                        if ((hsv[0] >= 250 || hsv[0] <= 7) && hsv[1] >= 0.3 && hsv[2] >= 0.3)
                        {
                            (dataPtr + nChan * x + step * y)[0] = (byte)255;
                            (dataPtr + nChan * x + step * y)[1] = (byte)255;
                            (dataPtr + nChan * x + step * y)[2] = (byte)255;
                        }
                        else
                        {
                            (dataPtr + nChan * x + step * y)[0] = (byte)0;
                            (dataPtr + nChan * x + step * y)[1] = (byte)0;
                            (dataPtr + nChan * x + step * y)[2] = (byte)0;
                        }
                    }
                }
                return img;
            }
        }

        public static double[] RgbToHsv(double r, double g, double b)
        {
            double[] hsv = new double[3];

            r = r / 255.0;
            g = g / 255.0;
            b = b / 255.0;

            double cmax = Math.Max(r, Math.Max(g, b));
            double cmin = Math.Min(r, Math.Min(g, b));
            double diff = cmax - cmin;
            double h = 0, s = 0;

            if (cmax == r && g >= b)
                h = 60 * ((g - b) / diff) + 0;

            else if (cmax == r && g < b)
                h = 60 * ((g - b) / diff) + 360;

            else if (cmax == g)
                h = (60 * ((b - r) / diff) + 120);

            else if (cmax == b)
                h = (60 * ((r - g) / diff) + 240);

            if (cmax == 0)
            {
                s = 0;
            }
            else if (cmax > 0)
            {
                s = diff / cmax;
            }

            double v = cmax;

            hsv[0] = h;
            hsv[1] = s;
            hsv[2] = v;

            return hsv;


        }

        // ************************************************************************************************************************************************************

        /*                                                                   Optional Methods                                                                        */

        //*************************************************************************************************************************************************************



        public static int[,] Histogram_RGB(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                byte blue, green, red;

                int[,] values = new int[3, 256];
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            values[0, blue]++;
                            values[1, green]++;
                            values[2, red]++;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
                return values;
            }
        }


        public static int[,] Histogram_All(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();
                byte blue, green, red;

                int[,] values = new int[4, 256];
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int valorPixel;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];
                            valorPixel = (int)Math.Round((blue + green + red) / 3.0);



                            values[1, blue]++;
                            values[2, green]++;
                            values[3, red]++;
                            values[0, valorPixel]++;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
                return values;
            }
        }


        public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mCopy = imgCopy.MIplImage;

                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int nChanCopy = mCopy.nChannels;
                int x, y;
                int pixel, diagnlUpRight, diagnlBottomRight, diagnlBottomLeft;
                int blue, green, red;
                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;


                if (nChan == 3) // image in RGB
                {

                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //BLUE
                            pixel = (dataPtrCopy + nChanCopy * x + stepCopy * y)[0];
                            diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                            diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0];
                            diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];

                            blue = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                            blue = (blue < 0) ? 0 : blue;
                            blue = (blue > 255) ? 255 : blue;

                            (dataPtr + nChan * x + step * y)[0] = (byte)blue;


                            //GREEN
                            pixel = (dataPtrCopy + nChanCopy * x + stepCopy * y)[1];
                            diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                            diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1];
                            diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];

                            green = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                            green = (green < 0) ? 0 : green;
                            green = (green > 255) ? 255 : green;

                            (dataPtr + nChan * x + step * y)[1] = (byte)green;


                            //RED
                            pixel = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                            diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                            diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2];
                            diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];

                            red = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                            red = (red < 0) ? 0 : red;
                            red = (red > 255) ? 255 : red;

                            (dataPtr + nChan * x + step * y)[2] = (byte)red;

                        }

                    }

                    //HORIZONTAL

                    //DOWN


                    for (x = 0; x < width - 1; x++)
                    {
                        //BLUE
                        pixel = diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[0];
                        diagnlUpRight = diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[0];
                        //diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0];
                        //  diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[0];

                        blue = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                        blue = (blue < 0) ? 0 : blue;
                        blue = (blue > 255) ? 255 : blue;

                        (dataPtr + nChan * x + step * (height - 1))[0] = (byte)blue;


                        //GREEN
                        pixel = diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[1];
                        diagnlUpRight = diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[1];
                        /*    pixel = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[1];
                            diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[1];
                            diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1];
                            diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];*/

                        green = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                        green = (green < 0) ? 0 : green;
                        green = (green > 255) ? 255 : green;

                        (dataPtr + nChan * x + step * (height - 1))[1] = (byte)green;


                        //RED
                        pixel = diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[2];
                        diagnlUpRight = diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[2];
                        /*      pixel = (dataPtrCopy + nChanCopy * x + stepCopy * (height - 1))[2];
                              diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (height - 1))[2];
                              diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2];
                              diagnlBottomLeft = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];*/

                        red = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                        red = (red < 0) ? 0 : red;
                        red = (red > 255) ? 255 : red;

                        (dataPtr + nChan * x + step * (height - 1))[2] = (byte)red;
                    }





                    for (y = 0; y < height - 1; y++)
                    {
                        pixel = diagnlUpRight = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[0];
                        //  diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[0];
                        // diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[0];
                        diagnlBottomLeft = diagnlBottomRight = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * (y + 1))[0];

                        blue = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                        blue = (blue < 0) ? 0 : blue;
                        blue = (blue > 255) ? 255 : blue;

                        (dataPtr + nChan * (width - 1) + step * y)[0] = (byte)blue;


                        //GREEN
                        pixel = diagnlUpRight = (dataPtrCopy + nChanCopy * (width - 1) + stepCopy * y)[1];
                        // diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[1];
                        // diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[1];
                        diagnlBottomLeft = diagnlBottomRight = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[1];

                        green = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                        green = (green < 0) ? 0 : green;
                        green = (green > 255) ? 255 : green;

                        (dataPtr + nChan * (width - 1) + step * y)[1] = (byte)green;


                        //RED
                        pixel = diagnlUpRight = (dataPtrCopy + nChanCopy * x + stepCopy * y)[2];
                        //   diagnlUpRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * y)[2];
                        //  diagnlBottomRight = (dataPtrCopy + nChanCopy * (x + 1) + stepCopy * (y + 1))[2];
                        diagnlBottomLeft = diagnlBottomRight = (dataPtrCopy + nChanCopy * x + stepCopy * (y + 1))[2];

                        red = Math.Abs(pixel - diagnlBottomRight) + Math.Abs(diagnlUpRight - diagnlBottomLeft);

                        red = (red < 0) ? 0 : red;
                        red = (red > 255) ? 255 : red;

                        (dataPtr + nChan * (width - 1) + step * y)[2] = (byte)red;
                    }

                    (dataPtr + nChan * (width - 1) + step * (height - 1))[0] = (byte)0;
                    (dataPtr + nChan * (width - 1) + step * (height - 1))[1] = (byte)0;
                    (dataPtr + nChan * (width - 1) + step * (height - 1))[2] = (byte)0;


                }
            }
        }


        public static void Rotation_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                //obter apontador do inicio da imagem
                MIplImage m = img.MIplImage;//imagem de destino
                MIplImage mUndo = imgCopy.MIplImage;//imagem original

                byte* dataPtrRead = (byte*)mUndo.imageData.ToPointer();//Pointer to the original image
                byte* dataPtrWrite = (byte*)m.imageData.ToPointer();//Pointer to the destiny image

                int width = imgCopy.Width;
                int height = imgCopy.Height;
                int nChan = mUndo.nChannels;
                int widthStep = mUndo.widthStep;
                byte red, green, blue;
                int x0, y0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        x0 = (int)Math.Round((x - (width / 2.0)) * Math.Cos(angle) - ((height / 2.0) - y) * Math.Sin(angle) + (width / 2.0));
                        y0 = (int)Math.Round((height / 2.0) - (x - (width / 2.0)) * Math.Sin(angle) - ((height / 2.0) - y) * Math.Cos(angle));

                        if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
                        {
                            blue = (dataPtrRead + nChan * x0 + widthStep * y0)[0];
                            green = (dataPtrRead + nChan * x0 + widthStep * y0)[1];
                            red = (dataPtrRead + nChan * x0 + widthStep * y0)[2];

                        }
                        else
                        {
                            blue = green = red = 0;
                        }

                        (dataPtrWrite + nChan * x + widthStep * y)[0] = blue;
                        (dataPtrWrite + nChan * x + widthStep * y)[1] = green;
                        (dataPtrWrite + nChan * x + widthStep * y)[2] = red;

                    }

                }

            }
        }


        public static void Scale_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                //obter apontador do inicio da imagem
                MIplImage m = img.MIplImage;//imagem de destino
                MIplImage mUndo = imgCopy.MIplImage;//imagem original

                byte* dataPtrR = (byte*)mUndo.imageData.ToPointer();//Pointer to the original image
                byte* dataPtrW = (byte*)m.imageData.ToPointer();//Pointer to the destiny image

                int width = imgCopy.Width;
                int height = imgCopy.Height;
                int nChan = mUndo.nChannels;
                int widthStep = mUndo.widthStep;
                byte red, green, blue;
                int x0, y0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        x0 = (int)Math.Round(x / scaleFactor);
                        y0 = (int)Math.Round(y / scaleFactor);

                        if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
                        {
                            blue = (dataPtrR + nChan * x0 + widthStep * y0)[0];
                            green = (dataPtrR + nChan * x0 + widthStep * y0)[1];
                            red = (dataPtrR + nChan * x0 + widthStep * y0)[2];

                        }
                        else
                        {
                            blue = green = red = 0;
                        }

                        (dataPtrW + nChan * x + widthStep * y)[0] = blue;
                        (dataPtrW + nChan * x + widthStep * y)[1] = green;
                        (dataPtrW + nChan * x + widthStep * y)[2] = red;

                    }

                }

            }
        }


        public static void Scale_point_xy_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage mUndo = imgCopy.MIplImage;

                byte* dataPtrRead = (byte*)mUndo.imageData.ToPointer();
                byte* dataPtrWrite = (byte*)m.imageData.ToPointer();

                int width = imgCopy.Width;
                int height = imgCopy.Height;
                int nChan = mUndo.nChannels;
                int widthStep = mUndo.widthStep;
                int padding = mUndo.widthStep - mUndo.nChannels * mUndo.width;
                byte red, green, blue;
                int xO, yO;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        xO = (int)Math.Round(centerX + (x - width / 2) / scaleFactor);
                        yO = (int)Math.Round(centerY + (y - height / 2) / scaleFactor);



                        if (xO >= 0 && xO < width && yO >= 0 && yO < height)
                        {
                            blue = (dataPtrRead + nChan * xO + widthStep * yO)[0];
                            green = (dataPtrRead + nChan * xO + widthStep * yO)[1];
                            red = (dataPtrRead + nChan * xO + widthStep * yO)[2];
                        }
                        else
                        {
                            blue = red = green = 0;
                        }

                        (dataPtrWrite + nChan * x + widthStep * y)[0] = blue;
                        (dataPtrWrite + nChan * x + widthStep * y)[1] = green;
                        (dataPtrWrite + nChan * x + widthStep * y)[2] = red;
                    }
                }



            }
        }

    }
}








