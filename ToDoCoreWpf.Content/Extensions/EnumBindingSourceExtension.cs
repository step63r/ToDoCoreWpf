using System;
using System.Windows.Markup;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Extensions
{
    /// <summary>
    /// enum値を生成するためのマークアップ拡張
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        #region プロパティ
        private Type _enumType;
        /// <summary>
        /// 対象のenum型
        /// </summary>
        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value != _enumType)
                {
                    if (null != value)
                    {
                        var enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                        {
                            throw new ArgumentException("Type must be for an Enum.");
                        }
                    }

                    _enumType = value;
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnumBindingSourceExtension() { }
        /// <summary>
        /// 引数付きコンストラクタ
        /// </summary>
        /// <param name="enumType"></param>
        public EnumBindingSourceExtension(Type enumType)
        {
            _enumType = enumType;
        }
        #endregion

        /// <summary>
        /// 生成した値を取得する
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == _enumType)
            {
                throw new InvalidOperationException("The EnumType must be specified.");
            }

            var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
            {
                return enumValues;
            }

            var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
