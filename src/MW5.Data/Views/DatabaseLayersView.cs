﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MW5.Api.Concrete;
using MW5.Api.Enums;
using MW5.Data.Model;
using MW5.Data.Properties;
using MW5.Data.Views.Abstract;
using MW5.Shared;
using MW5.UI.Forms;
using MW5.UI.Helpers;

namespace MW5.Data.Views
{
    public partial class DatabaseLayersView : DatabaseLayersViewBase, IDatabaseLayersView
    {
        private List<VectorLayerGridAdapter> _layers;

        public DatabaseLayersView()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            var layers = GetLayers().ToList();
            
            databaseLayersGrid1.DataSource = layers;
            _layers = layers;

            var style = databaseLayersGrid1.Adapter.GetColumnStyle(r => r.Name);
            style.ImageList = imageList1;
            style.ImageIndex = 0;
            databaseLayersGrid1.Adapter.SetColumnIcon(r => r, GetIcon);

            databaseLayersGrid1.AdjustColumnWidths();
            databaseLayersGrid1.Adapter.HotTracking = true;

            Text = @"Database Layers: " + Model.Connection.Name;
        }

        private int GetIcon(VectorLayerGridAdapter info)
        {
            switch (info.GeometryType)
            {
                case GeometryType.Point:
                case GeometryType.MultiPoint:
                    return 0;
                case GeometryType.Polyline:
                    return 1;
                case GeometryType.Polygon:
                    return 2;
            }

            return -1;
        }

        private IEnumerable<VectorLayerGridAdapter> GetLayers()
        {
            foreach (var layer in Model.Datasource.Where(l => l.GeometryType != GeometryType.None))
            {
                yield return new VectorLayerGridAdapter(layer);
            }
        }

        public ButtonBase OkButton
        {
            get { return btnOk; }
        }

        public IEnumerable<VectorLayerGridAdapter> Layers
        {
            get { return _layers; }
        }

        private void OnSelectAllChecked(object sender, EventArgs e)
        {
            databaseLayersGrid1.Adapter.SetPropertyForEach(item => item.Selected, chkSelectAll.Checked);
        }
    }

    public class DatabaseLayersViewBase: MapWindowView<DatabaseLayersModel> {}
}
