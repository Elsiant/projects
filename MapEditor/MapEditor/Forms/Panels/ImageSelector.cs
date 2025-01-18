using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MapEditor.Forms.Panels
{
    class ImageSelector : Panel
    {
        public event EventHandler<string> ImageSelected;

        private string _filePath;
        private FlowLayoutPanel _flowLayoutPanel;

        public ImageSelector()
        {
            InitializePanel();
            LoadThumnails(@"Tiles");
        }

        public void LoadThumnails(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                return;
            }
            
            var files = Directory.GetFiles(folderPath, "*.png");
            _flowLayoutPanel.Controls.Clear();

            foreach (var file in files)
            {
                var thumbnail = CreateThumbnail(file);
                if (thumbnail != null)
                {
                    var pictureBox = new PictureBox
                    {
                        Image = thumbnail,
                        Size = new Size(100, 100),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = file // Tag에 파일 경로 저장
                    };

                    pictureBox.Click += PictureBox_Click;
                    _flowLayoutPanel.Controls.Add(pictureBox);
                }
            }
        }

        private void InitializePanel()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            
            _flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            this.Controls.Add(_flowLayoutPanel);
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox && 
                pictureBox.Tag is string filePath)
            {
                _filePath = filePath;
                ImageSelected.Invoke(this, _filePath);
            }
        }

        private Image CreateThumbnail(string filePath)
        {
            try
            {
                var originalImage = Image.FromFile(filePath);
                return originalImage.GetThumbnailImage(100, 100, null, IntPtr.Zero); // 100x100 썸네일 생성
            }
            catch
            {
                // 썸네일 생성 실패 시 null 반환
                return null;
            }
        }
    }
}
