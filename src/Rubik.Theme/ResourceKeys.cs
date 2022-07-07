using System;
using System.Windows;

namespace Rubik.Theme
{
    public static class ResourceKeys
    {
        public static string LeftRightSplitterCur { get; } = @"/Rubik.Toolkit;component/Assets/Cursors/Splitter_lr.cur";
        public static string UpDownSplitterCur { get; } = @"/Rubik.Toolkit;component/Assets/Cursors/Splitter_ud.cur";

        public static string PackUri { get; } = "pack://application:,,,/Rubik.Theme;component/";

        #region Xaml

        public static Uri RubikThemeSource { get; } = new Uri($"{PackUri}Themes/Generic.xaml");

        public static Uri ButtonBaseStyleSource { get; } = new Uri($"{PackUri}Styles/ButtonBaseStyle.xaml");
        public static Uri ButtonStyleSource { get; } = new Uri($"{PackUri}Styles/ButtonStyle.xaml");

        public static Uri ConstantsSource { get; } = new Uri($"{PackUri}Styles/Constants.xaml");
        public static Uri ConvertersSource { get; } = new Uri($"{PackUri}Styles/Converters.xaml");
        public static Uri CheckBoxStyleSource { get; } = new Uri($"{PackUri}Styles/CheckBoxStyle.xaml");
        public static Uri CheckCodeStyleSource { get; } = new Uri($"{PackUri}Styles/CheckCodeStyle.xaml");
        public static Uri ComboBoxStyleSource { get; } = new Uri($"{PackUri}Styles/ComboBoxStyle.xaml");
        public static Uri ColorPickerStyleSource { get; } = new Uri($"{PackUri}Styles/ColorPickerStyle.xaml");

        public static Uri DateTimePickerStyleSource { get; } = new Uri($"{PackUri}Styles/DateTimePickerStyle.xaml");

        public static Uri GeometriesSource { get; } = new Uri($"{PackUri}Styles/Geometries.xaml");
        public static Uri GridSplitterStyleSource { get; } = new Uri($"{PackUri}Styles/GridSplitterStyle.xaml");

        public static Uri ImageChartStyleSource { get; } = new Uri($"{PackUri}Styles/ImageChartStyle.xaml");

        public static Uri LoadIndicatorStyleSource { get; } = new Uri($"{PackUri}Styles/LoadIndicatorStyle.xaml");
        public static Uri LiveChartStyleSource { get; } = new Uri($"{PackUri}Styles/LiveChartStyle.xaml");
        public static Uri ListBoxStyleSource { get; } = new Uri($"{PackUri}Styles/ListBoxStyle.xaml");

        public static Uri MenuStyleSource { get; } = new Uri($"{PackUri}Styles/MenuStyle.xaml");
        public static Uri MenuButtonStyleSource { get; } = new Uri($"{PackUri}Styles/MenuButtonStyle.xaml");
        public static Uri MediaPlayerStyleSource { get; } = new Uri($"{PackUri}Styles/MediaPlayerStyle.xaml");
        public static Uri MultiSelectTreeViewStyleSource { get; } = new Uri($"{PackUri}Styles/MultiSelectTreeViewStyle.xaml");

        public static Uri NumericBoxStyleSource { get; } = new Uri($"{PackUri}Styles/NumericBoxStyle.xaml");

        public static Uri PasswordBoxStyleSource { get; } = new Uri($"{PackUri}Styles/PasswordBoxStyle.xaml");
        public static Uri PaginationStyleSource { get; } = new Uri($"{PackUri}Styles/PaginationStyle.xaml");

        public static Uri RestrictTextBoxStyleSource { get; } = new Uri($"{PackUri}Styles/RestrictTextBoxStyle.xaml");
        public static Uri RadioButtonStyleSource { get; } = new Uri($"{PackUri}Styles/RadioButtonStyle.xaml");
        public static Uri RangeSliderStyleSource { get; } = new Uri($"{PackUri}Styles/RangeSliderStyle.xaml");
        public static Uri RepeatButtonStyleSource { get; } = new Uri($"{PackUri}Styles/RepeatButtonStyle.xaml");

        public static Uri SliderStyleSource { get; } = new Uri($"{PackUri}Styles/SliderStyle.xaml");
        public static Uri SpitButtonStyleSource { get; } = new Uri($"{PackUri}Styles/SpitButtonStyle.xaml");
        public static Uri SeparatorStyleSource { get; } = new Uri($"{PackUri}Styles/SeparatorStyle.xaml");
        public static Uri ScrollViewerStyleSource { get; } = new Uri($"{PackUri}Styles/ScrollViewerStyle.xaml");

        public static Uri TextBoxStyleSource { get; } = new Uri($"{PackUri}Styles/TextBoxStyle.xaml");
        public static Uri ToggleSwitchStyleSource { get; } = new Uri($"{PackUri}Styles/ToggleSwitchStyle.xaml");
        public static Uri ToggleStatusStyleSource { get; } = new Uri($"{PackUri}Styles/ToggleStatusStyle.xaml");
        public static Uri TreeViewStyleSource { get; } = new Uri($"{PackUri}Styles/TreeViewStyle.xaml");
        public static Uri TabControlStyleSource { get; } = new Uri($"{PackUri}Styles/TabControlStyle.xaml");

