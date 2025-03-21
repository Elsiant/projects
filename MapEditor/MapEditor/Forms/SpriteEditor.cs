﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using MapEditor.Forms.Panels;
using Newtonsoft.Json;

namespace MapEditor.Forms
{
    public partial class SpriteEditor : Form
    {
        private string _fileName;
        private Color _currentColor = Color.Transparent;
        private DotDrawingPanel _drawingPanel;

        public SpriteEditor()
        {
            InitializeComponent();

            InitializeForm();
        }

        private void InitializeForm()
        {
            _fileName = "";

            // 왼쪽 패널 그리기 화면
            _drawingPanel = new DotDrawingPanel();
            _drawingPanel.MouseLeftClicked += LeftPanel_Clicked;
            splitContainer.Panel1.Controls.Add(_drawingPanel);

            // 오른쪽 패널 색상 선택
            var rightPanel = new ColorPickerPanel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.ColorSelected += RightPanel_ColorSelected;
            splitContainer.Panel2.Controls.Add(rightPanel);
        }

        private void LeftPanel_Clicked(object sender, Point e)
        {
            var drawingPanel = sender as DotDrawingPanel;
            drawingPanel.SetDotColor(e, _currentColor);
        }

        private void RightPanel_ColorSelected(object sender, Color e)
        {
            _currentColor = e;
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            fileSave(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            fileSave(true);
        }

        private void loadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            fileLoad();
        }

        private void fileSave(bool newName)
        {
            if (newName || _fileName == string.Empty)
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "png 파일 (*.png)|*.png|모든 파일 (*.*)|*.*";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _fileName = dialog.FileName;
            }

            // png 파일로 저장
            var bitmap = _drawingPanel.GetBitmap();

            bitmap.Save(_fileName, ImageFormat.Png);
        }

        private void fileLoad()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "png 파일 (*.png)|*.png|모든 파일 (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _fileName = dialog.FileName;

            var bitmap = new Bitmap(_fileName);
            _drawingPanel.SetBitmap(bitmap);
        }
    }
}
