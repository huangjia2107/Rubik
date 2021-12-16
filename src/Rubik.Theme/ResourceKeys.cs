using System.Windows;

namespace Rubik.Theme
{
    public static class ResourceKeys
    {
        #region ButtonBase
        /// <summary>
        /// Style="{DynamicResource/StaticResource {x:Static themes:ResourceKeys.RubikButtonStyleKey}}"
        /// </summary>
        public const string RubikButtonBaseStyle = "RubikButtonBaseStyle";
        public static ComponentResourceKey RubikButtonBaseStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikButtonBaseStyle); }
        }

        public const string RubikRoundButtonBaseStyle = "RubikRoundButtonBaseStyle";
        public static ComponentResourceKey RubikRoundButtonBaseStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikRoundButtonBaseStyle); }
        }

        #endregion

        #region Button

        public const string OpacityButtonStyle = "OpacityButtonStyle";
        public static ComponentResourceKey OpacityButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), OpacityButtonStyle); }
        }

        public const string TitlebarButtonStyle = "TitlebarButtonStyle";
        public static ComponentResourceKey TitlebarButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), TitlebarButtonStyle); }
        }

        public const string TitlebarCloseBtnStyle = "TitlebarCloseBtnStyle";
        public static ComponentResourceKey TitlebarCloseBtnStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), TitlebarCloseBtnStyle); }
        }

        #endregion

        #region RepeatButton

        public const string RubikRepeatButtonStyle = "RubikRepeatButtonStyle";
        public static ComponentResourceKey RubikRepeatButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikRepeatButtonStyle); }
        }

        #endregion

        #region RadioButton

        public const string RubikRadioButtonStyle = "RubikRadioButtonStyle";
        public static ComponentResourceKey RubikRadioButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikRadioButtonStyle); }
        }

        #endregion

        #region ToggleStatus

        public const string RubikToggleStatusStyle = "RubikToggleStatusStyle";
        public static ComponentResourceKey RubikToggleStatusStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikToggleStatusStyle); }
        }

        #endregion

        #region CheckBox

        public const string RubikCheckBoxStyle = "RubikCheckBoxStyle";
        public static ComponentResourceKey RubikCheckBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikCheckBoxStyle); }
        }

        #endregion

        #region ListBox

        public const string RubikListBoxStyle = "RubikListBoxStyle";
        public static ComponentResourceKey RubikListBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikListBoxStyle); }
        }

        public const string RubikListBoxItemStyle = "RubikListBoxItemStyle";
        public static ComponentResourceKey RubikListBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikListBoxItemStyle); }
        }

        #endregion

        #region TextBox

        public const string RubikTextBoxStyle = "RubikTextBoxStyle";
        public static ComponentResourceKey RubikTextBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikTextBoxStyle); }
        }

        #endregion

        #region ComboBox

        public const string RubikComboBoxStyle = "RubikComboBoxStyle";
        public static ComponentResourceKey RubikComboBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikComboBoxStyle); }
        }

        public const string RubikComboBoxItemStyle = "RubikComboBoxItemStyle";
        public static ComponentResourceKey RubikComboBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikComboBoxItemStyle); }
        }

        #endregion

        #region UserWindow

        public const string NoTitleBarWindowStyle = "NoTitleBarWindowStyle";
        public static ComponentResourceKey NoTitleBarWindowStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NoTitleBarWindowStyle); }
        }

        #endregion

        #region ScrollViewer

        public const string RubikScrollBarStyle = "RubikScrollBarStyle";
        public static ComponentResourceKey RubikScrollBarStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikScrollBarStyle); }
        }

        public const string RubikScrollViewerStyle = "RubikScrollViewerStyle";
        public static ComponentResourceKey RubikScrollViewerStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikScrollViewerStyle); }
        }

        #endregion

        #region Slider

        public const string RubikSliderStyle = "RubikSliderStyle";
        public static ComponentResourceKey RubikSliderStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikSliderStyle); }
        }

        public const string ColorPickerSliderStyle = "ColorPickerSliderStyle";
        public static ComponentResourceKey ColorPickerSliderStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ColorPickerSliderStyle); }
        }

        #endregion

        #region GridSplitter

        public const string RubikGridSplitterStyle = "RubikGridSplitterStyle";
        public static ComponentResourceKey RubikGridSplitterStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RubikGridSplitterStyle); }
        }

        #endregion

        #region Brush

        public const string InfomationGeometry = "InfomationGeometry";
        public static ComponentResourceKey InfomationGeometryKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), InfomationGeometry); }
        }

        public const string WarningGeometry = "WarningGeometry";
        public static ComponentResourceKey WarningGeometryKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), WarningGeometry); }
        }

        public const string QuestionGeometry = "QuestionGeometry";
        public static ComponentResourceKey QuestionGeometryKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), QuestionGeometry); }
        }

        public const string ErrorGeometry = "ErrorGeometry";
        public static ComponentResourceKey ErrorGeometryKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ErrorGeometry); }
        }

        public const string AlphaVisualBrush = "AlphaVisualBrush";
        public static ComponentResourceKey AlphaVisualBrushKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), AlphaVisualBrush); }
        }

        public const string LogoDrawingGroup = "LogoDrawingGroup";
        public static ComponentResourceKey LogoDrawingGroupKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), LogoDrawingGroup); }
        }

        #endregion

        #region Separator

        public const string HorSeparatorStyle = "HorSeparatorStyle";
        public static ComponentResourceKey HorSeparatorStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), HorSeparatorStyle); }
        }

        public const string VerSeparatorStyle = "VerSeparatorStyle";
        public static ComponentResourceKey VerSeparatorStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), VerSeparatorStyle); }
        }

        #endregion

        #region Converter

        public const string BoolConverter = "BoolConverter";
        public static ComponentResourceKey BoolConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), BoolConverter); }
        }

        public const string BoolToVisibilityConverter = "BoolToVisibilityConverter";
        public static ComponentResourceKey BoolToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), BoolToVisibilityConverter); }
        }

        public const string ColorToBrushConverter = "ColorToBrushConverter";
        public static ComponentResourceKey ColorToBrushConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ColorToBrushConverter); }
        }

        public const string NullToVisibilityConverter = "NullToVisibilityConverter";
        public static ComponentResourceKey NullToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NullToVisibilityConverter); }
        }

        public const string EnumToVisibilityConverter = "EnumToVisibilityConverter";
        public static ComponentResourceKey EnumToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), EnumToVisibilityConverter); }
        }

        public const string KeyToValueConverter = "KeyToValueConverter";
        public static ComponentResourceKey KeyToValueConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), KeyToValueConverter); }
        }

        public const string VisibilityConverter = "VisibilityConverter";
        public static ComponentResourceKey VisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), VisibilityConverter); }
        }

        public const string TimeSpanToStringConverter = "TimeSpanToStringConverter";
        public static ComponentResourceKey TimeSpanToStringConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), TimeSpanToStringConverter); }
        }

        public const string DoubleToGridLengthConverter = "DoubleToGridLengthConverter";
        public static ComponentResourceKey DoubleToGridLengthConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), DoubleToGridLengthConverter); }
        }

        public const string DoubleToCornerRadiusConverter = "DoubleToCornerRadiusConverter";
        public static ComponentResourceKey DoubleToCornerRadiusConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), DoubleToCornerRadiusConverter); }
        }

        public const string ScrollOffsetToVisibilityMultiConverter = "ScrollOffsetToVisibilityMultiConverter";
        public static ComponentResourceKey ScrollOffsetToVisibilityMultiConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ScrollOffsetToVisibilityMultiConverter); }
        }
        
        #endregion
    }
}
