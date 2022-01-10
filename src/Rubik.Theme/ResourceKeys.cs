using System;
using System.Windows;

namespace Rubik.Theme
{
    public static class ResourceKeys
    {
        public static Uri RubikThemeSource { get; } = new Uri("pack://application:,,,/Rubik.Theme;component/Themes/Generic.xaml");

        #region ButtonBase

        /// <summary>
        /// Style="{DynamicResource/StaticResource {x:Static themes:ResourceKeys.RubikButtonStyleKey}}"
        /// </summary>
        public static ComponentResourceKey RubikButtonBaseStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikButtonBaseStyle");

        public static ComponentResourceKey RubikRoundButtonBaseStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikRoundButtonBaseStyle");

        #endregion

        #region Button

        public static ComponentResourceKey OpacityButtonStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "OpacityButtonStyle");

        public static ComponentResourceKey TitlebarButtonStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "TitlebarButtonStyle");

        public static ComponentResourceKey TitlebarCloseBtnStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "TitlebarCloseBtnStyle");

        #endregion

        #region RepeatButton

        public static ComponentResourceKey RubikRepeatButtonStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikRepeatButtonStyle");

        #endregion

        #region RadioButton

        public static ComponentResourceKey RubikRadioButtonStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikRadioButtonStyle");

        #endregion

        #region ToggleStatus

        public static ComponentResourceKey RubikToggleStatusStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikToggleStatusStyle");

        #endregion

        #region CheckBox

        public static ComponentResourceKey RubikCheckBoxStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikCheckBoxStyle");

        #endregion

        #region ListBox

        public static ComponentResourceKey RubikListBoxStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikListBoxStyle");

        public static ComponentResourceKey RubikListBoxItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikListBoxItemStyle");

        #endregion

        #region TextBox

        public static ComponentResourceKey RubikTextBoxStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikTextBoxStyle");

        #endregion

        #region ComboBox

        public static ComponentResourceKey RubikComboBoxStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikComboBoxStyle");

        public static ComponentResourceKey RubikComboBoxItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikComboBoxItemStyle");

        #endregion

        #region UserWindow

        public static ComponentResourceKey NoTitleBarWindowStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "NoTitleBarWindowStyle");

        #endregion

        #region ScrollViewer

        public static ComponentResourceKey RubikScrollBarStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikScrollBarStyle");

        public static ComponentResourceKey RubikScrollViewerStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikScrollViewerStyle");

        #endregion

        #region Slider

        public static ComponentResourceKey RubikSliderStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikSliderStyle");

        public static ComponentResourceKey ColorPickerSliderStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "ColorPickerSliderStyle");

        #endregion

        #region GridSplitter

        public static ComponentResourceKey RubikGridSplitterStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikGridSplitterStyle");

        #endregion

        #region Brush

        public static ComponentResourceKey InfomationGeometryKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "InfomationGeometry");

        public static ComponentResourceKey WarningGeometryKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "WarningGeometry");

        public static ComponentResourceKey QuestionGeometryKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "QuestionGeometry");

        public static ComponentResourceKey ErrorGeometryKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "ErrorGeometry");

        public static ComponentResourceKey AlphaVisualBrushKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "AlphaVisualBrush");

        public static ComponentResourceKey LogoDrawingGroupKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "LogoDrawingGroup");

        #endregion

        #region Separator

        public static ComponentResourceKey HorSeparatorStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "HorSeparatorStyle");

        public static ComponentResourceKey VerSeparatorStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "VerSeparatorStyle");

        #endregion

        #region Converter

        public static ComponentResourceKey BoolConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "BoolConverter");

        public static ComponentResourceKey BoolToVisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "BoolToVisibilityConverter");

        public static ComponentResourceKey ColorToBrushConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "ColorToBrushConverter");

        public static ComponentResourceKey NullToVisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "NullToVisibilityConverter");

        public static ComponentResourceKey EnumToVisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "EnumToVisibilityConverter");

        public static ComponentResourceKey KeyToValueConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "KeyToValueConverter");

        public static ComponentResourceKey VisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "VisibilityConverter");

        public static ComponentResourceKey TimeSpanToStringConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "TimeSpanToStringConverter");

        public static ComponentResourceKey DoubleToGridLengthConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "DoubleToGridLengthConverter");

        public static ComponentResourceKey DoubleToCornerRadiusConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "DoubleToCornerRadiusConverter");

        public static ComponentResourceKey ScrollOffsetToVisibilityMultiConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "ScrollOffsetToVisibilityMultiConverter");

        #endregion
    }
}
