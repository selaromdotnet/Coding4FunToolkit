﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

using Coding4Fun.Toolkit.Controls.Primitives;
using Windows.UI.Xaml;

namespace Coding4Fun.Toolkit.Controls
{
    /// <summary>
    /// Represents a page used by the DatespanPicker control that allows the user to choose a duration (hour/minute/second).
    /// </summary>
    public partial class TimeSpanPickerPage : TimeSpanPickerBasePage
    {
        /// <summary>
        /// Initializes a new instance of the TimespanPickerPage control.
        /// </summary>
        public TimeSpanPickerPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hook up the data sources
        /// </summary>
        public override void InitDataSource()
        {
			var stepSeconds = StepFrequency.Seconds;
			var maxSeconds = Maximum >= TimeSpan.FromMinutes(1) ? 60 : Math.Min(Maximum.Seconds + stepSeconds, 60);
            TertiarySelector.DataSource = new SecondTimeSpanDataSource(maxSeconds, stepSeconds);

			var stepMinutes = StepFrequency > TimeSpan.FromMinutes(1) ? StepFrequency.Minutes : 1;
			var maxMinutes = Maximum >= TimeSpan.FromHours(1) ? 60 : Math.Min(Maximum.Minutes + stepMinutes, 60);
            SecondarySelector.DataSource = new MinuteTimeSpanDataSource(maxMinutes, stepMinutes);

			var stepHours = StepFrequency > TimeSpan.FromHours(1) ? StepFrequency.Hours : 1;
			var maxHours = Maximum >= TimeSpan.FromHours(24) ? 24 : Maximum.Hours + stepHours;
            PrimarySelector.DataSource = new HourTimeSpanDataSource(maxHours, stepHours);

            InitializeValuePickerPage(PrimarySelector, SecondarySelector, TertiarySelector);
        }


		/// <summary>
		/// Gets a sequence of LoopingSelector parts ordered according to culture string for date/time formatting.
		/// </summary>
		/// <returns>LoopingSelectors ordered by culture-specific priority.</returns>
		protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern()
		{
			var selectors = GetSelectorsOrderedByCulturePattern(
				CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.ToUpperInvariant(),
				new char[] { 'H', 'M', 'S' },
				new LoopingSelector[] { PrimarySelector, SecondarySelector, TertiarySelector });

			var result = selectors.Where(s => !(s.DataSource.IsEmpty));
			return result;
		}

		/// <summary>
		/// Handles changes to the page's Orientation property.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		//protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		//{
		//    if (null == e)
		//    {
		//        throw new ArgumentNullException("e");
		//    }

		//    base.OnOrientationChanged(e);
		//    SystemTrayPlaceholder.Visibility = (0 != (PageOrientation.Portrait & e.Orientation)) ?
		//        Visibility.Visible :
		//        Visibility.Collapsed;
		//}

		/// <summary>
		/// Sets the selectors and title flow direction.
		/// </summary>
		/// <param name="flowDirection">Flow direction to set.</param>
		public override void SetFlowDirection(FlowDirection flowDirection)
		{
			HeaderTitle.FlowDirection = flowDirection;

			PrimarySelector.FlowDirection = flowDirection;
			SecondarySelector.FlowDirection = flowDirection;
			TertiarySelector.FlowDirection = flowDirection;
		}
	}
}
