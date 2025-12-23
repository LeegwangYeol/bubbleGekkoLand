using adviser.Models;
using adviser.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace adviser.component
{
    /// <summary>
    /// Interaction logic for HoneycombControl.xaml
    /// </summary>
    public partial class HoneycombControl : UserControl
    {
        public ObservableCollection<HoneybunButtonModel> Buttons { get; set; }

        // DependencyProperty for ButtonCount to be set from XAML
        public static readonly DependencyProperty ButtonCountProperty =
            DependencyProperty.Register("ButtonCount", typeof(int), typeof(HoneycombControl), new PropertyMetadata(0, OnButtonCountChanged));

        public int ButtonCount
        {
            get { return (int)GetValue(ButtonCountProperty); }
            set { SetValue(ButtonCountProperty, value); }
        }

        public HoneycombControl()
        {
            InitializeComponent();
            Buttons = new ObservableCollection<HoneybunButtonModel>();
            // Set the DataContext of the UserControl to itself
            // so the ItemsSource="{Binding Buttons}" in XAML works.
            this.DataContext = this;
        }

        private static void OnButtonCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HoneycombControl control)
            {
                control.GenerateHoneycombButtons((int)e.NewValue);
            }
        }

        private void GenerateHoneycombButtons(int count)
        {
            Buttons.Clear();
            if (count <= 0) return;

            double buttonSize = 80;
            double gap = 10;
            double centerX = 300; // Center of the canvas
            double centerY = 300;

            // Hexagonal constants
            double horizDist = buttonSize + gap;
            double vertDist = (buttonSize + gap) * Math.Sqrt(3) / 2;

            // (q, r) axial coordinates for hexagonal grid
            int[][] directions = {
                new[] { 1, 0 }, new[] { 0, 1 }, new[] { -1, 1 },
                new[] { -1, 0 }, new[] { 0, -1 }, new[] { 1, -1 }
            };

            int q = 0, r = 0;
            int ring = 0;

            // Add center button
            AddButton(0, q, r, centerX, centerY, horizDist, vertDist);

            for (int i = 1; i < count; )
            {
                ring++;
                // Move to the start of the next ring
                q += directions[4][0];
                r += directions[4][1];

                for (int j = 0; j < 6 && i < count; j++)
                {
                    for (int k = 0; k < ring && i < count; k++)
                    {
                        AddButton(i, q, r, centerX, centerY, horizDist, vertDist);
                        i++;
                        q += directions[j][0];
                        r += directions[j][1];
                    }
                }
            }
        }

        private void AddButton(int index, int q, int r, double centerX, double centerY, double horizDist, double vertDist)
        {
            // Convert axial to pixel coordinates
            double x = centerX + horizDist * (3.0 / 2.0 * q);
            double y = centerY + vertDist * (Math.Sqrt(3) / 2.0 * q + Math.Sqrt(3) * r);
            
            Buttons.Add(new HoneybunButtonModel
            {
                X = x,
                Y = y,
                Content = index.ToString()
            });
        }
    }
}
