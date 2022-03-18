using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PageReplaceSimulation
{
    public partial class Simulation : System.Web.UI.Page
    {
        int memoryPage;//最大内存页数，从0-memoryPage测试
        int maxPageSeq;//最大页面序号
        int page;//页面数
        Queue<Object> memory = new Queue<Object>();//内存中的页序列
        int[] visitPage;//要访问的页序列
        double[] faultRate1, faultRate2, faultRate3;//缺页率
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                show.Visible = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            show.Visible = true;
            memoryPage = int.Parse(DropDownList1.SelectedValue);
            maxPageSeq = int.Parse(DropDownList2.SelectedValue);
            page = int.Parse(TextBox1.Text);
            visitPage = getRandomArr();
            faultRate1 = new double[memoryPage-1];
            faultRate2 = new double[memoryPage-1];
            faultRate3 = new double[memoryPage-1];
            FIFO();
            OPT();
            LRU();
            Compare();
        }

        private void Compare()
        {
            Label7.Text = "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border='1' class='compareTable'>" +
                "<tr>" +
                "<th>内存容量</th><th>FIFO</th><th>OPT</th><th>LRU</th><tr>");
            int[] xvalue=new int[memoryPage-1];
            for (int i = 2; i <= memoryPage; i++)
            {
                xvalue[i - 2] = i;
                stringBuilder.Append("<tr>" +
                    "<td>" + i + "</td><td>" +
                   string.Format("{0:0.00%}", faultRate1[i - 2]) + "</td><td>" + string.Format("{0:0.00%}", faultRate2[i - 2]) 
                   + "</td><td>" + string.Format("{0:0.00%}", faultRate3[i - 2]) + "</td></tr>");
            }
            stringBuilder.Append("</table>");
            Label7.Text = stringBuilder.ToString();
            
            Chart1.Series["Series1"].Points.DataBindXY(xvalue,faultRate1);
            Chart1.Series["Series2"].Points.DataBindXY(xvalue, faultRate2);
            Chart1.Series["Series3"].Points.DataBindXY(xvalue, faultRate3);
        }

        public int[] getRandomArr()
        {
            int[] arr = new int[page];
            Random random = new Random();
            for (int i = 0; i < page; i++)
                arr[i] = random.Next(0, maxPageSeq + 1);//0-maxPageSeq之间的随机数
            return arr;
        }
        public void FIFO()
        {
            Label4.Text = "";
            for (int i = 2; i <= memoryPage; i++)
            {
                int faultNum = 0;
                string[,] arr = new string[i + 2, page];//最上面一行是访问页，最下面一行显示命中与否
                memory.Clear();
                for (int k = 0; k < i; k++)
                    memory.Enqueue("");
                for (int j = 0; j < page; j++)
                {
                    arr[0, j] = Convert.ToString(visitPage[j]);
                    if (memory.Contains(arr[0, j]))//命中
                    {
                        Object[] temp = memory.ToArray();
                        for (int k = 1; k <= i; k++)
                        {
                            arr[k, j] = temp[k-1].ToString();
                        }
                        arr[i + 1, j] = "0";//不缺页
                    }
                    else//缺页
                    {
                        faultNum++;
                        memory.Dequeue();//先进先出
                        memory.Enqueue(arr[0, j]);//调入内存
                        int k = 1;
                        foreach (var item in memory)
                        {
                            arr[k++, j] = item.ToString();
                        }
                        arr[i+1, j] = "1";
                    }
                }
                faultRate1[i - 2] =double.Parse(string.Format("{0:0.00}",faultNum * 1.0 / page));
                Label4.Text+=GenerateTable(i,arr);
            }
        }

        private string GenerateTable(int i,string[,] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<h1>内存容量：" + i + "</h1><table border='1'>");
            for (int j = 0; j < i + 2; j++)
            {
                stringBuilder.Append("<tr>");
                for (int k = 0; k < page; k++)
                {
                    if (j == 0)
                        stringBuilder.Append("<th>" + arr[j, k] + "</th>");
                    else if (j != (i + 1))
                        stringBuilder.Append("<td>" + arr[j, k] + "</td>");
                    else if (arr[j, k] == "1")
                        stringBuilder.Append("<td>❌</td>");
                    else
                        stringBuilder.Append("<td>✔</td>");
                }
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        public void OPT()
        {
            Label5.Text = "";
            for (int i = 2; i <= memoryPage; i++)
            {
                int faultNum = 0;
                string[,] arr = new string[i + 2, page];
                memory.Clear();
                for (int k = 0; k < i; k++)
                    memory.Enqueue("");
                for (int j = 0; j < page; j++)
                {
                    arr[0, j] = Convert.ToString(visitPage[j]);
                    if (!memory.Contains(arr[0, j]))//缺页
                    {
                        faultNum++;
                        if (memory.Contains(""))//缺页有空位
                        {
                            memory.Dequeue();
                            memory.Enqueue(arr[0, j]);
                        }
                        else//缺页无空位
                        {
                            //更新memory
                            int index=0;//最远距离的内存页下标
                            int maxdis = 1;//最远距离
                            Array array = memory.ToArray();
                            for (int m1 = 0; m1 < array.Length; m1++)
                            {
                                bool flag = false;
                                for (int m2 = j + 1; m2 < page; m2++)
                                {
                                    if (int.Parse(array.GetValue(m1).ToString()) == visitPage[m2])
                                    {
                                        flag = true;
                                        if (maxdis <= (m2 - j))
                                        {
                                            maxdis = m2 - j;
                                            index = m1;
                                        }
                                        break;
                                    }
                                }
                                if (flag == false)//后续序列未再使用过当前页
                                {
                                    index = m1;
                                    array.SetValue(arr[0, j], index);
                                    break;
                                }
                            }
                            array.SetValue(arr[0, j], index);
                            memory.Clear();
                            for (int m = 0; m < array.Length; m++)
                                memory.Enqueue(array.GetValue(m));
                        }
                        arr[i + 1, j] = "1";
                        int k = 1;
                        foreach (var item in memory)
                            arr[k++, j] = item.ToString();
                    }
                    else
                    {
                        Object[] temp = memory.ToArray();
                        for (int k = 1; k <= i; k++)
                        {
                            arr[k, j] = temp[k - 1].ToString();
                        }
                        arr[i + 1, j] = "0";
                    }
                }
                faultRate2[i - 2] = double.Parse(string.Format("{0:0.00}", faultNum * 1.0 / page));
                Label5.Text += GenerateTable(i,arr);
            }
        }
        public void LRU()
        {
            Label6.Text = "";
            for (int i = 2; i <= memoryPage; i++)
            {
                int faultNum = 0;
                string[,] arr = new string[i + 2, page];
                memory.Clear();
                for (int k = 0; k < i; k++)
                    memory.Enqueue("");
                for (int j = 0; j < page; j++)
                {
                    arr[0, j] = Convert.ToString(visitPage[j]);
                    if (!memory.Contains(arr[0, j]))//缺页
                    {
                        arr[i + 1, j] = "1";
                        faultNum++;
                        if (memory.Contains(""))//缺页有空位
                        {
                            memory.Dequeue();
                            memory.Enqueue(arr[0, j]);
                        }
                        else//缺页无空位
                        {
                            //更新memory
                            int index = 0;//最远距离的内存页下标
                            int maxdis = 1;//最远距离
                            Array array = memory.ToArray();
                            for (int m1 = 0; m1 < array.Length; m1++)
                            {
                                for (int m2 = j - 1; m2 >=0; m2--)
                                {
                                    if (int.Parse(array.GetValue(m1).ToString()) == visitPage[m2])
                                    {
                                        if (maxdis <= (j-m2))
                                        {
                                            maxdis = j-m2;
                                            index = m1;
                                        }
                                        break;
                                    }
                                }
                            }
                            array.SetValue(arr[0, j], index);
                            memory.Clear();
                            for (int m = 0; m < array.Length; m++)
                                memory.Enqueue(array.GetValue(m));
                        }
                        int k = 1;
                        foreach (var item in memory)
                            arr[k++, j] = item.ToString();
                    }
                    else
                    {
                        Object[] temp = memory.ToArray();
                        for (int k = 1; k <= i; k++)
                        {
                            arr[k, j] = temp[k - 1].ToString();
                        }
                        arr[i + 1, j] = "0";
                    }
                }
                faultRate3[i - 2] = double.Parse(string.Format("{0:0.00}", faultNum * 1.0 / page));
                Label6.Text += GenerateTable(i,arr);
            }
        }
    }
}