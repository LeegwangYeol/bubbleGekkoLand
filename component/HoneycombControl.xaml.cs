using adviser.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace adviser.component
{
    public partial class HoneycombControl : UserControl
    {
        public ObservableCollection<HoneybunButtonModel> Buttons { get; set; }

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
            this.DataContext = this;
        }
        
        private void HoneycombControl_Loaded(object sender, RoutedEventArgs e)
        {
            // The ItemsControl's panel is generated from a template, so we need to find it in the visual tree.
            var itemsPresenter = FindVisualChild<ItemsPresenter>(this);
            if (itemsPresenter == null) return;
            
            // Applying the template is what creates the panel.
            itemsPresenter.ApplyTemplate();
            var canvas = VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;

            if (canvas != null)
            {
                canvas.MouseMove += Canvas_MouseMove;
                canvas.MouseLeave += Canvas_MouseLeave;
            }
        }
        
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var canvas = sender as Panel;
            if (canvas == null) return;

            Point mousePos = e.GetPosition(canvas);
            double maxEffectRadius = 150.0;
            double maxScale = 1.5;

            foreach (var button in Buttons)
            {
                double buttonCenterX = button.X + 40; // Width/2
                double buttonCenterY = button.Y + 40; // Height/2

                double dx = mousePos.X - buttonCenterX;
                double dy = mousePos.Y - buttonCenterY;
                double distance = Math.Sqrt(dx * dx + dy * dy);

                if (distance < maxEffectRadius)
                {
                    // Use a cosine falloff for a smooth, bubble-like effect
                    double scaleFactor = (Math.Cos(distance / maxEffectRadius * Math.PI) + 1) / 2.0;
                    button.Scale = 1.0 + (maxScale - 1.0) * scaleFactor;
                }
                else
                {
                    button.Scale = 1.0;
                }
            }
        }

        private void Canvas_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            foreach (var button in Buttons)
            {
                button.Scale = 1.0;
            }
        }
        
        // ... (rest of the generation code remains the same)

        private static void OnButtonCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HoneycombControl control)
            {
                control.GenerateHoneycombButtons((int)e.NewValue);
            }
        }

        private void GenerateHoneycombButtons(int count)
        {
            if (Buttons == null)
            {
                Buttons = new ObservableCollection<HoneybunButtonModel>();
            }
            Buttons.Clear();
            if (count <= 0) return;

            double buttonSize = 80;
            double gap = -20;
            double centerX = 300;
            double centerY = 300;
            double horizDist = buttonSize + gap;
            double vertDist = (buttonSize + gap) * Math.Sqrt(3) / 2;

            int[][] directions = {
                new[] { 1, 0 }, new[] { 0, 1 }, new[] { -1, 1 },
                new[] { -1, 0 }, new[] { 0, -1 }, new[] { 1, -1 }
            };

            int q = 0, r = 0;
            int ring = 0;

            AddButton(0, q, r, centerX, centerY, horizDist, vertDist);

            for (int i = 1; i < count; )
            {
                ring++;
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
            double x = centerX + horizDist * (3.0 / 2.0 * q);
            double y = centerY + vertDist * (Math.Sqrt(3) / 2.0 * q + Math.Sqrt(3) * r);
            
            Buttons.Add(new HoneybunButtonModel
            {
                Q = q,
                R = r,
                X = x,
                Y = y,
                Content = index.ToString()
            });
        }
        
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
