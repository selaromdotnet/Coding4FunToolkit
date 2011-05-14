﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Coding4Fun.Phone.Controls.Helpers;

// This is a heavily modified version based on their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
namespace Coding4Fun.Phone.Controls
{
    public class ColorPicker : ColorBaseControl
    {
        double _sampleSelectorSize = 10;

        private float _hue;

        Point _position;

        #region controls on template
        protected Grid SampleSelector;
        private const string SampleSelectorName = "SampleSelector";

        protected Rectangle SelectedHueColor;
        private const string SelectedHueColorName = "SelectedHueColor";

        protected ColorSlider ColorSlider;
        private const string ColorSliderName = "ColorSlider";

        private MovementMonitor _monitor;
        private const string BodyName = "Body";
        #endregion

        public ColorPicker()
        {
            DefaultStyleKey = typeof (ColorPicker);

            Loaded += ColorPicker_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SampleSelector = GetTemplateChild(SampleSelectorName) as Grid;
            SelectedHueColor = GetTemplateChild(SelectedHueColorName) as Rectangle;

            var body = GetTemplateChild(BodyName) as Grid;

            if (body != null)
            {
                _monitor = new MovementMonitor();
                _monitor.Movement += _monitor_Movement;
                _monitor.MonitorControl(body);
            }

            ColorSlider = GetTemplateChild(ColorSliderName) as ColorSlider;

            if (ColorSlider != null)
            {
                ColorSlider.ColorChanged += ColorSlider_ColorChanged;
            }
        }

        #region events
        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            _sampleSelectorSize = SampleSelector.ActualHeight;

            if (Color.A == 0 && Color.R == 0 && Color.G == 0 && Color.B == 0)
            {
                _position.X = SelectedHueColor.ActualWidth;

                ColorSlider_ColorChanged(this, ColorSlider.Color);
            }
            //UpdateSample(ActualWidth, 0);
        }

        void ColorSlider_ColorChanged(object sender, Color color)
        {
            _hue = ColorSpace.CalculateHue(color);
            SelectedHueColor.Fill = new SolidColorBrush(color);

            UpdateSample();
        }
        #endregion

        void _monitor_Movement(object sender, MovementMonitorEventArgs e)
        {
            _position.X = e.X;
            _position.Y = e.Y;
            
            UpdateSample();
        }

        private void UpdateSample()
        {
            SetSampleLocation();

            var saturation = (float)(_position.X / SelectedHueColor.ActualHeight);
            var value = (float)(1 - (_position.Y / SelectedHueColor.ActualWidth));

            ColorChanging(ColorSpace.ConvertHsvToRgb(_hue, saturation, value));
        }

        private void SetSampleLocation()
        {
            var height = SelectedHueColor.ActualHeight;
            var width = SelectedHueColor.ActualWidth;

            _position.X = ControlHelper.CheckBound(_position.X, width);
            _position.Y = ControlHelper.CheckBound(_position.Y, height);

            var sampleLeft = _position.X - _sampleSelectorSize;
            var sampleTop = _position.Y - _sampleSelectorSize;

            sampleLeft = ControlHelper.CheckBound(sampleLeft, width);
            sampleTop = ControlHelper.CheckBound(sampleTop, height);

            SampleSelector.Margin = new Thickness(sampleLeft, sampleTop, 0, 0);
        }
    }
}
