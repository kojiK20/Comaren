using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReadFile;

namespace COMA_practice
{
    public partial class コマ練くん : Form
    {
        List<string> participants_names = new List<string>();//全参加者のノード集合
        List<string> participants_top = new List<string>();//参加者のランク上半分
        List<string> participants_bottom = new List<string>();//参加者のランク下半分
        List<string> participants_out = new List<string>();//外部参加者
        List<string[]> participants_edges = new List<string[]>();//参加者間の枝集合＝マッチング


        string participant1;//参加する人
        string[] participants1 = {""};
        string participant2 = "";
        string[] participants2 = {""};
        List<string> participant1_2dec = new List<string>();//1-2間で減った人 
        List<string> participant1_2inc = new List<string>();//1-2間で増えた人
        string participant3 = "";
        string[] participants3 = {""};
        List<string> participant2_3dec = new List<string>();//1-2間で減った人 
        List<string> participant2_3inc = new List<string>();//1-2間で増えた人
        string participant4 = "";
        string[] participants4 = {""};
        List<string> participant3_4dec = new List<string>();//1-2間で減った人 
        List<string> participant3_4inc = new List<string>();//1-2間で増えた人
        string participant5 = "";
        string[] participants5 = {""};
        List<string> participant4_5dec = new List<string>();//1-2間で減った人 
        List<string> participant4_5inc = new List<string>();//1-2間で増えた人
        static string fileMem = "members.txt";//使用するテキストファイルの名前
        static string fileSen = "serifu.txt";
        List<string> members = rFile.ReadMembers(fileMem);//部員のリストを得る
        List<string> sentences = rFile.ReadSentences(fileSen);//コマ練くんがしゃべる
        int final = 0;
        int seed = Environment.TickCount;
        int coma = 0;
        string numOfTable;
        string coma_pre;//フォーカスに入る直前のコマ練くんのセリフ
        public コマ練くん()
        {
            InitializeComponent();
            this.label1.BackColor = Color.Transparent;
            this.label2.BackColor = Color.Transparent;
            this.label3.BackColor = Color.Transparent;
            this.label4.BackColor = Color.Transparent;
            this.label5.BackColor = Color.Transparent;
            this.label6.BackColor = Color.Transparent;
            this.label7.BackColor = Color.Transparent;
            this.label8.BackColor = Color.Transparent;
            this.label9.BackColor = Color.Transparent;
            this.label10.BackColor = Color.Transparent;
            this.label11.BackColor = Color.Transparent;
            this.label12.BackColor = Color.Transparent;
            this.label13.BackColor = Color.Transparent;
            this.label14.BackColor = Color.Transparent;
            this.label15.BackColor = Color.Transparent;
            this.label16.BackColor = Color.Transparent;
            this.label17.BackColor = Color.Transparent;
            this.label18.BackColor = Color.Transparent;
            //this.label20.BackColor = Color.Transparent;
            //this.label21.BackColor = Color.Transparent;
            //this.label22.BackColor = Color.Transparent;
            //this.label23.BackColor = Color.Transparent;//各ラベルの背景を透過
            //this.textBox6.BackColor = Color.Transparent;
            if (sentences.Count != 0)
            {
                int[] firstComa = new int[sentences.Count];
                int c = 0;
                foreach (int i in firstComa)
                {
                    firstComa[c] = c;
                    c++;
                }
                firstComa = firstComa.OrderBy(i => Guid.NewGuid()).ToArray();
                coma = firstComa[0];
                this.label19.Text = sentences[coma];
            }
            else
            {
                this.label19.Text = "";
            }
            string memberList = "RANK\r\n";
            if (members.Count != 0)
            {
                foreach (string mem in members)
                {
                    memberList = memberList + mem + "\r\n";
                }
                this.textBox6.Text = memberList;
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            leave = 0;
            this.label19.Text = sentences[coma];
            numOfTable = textBoxNumTable.Text;
            //まず、ボタンが押された時点での参加者データの生成
            participant1 = "　" + textBox1.Text + "　";//名前の前後に全角スペースがあるかどうかで名前の区切りとする。最初と最後にスペースを加える
            participants1 = participant1.Split('　');
            participant2 = "　" + textBox2.Text + "　";
            participants2 = participant2.Split('　');
            participant1_2dec = new List<string>();
            participant1_2inc = new List<string>();
            participant3 = "　" + textBox3.Text + "　";
            participants3 = participant3.Split('　');
            participant2_3dec = new List<string>();
            participant2_3inc = new List<string>();
            participant4 = "　" + textBox4.Text + "　";
            participants4 = participant4.Split('　');
            participant3_4dec = new List<string>();
            participant3_4inc = new List<string>();
            participant5 = "　" + textBox5.Text + "　";
            participants5 = participant5.Split('　');
            participant4_5dec = new List<string>();
            participant4_5inc = new List<string>();
            if (participants2.Count() == 1 && participants2[0] == "")//空配列
            {
                participants2 = participants1;
                participant2 = participant1;
            }
            else
            {
                int i = 0;
                participant2 = participant1;
                while (i < participants2.Length)
                {
                    if (participants2[i] != "")//nullの場合0文字列なので一文字目の判定ができないので弾く
                    {
                        if (participants2[i][0] == '+')//人の追加
                        {
                            participant2 = participant2 + "　" + participants2[i].Replace("+", "");
                            participant1_2inc.Add(participants2[i].Replace("+", ""));
                        }
                        else if (participants2[i][0] == '-')//人の削除
                        {
                            participant2 = participant2.Replace("　" + participants2[i].Replace("-", "") + "　", "　");//実際はこれは削除ではなく"　"への置き換え
                            participant1_2dec.Add(participants2[i].Replace("-", ""));
                        }
                    }
                    i++;
                }
                participants2 = participant2.Split('　');
            }


            if (participants3.Count() == 1 && participants3[0] == "")//空配列
            {
                participants3 = participants2;
                participant3 = participant2;
            }
            else
            {
                int i = 0;
                participant3 = participant2;
                while (i < participants3.Length)
                {
                    if (participants3[i] != "")
                    {
                        if (participants3[i][0] == '+')//人の追加
                        {
                            participant3 = participant3 + "　" + participants3[i].Replace("+", "");
                            participant2_3inc.Add(participants3[i].Replace("+", ""));
                        }
                        else if (participants3[i][0] == '-')//人の削除
                        {
                            participant3 = participant3.Replace("　" + participants3[i].Replace("-", "") + "　", "　");
                            participant2_3dec.Add(participants3[i].Replace("-", ""));
                        }
                    }
                    i++;
                }
                participants3 = participant3.Split('　');
            }


            if (participants4.Count() == 1 && participants4[0] == "")//空配列
            {
                participants4 = participants3;
                participant4 = participant3;
            }
            else
            {
                int i = 0;
                participant4 = participant3;
                while (i < participants4.Length)
                {
                    if (participants4[i] != "")
                    {
                        if (participants4[i][0] == '+')//人の追加
                        {
                            participant4 = participant4 + "　" + participants4[i].Replace("+", "");
                            participant3_4inc.Add(participants4[i].Replace("+", ""));
                        }
                        else if (participants4[i][0] == '-')//人の削除
                        {
                            participant4 = participant4.Replace("　" + participants4[i].Replace("-", "") + "　", "　");
                            participant3_4dec.Add(participants4[i].Replace("-", ""));
                        }
                    }
                    i++;
                }
                participants4 = participant4.Split('　');
            }


            if (participants5.Count() == 1 && participants5[0] == "")//空配列
            {
                participants5 = participants4;
                participant5 = participant4;
            }
            else
            {
                int i = 0;
                participant5 = participant4;
                while (i < participants5.Length)
                {
                    if (participants5[i] != "")
                    {
                        if (participants5[i][0] == '+')//人の追加
                        {
                            participant5 = participant5 + "　" + participants5[i].Replace("+", "");
                            participant4_5inc.Add(participants5[i].Replace("+", ""));
                        }
                        else if (participants5[i][0] == '-')//人の削除
                        {
                            participant5 = participant5.Replace("　" + participants5[i].Replace("-", "") + "　", "　");
                            participant4_5dec.Add(participants5[i].Replace("-", ""));
                        }
                    }
                    i++;
                }
                participants5 = participant5.Split('　');
            }

            int roopStopper = -1;//ループ回数が1000を超えたら1コマ目から生成しなおす
            int table = 0;//使用可能台数
            int patNum1, patNum2, patNum3, patNum4, patNum5;
            patNum1 = 0;
            foreach (string pat in participants1)
            {
                if (pat == "")
                {
                }
                else
                {
                    patNum1++;
                }
            }
            patNum2 = 0;
            foreach (string pat in participants2)
            {
                if (pat == "")
                {
                }
                else
                {
                    patNum2++;
                }
            }
            patNum3 = 0;
            foreach (string pat in participants3)
            {
                if (pat == "")
                {
                }
                else
                {
                    patNum3++;
                }
            }
            patNum4 = 0;
            foreach (string pat in participants4)
            {
                if (pat == "")
                {
                }
                else
                {
                    patNum4++;
                }
            }
            patNum5 = 0;
            foreach (string pat in participants5)
            {
                if (pat == "")
                {
                }
                else
                {
                    patNum5++;
                }
            }
            if (patNum1 <= 5)
            {
                MessageBox.Show("1コマ目の参加者を6人以上入力してくれだコマ～");
                roopStopper = 1;
            }
            else if (patNum2 <= 5)
            {
                MessageBox.Show("2コマ目の参加者を6人以上にしてくれだコマ～");
                roopStopper = 1;
            }
            else if (patNum3 <= 5)
            {
                MessageBox.Show("3コマ目の参加者を6人以上にしてくれだコマ～");
                roopStopper = 1;
            }
            else if (patNum4 <= 5)
            {
                MessageBox.Show("4コマ目の参加者を6人以上にしてくれだコマ～");
                roopStopper = 1;
            }
            else if (patNum5 <= 5)
            {
                MessageBox.Show("5コマ目の参加者を6人以上にしてくれだコマ～");
                roopStopper = 1;
            }
            else if (!int.TryParse(numOfTable, out table))
            {
                MessageBox.Show("使用可能な台数には1以上10以下の整数を入力してくれコマ～");
                roopStopper = 1;
            }
            else if (table < 1 || table > 10)
            {
                MessageBox.Show("使用可能な台数は1以上10以下にしてくれコマ～");
                roopStopper = 1;
            }
            else if (patNum1 > table * 3 || patNum2 > table * 3 || patNum3 > table * 3 || patNum4 > table * 3 || patNum5 > table * 3)
            {
                MessageBox.Show("参加者が【使用可能台数×3】(人)を超えているコマ...");
                roopStopper = 1;
            }
            else
            {




                string result = "1コマ目　\n　";
                ////////////1コマ目。///////////////////////////////////////////////
                while (roopStopper == -1 || roopStopper > 1000)//無限ループしたときは、戻ってくる
                {
                    result = "1コマ目　\n　";
                    roopStopper = 0;
                    List<int> memNum1 = new List<int>();//コマ練に参加するメンバーのランク
                    List<string> memName1 = new List<string>();//コマ練に参加するメンバーのランク昇順の名前
                    List<string> notMemName1 = new List<string>();//ランクを持たない参加者の名前
                                                                  //string[] pairs1 = participants1.OrderBy(i => Guid.NewGuid()).ToArray();//ランダムソート
                    foreach (string per in participants1)
                    {
                        if (per == "")
                        {
                            //何もしない
                        }
                        else if (members.IndexOf(per) != -1)
                        {
                            memNum1.Add(members.IndexOf(per));
                        }
                        else//外部の参加者
                        {
                            notMemName1.Add(per);
                        }

                    }//memNum...参加者のランクのリスト、notMemName...外部の参加者の名前
                    memNum1.Sort();//参加者のランクが昇順に
                    foreach (int rank in memNum1)
                    {
                        memName1.Add(members[rank]);//memNum1をランクから名前に変換

                    }
                    foreach (string not in notMemName1)
                    {
                        if (memName1.Count % 2 == 1)//参加者が奇数
                        {
                            if (not[0] == '↑')
                            {
                                memName1.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName1.Count == 0)
                                {
                                    memName1.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName1.Insert(memName1.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName1.Insert((memName1.Count + 1) / 2, not);//中位
                            }
                        }
                        else//偶数
                        {
                            if (not[0] == '↑')
                            {
                                memName1.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName1.Count == 0)
                                {
                                    memName1.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName1.Insert(memName1.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName1.Insert(memName1.Count / 2, not);
                            }
                        }
                    }//ランクに外部の参加者を挿入
                    int seed = Environment.TickCount;
                    System.Random r = new System.Random(seed);
                    int high = memName1.Count / 2;//切り捨て
                    int[] oneSecondHigh = new int[high];
                    int[] oneSecondLow = new int[memName1.Count - high];
                    int[] oneSecondMiddle = new int[2 * (high / 2)];//偶数化
                    if (memName1.Count > 10)//11人以上参加する場合、マッチングの範囲は動的に決定すべき
                    {
                        int m = 0;
                        int l = 0;
                        int n = 1;
                        int ex = 0;
                        foreach (int i in oneSecondHigh)
                        {
                            oneSecondHigh[m] = m;
                            m++;
                        }
                        foreach (int i in oneSecondLow)
                        {
                            oneSecondLow[l] = m;
                            m++;
                            l++;
                        }
                        foreach (int i in oneSecondMiddle)
                        {
                            oneSecondMiddle[ex] = n * (int)Math.Pow(-1, ex);
                            ex++;
                            if ((int)Math.Pow(-1, ex) < 0)
                            {
                                n++;
                            }
                        }
                    }
                    int[] five = new int[] { 0, 1, 2, 3, 4 };//静的なマッチングの範囲
                    int[] three = new int[] { 1, 2, 3 };
                    int[] middle = new int[] { -1, -2, 1, 2 };
                    ///重複チェック用パラメータ///
                    string x1 = "", y1 = "", z1 = "";//1コマ目多球参加者
                    List<string> chohuku = new List<string>();//1コマ目の組を保存
                    //List<string> chohukuTakyu = new List<string>();//多球版
                    List<string> memName_pre = new List<string>(memName1);
                    string result_pre = result;
                    int choCheck = 0;
                    int memNum = memName1.Count;
                    List<string> takyurableMem = new List<string>(memName_pre);//重複度的に多球に選ばれうるリスト(これにより、多球は確実に重複しない)
                    ///重複チェック用パラメータ///
                    if (memName1.Count % 2 == 1 || memName1.Count - table * 2 > 0)//参加者が奇数又は台の制約により全員対人できない->多球
                    {
                        //memName1 = memName_pre;
                        int tableConstraint = 0;//参加者-使用可能台数x2＝多球の数
                        int constraint = memName1.Count - table * 2;
                        if (constraint < 0)//単純に奇数で一台多球のとき
                        {
                            tableConstraint = constraint - 1;
                        }
                        while (tableConstraint < constraint)
                        {
                            choCheck = 1;
                            tableConstraint++;
                            //多球を完全ランダムに構成
                            int[] m1 = new int[] { 0 };//初期化
                            if (takyurableMem.Count >= 3)//重複度が低い人が3人以上
                            {
                                m1 = new int[takyurableMem.Count];
                                for (int i = 0; i < takyurableMem.Count; i++)
                                {
                                    m1[i] = i;
                                }
                                m1 = m1.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                x1 = takyurableMem[m1[0]];
                                y1 = takyurableMem[m1[1]];
                                z1 = takyurableMem[m1[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                takyurableMem.Remove(x1);
                                takyurableMem.Remove(y1);
                                takyurableMem.Remove(z1);
                            }
                            else if (takyurableMem.Count == 2)//重複度が低い人が二人
                            {
                                x1 = takyurableMem[0];
                                y1 = takyurableMem[1];//この時点で全員の重複度が等しい
                                takyurableMem = new List<string>(memName_pre);//重複リストをリセット
                                r = new System.Random(seed++);
                                int i1 = r.Next(memName_pre.Count);//乱数
                                z1 = takyurableMem[i1];//takyurableから選ぶことで確実に重複度が競合しない
                                takyurableMem.Remove(z1);
                            }
                            else//重複度が低い人が一人
                            {
                                x1 = takyurableMem[0];//この時点で全員の重複度が等しい
                                takyurableMem = new List<string>(memName_pre);//重複リストをリセット
                                m1 = new int[takyurableMem.Count];
                                for (int i = 0; i < takyurableMem.Count; i++)
                                {
                                    m1[i] = i;
                                }
                                m1 = m1.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                y1 = takyurableMem[m1[0]];
                                z1 = takyurableMem[m1[1]];
                                takyurableMem.Remove(y1);
                                takyurableMem.Remove(z1);
                            }

                            result = result + "多球：" + x1 + "-" + y1 + "-" + z1 + "\n　";
                            memName1.Remove(x1);
                            memName1.Remove(y1);
                            memName1.Remove(z1);//その3人を参加者から削除

                        }
                    }//参加者が偶数
                    high = memName1.Count / 2;//切り捨て
                    while (memName1.Count > 1)
                    {
                        //ver4.0：まず上位半分と下位半分の2集合に分け、それぞれのなかからマッチングしつくす

                        if (high <= 1)
                        {
                            high = 0;
                        }

                        oneSecondHigh = new int[high];
                        oneSecondLow = new int[memName1.Count - high];


                        int m = 0;
                        int l = 0;
                        foreach (int i in oneSecondHigh)//動的にマッチング範囲を更新
                        {
                            oneSecondHigh[m] = m;
                            m++;
                        }

                        foreach (int i in oneSecondLow)
                        {
                            oneSecondLow[l] = m;
                            m++;
                            l++;
                        }

                        if (high > 1)//上位半分から2人を選ぶ
                        {

                            oneSecondHigh = oneSecondHigh.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName1[oneSecondHigh[0]];
                            string y = memName1[oneSecondHigh[1]];
                            result = result + x + "-" + y + "\n　";
                            memName1.Remove(x);
                            memName1.Remove(y);
                            chohuku.Add(x + " " + y);//組を保存
                            chohuku.Add(y + " " + x);
                        }
                        else//下位半分から2人を選ぶ
                        {
                            oneSecondLow = oneSecondLow.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName1[oneSecondLow[0]];
                            string y = memName1[oneSecondLow[1]];
                            result = result + x + "-" + y + "\n　";
                            memName1.Remove(x);
                            memName1.Remove(y);
                            chohuku.Add(x + " " + y);
                            chohuku.Add(y + " " + x);
                        }
                        high = high - 2;
                    }
                    result = result.TrimEnd() + "\n";
                    /*while (memName1.Count > 4)//残り4人になるまで
                    {
                        r = new System.Random(seed++);
                        int h1 = r.Next(2);//h1は0～1の乱数
                        if (h1 == 0)//上位5人から2人を選ぶ
                        {
                            five = five.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName1[five[0]];
                            string y = memName1[five[1]];
                            result = result + x + "-" + y + "\n　";
                            memName1.Remove(x);
                            memName1.Remove(y);
                            chohuku.Add(x + " " + y);//組を保存(ランク順は変動しないのでこの二人はこの順序(ランク昇順)しかありえない)
                            chohuku.Add(y + " " + x);
                        }
                        else//下位5人から2人を選ぶ
                        {
                            five = five.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName1[memName1.Count - 1 - five[0]];
                            string y = memName1[memName1.Count - 1 - five[1]];
                            result = result + x + "-" + y + "\n　";
                            memName1.Remove(x);
                            memName1.Remove(y);
                            chohuku.Add(x + " " + y);
                            chohuku.Add(y + " " + x);
                        }
                    }//残り4人
                    result = result + memName1[0] + "-" + memName1[1] + "\n　";
                    result = result + memName1[2] + "-" + memName1[3] + "\n";
                    chohuku.Add(memName1[0] + " " + memName1[1]);
                    chohuku.Add(memName1[2] + " " + memName1[3]);
                    chohuku.Add(memName1[1] + " " + memName1[0]);
                    chohuku.Add(memName1[3] + " " + memName1[2]);*/


                    /////////////////////////////////////////////////////////////////////////////
                    /////////////////2コマ目////////////////////////////
                    result = result + "2コマ目　\n　";
                    List<int> memNum2 = new List<int>();//コマ練に参加するメンバーのランク
                    List<string> memName2 = new List<string>();//コマ練に参加するメンバーのランク昇順の名前
                    List<string> notMemName2 = new List<string>();//ランクを持たない参加者の名前

                    foreach (string per in participants2)
                    {
                        if (per == "")
                        {
                            //何もしない
                        }
                        else if (members.IndexOf(per) != -1)
                        {
                            memNum2.Add(members.IndexOf(per));
                        }
                        else//外部の参加者
                        {
                            notMemName2.Add(per);
                        }

                    }//memNum...参加者のランクのリスト、notMemName...外部の参加者の名前
                    memNum2.Sort();//参加者のランクが昇順に
                    foreach (int rank in memNum2)
                    {
                        memName2.Add(members[rank]);//memNum1をランクから名前に変換

                    }
                    foreach (string not in notMemName2)
                    {
                        if (memName2.Count % 2 == 1)//参加者が奇数
                        {
                            if (not[0] == '↑')
                            {
                                memName2.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName2.Count == 0)
                                {
                                    memName2.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName2.Insert(memName2.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName2.Insert((memName2.Count + 1) / 2, not);
                            }
                        }
                        else//偶数
                        {
                            if (not[0] == '↑')
                            {
                                memName2.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName2.Count == 0)
                                {
                                    memName2.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName2.Insert(memName2.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName2.Insert(memName2.Count / 2, not);
                            }
                        }
                    }//ランクの真ん中に外部の参加者を挿入

                    seed = Environment.TickCount;
                    r = new System.Random(seed);
                    high = memName2.Count / 2;//切り捨て
                    int[] oneSecondHighT2 = new int[high];
                    int[] oneSecondLowT2 = new int[memName2.Count - high];
                    int[] oneSecondMiddleT2 = new int[2 * (high / 2)];//偶数化
                    if (memName2.Count > 10)//11人以上参加する場合、マッチングの範囲は動的に決定すべき
                    {
                        int m = 0;
                        int l = 0;
                        int n = 1;
                        int ex = 0;
                        foreach (int i in oneSecondHighT2)
                        {
                            oneSecondHighT2[m] = m;
                            m++;
                        }
                        foreach (int i in oneSecondLowT2)
                        {
                            oneSecondLowT2[l] = m;
                            m++;
                            l++;
                        }
                        foreach (int i in oneSecondMiddleT2)
                        {
                            oneSecondMiddleT2[ex] = n * (int)Math.Pow(-1, ex);
                            ex++;
                            if ((int)Math.Pow(-1, ex) < 0)
                            {
                                n++;
                            }
                        }
                    }
                    ///重複チェック用パラメータ///
                    string x2 = "", y2 = "", z2 = "";//2コマ目多球参加者
                    List<string> chohuku_pre = new List<string>(chohuku);//1コマ目の組を保存(重複した際に、chohuku_preからやり直す)
                    List<string> chohukuTakyu = new List<string>();//重複度は守られるが、同じコマで同一人物が被るのは防ぐ必要がある
                    memName_pre = new List<string>(memName2);
                    result_pre = result;
                    choCheck = 0;//重複していると0
                    foreach (string i in participant1_2inc)
                    {
                        takyurableMem.Add(i);
                    }
                    foreach (string i in participant1_2dec)
                    {
                        takyurableMem.Remove(i);
                    }
                    List<string> takyurableMem_pre = new List<string>(takyurableMem);
                    ///重複チェック用パラメータ///
                    while (choCheck == 0)//重複が確認される限り0
                    {
                        if (roopStopper > 10000)//無限ループ中
                        {
                            break;
                        }
                        choCheck = 1;
                        chohuku = new List<string>(chohuku_pre);//1コマ目生成直後時点の保存されていたリストに戻り再度生成
                        memName2 = new List<string>(memName_pre);
                        takyurableMem = new List<string>(takyurableMem_pre);

                        result = result_pre;
                        chohukuTakyu = new List<string>();
                        if (memName2.Count % 2 == 1 || memName2.Count - table * 2 > 0)//参加者が奇数又は台の制約により全員対人できない->多球
                        {
                            //memName1 = memName_pre;
                            int tableConstraint = 0;//参加者-使用可能台数x2＝多球の数
                            int constraint = memName2.Count - table * 2;
                            if (constraint < 0)//単純に奇数で一台多球のとき
                            {
                                tableConstraint = constraint - 1;
                            }
                            //MessageBox.Show(memName2.Count.ToString());
                            while (tableConstraint < constraint)
                            {
                                //MessageBox.Show("きてる？");
                                //MessageBox.Show(tableConstraint.ToString() + constraint.ToString());
                                tableConstraint++;
                                //多球を完全ランダムに構成
                                int[] m2 = new int[] { 0 };//初期化
                                r = new System.Random(seed++);
                                if (takyurableMem.Count >= 4)//重複度が低い人が4人以上
                                {
                                    m2 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m2[i] = i;
                                    }
                                    m2 = m2.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x2 = takyurableMem[m2[0]];
                                    y2 = takyurableMem[m2[1]];
                                    z2 = takyurableMem[m2[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x2);
                                    takyurableMem.Remove(y2);
                                    takyurableMem.Remove(z2);
                                    memName2.Remove(x2);
                                    memName2.Remove(y2);
                                    memName2.Remove(z2);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count >= 3)//重複度が低い人が3人
                                {
                                    x2 = takyurableMem[0];
                                    y2 = takyurableMem[1];
                                    z2 = takyurableMem[2];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x2);
                                    takyurableMem.Remove(y2);
                                    takyurableMem.Remove(z2);
                                    memName2.Remove(x2);
                                    memName2.Remove(y2);
                                    memName2.Remove(z2);//その3人を参加者から削除
                                    takyurableMem = new List<string>(memName2);//重複リストをリセット
                                }
                                else if (takyurableMem.Count == 2)//重複度が低い人が二人
                                {
                                    x2 = takyurableMem[0];
                                    y2 = takyurableMem[1];//この時点で全員の重複度が等しい
                                    memName2.Remove(x2);
                                    memName2.Remove(y2);
                                    takyurableMem = new List<string>(memName2);//重複リストをリセット

                                    int i2 = r.Next(takyurableMem.Count);//乱数
                                    z2 = takyurableMem[i2];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(z2);

                                    memName2.Remove(z2);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count == 1)//重複度が低い人が一人
                                {
                                    x2 = takyurableMem[0];//この時点で全員の重複度が等しい
                                    memName2.Remove(x2);
                                    takyurableMem = new List<string>(memName2);//重複リストをリセット
                                    m2 = new int[memName2.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m2[i] = i;
                                    }
                                    m2 = m2.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    y2 = takyurableMem[m2[0]];
                                    z2 = takyurableMem[m2[1]];
                                    takyurableMem.Remove(y2);
                                    takyurableMem.Remove(z2);

                                    memName2.Remove(y2);
                                    memName2.Remove(z2);//その3人を参加者から削除
                                }
                                else//0人
                                {
                                    takyurableMem = new List<string>(memName2);//重複リストをリセット
                                    m2 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m2[i] = i;
                                    }
                                    m2 = m2.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x2 = takyurableMem[m2[0]];
                                    y2 = takyurableMem[m2[1]];
                                    z2 = takyurableMem[m2[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x2);
                                    takyurableMem.Remove(y2);
                                    takyurableMem.Remove(z2);
                                    memName2.Remove(x2);
                                    memName2.Remove(y2);
                                    memName2.Remove(z2);//その3人を参加者から削除
                                }

                                if (chohukuTakyu.Contains(x2) || chohukuTakyu.Contains(y2) || chohukuTakyu.Contains(z2)
                                    || (x2 == y2) || (y2 == z2) || (x2 == z2))//takyurableを更新前と更新後で同じ人を選んだらx2==y2になりうる
                                {
                                    choCheck = 0;
                                    break;
                                }
                                chohukuTakyu.Add(x2);
                                chohukuTakyu.Add(y2);
                                chohukuTakyu.Add(z2);

                                /*chohukuTakyu.Add(x2 + "-" + y2 + "-" + z2);
                                chohukuTakyu.Add(x2 + "-" + z2 + "-" + y2);
                                chohukuTakyu.Add(y2 + "-" + x2 + "-" + z2);
                                chohukuTakyu.Add(y2 + "-" + z2 + "-" + x2);
                                chohukuTakyu.Add(z2 + "-" + x2 + "-" + y2);
                                chohukuTakyu.Add(z2 + "-" + y2 + "-" + x2);*/

                                result = result + "多球：" + x2 + "-" + y2 + "-" + z2 + "\n　";


                            }

                        }//参加者が偶数


                        high = memName2.Count / 2;//切り捨て
                        while (memName2.Count > 0)
                        {
                            if (high <= 1)//ver4.0：まず上位半分と下位半分の2集合に分け、それぞれのなかからマッチングしつくす
                            {
                                high = 0;
                            }

                            oneSecondHigh = new int[high];
                            oneSecondLow = new int[memName2.Count - high];
                            int m = 0;
                            int l = 0;
                            foreach (int i in oneSecondHigh)//動的にマッチング範囲を更新
                            {
                                oneSecondHigh[m] = m;
                                m++;
                            }
                            foreach (int i in oneSecondLow)
                            {
                                oneSecondLow[l] = m;
                                m++;
                                l++;
                            }

                            if (high > 1)//上位半分から2人を選ぶ
                            {

                                oneSecondHigh = oneSecondHigh.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName2[oneSecondHigh[0]];
                                string y = memName2[oneSecondHigh[1]];
                                result = result + x + "-" + y + "\n　";
                                memName2.Remove(x);
                                memName2.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);//組を保存
                                    chohuku.Add(y + " " + x);
                                }
                            }
                            else//下位半分から2人を選ぶ
                            {
                                oneSecondLow = oneSecondLow.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName2[oneSecondLow[0]];
                                string y = memName2[oneSecondLow[1]];
                                result = result + x + "-" + y + "\n　";
                                memName2.Remove(x);
                                memName2.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);//組を保存
                                    chohuku.Add(y + " " + x);
                                }
                            }
                            high = high - 2;
                        }

                        result = result.TrimEnd() + "\n";

                        roopStopper++;

                    }
                    //MessageBox.Show("2komameend");
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////3コマ目//////////////////////////////
                    result = result + "3コマ目　\n　";
                    List<int> memNum3 = new List<int>();//コマ練に参加するメンバーのランク
                    List<string> memName3 = new List<string>();//コマ練に参加するメンバーのランク昇順の名前
                    List<string> notMemName3 = new List<string>();//ランクを持たない参加者の名前
                    //MessageBox.Show("3コマ目まできてる？？？");
                    foreach (string per in participants3)
                    {
                        if (per == "")
                        {
                            //何もしない
                        }
                        else if (members.IndexOf(per) != -1)
                        {
                            memNum3.Add(members.IndexOf(per));
                        }
                        else//外部の参加者
                        {
                            notMemName3.Add(per);
                        }

                    }//memNum...参加者のランクのリスト、notMemName...外部の参加者の名前
                    memNum3.Sort();//参加者のランクが昇順に
                    foreach (int rank in memNum3)
                    {
                        memName3.Add(members[rank]);//memNum1をランクから名前に変換      
                    }
                    foreach (string not in notMemName3)
                    {
                        if (memName3.Count % 2 == 1)//参加者が奇数
                        {
                            if (not[0] == '↑')
                            {
                                memName3.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName3.Count == 0)
                                {
                                    memName3.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName3.Insert(memName3.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName3.Insert((memName3.Count + 1) / 2, not);
                            }
                        }
                        else//偶数
                        {
                            if (not[0] == '↑')
                            {
                                memName3.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName3.Count == 0)
                                {
                                    memName3.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName3.Insert(memName3.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName3.Insert(memName3.Count / 2, not);
                            }
                        }
                    }//ランクの真ん中に外部の参加者を挿入
                    seed = Environment.TickCount;
                    r = new System.Random(seed);
                    high = memName3.Count / 2;//切り捨て
                    oneSecondHigh = new int[high];
                    oneSecondLow = new int[memName3.Count - high];
                    oneSecondMiddle = new int[2 * (high / 2)];//偶数化

                    ///重複チェック用パラメータ///
                    string x3 = "", y3 = "", z3 = "";//2コマ目多球参加者
                    chohuku_pre = new List<string>(chohuku);//1コマ目の組を保存(重複した際に、chohuku_preからやり直す)
                    List<string> chohukuT_pre = new List<string>(chohukuTakyu);
                    memName_pre = new List<string>(memName3);
                    result_pre = result;
                    List<string> chohukuTakyuSec = new List<string>();//既に二回多球に参加している人のリスト
                    choCheck = 0;//重複していると0
                    foreach (string i in participant2_3inc)
                    {
                        takyurableMem.Add(i);
                    }
                    foreach (string i in participant2_3dec)
                    {
                        takyurableMem.Remove(i);
                    }
                    takyurableMem_pre = new List<string>(takyurableMem);
                    ///重複チェック用パラメータ///
                    while (choCheck == 0)//重複が確認される限り0
                    {
                        //break;

                        if (roopStopper > 10000)//無限ループ中
                        {
                            break;
                        }
                        choCheck = 1;
                        chohuku = new List<string>(chohuku_pre);//1コマ目生成直後時点の保存されていたリストに戻り再度生成
                        chohukuTakyuSec = new List<string>();
                        memName3 = new List<string>(memName_pre);
                        result = result_pre;

                        takyurableMem = new List<string>(takyurableMem_pre);
                        chohukuTakyu = new List<string>();
                        if (memName3.Count % 2 == 1 || memName3.Count - table * 2 > 0)//参加者が奇数又は台の制約により全員対人できない->多球
                        {
                            //memName1 = memName_pre;
                            int tableConstraint = 0;//参加者-使用可能台数x2＝多球の数
                            int constraint = memName3.Count - table * 2;
                            if (constraint < 0)//単純に奇数で一台多球のとき
                            {
                                tableConstraint = constraint - 1;
                            }
                            //MessageBox.Show(memName3.Count.ToString());
                            while (tableConstraint < constraint)
                            {
                                //MessageBox.Show("3きてる？");
                                //MessageBox.Show(tableConstraint.ToString() + constraint.ToString());
                                tableConstraint++;
                                //多球を完全ランダムに構成
                                int[] m3 = new int[] { 0 };//初期化
                                r = new System.Random(seed++);
                                if (takyurableMem.Count >= 4)//重複度が低い人が4人以上
                                {
                                    m3 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m3[i] = i;
                                    }
                                    m3 = m3.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x3 = takyurableMem[m3[0]];
                                    y3 = takyurableMem[m3[1]];
                                    z3 = takyurableMem[m3[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x3);
                                    takyurableMem.Remove(y3);
                                    takyurableMem.Remove(z3);
                                    memName3.Remove(x3);
                                    memName3.Remove(y3);
                                    memName3.Remove(z3);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count >= 3)//重複度が低い人が3人
                                {
                                    x3 = takyurableMem[0];
                                    y3 = takyurableMem[1];
                                    z3 = takyurableMem[2];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x3);
                                    takyurableMem.Remove(y3);
                                    takyurableMem.Remove(z3);
                                    memName3.Remove(x3);
                                    memName3.Remove(y3);
                                    memName3.Remove(z3);//その3人を参加者から削除
                                    takyurableMem = new List<string>(memName3);//重複リストをリセット
                                }
                                else if (takyurableMem.Count == 2)//重複度が低い人が二人
                                {
                                    x3 = takyurableMem[0];
                                    y3 = takyurableMem[1];//この時点で全員の重複度が等しい
                                    memName3.Remove(x3);
                                    memName3.Remove(y3);
                                    takyurableMem = new List<string>(memName3);//重複リストをリセット

                                    int i3 = r.Next(takyurableMem.Count);//乱数
                                    z3 = takyurableMem[i3];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(z3);

                                    memName3.Remove(z3);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count == 1)//重複度が低い人が一人
                                {
                                    x3 = takyurableMem[0];//この時点で全員の重複度が等しい
                                    memName3.Remove(x3);
                                    takyurableMem = new List<string>(memName3);//重複リストをリセット
                                    m3 = new int[memName3.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m3[i] = i;
                                    }
                                    m3 = m3.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    y3 = takyurableMem[m3[0]];
                                    z3 = takyurableMem[m3[1]];
                                    takyurableMem.Remove(y3);
                                    takyurableMem.Remove(z3);

                                    memName3.Remove(y3);
                                    memName3.Remove(z3);//その3人を参加者から削除
                                }
                                else//0人
                                {
                                    takyurableMem = new List<string>(memName3);//重複リストをリセット
                                    m3 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m3[i] = i;
                                    }
                                    m3 = m3.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x3 = takyurableMem[m3[0]];
                                    y3 = takyurableMem[m3[1]];
                                    z3 = takyurableMem[m3[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x3);
                                    takyurableMem.Remove(y3);
                                    takyurableMem.Remove(z3);
                                    memName3.Remove(x3);
                                    memName3.Remove(y3);
                                    memName3.Remove(z3);//その3人を参加者から削除
                                }

                                if (chohukuTakyu.Contains(x3) || chohukuTakyu.Contains(y3) || chohukuTakyu.Contains(z3)
                                    || (x3 == y3) || (y3 == z3) || (x3 == z3))//takyurableを更新前と更新後で同じ人を選んだらx2==y2になりうる
                                {
                                    choCheck = 0;
                                    break;
                                }
                                chohukuTakyu.Add(x3);
                                chohukuTakyu.Add(y3);
                                chohukuTakyu.Add(z3);

                                /*chohukuTakyu.Add(x2 + "-" + y2 + "-" + z2);
                                chohukuTakyu.Add(x2 + "-" + z2 + "-" + y2);
                                chohukuTakyu.Add(y2 + "-" + x2 + "-" + z2);
                                chohukuTakyu.Add(y2 + "-" + z2 + "-" + x2);
                                chohukuTakyu.Add(z2 + "-" + x2 + "-" + y2);
                                chohukuTakyu.Add(z2 + "-" + y2 + "-" + x2);*/

                                result = result + "多球：" + x3 + "-" + y3 + "-" + z3 + "\n　";


                            }

                        }//参加者が偶数
                        while (memName3.Count > 10)//残り10人になるまで
                        {
                            high = memName3.Count / 2;//切り捨て
                            oneSecondMiddle = new int[2 * (high / 2)];
                            int n = 1;
                            int ex = 0;
                            foreach (int i in oneSecondMiddle)
                            {
                                oneSecondMiddle[ex] = n * (int)Math.Pow(-1, ex);
                                ex++;
                                if ((int)Math.Pow(-1, ex) < 0)
                                {
                                    n++;
                                }
                            }
                            r = new System.Random(seed++);
                            int h1 = r.Next(2);//h1は0～1の乱数
                            if (h1 == 0)//最上位と、真ん中＋－の範囲から一人選ぶ
                            {
                                oneSecondMiddle = oneSecondMiddle.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName3[0];
                                string y = memName3[(memName3.Count / 2) - oneSecondMiddle[0]];
                                result = result + x + "-" + y + "\n　";
                                memName3.Remove(x);
                                memName3.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);//組を保存
                                    chohuku.Add(y + " " + x);
                                }
                            }
                            else//最下位と、真ん中＋－の範囲から一人選ぶ
                            {
                                oneSecondMiddle = oneSecondMiddle.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName3[(memName3.Count / 2) - oneSecondMiddle[0]];
                                string y = memName3[memName3.Count - 1];
                                result = result + x + "-" + y + "\n　";
                                memName3.Remove(x);
                                memName3.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);//組を保存
                                    chohuku.Add(y + " " + x);
                                }
                            }
                        }
                        while (memName3.Count > 4)//残り4人になるまで
                        {
                            int[] three2 = new int[] { -1, 0, 1 };
                            r = new System.Random(seed++);
                            int h1 = r.Next(2);//h1は0～1の乱数
                            if (h1 == 0)//最上位と、真ん中+-1の範囲から一人選ぶ
                            {
                                three2 = three2.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName3[0];
                                string y = memName3[(memName3.Count / 2) - three2[0]];
                                result = result + x + "-" + y + "\n　";
                                memName3.Remove(x);
                                memName3.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);//組を保存
                                    chohuku.Add(y + " " + x);
                                }
                            }
                            else//最下位と、真ん中+-1の範囲から1人選ぶ
                            {
                                three2 = three2.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName3[(memName3.Count / 2) - 1 - three2[0]];
                                string y = memName3[memName3.Count - 1];
                                result = result + x + "-" + y + "\n　";
                                memName3.Remove(x);
                                memName3.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);
                                    chohuku.Add(y + " " + x);
                                }
                            }
                        }
                        if (memName3.Count == 4)//残り4人
                        {
                            if (chohuku.Contains(memName3[0] + " " + memName3[1]) || chohuku.Contains(memName3[2] + " " + memName3[3]))//この組み合わせが既にあった場合
                            {
                                if (chohuku.Contains(memName3[0] + " " + memName3[2]) || chohuku.Contains(memName3[1] + " " + memName3[3]))
                                {
                                    if (chohuku.Contains(memName3[0] + " " + memName3[3]) || chohuku.Contains(memName3[1] + " " + memName3[2]))
                                    {
                                        choCheck = 0;
                                    }
                                    else
                                    {
                                        result = result + memName3[0] + "-" + memName3[3] + "\n　";
                                        result = result + memName3[1] + "-" + memName3[2] + "\n";
                                        chohuku.Add(memName3[0] + " " + memName3[3]);
                                        chohuku.Add(memName3[1] + " " + memName3[2]);
                                        chohuku.Add(memName3[3] + " " + memName3[0]);
                                        chohuku.Add(memName3[2] + " " + memName3[1]);
                                    }
                                }
                                else
                                {
                                    result = result + memName3[0] + "-" + memName3[2] + "\n　";
                                    result = result + memName3[1] + "-" + memName3[3] + "\n";
                                    chohuku.Add(memName3[0] + " " + memName3[2]);
                                    chohuku.Add(memName3[1] + " " + memName3[3]);
                                    chohuku.Add(memName3[2] + " " + memName3[0]);
                                    chohuku.Add(memName3[3] + " " + memName3[1]);
                                }
                            }
                            else
                            {
                                result = result + memName3[0] + "-" + memName3[1] + "\n　";
                                result = result + memName3[2] + "-" + memName3[3] + "\n";
                                chohuku.Add(memName3[0] + " " + memName3[1]);
                                chohuku.Add(memName3[2] + " " + memName3[3]);
                                chohuku.Add(memName3[1] + " " + memName3[0]);
                                chohuku.Add(memName3[3] + " " + memName3[2]);
                            }
                        }
                        else if (memName3.Count == 2)//残り二人
                        {
                            if (chohuku.Contains(memName3[0] + " " + memName3[1]))
                            {
                                choCheck = 0;
                            }
                            else
                            {
                                result = result + memName3[0] + "-" + memName3[1] + "\n　";
                                chohuku.Add(memName3[0] + " " + memName3[1]);
                                chohuku.Add(memName3[1] + " " + memName3[0]);
                            }
                        }
                        else//0人(全員多球)
                        {

                        }
                        roopStopper++;

                    }
                    //MessageBox.Show("3komameend");
                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////4コマ目//////////////////////////
                    //MessageBox.Show("４コマ目まできてる？？？");
                    result = result + "4コマ目　\n　";
                    List<int> memNum4 = new List<int>();//コマ練に参加するメンバーのランク
                    List<string> memName4 = new List<string>();//コマ練に参加するメンバーのランク昇順の名前
                    List<string> notMemName4 = new List<string>();//ランクを持たない参加者の名前

                    foreach (string per in participants4)
                    {
                        if (per == "")
                        {
                            //何もしない
                        }
                        else if (members.IndexOf(per) != -1)
                        {
                            memNum4.Add(members.IndexOf(per));
                        }
                        else//外部の参加者
                        {
                            notMemName4.Add(per);
                        }

                    }//memNum...参加者のランクのリスト、notMemName...外部の参加者の名前
                    memNum4.Sort();//参加者のランクが昇順に
                    foreach (int rank in memNum4)
                    {
                        memName4.Add(members[rank]);//memNum1をランクから名前に変換

                    }
                    foreach (string not in notMemName4)
                    {
                        if (memName4.Count % 2 == 1)//参加者が奇数
                        {
                            if (not[0] == '↑')
                            {
                                memName4.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName4.Count == 0)
                                {
                                    memName4.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName4.Insert(memName4.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName4.Insert((memName4.Count + 1) / 2, not);
                            }
                        }
                        else//偶数
                        {
                            if (not[0] == '↑')
                            {
                                memName4.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName4.Count == 0)
                                {
                                    memName4.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName4.Insert(memName4.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName4.Insert(memName4.Count / 2, not);
                            }
                        }
                    }//ランクの真ん中に外部の参加者を挿入
                    seed = Environment.TickCount;
                    r = new System.Random(seed);
                    high = memName4.Count / 2;//切り捨て
                    oneSecondHigh = new int[high];
                    oneSecondLow = new int[memName4.Count - high];
                    oneSecondMiddle = new int[2 * (high / 2)];//偶数化
                    ///重複チェック用パラメータ///
                    string x4 = "", y4 = "", z4 = "";//2コマ目多球参加者
                    chohuku_pre = new List<string>(chohuku);//1コマ目の組を保存(重複した際に、chohuku_preからやり直す)
                    chohukuT_pre = new List<string>(chohukuTakyu);
                    List<string> chohukuTS_pre = new List<string>(chohukuTakyuSec);
                    memName_pre = new List<string>(memName4);
                    result_pre = result;
                    choCheck = 0;//重複していると0
                    foreach (string i in participant3_4inc)
                    {
                        takyurableMem.Add(i);
                    }
                    foreach (string i in participant3_4dec)
                    {
                        takyurableMem.Remove(i);
                    }
                    takyurableMem_pre = new List<string>(takyurableMem);
                    ///重複チェック用パラメータ///
                    while (choCheck == 0)//重複が確認される限り0
                    {
                        if (roopStopper > 10000)//無限ループ中
                        {
                            break;
                        }
                        choCheck = 1;
                        chohuku = new List<string>(chohuku_pre);//1コマ目生成直後時点の保存されていたリストに戻り再度生成
                        chohukuTakyuSec = new List<string>(chohukuTS_pre);
                        memName4 = new List<string>(memName_pre);
                        result = result_pre;
                        chohukuTakyu = new List<string>();
                        takyurableMem = new List<string>(takyurableMem_pre);
                        if (memName4.Count % 2 == 1 || memName4.Count - table * 2 > 0)//参加者が奇数又は台の制約により全員対人できない->多球
                        {
                            //memName1 = memName_pre;
                            int tableConstraint = 0;//参加者-使用可能台数x2＝多球の数
                            int constraint = memName4.Count - table * 2;
                            if (constraint < 0)//単純に奇数で一台多球のとき
                            {
                                tableConstraint = constraint - 1;
                            }
                            //MessageBox.Show(memName4.Count.ToString());
                            while (tableConstraint < constraint)
                            {
                                //MessageBox.Show("4きてる？");
                                //MessageBox.Show(tableConstraint.ToString() + constraint.ToString());
                                tableConstraint++;
                                //多球を完全ランダムに構成
                                int[] m4 = new int[] { 0 };//初期化
                                r = new System.Random(seed++);
                                if (takyurableMem.Count >= 3)//重複度が低い人が4人以上
                                {
                                    m4 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m4[i] = i;
                                    }
                                    m4 = m4.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x4 = takyurableMem[m4[0]];
                                    y4 = takyurableMem[m4[1]];
                                    z4 = takyurableMem[m4[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x4);
                                    takyurableMem.Remove(y4);
                                    takyurableMem.Remove(z4);
                                    memName4.Remove(x4);
                                    memName4.Remove(y4);
                                    memName4.Remove(z4);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count >= 3)//重複度が低い人が3人
                                {
                                    x4 = takyurableMem[0];
                                    y4 = takyurableMem[1];
                                    z4 = takyurableMem[2];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x4);
                                    takyurableMem.Remove(y4);
                                    takyurableMem.Remove(z4);
                                    memName4.Remove(x4);
                                    memName4.Remove(y4);
                                    memName4.Remove(z4);//その3人を参加者から削除
                                    takyurableMem = new List<string>(memName4);//重複リストをリセット
                                }
                                else if (takyurableMem.Count == 2)//重複度が低い人が二人
                                {
                                    x4 = takyurableMem[0];
                                    y4 = takyurableMem[1];//この時点で全員の重複度が等しい
                                    memName4.Remove(x4);
                                    memName4.Remove(y4);
                                    takyurableMem = new List<string>(memName4);//重複リストをリセット

                                    int i4 = r.Next(takyurableMem.Count);//乱数
                                    z4 = takyurableMem[i4];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(z4);

                                    memName4.Remove(z4);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count == 1)//重複度が低い人が一人
                                {
                                    x4 = takyurableMem[0];//この時点で全員の重複度が等しい
                                    memName4.Remove(x4);
                                    takyurableMem = new List<string>(memName4);//重複リストをリセット
                                    m4 = new int[memName4.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m4[i] = i;
                                    }
                                    m4 = m4.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    y4 = takyurableMem[m4[0]];
                                    z4 = takyurableMem[m4[1]];
                                    takyurableMem.Remove(y4);
                                    takyurableMem.Remove(z4);

                                    memName4.Remove(y4);
                                    memName4.Remove(z4);//その3人を参加者から削除
                                }
                                else//0人
                                {
                                    takyurableMem = new List<string>(memName4);//重複リストをリセット
                                    m4 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m4[i] = i;
                                    }
                                    m4 = m4.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x4 = takyurableMem[m4[0]];
                                    y4 = takyurableMem[m4[1]];
                                    z4 = takyurableMem[m4[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x4);
                                    takyurableMem.Remove(y4);
                                    takyurableMem.Remove(z4);
                                    memName4.Remove(x4);
                                    memName4.Remove(y4);
                                    memName4.Remove(z4);//その3人を参加者から削除
                                }

                                if (chohukuTakyu.Contains(x4) || chohukuTakyu.Contains(y4) || chohukuTakyu.Contains(z4)
                                    || (x4 == y4) || (y4 == z4) || (x4 == z4))//takyurableを更新前と更新後で同じ人を選んだらx2==y2になりうる
                                {
                                    choCheck = 0;
                                    break;
                                }
                                chohukuTakyu.Add(x4);
                                chohukuTakyu.Add(y4);
                                chohukuTakyu.Add(z4);

                                /*chohukuTakyu.Add(x2 + "-" + y2 + "-" + z2);
                                chohukuTakyu.Add(x2 + "-" + z2 + "-" + y2);
                                chohukuTakyu.Add(y2 + "-" + x2 + "-" + z2);
                                chohukuTakyu.Add(y2 + "-" + z2 + "-" + x2);
                                chohukuTakyu.Add(z2 + "-" + x2 + "-" + y2);
                                chohukuTakyu.Add(z2 + "-" + y2 + "-" + x2);*/

                                result = result + "多球：" + x4 + "-" + y4 + "-" + z4 + "\n　";


                            }


                        }//参加者が偶数

                        high = memName4.Count / 2;//切り捨て
                        while (memName4.Count > 0)
                        {
                            if (high <= 1)//ver4.0：まず上位半分と下位半分の2集合に分け、それぞれのなかからマッチングしつくす
                            {
                                high = 0;
                            }

                            oneSecondHigh = new int[high];
                            oneSecondLow = new int[memName4.Count - high];
                            int m = 0;
                            int l = 0;
                            foreach (int i in oneSecondHigh)//動的にマッチング範囲を更新
                            {
                                oneSecondHigh[m] = m;
                                m++;
                            }
                            foreach (int i in oneSecondLow)
                            {
                                oneSecondLow[l] = m;
                                m++;
                                l++;
                            }

                            if (high > 1)//上位半分から2人を選ぶ
                            {

                                oneSecondHigh = oneSecondHigh.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName4[oneSecondHigh[0]];
                                string y = memName4[oneSecondHigh[1]];
                                result = result + x + "-" + y + "\n　";
                                memName4.Remove(x);
                                memName4.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);//組を保存
                                    chohuku.Add(y + " " + x);
                                }
                            }
                            else//下位半分から2人を選ぶ
                            {
                                oneSecondLow = oneSecondLow.OrderBy(i => Guid.NewGuid()).ToArray();
                                string x = memName4[oneSecondLow[0]];
                                string y = memName4[oneSecondLow[1]];
                                result = result + x + "-" + y + "\n　";
                                memName4.Remove(x);
                                memName4.Remove(y);
                                if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    chohuku.Add(x + " " + y);//組を保存
                                    chohuku.Add(y + " " + x);
                                }
                            }
                            high = high - 2;
                        }
                        result = result.TrimEnd() + "\n";
                        /*while (memName4.Count > 10)//残り10人になるまで
                        {
                            high = memName4.Count / 4;//切り捨て
                            int[] oneFourthHigh = new int[high];
                            int[] oneFourthLow = new int[high];
                            int m = 0;
                            int l = 0;
                            int n = memName4.Count - 1;
                            foreach (int i in oneFourthHigh)//動的にマッチング範囲を更新
                            {
                                oneFourthHigh[m] = m;
                                m++;
                            }
                            foreach (int i in oneFourthLow)
                            {
                                oneFourthLow[l] = n;
                                n--;
                                l++;
                            }
                            //上位1/4と下位1/4から2人を選ぶ(最上位とその他)
                            
                            oneFourthHigh = oneFourthHigh.OrderBy(i => Guid.NewGuid()).ToArray();
                            oneFourthLow = oneFourthLow.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName4[oneFourthHigh[0]];
                            string y = memName4[oneFourthLow[0]];
                            result = result + x + "-" + y + "\n　";
                            memName4.Remove(x);
                            memName4.Remove(y);
                            if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                            {
                                choCheck = 0;
                            }
                            else
                            {
                                chohuku.Add(x + " " + y);//組を保存
                                chohuku.Add(y + " " + x);
                            }

                        }
                        while (memName4.Count > 4)//残り4人になるまで
                        {
                            //上位と、下位3人からひとり)
                            int[] threeHigh = new int[] { 0, 1, 2 };
                            int[] threeLow = new int[] { memName4.Count - 1, memName4.Count - 2, memName4.Count - 3 };
                            threeHigh = threeHigh.OrderBy(i => Guid.NewGuid()).ToArray();
                            threeLow = threeLow.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName4[threeHigh[0]];
                            string y = memName4[threeLow[0]];
                            result = result + x + "-" + y + "\n　";
                            memName4.Remove(x);
                            memName4.Remove(y);
                            if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                            {
                                 choCheck = 0;
                            }
                            else
                            {
                                chohuku.Add(x + " " + y);//組を保存
                                chohuku.Add(y + " " + x);
                            }

                        }//残り4人

                        if (chohuku.Contains(memName4[0] + " " + memName4[1]) || chohuku.Contains(memName4[2] + " " + memName4[3]))//この組み合わせが既にあった場合
                        {
                            if (chohuku.Contains(memName4[0] + " " + memName4[2]) || chohuku.Contains(memName4[1] + " " + memName4[3]))
                            {
                                if (chohuku.Contains(memName4[0] + " " + memName4[3]) || chohuku.Contains(memName4[1] + " " + memName4[2]))
                                {
                                    choCheck = 0;
                                }
                                else
                                {
                                    result = result + memName4[0] + "-" + memName4[3] + "\n　";
                                    result = result + memName4[1] + "-" + memName4[2] + "\n";
                                    chohuku.Add(memName4[0] + " " + memName4[3]);
                                    chohuku.Add(memName4[1] + " " + memName4[2]);
                                    chohuku.Add(memName4[3] + " " + memName4[0]);
                                    chohuku.Add(memName4[2] + " " + memName4[1]);
                                }
                            }
                            else
                            {
                                result = result + memName4[0] + "-" + memName4[2] + "\n　";
                                result = result + memName4[1] + "-" + memName4[3] + "\n";
                                chohuku.Add(memName4[0] + " " + memName4[2]);
                                chohuku.Add(memName4[1] + " " + memName4[3]);
                                chohuku.Add(memName4[2] + " " + memName4[0]);
                                chohuku.Add(memName4[3] + " " + memName4[1]);
                            }
                        }
                        else
                        {
                            result = result + memName4[0] + "-" + memName4[1] + "\n　";
                            result = result + memName4[2] + "-" + memName4[3] + "\n";
                            chohuku.Add(memName4[0] + " " + memName4[1]);
                            chohuku.Add(memName4[2] + " " + memName4[3]);
                            chohuku.Add(memName4[1] + " " + memName4[0]);
                            chohuku.Add(memName4[3] + " " + memName4[2]);
                        }*/
                        roopStopper++;
                        //MessageBox.Show("４コマ目さいごまできてる？？？");
                    }
                    //MessageBox.Show("4komameend");
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////5コマ目///////////////////////////////
                    result = result + "5コマ目　\n　";
                    List<int> memNum5 = new List<int>();//コマ練に参加するメンバーのランク
                    List<string> memName5 = new List<string>();//コマ練に参加するメンバーのランク昇順の名前
                    List<string> notMemName5 = new List<string>();//ランクを持たない参加者の名前

                    foreach (string per in participants5)
                    {
                        if (per == "")
                        {
                            //何もしない
                        }
                        else if (members.IndexOf(per) != -1)
                        {
                            memNum5.Add(members.IndexOf(per));
                        }
                        else//外部の参加者
                        {
                            notMemName5.Add(per);
                        }

                    }//memNum...参加者のランクのリスト、notMemName...外部の参加者の名前
                    memNum5.Sort();//参加者のランクが昇順に
                    foreach (int rank in memNum5)
                    {
                        memName5.Add(members[rank]);//memNum1をランクから名前に変換

                    }
                    foreach (string not in notMemName5)
                    {
                        if (memName5.Count % 2 == 1)//参加者が奇数
                        {
                            if (not[0] == '↑')
                            {
                                memName5.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName5.Count == 0)
                                {
                                    memName5.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName5.Insert(memName5.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName5.Insert((memName5.Count + 1) / 2, not);
                            }
                        }
                        else//偶数
                        {
                            if (not[0] == '↑')
                            {
                                memName5.Insert(0, not.Replace("↑", ""));//上位
                            }
                            else if (not[0] == '↓')
                            {
                                if (memName5.Count == 0)
                                {
                                    memName5.Add(not.Replace("↓", ""));
                                }
                                else
                                {
                                    memName5.Insert(memName5.Count, not.Replace("↓", ""));//下位
                                }
                            }
                            else
                            {
                                memName5.Insert(memName5.Count / 2, not);
                            }
                        }
                    }//ランクの真ん中に外部の参加者を挿入
                     //System.Random r = new System.Random();
                     //int[] five = new int[] { 0, 1, 2, 3, 4 };
                     //int[] three = new int[] { 1, 2, 3 };
                     //int[] middle = new int[] { -1, -2, 1, 2 };
                     ///重複チェック用パラメータ///
                    string x5 = "", y5 = "", z5 = "";//2コマ目多球参加者
                    chohuku_pre = new List<string>(chohuku);//1コマ目の組を保存(重複した際に、chohuku_preからやり直す)
                    chohukuT_pre = new List<string>(chohukuTakyu);
                    chohukuTS_pre = new List<string>(chohukuTakyuSec);
                    memName_pre = new List<string>(memName5);
                    result_pre = result;
                    choCheck = 0;//重複していると0
                    foreach (string i in participant4_5inc)
                    {
                        takyurableMem.Add(i);
                    }
                    foreach (string i in participant4_5dec)
                    {
                        takyurableMem.Remove(i);
                    }
                    takyurableMem_pre = new List<string>(takyurableMem);
                    ///重複チェック用パラメータ///
                    while (choCheck == 0)//重複が確認される限り0
                    {
                        if (roopStopper > 10000)//無限ループ中
                        {
                            break;
                        }
                        choCheck = 1;
                        chohuku = new List<string>(chohuku_pre);//1コマ目生成直後時点の保存されていたリストに戻り再度生成
                        chohukuTakyuSec = new List<string>(chohukuTS_pre);
                        memName5 = new List<string>(memName_pre);
                        result = result_pre;
                        chohukuTakyu = new List<string>();
                        takyurableMem = new List<string>(takyurableMem_pre);
                        if (memName5.Count % 2 == 1 || memName5.Count - table * 2 > 0)//参加者が奇数又は台の制約により全員対人できない->多球
                        {
                            //memName1 = memName_pre;
                            int tableConstraint = 0;//参加者-使用可能台数x2＝多球の数
                            int constraint = memName5.Count - table * 2;
                            if (constraint < 0)//単純に奇数で一台多球のとき
                            {
                                tableConstraint = constraint - 1;
                            }
                            //MessageBox.Show(memName5.Count.ToString());
                            while (tableConstraint < constraint)
                            {
                                //MessageBox.Show("5きてる？");
                                //MessageBox.Show(tableConstraint.ToString() + constraint.ToString());
                                tableConstraint++;
                                //多球を完全ランダムに構成
                                int[] m5 = new int[] { 0 };//初期化
                                r = new System.Random(seed++);
                                if (takyurableMem.Count >= 3)//重複度が低い人が4人以上
                                {
                                    m5 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m5[i] = i;
                                    }
                                    m5 = m5.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x5 = takyurableMem[m5[0]];
                                    y5 = takyurableMem[m5[1]];
                                    z5 = takyurableMem[m5[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x5);
                                    takyurableMem.Remove(y5);
                                    takyurableMem.Remove(z5);
                                    memName5.Remove(x5);
                                    memName5.Remove(y5);
                                    memName5.Remove(z5);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count >= 3)//重複度が低い人が3人
                                {
                                    x5 = takyurableMem[0];
                                    y5 = takyurableMem[1];
                                    z5 = takyurableMem[2];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x5);
                                    takyurableMem.Remove(y5);
                                    takyurableMem.Remove(z5);
                                    memName5.Remove(x5);
                                    memName5.Remove(y5);
                                    memName5.Remove(z5);//その3人を参加者から削除
                                    takyurableMem = new List<string>(memName5);//重複リストをリセット
                                }
                                else if (takyurableMem.Count == 2)//重複度が低い人が二人
                                {
                                    x5 = takyurableMem[0];
                                    y5 = takyurableMem[1];//この時点で全員の重複度が等しい
                                    memName5.Remove(x5);
                                    memName5.Remove(y5);
                                    takyurableMem = new List<string>(memName5);//重複リストをリセット

                                    int i5 = r.Next(takyurableMem.Count);//乱数
                                    z5 = takyurableMem[i5];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(z5);

                                    memName5.Remove(z5);//その3人を参加者から削除
                                }
                                else if (takyurableMem.Count == 1)//重複度が低い人が一人
                                {
                                    x5 = takyurableMem[0];//この時点で全員の重複度が等しい
                                    memName5.Remove(x5);
                                    takyurableMem = new List<string>(memName5);//重複リストをリセット
                                    m5 = new int[memName5.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m5[i] = i;
                                    }
                                    m5 = m5.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    y5 = takyurableMem[m5[0]];
                                    z5 = takyurableMem[m5[1]];
                                    takyurableMem.Remove(y5);
                                    takyurableMem.Remove(z5);

                                    memName5.Remove(y5);
                                    memName5.Remove(z5);//その3人を参加者から削除
                                }
                                else//0人
                                {
                                    takyurableMem = new List<string>(memName5);//重複リストをリセット
                                    m5 = new int[takyurableMem.Count];
                                    for (int i = 0; i < takyurableMem.Count; i++)
                                    {
                                        m5[i] = i;
                                    }
                                    m5 = m5.OrderBy(i => Guid.NewGuid()).ToArray();//参加者をランダムに並べ、上から3つ選ぶ
                                    x5 = takyurableMem[m5[0]];
                                    y5 = takyurableMem[m5[1]];
                                    z5 = takyurableMem[m5[2]];//takyurableから選ぶことで確実に重複度が競合しない
                                    takyurableMem.Remove(x5);
                                    takyurableMem.Remove(y5);
                                    takyurableMem.Remove(z5);
                                    memName5.Remove(x5);
                                    memName5.Remove(y5);
                                    memName5.Remove(z5);//その3人を参加者から削除
                                }

                                if (chohukuTakyu.Contains(x5) || chohukuTakyu.Contains(y5) || chohukuTakyu.Contains(z5)
                                    || (x5 == y5) || (y5 == z5) || (x5 == z5))//takyurableを更新前と更新後で同じ人を選んだらx2==y2になりうる
                                {
                                    choCheck = 0;
                                    break;
                                }
                                chohukuTakyu.Add(x5);
                                chohukuTakyu.Add(y5);
                                chohukuTakyu.Add(z5);

                                /*chohukuTakyu.Add(x2 + "-" + y2 + "-" + z2);
                                chohukuTakyu.Add(x2 + "-" + z2 + "-" + y2);
                                chohukuTakyu.Add(y2 + "-" + x2 + "-" + z2);
                                chohukuTakyu.Add(y2 + "-" + z2 + "-" + x2);
                                chohukuTakyu.Add(z2 + "-" + x2 + "-" + y2);
                                chohukuTakyu.Add(z2 + "-" + y2 + "-" + x2);*/

                                result = result + "多球：" + x5 + "-" + y5 + "-" + z5 + "\n　";


                            }


                        }//参加者が偶数
                        while (memName5.Count > 10)//残り10人になるまで
                        {
                            high = memName5.Count / 4;//切り捨て(1/4)
                            int[] oneFourthHigh = new int[high];
                            int[] oneFourthLow = new int[high];
                            int m = 0;
                            int l = 0;
                            int n = memName5.Count - 1;
                            foreach (int i in oneFourthHigh)//動的にマッチング範囲を更新
                            {
                                oneFourthHigh[m] = m;
                                m++;
                            }
                            foreach (int i in oneFourthLow)
                            {
                                oneFourthLow[l] = n;
                                n--;
                                l++;
                            }
                            //上位1/4と下位1/4から2人を選ぶ(最上位とその他)

                            oneFourthHigh = oneFourthHigh.OrderBy(i => Guid.NewGuid()).ToArray();
                            oneFourthLow = oneFourthLow.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName5[oneFourthHigh[0]];
                            string y = memName5[oneFourthLow[0]];
                            result = result + x + "-" + y + "\n　";
                            memName5.Remove(x);
                            memName5.Remove(y);
                            if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                            {
                                choCheck = 0;
                            }
                            else
                            {
                                chohuku.Add(x + " " + y);//組を保存
                                chohuku.Add(y + " " + x);
                            }

                        }
                        while (memName5.Count > 4)//残り4人になるまで
                        {
                            //上位と、下位3人からひとり)
                            int[] threeHigh = new int[] { 0, 1, 2 };
                            int[] threeLow = new int[] { memName5.Count - 1, memName5.Count - 2, memName5.Count - 3 };
                            threeHigh = threeHigh.OrderBy(i => Guid.NewGuid()).ToArray();
                            threeLow = threeLow.OrderBy(i => Guid.NewGuid()).ToArray();
                            string x = memName5[threeHigh[0]];
                            string y = memName5[threeLow[0]];
                            result = result + x + "-" + y + "\n　";
                            memName5.Remove(x);
                            memName5.Remove(y);
                            if (chohuku.Contains(x + " " + y) == true)//この組み合わせが既にあった場合
                            {
                                choCheck = 0;
                            }
                            else
                            {
                                chohuku.Add(x + " " + y);//組を保存
                                chohuku.Add(y + " " + x);
                            }
                        }
                        if (memName5.Count == 4)//残り4人
                        {

                            if (chohuku.Contains(memName5[0] + " " + memName5[1]) || chohuku.Contains(memName5[2] + " " + memName5[3]))//この組み合わせが既にあった場合
                            {
                                if (chohuku.Contains(memName5[0] + " " + memName5[2]) || chohuku.Contains(memName5[1] + " " + memName5[3]))
                                {
                                    if (chohuku.Contains(memName5[0] + " " + memName5[3]) || chohuku.Contains(memName5[1] + " " + memName5[2]))
                                    {
                                        choCheck = 0;
                                    }
                                    else
                                    {
                                        result = result + memName5[0] + "-" + memName5[3] + "\n　";
                                        result = result + memName5[1] + "-" + memName5[2] + "\n";
                                        chohuku.Add(memName5[0] + " " + memName5[3]);
                                        chohuku.Add(memName5[1] + " " + memName5[2]);
                                        chohuku.Add(memName5[3] + " " + memName5[0]);
                                        chohuku.Add(memName5[2] + " " + memName5[1]);
                                    }
                                }
                                else
                                {
                                    result = result + memName5[0] + "-" + memName5[2] + "\n　";
                                    result = result + memName5[1] + "-" + memName5[3] + "\n";
                                    chohuku.Add(memName5[0] + " " + memName5[2]);
                                    chohuku.Add(memName5[1] + " " + memName5[3]);
                                    chohuku.Add(memName5[2] + " " + memName5[0]);
                                    chohuku.Add(memName5[3] + " " + memName5[1]);
                                }
                            }
                            else
                            {
                                result = result + memName5[0] + "-" + memName5[1] + "\n　";
                                result = result + memName5[2] + "-" + memName5[3] + "\n";
                                chohuku.Add(memName5[0] + " " + memName5[1]);
                                chohuku.Add(memName5[2] + " " + memName5[3]);
                                chohuku.Add(memName5[1] + " " + memName5[0]);
                                chohuku.Add(memName5[3] + " " + memName5[2]);
                            }
                        }
                        else if (memName5.Count == 2)//残り二人
                        {
                            if (chohuku.Contains(memName5[0] + " " + memName5[1]))
                            {
                                choCheck = 0;
                            }
                            else
                            {
                                result = result + memName5[0] + "-" + memName5[1] + "\n　";
                                chohuku.Add(memName5[0] + " " + memName5[1]);
                                chohuku.Add(memName5[1] + " " + memName5[0]);
                            }
                        }
                        else//0人
                        {

                        }
                        roopStopper++;
                        //MessageBox.Show("５コマ目さいごまできてる？？？");

                    }
                    //MessageBox.Show(result.ToString());


                    //result = "1コマ目\n　多球：大橋-大隅-弓場\n　村上-宮武\n　椴木-松原\n　小出-山井\n　河上-池田\n2コマ目\n　多球：村上-宮武-椴木\n　大橋-松原\n　大隅-小出\n　弓場-山井\n　河上-井上";


                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    foreach (string per1 in memName1)//同じ名前が含まれてたら結果を表示しない
                    {
                        int check1 = 0;
                        foreach (string per11 in memName1)
                        {
                            if (per1 == per11)
                            {
                                check1++;
                            }
                        }
                        if (check1 >= 2)
                        {
                            final = 1;
                            break;
                        }
                    }
                    foreach (string per2 in memName2)//同じ名前が含まれてたら結果を表示しない
                    {
                        int check1 = 0;
                        foreach (string per22 in memName2)
                        {
                            if (per2 == per22)
                            {
                                check1++;
                            }
                        }
                        if (check1 >= 2)
                        {
                            final = 1;
                            break;
                        }
                    }
                    foreach (string per3 in memName3)//同じ名前が含まれてたら結果を表示しない
                    {
                        int check1 = 0;
                        foreach (string per33 in memName3)
                        {
                            if (per3 == per33)
                            {
                                check1++;
                            }
                        }
                        if (check1 >= 2)
                        {
                            final = 1;
                            break;
                        }
                    }
                    foreach (string per4 in memName4)//同じ名前が含まれてたら結果を表示しない
                    {
                        int check1 = 0;
                        foreach (string per44 in memName4)
                        {
                            if (per4 == per44)
                            {
                                check1++;
                            }
                        }
                        if (check1 >= 2)
                        {
                            final = 1;
                            break;
                        }
                    }
                    foreach (string per5 in memName5)//同じ名前が含まれてたら結果を表示しない
                    {
                        int check1 = 0;
                        foreach (string per55 in memName5)
                        {
                            if (per5 == per55)
                            {
                                check1++;
                            }
                        }
                        if (check1 >= 2)
                        {
                            final = 1;
                            break;
                        }
                    }

                    if (final == 1)
                    {
                        break;
                    }

                }


                List<string> finalCheck = new List<string>();//resultに万が一名前被りがある場合、もう一度組みなおすように促す
                int finalCheckP = 0;
                string errorPair = "";
                string[] separator = new string[] { "　", "多球", "1コマ目", "2コマ目", "3コマ目", "4コマ目", "5コマ目", "\n" };//対人の組を抽出
                string[] splittedRes = result.Split(separator, StringSplitOptions.None);
                foreach (string resPart in splittedRes)
                {
                    if (resPart == "")
                    {
                    }
                    else
                    {
                        char a = resPart[0];
                        if (a == '：')//多球の組を表すstring
                        {

                        }
                        else
                        {
                            string[] resPart2Pre = resPart.Split('-');
                            string resPart2 = resPart2Pre[1] + "-" + resPart2Pre[0];//対人の組の左右を入れ替えたものを作成
                            if (finalCheck.Contains(resPart) || finalCheck.Contains(resPart2))//重複してる
                            {
                                finalCheckP = 1;
                                errorPair = resPart;
                                break;
                            }
                            finalCheck.Add(resPart);
                            finalCheck.Add(resPart2);
                        }
                    }
                }

                if (final == 0 && finalCheckP == 0)
                {
                    MessageBox.Show(result);
                }
                else if (final == 1)
                {
                    MessageBox.Show("同じ名前の人が複数いるコマよ！");
                    final = 0;
                }
                else if (final == 0 && finalCheckP == 1)
                {
                    MessageBox.Show("###################################################"
                        + "\n" + "不具合で対人(" + errorPair + ")が重複しましたコマ...もう一回組みなおしてコマ！ごめんコマ！\n" +
                        "###################################################\n" + result);
                }

                //Form1 form = new Form1();
                //form.Show();

            }

        }

        private void コマ練くん_Load(object sender, EventArgs e)//ロード時、フォーカスを無難な場所に移動
        {
            this.ActiveControl = this.label18;
        }
        private void コマ練くん_Click(object sender, EventArgs e)//フォームをクリックすると無難なところにフォーカスが移動
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }
        private void label5_Click_1(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label16_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label13_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label19_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.label18.Focus();
            this.label19.Text = sentences[coma];
        }

        /*private void label20_Click(object sender, EventArgs e)
        {
            this.label20.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label21_Click(object sender, EventArgs e)
        {
            this.label21.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label22_Click(object sender, EventArgs e)
        {
            this.label22.Focus();
            this.label19.Text = sentences[coma];
        }

        private void label23_Click(object sender, EventArgs e)
        {
            this.label23.Focus();
            this.label19.Text = sentences[coma];
        }*/

        private void coma_Click(object sender, EventArgs e)//コマ練くんをクリックするとしゃべる
        {
            int coma2 = coma;
            int seed = Environment.TickCount;
            if(sentences.Count > 1)
            {
                while (coma2 == coma)
                {
                    System.Random r = new System.Random(seed++);
                    coma2 = r.Next(sentences.Count);
                }

                coma = coma2;
                this.label19.Text = sentences[coma];
            }
            else
            {

            }
        }
        private void coma_DoubleClick(object sender, EventArgs e)
        {
            int coma2 = coma;
            int seed = Environment.TickCount;
            if (sentences.Count > 1)
            {
                while (coma2 == coma)
                {
                    System.Random r = new System.Random(seed++);
                    coma2 = r.Next(sentences.Count);
                }

                coma = coma2;
                this.label19.Text = sentences[coma];
            }
            else
            {

            }
        }
        int leave = 0;
        private void CreateButton_MouseEnter(object sender, EventArgs e)//コマ練くんの反応
        {
            if (textBox1.Focused || textBox2.Focused || textBox3.Focused || textBox4.Focused || textBox5.Focused || textBox6.Focused)
            {
                leave = 1;
                coma_pre = this.label19.Text;
            }
            else
            {
                coma_pre = sentences[coma];
            }
            this.label19.Text = "組み合わせ生成するコマ？";
        }
        private void CreateButton_MouseLeave(object sender, EventArgs e)//ボタン上にマウスポインタが来た時はフォーカスを得ていないためcoma_preを用いて特別な処理をする
        {
            if (leave == 1)//恐らくボタンをクリックした直後、txtBoxにフォーカスされた状態でリーヴしている(ボタンをクリックしてもtxtBoxのフォーカスは解除されてない)
            {
                this.label19.Text = coma_pre;
                leave = 0;
            }
            else
            {
                this.label19.Text = sentences[coma];
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.label19.Text = "そこは一コマ目の参加者を入力するところコマ";
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.label19.Focus();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            this.label19.Text = "2コマ目に参加者の増減があれば書くコマ～";
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            this.label19.Text = sentences[coma];
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            this.label19.Text = "プラスマイナスは半角、スペースは全角でお願いコマ";
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            this.label19.Text = sentences[coma];
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            this.label19.Text = "休憩後の増減を書くコマ～";
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            this.label19.Text = sentences[coma];
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            this.label19.Text = "最後だコマ\n小生への要望、\n募集中だコマ";
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            this.label19.Text = sentences[coma];
        }

        private void textBox6_Enter(object sender, EventArgs e)//memberリスト
        {
            this.label19.Text = "そこはmembers.txtの内容を表示しているのコマ";
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            this.label19.Text = sentences[coma];
        }

        private void textBoxNumTable_Enter(object sender, EventArgs e)
        {
            this.label19.Text = "今日は何台空いてるコマか？";
        }

        


    }
}
