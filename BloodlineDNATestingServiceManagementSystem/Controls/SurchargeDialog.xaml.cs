using System.Windows;
using DAL.Entities;

namespace BloodlineDNATestingServiceManagementSystem.Controls;

public partial class SurchargeDialog : Window
{
    public SurchargePrice Surcharge { get; private set; }

    public SurchargeDialog(SurchargePrice? surcharge = null)
    {
        InitializeComponent();
        if (surcharge != null)
        {
            Surcharge = new SurchargePrice
            {
                SurchargeId = surcharge.SurchargeId,
                SampleType = surcharge.SampleType,
                Surcharge = surcharge.Surcharge,
                Note = surcharge.Note
            };
            txtSampleType.Text = Surcharge.SampleType;
            txtSurcharge.Text = Surcharge.Surcharge?.ToString() ?? "";
            txtNote.Text = Surcharge.Note;
        }
        else
        {
            Surcharge = new SurchargePrice();
        }
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        Surcharge.SampleType = txtSampleType.Text.Trim();
        if (decimal.TryParse(txtSurcharge.Text, out var surchargeValue)) Surcharge.Surcharge = surchargeValue;
        Surcharge.Note = txtNote.Text.Trim();
        DialogResult = true;
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
} 