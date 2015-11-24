using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Semiodesk.Trinity;
using Artivity.DataModel;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer.Dialogs.ExportDialog
{
    public class ExportDialog : Dialog
    {
        TextBox _directoryBox;

        Button _directoryButton;

        TextBox _filenameBox;

        TabControl _formatControl;

        ComboBox _rdfFormatBox;

        GridView _rdfModelsView;

        Label _progressLabel;

        ProgressBar _progressBar;

        BackgroundWorker _exportWorker;

        Button _exportButton;

        Button _cancelButton;

        public ExportDialog()
        {
            Title = "Export";
            Width = 500;
            Height = 550;

            Label directoryLabel = new Label();
            directoryLabel.Text = "Directory";
            directoryLabel.Font = SystemFonts.Bold();

            _directoryBox = new TextBox();
            _directoryBox.Text = Setup.GetUserHomeFolder();

            _directoryButton = new Button();
            _directoryButton.Width = 40;
            _directoryButton.Click += OnDirectoryButtonClicked;

            StackLayout directoryLayout = new StackLayout();
            directoryLayout.Spacing = 7;
            directoryLayout.Orientation = Orientation.Horizontal;
            directoryLayout.Items.Add(new StackLayoutItem(_directoryBox, true));
            directoryLayout.Items.Add(new StackLayoutItem(_directoryButton));

            Label filenameLabel = new Label();
            filenameLabel.Text = "File";
            filenameLabel.Font = SystemFonts.Bold();

            _filenameBox = new TextBox();
            _filenameBox.Text = "export.rdf";

            Label formatLabel = new Label();
            formatLabel.Text = "Format";
            formatLabel.Font = SystemFonts.Bold();

            _rdfFormatBox = new ComboBox();
            _rdfFormatBox.Items.Add("RDF / XML");
            _rdfFormatBox.Items.Add("Turtle");
            _rdfFormatBox.SelectedIndex = 0;

            List<ModelExportOption> options = new List<ModelExportOption>()
            {
                new ModelExportOption(true, Models.Agents, "Agents", "The agents involved in the creation process."),
                new ModelExportOption(true, Models.Activities, "Activities", "File editing steps and movements."),
                new ModelExportOption(true, Models.WebActivities, "Web Activities", "Web browsing activities."),
            };

            _rdfModelsView = new GridView();
            _rdfModelsView.DataStore = options;
            _rdfModelsView.Columns.Add(new GridColumn()
            {
                DataCell = new CheckBoxCell { Binding = Binding.Property<ModelExportOption, bool?>(a => a.IsExportEnabled) },
                Editable = true,
                HeaderText = ""
            });
            _rdfModelsView.Columns.Add(new GridColumn()
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<ModelExportOption, string>(a => a.Name) },
                HeaderText = "Model"
            });
            _rdfModelsView.Columns.Add(new GridColumn()
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<ModelExportOption, string>(a => a.Description) },
                HeaderText = "Description"
            });

            StackLayout rdfLayout = new StackLayout();
            rdfLayout.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            rdfLayout.Orientation = Orientation.Vertical;
            rdfLayout.Padding = new Padding(10);
            rdfLayout.Spacing = 10;
            rdfLayout.Items.Add(new Label() { Text = "Choose this format if you want to import Artivity data into a graph database compatible with the Resource Description Format." });
            rdfLayout.Items.Add(new StackLayoutItem(_rdfModelsView, true));
            rdfLayout.Items.Add(new StackLayoutItem(_rdfFormatBox, HorizontalAlignment.Right));

            StackLayout csvLayout = new StackLayout();
            csvLayout.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            csvLayout.Orientation = Orientation.Vertical;
            csvLayout.Padding = new Padding(10);
            csvLayout.Spacing = 10;
            csvLayout.Items.Add(new Label() { Text = "Choose this format if you want to import Artivity data into statistics applications and spreadsheet editors." });

            _formatControl = new TabControl();
            _formatControl.Pages.Add(new TabPage(rdfLayout) { Text = " RDF " });
            _formatControl.Pages.Add(new TabPage(csvLayout) { Text = " CSV " });
            _formatControl.SelectedIndexChanged += OnFormatControlSelectedIndexChanged;

            _progressLabel = new Label();
            _progressLabel.Text = "";

            _progressBar = new ProgressBar();
            _progressBar.MaxValue = 1;

            _exportButton = new Button() { Text = "Export" };
            _exportButton.Click += OnExportButtonClicked;

            _cancelButton = new Button() { Text = "Cancel" };
            _cancelButton.Click += OnCancelButtonClicked;

            StackLayout buttonLayout = new StackLayout();
            buttonLayout.Orientation = Orientation.Horizontal;
            buttonLayout.Spacing = 7;
            buttonLayout.Items.Add(_cancelButton);
            buttonLayout.Items.Add(_exportButton);

            StackLayout layout = new StackLayout();
            layout.Padding = new Padding(7, 0);
            layout.Spacing = 7;
            layout.Items.Add(new StackLayoutItem(directoryLabel, HorizontalAlignment.Left));
            layout.Items.Add(new StackLayoutItem(directoryLayout, HorizontalAlignment.Stretch));
            layout.Items.Add(new StackLayoutItem(filenameLabel, HorizontalAlignment.Left));
            layout.Items.Add(new StackLayoutItem(_filenameBox, HorizontalAlignment.Stretch));
            layout.Items.Add(new StackLayoutItem(formatLabel, HorizontalAlignment.Left));
            layout.Items.Add(new StackLayoutItem(_formatControl, HorizontalAlignment.Stretch, true));
            layout.Items.Add(new StackLayoutItem(_progressLabel, HorizontalAlignment.Center));
            layout.Items.Add(new StackLayoutItem(_progressBar, HorizontalAlignment.Stretch));
            layout.Items.Add(new StackLayoutItem(buttonLayout, HorizontalAlignment.Right));

            Content = layout;
        }

        private void OnFormatControlSelectedIndexChanged(object sender, EventArgs e)
        {
            string extension = Path.GetExtension(_filenameBox.Text);

            if (_formatControl.SelectedIndex == 0)
            {
                _filenameBox.Text = _filenameBox.Text.Replace(extension, ".rdf");
            }
            else
            {
                _filenameBox.Text = _filenameBox.Text.Replace(extension, ".csv");
            }
        }

        private void OnDirectoryButtonClicked(object sender, EventArgs e)
        {
            SelectFolderDialog dialog = new SelectFolderDialog();
            dialog.Directory = _directoryBox.Text;

            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                _directoryBox.Text = dialog.Directory;
            }
        }

        private void OnExportButtonClicked(object sender, EventArgs e)
        {
            if (_exportWorker == null)
            {
                string filename = Path.Combine(_directoryBox.Text, _filenameBox.Text);

                if (_formatControl.SelectedIndex == 0)
                {
                    _exportWorker = GetWriteRdfWorker(filename);
                }
                else
                {
                    _exportWorker = GetWriteCsvWorker(filename);
                }

                _exportWorker.RunWorkerCompleted += OnExportWorkerCompleted;
                _exportWorker.RunWorkerAsync();

                _exportButton.Enabled = false;
                _exportButton.Text = "OK";

                _directoryBox.Enabled = false;
                _directoryButton.Enabled = false;
                _filenameBox.Enabled = false;
                _formatControl.Enabled = false;

                _progressLabel.Text = "Exporting..";
                _progressBar.Indeterminate = true;
            }
            else
            {
                Close();
            }
        }

        private void OnExportWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                _progressLabel.Text = "Done.";
                _progressBar.Indeterminate = false;
                _progressBar.Value = 1;

                _exportButton.Enabled = true;
            }
            else
            {
                _progressLabel.Text = "Error.";
                _progressBar.Indeterminate = false;
                _progressBar.Value = 0;
            }
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Close();
        }

        private BackgroundWorker GetWriteRdfWorker(string filename)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    try
                    {
                        IModel model = Models.GetActivities();
                        model.Write(stream, RdfSerializationFormat.Turtle);

                        e.Result = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                        e.Result = false;
                    }
                }
            };

            return worker;
        }

        private BackgroundWorker GetWriteCsvWorker(string filename)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                using (TextWriter writer = File.CreateText(filename))
                {
                    try
                    {
                        string queryString = @"
                            PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                            PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                            PREFIX prov: <http://www.w3.org/ns/prov#>
                            PREFIX dces: <http://purl.org/dc/elements/1.1/>
                            PREFIX art: <http://semiodesk.com/artivity/1.0/>

                            SELECT ?activity ?agent ?influenceTime ?influenceType ?entity ?entityType ?description ?value ?bounds WHERE 
                            {
                                ?activity prov:qualifiedAssociation ?association .

                                ?association prov:agent ?agent .

                                {
                                    ?activity prov:used ?file ;
                                        prov:generated ?entity .

                                    ?entity a ?entityType ;
                                        prov:qualifiedGeneration ?generation .

                                    ?generation a ?influenceType ;
                                        prov:atTime ?influenceTime .

                                    OPTIONAL { ?generation art:hadBoundaries ?bounds . }
                                    OPTIONAL { ?generation dces:description ?description . }
                                    OPTIONAL { ?generation prov:value ?value . }
                                }
                                UNION
                                {
                                    ?editing prov:used ?file;
                                                prov:startedAtTime ?startTime ;
                                                prov:endedAtTime ?endTime .

                                    ?activity prov:startedAtTime ?time ;
                                        prov:qualifiedUsage ?usage .

                                    ?usage a ?influenceType ;
                                        prov:entity ?entity ;
                                        prov:atTime ?influenceTime .

                                    ?entity a ?entityType .

                                    FILTER(?startTime <= ?time && ?time <= ?endTime) .
                                }
                            }
                            ORDER BY DESC(?influenceTime)";
                
                        IModel model = Models.GetAllActivities();

                        SparqlQuery query = new SparqlQuery(queryString);
                        ISparqlQueryResult result = model.ExecuteQuery(query);

                        foreach(BindingSet binding in result.GetBindings())
                        {
                            writer.WriteLine(String.Join(",", binding.Values));
                        }

                        e.Result = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                        e.Result = false;
                    }
                }
            };

            return worker;
        }
    }

    internal class ModelExportOption
    {
        #region Members

        public bool? IsExportEnabled { get; set; }

        public Uri Uri { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        #endregion

        #region Constructors

        public ModelExportOption(bool? enabled, Uri uri, string name, string description)
        {
            IsExportEnabled = enabled;
            Uri = uri;
            Name = name;
            Description = description;
        }

        #endregion
    }
}

