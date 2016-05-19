/*/*
Assigment-: YouTube downloader
Author: Brighto Paul
Purpose:To become comfortable with building C# applications in Visual Studio, 
building an application  that dowload videos from YouTube in different resolution.
Date of Submission:4th April,2016
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExtractor;
using System.IO;

namespace Assignment_3
{
    public partial class YouTubeDownloader : Form
    {
        public YouTubeDownloader()
        {
            InitializeComponent();
        }
        private VideoInfo[] videoInfos;
        private string FilePath;

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Declared the About Box
            AboutBox abtbox = new AboutBox();
            abtbox.Show();
        }

        private void getVideoButton_Click(object sender, EventArgs e)//grab video
        {
            if (UrlTextBox.Text!=String.Empty)
            {
                try {
                    videoQualityComboBox.Items.Clear();
                    // YouTube URL link 
                    string youTubeURLString = UrlTextBox.Text;

                    // Get all the available video formats
                    videoInfos = DownloadUrlResolver.GetDownloadUrls(youTubeURLString).ToArray();

                    string quality;//temp variable to store the quality of video
                    //save different videos to the collection 
                    foreach (VideoInfo video in videoInfos) {
                        quality = "Resolution:"+video.Resolution + "p," + "Video Extentsion:(" + video.VideoExtension + ")";
                        videoQualityComboBox.Items.Add(quality);
                    }
                    videoQualityComboBox.SelectedIndex = 0;//set the default resolution                
                }
                catch 
                {
                    MessageBox.Show("Invalid URL");
                }
            }
            else
            {
                MessageBox.Show("Please input a video URL");
            }
            
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();//exit program
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (videoQualityComboBox.Text != String.Empty)
            {
                try {
                    //create the saveDialogbox 
                    VideoInfo video = videoInfos[videoQualityComboBox.SelectedIndex];//selct the video to download
                    saveFileDialog.Filter = "Your video File(*"+video.VideoExtension+")|*"+video.VideoExtension+"|All Files(*.*)|*.*";//adding filter
                    saveFileDialog.FileName = video.Title + "_" + video.Resolution + "p";
                    saveFileDialog.DefaultExt = video.VideoExtension;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)//checks whether the user clicked cancel
                    saveTextBox.Text = saveFileDialog.FileName;//passing the file path
                }
                catch
                {
                    MessageBox.Show("An Error occured in setting up the Location, Please try again");
                }
            }
            else
            {
                MessageBox.Show("Please select the video quality");
            }                   
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            //check whether the text boxes are empty
            if (UrlTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please input a video URL");
            }
            else if(videoQualityComboBox.Text== String.Empty)
            {
                MessageBox.Show("Please Select the video quality");
            }
            else if (saveTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please set the location to store the video");
            }
            else
            {
                FilePath = System.IO.Path.GetDirectoryName(saveTextBox.Text);
                if (Directory.Exists(FilePath))
                {
                    try
                    {
                        // Set cursor as hourglass
                        Cursor.Current = Cursors.WaitCursor;
                        //unhide Progressbar
                        progressBar.Show();
                        progressBar.Value = 0;//set initial value to 0

                        VideoInfo video = videoInfos[videoQualityComboBox.SelectedIndex];
                        // Decipher the video if it has a decrypted signature 
                        if (video.RequiresDecryption)
                            DownloadUrlResolver.DecryptDownloadUrl(video);

                        // Initiate a new VideoDownloader object, passing the VideoInfo object and save path 
                        var videoDownloader = new VideoDownloader(video, saveTextBox.Text);


                        // Execute the video downloader 
                        videoDownloader.Execute();

                        progressBar.Increment(100);//set it complete
                                                   // Set cursor as default arrow
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("Download Complete");//pops up a message indicate the download is complete
                        progressBar.Hide();// hides the profress bar
                    }
                    catch
                    {
                        MessageBox.Show("Error occured, Please try again");
                    }
                }
                else {
                    MessageBox.Show("Invalid File Path!");
                }
            }
        }

        private void YouTubeDownloader_Load(object sender, EventArgs e)
        {
            progressBar.Hide();//set the progressbar hide intially
        }
    }
}
