using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Semiodesk.Trinity;
using Artivity.DataModel;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer
{
    public class AgentSettingsControl : GridView
    {
        #region Members

        private ObservableCollection<SoftwareAgent> _agents;

        #endregion

        #region Constructors

        public AgentSettingsControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            _agents = new ObservableCollection<SoftwareAgent>(Models.GetAgents().GetResources<SoftwareAgent>());

            DataStore = _agents;

            Columns.Add(new GridColumn {
                DataCell = new CheckBoxCell { Binding = Binding.Property<SoftwareAgent, bool?>(a => a.IsCaptureEnabled) },
                Editable = true,
                HeaderText = ""
            });

            Columns.Add(new GridColumn {
                DataCell = new ColourPickerCell { Binding = Binding.Property<SoftwareAgent, string>(a => a.ColourCode) },
                Editable = true,
                HeaderText = "",
                Resizable = false,
                Width = 30
            });

            Columns.Add(new GridColumn {
                DataCell = new TextBoxCell { Binding = Binding.Property<SoftwareAgent, string>(a => a.Name) },
                Editable = true,
                HeaderText = "Name",
                Width = 200
            });

            Columns.Add(new GridColumn {
                DataCell = new TextBoxCell { Binding = Binding.Property<SoftwareAgent, string>(a => a.ExecutableName) },
                Editable = true,
                HeaderText = "Executable",
                Width = 150
            });
        }

        protected override void OnCellClick(GridViewCellEventArgs e)
        {
            base.OnCellClick(e);

            if (e.GridColumn.DataCell is ColourPickerCell)
            {
                ColourPickerCell cell = e.GridColumn.DataCell as ColourPickerCell;

                Color color = Color.Parse(cell.Binding.GetValue(e.Item));

                ColorPicker picker = new ColorPicker();
                picker.Value = color;

                Dialog dialog = new Dialog();
                dialog.Content = picker;
                dialog.ShowModal();

                cell.Binding.SetValue(e.Item, picker.Value.ToHex(false));
            }
        }

        public void Save()
        {
            Regex expression = new Regex("^#([A-Fa-f0-9]{6})$");

            foreach (SoftwareAgent agent in _agents)
            {
                if (!expression.IsMatch(agent.ColourCode))
                {
                    continue;
                }
                    
                agent.Commit();
            }
        }

        #endregion
    }
}

