using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matematiko
{
    public partial class Form1 : Form
    {
        bool start = false;
        List<int> nums = new List<int>();
        int maxn = 51;
        int curr, step, next, currptsP, currptsB;
        bool firststep = true;
        Random rnd = new Random();
        int newbotpts = 0;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Width = 350;
            dataGridView1.Height = 350;
            dataGridView2.Width = 350;
            dataGridView2.Height = 350;
            dataGridView1.RowCount = 5;
            dataGridView1.ColumnCount = 5;
            dataGridView2.RowCount = 5;
            dataGridView2.ColumnCount = 5;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Height = 70;
                dataGridView1.Columns[i].Width = 70;
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
            }
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2.Rows[i].Height = 70;
                dataGridView2.Columns[i].Width = 70;
                dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                dataGridView2.Rows[i].DefaultCellStyle.ForeColor = Color.White;
            }
            for(int i = 1; i < 14; i++) // Заполнение списка колоды. 
            {
                for(int j = 0; j < 4; j++)
                {
                    nums.Add(i);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) // Кнопка начала игры.
        {
            button1.Visible = false;
            start = true;
            step = rnd.Next(0, maxn + 1);
            next = nums[step];
            nums.RemoveAt(step);
            maxn--;
            textBox3.Text = next.ToString();
        }

        private void Form1_Load(object sender, EventArgs e) // Снять выделение при каждом нажатии на форму
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }

        public void RobStep() // Алгоритм бота.
        {
            int botpts = Check(dataGridView2);
            int X = 0, Y = 0, botold = newbotpts;

            if(firststep)
            {
                dataGridView2[0, 0].Value = curr;
                return;
            }

            for(int i = 0; i < dataGridView2.RowCount; i++)
            {
                for(int j = 0; j < dataGridView2.ColumnCount; j++)
                {
                    if(dataGridView2[i,j].Value == null)
                    {
                        dataGridView2[i, j].Value = curr;
                        newbotpts = Check(dataGridView2);
                        if(newbotpts > botpts)
                        {
                            botpts = newbotpts;
                            X = i;
                            Y = j;
                        }
                        dataGridView2[i, j].Value = null;
                    }
                }
            }

            if (botold == newbotpts)
            {
                randomstep(ref X, ref Y);
                while (dataGridView2[X, Y].Value != null)
                {
                    randomstep(ref X, ref Y);
                }
            }

            if (dataGridView2[X, Y].Value != null)
            {
                RobStep();
            }
            else
            {
                dataGridView2[X, Y].Value = curr;
            }
            
        }

        public void randomstep(ref int a, ref int b) // Выбор случайной клетки на поле.
        {
            a = rnd.Next(0, 5);
            b = rnd.Next(0, 5);
        }

        public bool final() // Проверка на заполненность полей.
        {
            for(int i = 0; i < dataGridView1.RowCount; i++)
            {
                for(int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if(dataGridView1[i,j].Value == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void restart() // Пересоздание игры.
                              // Установка начальных значений для переменных и полей.
        {
            nums = new List<int>();
            firststep = true;
            for (int i = 1; i <= 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    nums.Add(i);
                }
            }
            start = true;
            maxn = 51;
            step = rnd.Next(1, maxn);
            next = nums[step];
            nums.RemoveAt(step);
            maxn--;
            textBox3.Text = next.ToString();
            textBox1.Text = null;
            textBox2.Text = null;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    dataGridView1[i, j].Value = null;
                    dataGridView2[i, j].Value = null;
                }
            }
        }

        public int resreturn(int[] a, bool b) // Подсчёт очков в строке, столбце или диагонали.
        {
            int res = 0;

            if((a[0] == 3) && ( a[12] == 2))
            {
                if (b)
                {
                    res += 110;
                }
                else
                {
                    res += 100;
                }
            }

            if ((a[0] != 0) && (a[12] != 0) && (a[11] != 0) && (a[10] != 0) && (a[9] != 0))
            {
                if (b)
                {
                    res += 160;
                }
                else
                {
                    res += 150;
                }
            }

            if (a[0] == 4)
            {
                if(b)
                {
                    res += 210;
                }
                else
                {
                    res += 200;
                }
            }

            for(int i = 0; i < 13; i++)
            {
                if(a[i] == 2)
                {
                    if (b)
                    {
                        res += 20;
                    }
                    else
                    {
                        res += 10;
                    }
                    for(int k = 0; k < 13; k++)
                    {
                        if(k != i)
                        {
                            if(a[k] == 2)
                            {
                                if (b)
                                {
                                    res += 30;
                                }
                                else
                                {
                                    res += 20;
                                }
                            }
                            if(a[k] == 3)
                            {
                                if (b)
                                {
                                    res += 90;
                                }
                                else
                                {
                                    res += 80;
                                }
                            }
                        }
                    }
                }
                if(a[i] == 3)
                {
                    if (b)
                    {
                        res += 50;
                    }
                    else
                    {
                        res += 40;
                    }
                }
                if (a[i] == 4)
                {
                    if (b)
                    {
                        res += 170;
                    }
                    else
                    {
                        res += 160;
                    }
                }
            }

            for(int i = 0; i < 9; i ++)
            {
                if(a[i] * a[i+1] * a[i+2] * a[i+3] * a[i+4] != 0)
                {
                    if (b)
                    {
                        res += 60;
                    }
                    else
                    {
                        res += 50;
                    }
                }
            }

            return res;
        }

        private void button2_Click(object sender, EventArgs e) // Кнопка рестарта.
        {
            restart();
        }

        public int Check(DataGridView a) // Создание массива с числами от 1 до 13, в котором в каждом элементе
                                         // содержится количество того или иного числа в строке, столбце или диагонали,
                                         // а затем вызов метода подсчёта очков.
        {
            int[] num = new int[13];
            int ptssum = 0;
            int finalres = 0;
            for(int i = 0; i < a.RowCount; i++)
            {
                for(int j = 0; j < a.ColumnCount; j++)
                {
                    if(a[i, j].Value != null)
                    {
                        num[int.Parse(a[i, j].Value.ToString()) - 1]++;
                    }
                }
                ptssum = resreturn(num, false);
                finalres += ptssum;
                num = new int[13];
            }
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    if (a[j, i].Value != null)
                    {
                        num[int.Parse(a[j, i].Value.ToString()) - 1]++;
                    }
                }
                ptssum = resreturn(num, false);
                finalres += ptssum;
                num = new int[13];
            }
            for(int i = 0; i < a.RowCount; i++)
            {
                if (a[i, i].Value != null)
                {
                    num[int.Parse(a[i, i].Value.ToString()) - 1]++;
                }
            }
            ptssum = resreturn(num, true);
            finalres += ptssum;
            return finalres;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) // При каждом нажатии на какую-то ячейку поля
                                                                                                   // на него заносится текущее число колоды, производится ход бота и в
                                                                                                   // textbox'ы выводится количество очков двух полей.
        {
            step = rnd.Next(1, maxn);
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            if (start == true)
            {
                if(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    curr = int.Parse(textBox3.Text);
                    next = nums[step];
                    nums.RemoveAt(step);
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = curr;
                    maxn--;
                    RobStep();
                    maxn--;
                    currptsP = Check(dataGridView1);
                    currptsB = Check(dataGridView2);
                    textBox1.Text = currptsP.ToString();
                    textBox2.Text = currptsB.ToString();
                    firststep = false;
                    if (final())
                    {
                        textBox3.Text = "";
                        int p1 = Check(dataGridView1);
                        int p2 = Check(dataGridView2);
                        textBox1.Text = p1.ToString();
                        textBox2.Text = p2.ToString();
                        if (p1 > p2)
                        {
                            MessageBox.Show("Победил игрок!");
                            restart();
                        }
                        else if (p1 < p2)
                        {
                            MessageBox.Show("Победил компьютер!");
                            restart();
                        }
                        else
                        {
                            MessageBox.Show("Ничья!");
                            restart();
                        }
                    }
                    else
                    {
                        textBox3.Text = next.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Ячейка занята!");
                }
            }
        }
    }
}
