using System;
using System.Linq;
using System.Collections.Generic;
using Eto;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Explorer.Controls;

namespace Artivity.Explorer
{
    public class WizardPage
    {
        #region Members

        protected Wizard Wizard { get; private set; }

        public string Title { get; set; }

        public Control Content { get; protected set; }

        public Button DefaultButton
        {
            get { return Wizard.DefaultButton; }
            set { Wizard.DefaultButton = value; }
        }

        public Button AbortButton
        {
            get { return Wizard.AbortButton; }
        }

        public Button OkButton
        {
            get { return Wizard.OkButton; }
        }

        public Button BackButton
        {
            get { return Wizard.BackButton; }
        }

        public Button NextButton
        {
            get { return Wizard.NextButton; }
        }

        public readonly IList<Button> Buttons = new List<Button>();

        #endregion

        #region Constructors

        public WizardPage(Wizard wizard)
        {
            Wizard = wizard;
            Wizard.AbortButton.Click += OnAbortButtonClicked;
            Wizard.OkButton.Click += OnOkButtonClicked;
            Wizard.NextButton.Click += OnNextButtonClicked;
            Wizard.BackButton.Click += OnBackButtonClicked;

            InitializeComponent();
        }

        #endregion

        #region Methods

        protected virtual void InitializeComponent()
        {
            AbortButton.Enabled = false;
            OkButton.Enabled = false;
            BackButton.Enabled = false;
            NextButton.Enabled = false;
        }

        protected virtual void Refresh()
        {
        }

        protected virtual void OnOkButtonClicked(object sender, EventArgs e)
        {
            Wizard.Close(DialogResult.Ok);
        }

        protected virtual void OnAbortButtonClicked(object sender, EventArgs e)
        {
            Wizard.Close(DialogResult.Abort);
        }

        protected virtual void OnNextButtonClicked(object sender, EventArgs e)
        {
        }

        protected virtual void OnBackButtonClicked(object sender, EventArgs e)
        {
        }

        #endregion
    }
}

