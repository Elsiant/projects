using System;
using System.Drawing;
using System.Windows.Forms;

using MapEditor.Forms;
using MapEditor.Forms.Panels;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using MapEditor.Types;

namespace MapEditor
{
    public partial class MainForm : Form
    {
        private string _fileName = string.Empty;
        private string _currentImagePath = string.Empty;
        private MapDrawingPanel _mapDrawingPanel;
        private ImageSelector _imageSelectorPanel;

        public MainForm()
        {
            InitializeComponent();

            InitializeForm();
        }

        private void InitializeForm()
        {
            _mapDrawingPanel = new MapDrawingPanel();
            _mapDrawingPanel.MouseLeftClicked += LeftPanelClicked;
            _mapDrawingPanel.Dock = DockStyle.Fill;

            splitContainer.Panel1.Controls.Add(_mapDrawingPanel);

            _imageSelectorPanel = new ImageSelector();
            //_imageSelectorPanel.ImageSelected += RigthPanelClicked;
            _imageSelectorPanel.Dock = DockStyle.Fill;

            splitContainer.Panel2.Controls.Add(_imageSelectorPanel);
        }

        private void spriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var spriteEditor = new SpriteEditor();
            spriteEditor.Show();
        }

        private void LeftPanelClicked(object sender, Point e)
        {
            var drawingPanel = sender as MapDrawingPanel;
            drawingPanel.SetImage(e, _currentImagePath);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData(false);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData(true);
        }

        private void SaveData(bool newName)
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

            var tiles = _mapDrawingPanel.GetTilesData();
            string json = JsonConvert.SerializeObject(tiles, Formatting.Indented);
            File.WriteAllText(_fileName, json);
        }

        private void LoadData()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "png 파일 (*.png)|*.png|모든 파일 (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _fileName = dialog.FileName;

            string json = File.ReadAllText(_fileName);
            var tiles = JsonConvert.DeserializeObject<Dictionary<string, Tile>[]>(json);
            _mapDrawingPanel.SetTilesData(tiles);
        }
    }
}
