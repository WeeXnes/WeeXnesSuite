using System;
using System.Windows;
using System.Windows.Controls;

public partial class ExampleUi : Page
{
    private int counter = 0;
    public ExampleUi()
    {
        InitializeComponent();
    }

    private void ExampleUi_OnLoaded(object sender, RoutedEventArgs e)
    {
        counter++;
        TestLabel.Text = counter.ToString();
    }
}