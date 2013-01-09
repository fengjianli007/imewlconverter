﻿using System.Collections.Generic;
using System.Text;
using Studyzy.IMEWLConverter.Generaters;

namespace Studyzy.IMEWLConverter.Entities
{
    /// <summary>
    /// 词条类
    /// </summary>
    public class WordLibrary
    {
        public WordLibrary()
        {
            this.CodeType = CodeType.Pinyin;
            this.Codes=new List<List<string>>();
        }

        #region 基本属性

        private int count = 1;
        private bool isEnglish;
        //private string[] pinYin;
        private string pinYinString = "";
        private string word;

        /// <summary>
        /// 该词条是否是英文词条
        /// </summary>
        public bool IsEnglish
        {
            get { return isEnglish; }
            set { isEnglish = value; }
        }

        /// <summary>
        /// 词语
        /// </summary>
        public string Word
        {
            get { return word; }
            set { word = value; }
        }

        /// <summary>
        /// 词频，打字出现次数
        /// </summary>
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

       

        public CodeType CodeType { get; set; }
        public IList<List<string>> Codes { get; set; }

        #endregion

        #region 扩展属性和方法

        /// <summary>
        /// 词中每个字的拼音(已消除多音字)
        /// </summary>
        public string[] PinYin
        {
            get
            {
                if(CodeType==CodeType.Pinyin&&Codes.Count>0)
                {
                    return Codes[0].ToArray();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                CodeType=CodeType.Pinyin;
                Codes=new List<string>[1];
                Codes[0]=new List<string>(value);
            }
        }
        /// <summary>
        /// 词的拼音字符串，可以单独设置的一个属性，如果没有设置该属性，而获取该属性，则返回PinYin属性和“'”组合的字符串
        /// </summary>
        public string PinYinString
        {
            get
            {
                if (pinYinString == "" && !isEnglish)
                {
                    pinYinString = string.Join("'", PinYin);
                }
                return pinYinString;
            }
            set { pinYinString = value; }
        }

        public string WubiCode
        {
            get
            {
                if (this.CodeType== CodeType.Wubi)
                {
                    return Codes[0][0];
                }
                return null;
            }
        }

        /// <summary>
        /// 添加一种编码类型和词对应的编码，针对一词一码的情况
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        public void AddCode(CodeType type, string str)
        {
            this.CodeType = type;
            this.Codes= new List<List<string>> {new List<string> {str}};
        }

        /// <summary>
        /// 获得拼音字符串
        /// </summary>
        /// <param name="split">每个拼音之间的分隔符</param>
        /// <param name="buildType">组装拼音字符串的方式</param>
        /// <returns></returns>
        public string GetPinYinString(string split, BuildType buildType)
        {
            var sb = new StringBuilder();
            IList<string> list = null;
            if (PinYin != null)
            {
                list = new List<string>(PinYin);
            }

            if (list == null || list.Count == 0)
            {
                var pyGenerater = new PinyinGenerater();
                list = pyGenerater.GetCodeOfString(word);
            }
            if (list.Count == 0)
            {
                return "";
            }
            foreach (string s in list)
            {
                sb.Append(s + split);
            }
            if (buildType == BuildType.RightContain)
            {
                return sb.ToString();
            }
            if (buildType == BuildType.FullContain)
            {
                return split + sb;
            }
            string str = sb.ToString();
            if (split.Length > 0)
            {
                str = str.Remove(sb.Length - 1);
            }
            if (buildType == BuildType.None)
            {
                return str;
            }
            else //BuildType.LeftContain
            {
                return split + str;
            }
        }

        public string ToDisplayString()
        {
            return "汉字：" + word + (string.IsNullOrEmpty(WubiCode) ? "；拼音：" + PinYinString : "五笔：" + WubiCode) + "；词频：" +
                   count;
        }

        public override string ToString()
        {
            return "WordLibrary 汉字：" + word +
                   (string.IsNullOrEmpty(WubiCode) ? "；拼音：" + PinYinString : "五笔：" + WubiCode) + "；词频：" + count;
        }

        #endregion
    }
}