        public static Uri UserWindowStyleSource { get; } = new Uri($"{PackUri}Styles/UserWindowStyle.xaml");

        public static Uri ZoomBoxStyleSource { get; } = new Uri($"{PackUri}Styles/ZoomBoxStyle.xaml");

        #endregion

        #region Constants

        public static ComponentResourceKey TrueValueKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "TrueValue");
        public static ComponentResourceKey FalseValueKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "FalseValue");

        #endregion

        #region ButtonBase

        /// <summary>
        /// Style="{DynamicResource/StaticResource {x:Static themes:ResourceKeys.RubikButtonStyleKey}}"
        /// </summary>
        public static ComponentResourceKey RubikButtonBaseStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikButtonBaseStyle");

        #endregion

        #region Button

        public static ComponentResourceKey OpacityButtonStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "OpacityButtonStyle");

        public static ComponentResourceKey NormalButtonStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "NormalButtonStyle");

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

        #region TabControl

        public static ComponentResourceKey RubikTabControlStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikTabControlStyle");
        public static ComponentResourceKey RubikTabItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikTabItemStyle");

        #endregion

        #region TreeView

        public static ComponentResourceKey RubikTreeViewStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikTreeViewStyle");

        public static ComponentResourceKey RubikTreeViewExpandCollapseToggleStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikTreeViewExpandCollapseToggleStyleKey");
        public static ComponentResourceKey RubikTreeViewItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikTreeViewItemStyle");

        #endregion

        #region TextBox

        public static ComponentResourceKey RubikTextBoxStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikTextBoxStyle");

        #endregion

        #region PasswordBox

        public static ComponentResourceKey RubikPasswordBoxStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikPasswordBoxStyle");

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

        #region Menu

        public static ComponentResourceKey RubikContextMenuStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikContextMenuStyle");
        public static ComponentResourceKey RubikMenuItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikMenuItemStyle");
        public static ComponentResourceKey RubikMenuItemSubmenuContentKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikMenuItemSubmenuContent");
        public static ComponentResourceKey RubikMenuItemTopLevelHeaderTemplateKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikMenuItemTopLevelHeaderTemplate");
        public static ComponentResourceKey RubikMenuItemTopLevelItemTemplateKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikMenuItemTopLevelItemTemplate");
        public static ComponentResourceKey RubikMenuItemSubmenuHeaderTemplateKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikMenuItemSubmenuHeaderTemplate");
        public static ComponentResourceKey RubikMenuItemSubmenuItemTemplateKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "RubikMenuItemSubmenuItemTemplate");

        #endregion

        #region Converter

        public static ComponentResourceKey BoolConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "BoolConverter");
        public static ComponentResourceKey BoolToVisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "BoolToVisibilityConverter");
        public static ComponentResourceKey BoolToVisibilityWithHiddenConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "BoolToVisibilityWithHiddenConverter");
        public static ComponentResourceKey BoolToTextDecorationsConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "BoolToTextDecorationsConverter");

        public static ComponentResourceKey ColorToBrushConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "ColorToBrushConverter");

        public static ComponentResourceKey DaysInMonthMultiConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "DaysInMonthMultiConverter");
        public static ComponentResourceKey DoubleToGridLengthConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "DoubleToGridLengthConverter");
        public static ComponentResourceKey DoubleToCornerRadiusConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "DoubleToCornerRadiusConverter");

        public static ComponentResourceKey EnumToBoolWithTrueConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "EnumToBoolWithTrueConverter");
        public static ComponentResourceKey EnumToBoolWithFalseConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "EnumToBoolWithFalseConverter");
        public static ComponentResourceKey EnumToVisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "EnumToVisibilityConverter");

        public static ComponentResourceKey FirstItemOfItemsControlMultiConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "FirstItemOfItemsControlMultiConverter");

        public static ComponentResourceKey KeyToValueConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "KeyToValueConverter");

        public static ComponentResourceKey LastItemOfItemsControlMultiConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "LastItemOfItemsControlMultiConverterKey");

        public static ComponentResourceKey NullToBoolConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "NullToBoolConverter");
        public static ComponentResourceKey NullToVisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "NullToVisibilityConverter");

        public static ComponentResourceKey OnlyOneItemOfItemsControlConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "OnlyOneItemOfItemsControlConverter");

        public static ComponentResourceKey StringToVisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "StringToVisibilityConverter");
        public static ComponentResourceKey StringToBoolConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "StringToBoolConverter");
        public static ComponentResourceKey ScrollOffsetToVisibilityMultiConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "ScrollOffsetToVisibilityMultiConverter");

        public static ComponentResourceKey TimeSpanToStringConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "TimeSpanToStringConverter");
        
        public static ComponentResourceKey VisibilityWithHiddenConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "VisibilityWithHiddenConverter");
        public static ComponentResourceKey VisibilityConverterKey { get; } = new ComponentResourceKey(typeof(ResourceKeys), "VisibilityConverter");

        #endregion
    }
}
