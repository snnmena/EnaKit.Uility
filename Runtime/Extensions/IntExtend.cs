using System;

namespace EnaKit.Utility
{
    public static class IntExtend
    {
        private static readonly string[] ChineseNumbers = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        private static readonly string[] Units = { "", "十", "百", "千", "万", "十万", "百万", "千万", "亿", "十亿", "百亿", "千亿", "万亿" };

        public static string ToChinese(this int self)
        {
            if (self == 0)
                return ChineseNumbers[0];

            string str = self.ToString();
            int len = str.Length;
            string chineseNumber = "";

            for (int i = 0; i < len; i++)
            {
                int num = int.Parse(str[i].ToString());
                if (num > 0)
                {
                    chineseNumber += ChineseNumbers[num] + Units[len - i - 1];
                }
                else
                {
                    if (!chineseNumber.EndsWith(ChineseNumbers[0]))
                        chineseNumber += ChineseNumbers[num];
                }
            }

            // 处理特殊情况，例如“一十”应该是“十”
            chineseNumber = chineseNumber.Replace("一十", "十");

            return chineseNumber;
        }
    }
}