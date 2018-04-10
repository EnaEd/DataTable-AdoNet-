using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace TestDataTable
{
    public partial class Form1 : Form
    {
        SqlConnection conn = null;
        DataTable table;

        public Form1()
        {
            InitializeComponent();
            //using (conn=new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString)) {
            //    conn.Open();
            //    //DataTable dt = new DataTable();//создаем обьект класса
            //    //dt.Columns.Add("Id");//создаем колонки и даем им имена
            //    //dt.Columns.Add("FirstName");
            //    //dt.Columns.Add("LastName");
            //    //DataRow dr = dt.NewRow();//метод newRow создает строку по колонкам таблицы от которй вызван
            //    //dr[0] = 1;//строка как массив и добавляем по индексу
            //    //dr[1] = "Френсис";
            //    //dr[2] = "Беккон";
            //    //dt.Rows.Add(dr);//вставляем в таблицу

            //}
        }
        //используем присоединенный или автономный режим
        private void btnExec_Click(object sender, EventArgs e)
        {
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString)) {//строка подключения из конфиг файла
                SqlDataReader rdr = null;//обьект который принимает результат запроса
                conn.Open();
                using (SqlCommand cmd=new SqlCommand(tBQuery.Text,conn)) {//команда с запросом
                     rdr = cmd.ExecuteReader();//выполняем запрос и записываем результат
                    int line = 0;//счетчик для подписи шапки таблицы
                    table = new DataTable(); //создаем обьект таблицы
                    do//цикл для обработки нескольких запросов
                    {
                        while (rdr.Read()) {//цикл обработки запроса
                            if (line == 0) {//формируем шапку
                                for (int i = 0; i < rdr.FieldCount; i++)
                                {
                                    table.Columns.Add(rdr.GetName(i));//получаем название колонок
                                }
                                line++;
                            }
                            DataRow dRow = table.NewRow();//создаем обьект для строк
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                dRow[i] = rdr[i];//записываем строки в таблицу
                            }
                            table.Rows.Add(dRow);//добавляем 
                        }
                    } while (rdr.NextResult());
                    dataGridView1.DataSource = table;//выводим в dataGridView данные из таблицы
                }
            }
        }
    }
}
