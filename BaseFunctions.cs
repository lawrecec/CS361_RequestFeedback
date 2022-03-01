using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace BookTracker
{
    class BaseFunctions
    {
        //gets the image location from teammate's microservice based on isbn
        public string getImageLocation(string isbn)
        {
            string imageLocation = "";
            if (!string.IsNullOrEmpty(isbn))
            {
               
                string url = "http://127.0.0.1:3000/thumbnail/" + isbn;

                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();

                var reader = new StreamReader(webStream);
                imageLocation = reader.ReadToEnd();
                
            }
            return imageLocation;
        }

        public bool deleteBookFromCategory(string isbn, string category)
        {
            bool success = true;
            return success;
        }

        public bool addBookToCategory(string isbn, string category)
        {
            bool success = true;
            return success;
        }

        public DataTable getAllBooksWithinCategory(string category)
        {
            DataTable results = new DataTable();
            return results;
        }

        public DataTable getAllBooksBySearchCriteria(string searchCriteria)
        {
            DataTable results = new DataTable();
            OleDbConnection conn;
            conn = new OleDbConnection(Properties.Settings.Default.connection);
            conn.Open();
            //if (conn.State == ConnectionState.Open)
            //lbl_Test.Text = "Connected";
            string strSql = "Select title, authors as Author, num_pages as Pages, isbn From Books WHERE title like '%" + searchCriteria + "%' OR authors like  '%" + searchCriteria + "%';";
            

            OleDbCommand cmd = new OleDbCommand(strSql, conn);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            
            da.Fill(results);
            conn.Close();
            return results;
        }

        public string[] getCategoriesInUserBooksByISBN(string isbn)
        {
            string[] categories;


            DataTable results = new DataTable();
            OleDbConnection conn;
            conn = new OleDbConnection(Properties.Settings.Default.connection);
            conn.Open();
            //if (conn.State == ConnectionState.Open)
            //lbl_Test.Text = "Connected";
            string strSql = "Select category FROM UserBooks WHERE isbn = '" + isbn + "';";


            OleDbCommand cmd = new OleDbCommand(strSql, conn);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            da.Fill(results);
            conn.Close();

            //fill array from datatable
            categories = new string[results.Rows.Count];
            int index = 0;
            foreach(DataRow currentRow in results.Rows)
            {
                categories[index] = currentRow["category"].ToString();
                index++;
            }

            return categories;
        }

        public Dictionary<string, string> getAllCategoriesWithNumber()
        {
            Dictionary<string, string> categories = new Dictionary<string, string>();

            DataTable results = new DataTable();
            OleDbConnection conn;
            conn = new OleDbConnection(Properties.Settings.Default.connection);
            conn.Open();
            string strSql = "Select * From Categories";


            OleDbCommand cmd = new OleDbCommand(strSql, conn);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            da.Fill(results);
            conn.Close();

            //loop through datatable and add to dictionary
            foreach(DataRow currentRow in results.Rows)
            {
                categories.Add(currentRow["ID"].ToString(), currentRow["CategoryType"].ToString());
            }

            return categories;
        }

        public string getCategoryNumberByName(string categoryName)
        {
            string categoryNumber = "";

            DataTable results = new DataTable();
            OleDbConnection conn;
            conn = new OleDbConnection(Properties.Settings.Default.connection);
            conn.Open();
            //if (conn.State == ConnectionState.Open)
            //lbl_Test.Text = "Connected";
            string strSql = "Select ID FROM Categories WHERE CategoryType = '" + categoryName + "';";


            OleDbCommand cmd = new OleDbCommand(strSql, conn);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            da.Fill(results);
            conn.Close();

            if (results.Rows.Count > 0)
            {
                DataRow topRow = results.Rows[0];
                categoryNumber = topRow["ID"].ToString();
            }

            return categoryNumber;
        }

        public Dictionary<string, string> getAllBooksInCategoryByName(string categoryName)
        {
            Dictionary<string, string> books = new Dictionary<string, string>();

            string categoryNumber = getCategoryNumberByName(categoryName);


            DataTable results = new DataTable();
            OleDbConnection conn;
            conn = new OleDbConnection(Properties.Settings.Default.connection);
            conn.Open();
            string strSql = "Select * From UserBooks WHERE category = " +  categoryNumber + ";";


            OleDbCommand cmd = new OleDbCommand(strSql, conn);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            da.Fill(results);
            conn.Close();

            //loop through datatable and add to dictionary
            foreach (DataRow currentRow in results.Rows)
            {
                books.Add(currentRow["isbn"].ToString(), currentRow["image"].ToString());
            }

            return books;
        }
    }
}
