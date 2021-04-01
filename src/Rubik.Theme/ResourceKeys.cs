using System.Windows;

namespace Rubik.Theme
{
    public static class ResourceKeys
    {
        #region ButtonBase
        /// <summary>
        /// Style="{DynamicResource/StaticResource {x:Static themes:ResourceKeys.NoBgButtonStyleKey}}"
        /// </summary>
        public const string NoBgButtonBaseStyle = "NoBgButtonBaseStyle";
        public static ComponentResourceKey NoBgButtonBaseStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NoBgButtonBaseStyle); }
        }

        public const string NoBgRoundButtonBaseStyle = "NoBgRoundButtonBaseStyle";
        public static ComponentResourceKey NoBgRoundButtonBaseStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NoBgRoundButtonBaseStyle); }
        }

        #endregion

        #region Button

        public const string NormalButtonStyle = "NormalButtonStyle";
        public static ComponentResourceKey NormalButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalButtonStyle); }
        }

        public const string RoundButtonStyle = "RoundButtonStyle";
        public static ComponentResourceKey RoundButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), RoundButtonStyle); }
        }

        public const string StatusbarButtonStyle = "StatusbarButtonStyle";
        public static ComponentResourceKey StatusbarButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), StatusbarButtonStyle); }
        }

        public const string TitlebarButtonStyle = "TitlebarButtonStyle";
        public static ComponentResourceKey TitlebarButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), TitlebarButtonStyle); }
        }

        public const string ToolbarButtonStyle = "ToolbarButtonStyle";
        public static ComponentResourceKey ToolbarButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ToolbarButtonStyle); }
        }

        public const string TitlebarCloseBtnStyle = "TitlebarCloseBtnStyle";
        public static ComponentResourceKey TitlebarCloseBtnStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), TitlebarCloseBtnStyle); }
        }

        public const string SelectionCloseBtnStyle = "SelectionCloseBtnStyle";
        public static ComponentResourceKey SelectionCloseBtnStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), SelectionCloseBtnStyle); }
        }

        public const string OpacityButtonStyle = "OpacityButtonStyle";
        public static ComponentResourceKey OpacityButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), OpacityButtonStyle); }
        }

        public const string CircleBtnStyle = "CircleBtnStyle";
        public static ComponentResourceKey CircleBtnStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), CircleBtnStyle); }
        }

        #endregion

        #region RepeatButton

        public const string NormalRepeatButtonStyle = "NormalRepeatButtonStyle";
        public static ComponentResourceKey NormalRepeatButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalRepeatButtonStyle); }
        }

        public const string ScrollRepeatButtonStyle = "ScrollRepeatButtonStyle";
        public static ComponentResourceKey ScrollRepeatButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ScrollRepeatButtonStyle); }
        }

        #endregion

        #region ToggleButton

        public const string NormalToggleButtonStyle = "NormalToggleButtonStyle";
        public static ComponentResourceKey NormalToggleButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalToggleButtonStyle); }
        }

        public const string SelectionToggleButtonStyle = "SelectionToggleButtonStyle";
        public static ComponentResourceKey SelectionToggleButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), SelectionToggleButtonStyle); }
        }
        
        #endregion

        #region RadioButton

        public const string NormalRadioButtonStyle = "NormalRadioButtonStyle";
        public static ComponentResourceKey NormalRadioButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalRadioButtonStyle); }
        }

        public const string SelectionRadioButtonStyle = "SelectionRadioButtonStyle";
        public static ComponentResourceKey SelectionRadioButtonStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), SelectionRadioButtonStyle); }
        }

        #endregion

        #region ToggleStatus

        public const string NoBgToggleStatusStyle = "NoBgToggleStatusStyle";
        public static ComponentResourceKey NoBgToggleStatusStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NoBgToggleStatusStyle); }
        }

        #endregion

        #region CheckBox

        public const string NormalCheckBoxStyle = "NormalCheckBoxStyle";
        public static ComponentResourceKey NormalCheckBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalCheckBoxStyle); }
        }

        #endregion

        #region MenuItem

        public const string ContextMenuStyle = "ContextMenuStyle";
        public static ComponentResourceKey ContextMenuStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ContextMenuStyle); }
        }

        public const string MenuItemStyle = "MenuItemStyle";
        public static ComponentResourceKey MenuItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MenuItemStyle); }
        }

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

        public const string MenuItemSubmenuContent = "MenuItemSubmenuContent";
        public static ComponentResourceKey MenuItemSubmenuContentKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MenuItemSubmenuContent); }
        }

        public const string MenuItemTopLevelHeaderTemplate = "MenuItemTopLevelHeaderTemplate";
        public static ComponentResourceKey MenuItemTopLevelHeaderTemplateKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MenuItemTopLevelHeaderTemplate); }
        }

        public const string MenuItemTopLevelItemTemplate = "MenuItemTopLevelItemTemplate";
        public static ComponentResourceKey MenuItemTopLevelItemTemplateKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MenuItemTopLevelItemTemplate); }
        }

        public const string MenuItemSubmenuHeaderTemplate = "MenuItemSubmenuHeaderTemplate";
        public static ComponentResourceKey MenuItemSubmenuHeaderTemplateKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MenuItemSubmenuHeaderTemplate); }
        }

        public const string MenuItemSubmenuItemTemplate = "MenuItemSubmenuItemTemplate";
        public static ComponentResourceKey MenuItemSubmenuItemTemplateKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MenuItemSubmenuItemTemplate); }
        }

        #endregion

        #region ListBox

        public const string NormalListBoxStyle = "NormalListBoxStyle";
        public static ComponentResourceKey NormalListBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalListBoxStyle); }
        }

        public const string NormalListBoxItemStyle = "NormalListBoxItemStyle";
        public static ComponentResourceKey NormalListBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalListBoxItemStyle); }
        }

        public const string ServerListBoxItemStyle = "ServerListBoxItemStyle";
        public static ComponentResourceKey ServerListBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ServerListBoxItemStyle); }
        }

        public const string TabListBoxItemStyle = "TabListBoxItemStyle";
        public static ComponentResourceKey TabListBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), TabListBoxItemStyle); }
        }

        public const string SelectionListBoxItemStyle = "SelectionListBoxItemStyle";
        public static ComponentResourceKey SelectionListBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), SelectionListBoxItemStyle); }
        }

        public const string VerticalSelectionListBoxItemStyle = "VerticalSelectionListBoxItemStyle";
        public static ComponentResourceKey VerticalSelectionListBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), VerticalSelectionListBoxItemStyle); }
        }

        #endregion

        #region ComboBox

        public const string NormalComboBoxStyle = "NormalComboBoxStyle";
        public static ComponentResourceKey NormalComboBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalComboBoxStyle); }
        }

        public const string NormalComboBoxItemStyle = "NormalComboBoxItemStyle";
        public static ComponentResourceKey NormalComboBoxItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalComboBoxItemStyle); }
        }

        #endregion

        #region TabControl

        public const string NormalTabControlStyle = "NormalTabControlStyle";
        public static ComponentResourceKey NormalTabControlStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalTabControlStyle); }
        }

        public const string NormalTabItemStyle = "NormalTabItemStyle";
        public static ComponentResourceKey NormalTabItemStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalTabItemStyle); }
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

        public const string NormalScrollBarStyle = "NormalScrollBarStyle";
        public static ComponentResourceKey NormalScrollBarStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalScrollBarStyle); }
        }

        public const string NormalScrollViewerStyle = "NormalScrollViewerStyle";
        public static ComponentResourceKey NormalScrollViewerStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalScrollViewerStyle); }
        }

        #endregion

        #region Slider

        public const string NormalSliderStyle = "NormalSliderStyle";
        public static ComponentResourceKey NormalSliderStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalSliderStyle); }
        }

        #endregion

        #region GridSplitter

        public const string NormalGridSplitterStyle = "NormalGridSplitterStyle";
        public static ComponentResourceKey NormalGridSplitterStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalGridSplitterStyle); }
        }

        #endregion

        #region TextBox

        public const string NormalTextBoxStyle = "NormalTextBoxStyle";
        public static ComponentResourceKey NormalTextBoxStyleKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NormalTextBoxStyle); }
        }

        #endregion

        #region Geometry

        public const string AlphaVisualBrush = "AlphaVisualBrush";
        public static ComponentResourceKey AlphaVisualBrushKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), AlphaVisualBrush); }
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

        public const string StringToVisibilityConverter = "StringToVisibilityConverter";
        public static ComponentResourceKey StringToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), StringToVisibilityConverter); }
        }

        public const string StringToBoolConverter = "StringToBoolConverter";
        public static ComponentResourceKey StringToBoolConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), StringToBoolConverter); }
        }

        public const string NullToVisibilityConverter = "NullToVisibilityConverter";
        public static ComponentResourceKey NullToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), NullToVisibilityConverter); }
        }

        public const string EqualIntToVisibilityConverter = "EqualIntToVisibilityConverter";
        public static ComponentResourceKey EqualIntToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), EqualIntToVisibilityConverter); }
        }

        public const string EnumToVisibilityConverter = "EnumToVisibilityConverter";
        public static ComponentResourceKey EnumToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), EnumToVisibilityConverter); }
        }

        public const string CollapsedEnumToVisibilityConverter = "CollapsedEnumToVisibilityConverter";
        public static ComponentResourceKey CollapsedEnumToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), CollapsedEnumToVisibilityConverter); }
        }

        public const string GreaterThanIntToVisibilityConverter = "GreaterThanIntToVisibilityConverter";
        public static ComponentResourceKey GreaterThanIntToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), GreaterThanIntToVisibilityConverter); }
        }

        public const string KeyToValueConverter = "KeyToValueConverter";
        public static ComponentResourceKey KeyToValueConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), KeyToValueConverter); }
        }

        public const string ArrayLengthToVisibilityConverter = "ArrayLengthToVisibilityConverter";
        public static ComponentResourceKey ArrayLengthToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ArrayLengthToVisibilityConverter); }
        }

        public const string ArrayLengthGreaterThanIntToVisibilityConverter = "ArrayLengthGreaterThanIntToVisibilityConverter";
        public static ComponentResourceKey ArrayLengthGreaterThanIntToVisibilityConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ArrayLengthGreaterThanIntToVisibilityConverter); }
        }

        public const string StringArrayAddIndexConverter = "StringArrayAddIndexConverter";
        public static ComponentResourceKey StringArrayAddIndexConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), StringArrayAddIndexConverter); }
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

        public const string DoubleToCornerRadiusConverter = "DoubleToCornerRadiusConverter";
        public static ComponentResourceKey DoubleToCornerRadiusConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), DoubleToCornerRadiusConverter); }
        }

        public const string DoubleToGridLengthConverter = "DoubleToGridLengthConverter";
        public static ComponentResourceKey DoubleToGridLengthConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), DoubleToGridLengthConverter); }
        }

        public const string MultiBoolOrToVisibilityMultiConverter = "MultiBoolOrToVisibilityMultiConverter";
        public static ComponentResourceKey MultiBoolOrToVisibilityMultiConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MultiBoolOrToVisibilityMultiConverter); }
        }
        

        public const string MultiVisibilityOrToVisibilityMultiConverter = "MultiVisibilityOrToVisibilityMultiConverter";
        public static ComponentResourceKey MultiVisibilityOrToVisibilityMultiConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), MultiVisibilityOrToVisibilityMultiConverter); }
        }

        public const string DaysInMonthMultiConverter = "DaysInMonthMultiConverter";
        public static ComponentResourceKey DaysInMonthMultiConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), DaysInMonthMultiConverter); }
        }

        public const string ScrollOffsetToVisibilityMultiConverter = "ScrollOffsetToVisibilityMultiConverter";
        public static ComponentResourceKey ScrollOffsetToVisibilityMultiConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ScrollOffsetToVisibilityMultiConverter); }
        }
        
        public const string LastItemCollapsedMultiConverter = "LastItemCollapsedMultiConverter";
        public static ComponentResourceKey LastItemCollapsedMultiConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), LastItemCollapsedMultiConverter); }
        }

        public const string ItemIndexMultiConverter = "ItemIndexMultiConverter";
        public static ComponentResourceKey ItemIndexMultiConverterKey
        {
            get { return new ComponentResourceKey(typeof(ResourceKeys), ItemIndexMultiConverter); }
        }

        #endregion
    }
}
