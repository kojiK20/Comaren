using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReadFile
{
    public class rFile
    {
        
        public rFile()
        {
            
        }

        public static List<string> ReadMembers(string fileName)//部員名＋ランクのテキストファイルを読む
        {
            StreamReader sr = new StreamReader(
                fileName, Encoding.GetEncoding("Shift_JIS"));
            List<string> members = new List<string>();
            string line = "";
            while ((line = sr.ReadLine()) != null) //テキストがなくなるまで読む
            {
                if (line != "")
                {
                    string[] memberName = line.Split(':');//memberName->[部内ランク:名前]の2要素文字配列
                    members.Add(memberName[1]);//リストmembersは部員名を持つ(昇順に、ランクに対応)
                }
            }

            return members;
        }

        public static List<string> ReadSentences(string fileName)//コマ練くんがしゃべる文集合
        {
            StreamReader sr = new StreamReader(
                fileName, Encoding.GetEncoding("Shift_JIS"));
            List<string> sentences = new List<string>();
            string line = "";
            while ((line = sr.ReadLine()) != null) //テキストがなくなるまで読む
            {
                if (line.Length <= 27 && line != "")//現状27文字以上だと吹き出しの大きさを超える
                {
                    sentences.Add(line);
                }
            }

            return sentences;
        }

    }
}
