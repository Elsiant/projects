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
            layerComboBox.Items.Add("배경");
            layerComboBox.Items.Add("건물");
            layerComboBox.Items.Add("오브젝트");
            layerComboBox.SelectedIndex = 0;
            layerComboBox.SelectedIndexChanged += LayerComboBox_SelectedIndexChanged;

            _mapDrawingPanel = new MapDrawingPanel();
            _mapDrawingPanel.DrawAtPoint += LeftPanelClicked;
            _mapDrawingPanel.Dock = DockStyle.Fill;

            splitContainer.Panel1.Controls.Add(_mapDrawingPanel);

            _imageSelectorPanel = new ImageSelector();
            _imageSelectorPanel.ImageSelected += RightPanelClicked;
            _imageSelectorPanel.Dock = DockStyle.Fill;

            splitContainer.Panel2.Controls.Add(_imageSelectorPanel);
        }

        private void LayerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            _mapDrawingPanel.ChangeLayer(comboBox.SelectedIndex);
        }

        private void spriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var spriteEditor = new SpriteEditor();
            spriteEditor.Show();
        }

        private void LeftPanelClicked(object sender, Point e)
        {
            if (string.IsNullOrEmpty(_currentImagePath))
            {
                return;
            }

            var drawingPanel = sender as MapDrawingPanel;
            drawingPanel.SetImage(e, _currentImagePath);
        }

        private void RightPanelClicked(object sender, string e)
        {
            _currentImagePath = e;
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
                dialog.Filter = "json 파일 (*.json)|*.json|모든 파일 (*.*)|*.*";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _fileName = dialog.FileName;
            }

            var tiles = _mapDrawingPanel.GetTilesData();
            List<string>[] saveData = new List<string>[tiles.Length];

            for(int i = 0; i < tiles.Length; i++)
            {
                saveData[i] = new List<string>();
                foreach (var tile in tiles[i])
                {
                    saveData[i].Add(tile.Value.GetData());
                }
            }

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(_fileName, json);
        }

        private void LoadData()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "json 파일 (*.json)|*.json|모든 파일 (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _fileName = dialog.FileName;

            string json = File.ReadAllText(_fileName);
            List<string>[] tiles = JsonConvert.DeserializeObject<List<string>[]>(json);
            _mapDrawingPanel.SetTilesData(tiles);
        }
    }
}

