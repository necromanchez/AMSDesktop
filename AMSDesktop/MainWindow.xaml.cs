using AMSDesktop.Config;
using AMSDesktop.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMSDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Line.SelectedValuePath = "Key";
            this.Line.DisplayMemberPath = "Value";
            this.Line.Items.Add(new KeyValuePair<int, string>(0, "0"));
            this.Line.Items.Add(new KeyValuePair<int, string>(30, "300"));
            this.Line.Items.Add(new KeyValuePair<int, string>(50, "500"));
            this.Line.Items.Add(new KeyValuePair<int, string>(100, "1000"));
        }

        string TapStatus = "IN";

        private void timein_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            timein.Background = (Brush)bc.ConvertFrom("#FF079AD9");
            timeout.Background = (Brush)bc.ConvertFrom("#FFCDCDCD");
            TapStatus = "IN";
        }

        private void timeout_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            timeout.Background = (Brush)bc.ConvertFrom("#FF079AD9");
            timein.Background = (Brush)bc.ConvertFrom("#FFCDCDCD");
            TapStatus = "OUT";
        }

        private string dec2Hex(long val)
        {
            return Convert.ToString(val, 16);
        }

        private double hex2Dec(string strHex)
        {
            return Convert.ToInt16(strHex, 16);
        }

        public string GetEmployeeStatus(string EmpNo)
        {
            M_Employee_Status status = new M_Employee_Status();
            using (SQLiteConnection c = new SQLiteConnection(AppConfig.AMSDBSQLite))
            {
                if (c.State == System.Data.ConnectionState.Open)
                {
                    c.Close();
                }
                c.Open();
                string sql = "SELECT * from M_Employee_Status WHERE EmployNo = '" + EmpNo + "' ORDER BY ID DESC LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    try
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    status.Status = rdr["Status"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
            return status.Status;
        }

        public string GetEmployeePosition(string EmpNo)
        {
            M_Employee_Position pos = new M_Employee_Position();
            using (SQLiteConnection c = new SQLiteConnection(AppConfig.AMSDBSQLite))
            {
                if (c.State == System.Data.ConnectionState.Open)
                {
                    c.Close();
                }
                c.Open();
                string sql = "SELECT * from M_Employee_Position WHERE EmployNo = '" + EmpNo + "' ORDER BY ID DESC LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    try
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    pos.Position = rdr["Position"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
            return pos.Position;
        }

        public string GetEmployeeCostCode(string EmpNo)
        {

            M_Employee_CostCenter costcode = new M_Employee_CostCenter();
            using (SQLiteConnection c = new SQLiteConnection(AppConfig.AMSDBSQLite))
            {
                if (c.State == System.Data.ConnectionState.Open)
                {
                    c.Close();
                }
                c.Open();
                string sql = "SELECT * from M_Employee_CostCenter WHERE EmployNo = '" + EmpNo + "' ORDER BY ID DESC LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    try
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    costcode.CostCenter_AMS = rdr["CostCenter_AMS"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
            return costcode.CostCenter_AMS;
        }

        private void GetEmployeeDetails(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    string SourceValue = dec2Hex(Convert.ToInt64(RFID.Text));
                    string Hexvalue = SourceValue.Substring(SourceValue.Length - 4);
                    string Prefix = SourceValue.Remove(SourceValue.Length - 4).ToUpper();
                    string THERFID = hex2Dec(Hexvalue).ToString();
                    string Empnumber = "";
                    string rawrfid = Convert.ToInt64(RFID.Text).ToString();

                    bool rfidconverted = false;
                    M_Employee_Master_List list = new M_Employee_Master_List();
                    using (SQLiteConnection c = new SQLiteConnection(AppConfig.AMSDBSQLite))
                    {
                        if (c.State == System.Data.ConnectionState.Open)
                        {
                           c.Close();
                        }
                        c.Open();

                        //Converted RFID
                        string sql = "SELECT * from M_Employee_Master_List WHERE Status = 'ACTIVE' AND RFID = "+ THERFID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                        {
                            try
                            {
                                int affectedRows = cmd.ExecuteNonQuery();
                                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                                {
                                    
                                    if (rdr.HasRows)
                                    {
                                        rfidconverted = true;
                                        while (rdr.Read())
                                        {
                                            Empnumber = rdr["EmpNo"].ToString();
                                            list.ID = Convert.ToInt64(rdr["ID"]);
                                            list.REFID = rdr["REFID"].ToString();
                                            list.ADID = rdr["ADID"].ToString();
                                            list.EmpNo = rdr["EmpNo"].ToString();
                                            list.Family_Name_Suffix = rdr["Family_Name_Suffix"].ToString();
                                            list.Family_Name = rdr["Family_Name"].ToString();
                                            list.First_Name = rdr["First_Name"].ToString();
                                            list.Middle_Name = rdr["Middle_Name"].ToString();
                                            list.Date_Hired = rdr["Date_Hired"].ToString();
                                            list.Date_Resigned = rdr["Date_Resigned"].ToString();
                                            list.Status = rdr["Status"].ToString();
                                            list.Emp_Category = rdr["Emp_Category"].ToString();
                                            list.Date_Regularized = rdr["Date_Regularized"].ToString();
                                            list.Position = rdr["Position"].ToString();
                                            list.Email = rdr["Email"].ToString();
                                            list.Gender = rdr["Gender"].ToString();
                                            list.RFID = rdr["RFID"].ToString();
                                            list.EmployeePhoto = rdr["EmployeePhoto"].ToString();
                                            list.Section = rdr["Section"].ToString();
                                            list.Department = rdr["Department"].ToString();
                                            list.Company = rdr["Company"].ToString();
                                            list.CostCode = rdr["CostCode"].ToString();

                                          
                                            //Show the Details
                                            Empno.Content = Empnumber;
                                            Name.Content = list.First_Name + ' ' + list.Family_Name;
                                            Position.Content = GetEmployeePosition(Empnumber);
                                            Status.Content = GetEmployeeStatus(Empnumber);
                                            Comp.Content = list.Company;
                                            this.empphoto.Source = new BitmapImage(new Uri(AppConfig.forderPath + "\\PictureResources\\EmployeePhoto\\" + list.EmployeePhoto));
                                        }
                                    }
                                    else
                                    {
                                        rfidconverted = false;
                                    }
                                       
                                   
                                }
                            }
                            catch (Exception err)
                            {
                             
                            }
                        }


                        if (rfidconverted == false)
                        {
                            //Unconverted
                            sql = "SELECT * from M_Employee_Master_List WHERE Status = 'ACTIVE' AND RFID = " + RFID.Text;
                            using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                            {
                                try
                                {
                                    int affectedRows = cmd.ExecuteNonQuery();
                                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                                    {

                                        if (rdr.HasRows)
                                        {
                                            while (rdr.Read())
                                            {
                                                Empnumber = rdr["EmpNo"].ToString();
                                                list.ID = Convert.ToInt64(rdr["ID"]);
                                                list.REFID = rdr["REFID"].ToString();
                                                list.ADID = rdr["ADID"].ToString();
                                                list.EmpNo = rdr["EmpNo"].ToString();
                                                list.Family_Name_Suffix = rdr["Family_Name_Suffix"].ToString();
                                                list.Family_Name = rdr["Family_Name"].ToString();
                                                list.First_Name = rdr["First_Name"].ToString();
                                                list.Middle_Name = rdr["Middle_Name"].ToString();
                                                list.Date_Hired = rdr["Date_Hired"].ToString();
                                                list.Date_Resigned = rdr["Date_Resigned"].ToString();
                                                list.Status = rdr["Status"].ToString();
                                                list.Emp_Category = rdr["Emp_Category"].ToString();
                                                list.Date_Regularized = rdr["Date_Regularized"].ToString();
                                                list.Position = rdr["Position"].ToString();
                                                list.Email = rdr["Email"].ToString();
                                                list.Gender = rdr["Gender"].ToString();
                                                list.RFID = rdr["RFID"].ToString();
                                                list.EmployeePhoto = rdr["EmployeePhoto"].ToString();
                                                list.Section = rdr["Section"].ToString();
                                                list.Department = rdr["Department"].ToString();
                                                list.Company = rdr["Company"].ToString();
                                                list.CostCode = rdr["CostCode"].ToString();

                                                //Show the Details
                                                Empno.Content = Empnumber;
                                                Name.Content = list.First_Name + ' ' + list.Family_Name;
                                                Position.Content = GetEmployeePosition(Empnumber);
                                                Status.Content = GetEmployeeStatus(Empnumber);
                                                Comp.Content = list.Company;
                                                this.empphoto.Source = new BitmapImage(new Uri(AppConfig.forderPath + "\\PictureResources\\EmployeePhoto\\" + list.EmployeePhoto));
                                            }
                                        }
                                        else
                                        {
                                            rfidconverted = false;
                                        }
                                    }
                                }
                                catch (Exception err)
                                {

                                }
                            }

                        }
                    }
                    string EmpCostCode = GetEmployeeCostCode(Empnumber);
                }

                

            }
            catch(Exception err)
            {

            }
        }
        
        

    }
}
